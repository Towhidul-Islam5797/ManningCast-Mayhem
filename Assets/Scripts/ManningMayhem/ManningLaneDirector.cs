using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Runs seven alternating, increasingly fast lanes. Lane positions, world bounds, and all ten
/// obstacle templates are scene-authored so designers can tune the game directly in Hierarchy.
/// </summary>
public sealed class ManningLaneDirector : MonoBehaviour
{
    private const int RequiredLaneCount = 7;

    public static ManningLaneDirector Instance { get; private set; }

    [Header("Hierarchy - move these transforms to edit the course")]
    [SerializeField] private Transform leftBoundary;
    [SerializeField] private Transform rightBoundary;
    [SerializeField] private Transform[] laneRoots = new Transform[RequiredLaneCount];

    [Header("Hierarchy - edit child Visual and Hitbox objects")]
    [SerializeField] private ManningLaneItem[] obstacleTemplates = new ManningLaneItem[0];

    [Header("Hard Gameplay")]
    [SerializeField, Min(2)] private int initialItemsPerLane = 4;
    [SerializeField, Min(0.4f)] private float minimumSpawnInterval = 1.35f;
    [SerializeField, Min(0.5f)] private float maximumSpawnInterval = 2.1f;
    [SerializeField, Min(0.25f)] private float firstLaneSpeed = 1.65f;
    [SerializeField, Min(0f)] private float speedAddedPerLane = 0.13f;
    [SerializeField, Range(0f, 1f)] private float spawnAccelerationAtMaxDifficulty = 0.38f;
    [SerializeField, Min(14)] private int maximumActiveItems = 56;
    [SerializeField, Min(0.2f)] private float despawnPadding = 1.4f;

    private readonly List<ManningLaneItem> activeItems = new List<ManningLaneItem>();
    private float[] spawnTimers;

    public int LaneCount => laneRoots != null && laneRoots.Length > 0 ? laneRoots.Length : RequiredLaneCount;
    public IReadOnlyList<ManningLaneItem> ObstacleTemplates => obstacleTemplates;

    public void ConfigureHierarchy(Transform left, Transform right, Transform[] lanes, ManningLaneItem[] templates)
    {
        leftBoundary = left;
        rightBoundary = right;
        laneRoots = lanes;
        obstacleTemplates = templates;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        EnsureLaneRoots();
        spawnTimers = new float[LaneCount];

        for (int lane = 0; lane < LaneCount; lane++)
        {
            spawnTimers[lane] = Random.Range(0.65f, 1.2f);
            PopulateLane(lane);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void OnValidate()
    {
        if (maximumSpawnInterval < minimumSpawnInterval) maximumSpawnInterval = minimumSpawnInterval;
        initialItemsPerLane = Mathf.Max(2, initialItemsPerLane);
        maximumActiveItems = Mathf.Max(RequiredLaneCount * 2, maximumActiveItems);
    }

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameManager.GameState.Playing ||
            PauseManager.IsPaused || spawnTimers == null) return;

        float difficulty = GameManager.Instance.DifficultyMultiplier;
        float difficulty01 = Mathf.InverseLerp(GameManager.Instance.MinimumDifficultyMultiplier,
            GameManager.Instance.MaximumDifficultyMultiplier, difficulty);
        float timerRate = Mathf.Lerp(1f, 1f + spawnAccelerationAtMaxDifficulty, difficulty01);

        for (int lane = 0; lane < LaneCount; lane++)
        {
            spawnTimers[lane] -= Time.deltaTime * timerRate;
            if (spawnTimers[lane] > 0f) continue;

            Spawn(lane, PickWeightedKind(), lane % 2 == 0 ? LeftX : RightX);
            spawnTimers[lane] = Random.Range(minimumSpawnInterval, maximumSpawnInterval);
        }
    }

    public bool IsOutsideLane(float x) => x < LeftX - despawnPadding || x > RightX + despawnPadding;

    public void Despawn(ManningLaneItem item)
    {
        if (item == null) return;
        activeItems.Remove(item);
        Destroy(item.gameObject);
    }

    public bool TryDistractNearestAthlete(Vector3 playerPosition)
    {
        ManningLaneItem nearest = null;
        float nearestDistance = float.MaxValue;

        for (int i = activeItems.Count - 1; i >= 0; i--)
        {
            ManningLaneItem item = activeItems[i];
            if (item == null)
            {
                activeItems.RemoveAt(i);
                continue;
            }

            if (!item.IsAthlete) continue;
            float distance = (item.transform.position - playerPosition).sqrMagnitude;
            if (distance >= nearestDistance) continue;
            nearest = item;
            nearestDistance = distance;
        }

        if (nearest == null || GameManager.Instance == null || !GameManager.Instance.SpendFootball()) return false;

        nearest.Distract();
        GameManager.Instance.AddBonusScore(150, "Athlete distracted - opening created! +150");
        ManningAudio.Instance?.PlayBonus();
        return true;
    }

    public float GetFallbackVisualSize(ManningLaneItemKind kind)
    {
        switch (kind)
        {
            case ManningLaneItemKind.AthleteGray:
            case ManningLaneItemKind.AthleteRed: return 1.2f;
            case ManningLaneItemKind.Recliner: return 1.05f;
            case ManningLaneItemKind.ChallengeFlag: return 0.9f;
            case ManningLaneItemKind.BlueQuarterZip: return 0.78f;
            case ManningLaneItemKind.GoldenQuarterZip: return 0.84f;
            case ManningLaneItemKind.SandwichChicken: return 0.78f;
            case ManningLaneItemKind.SandwichJersey: return 0.68f;
            case ManningLaneItemKind.Remote: return 0.68f;
            default: return 0.62f;
        }
    }

    public Vector2 GetFallbackColliderSize(ManningLaneItemKind kind)
    {
        switch (kind)
        {
            case ManningLaneItemKind.AthleteGray:
            case ManningLaneItemKind.AthleteRed: return new Vector2(0.68f, 0.92f);
            case ManningLaneItemKind.Recliner: return new Vector2(0.78f, 0.65f);
            case ManningLaneItemKind.ChallengeFlag: return new Vector2(0.66f, 0.44f);
            case ManningLaneItemKind.BlueQuarterZip: return new Vector2(0.5f, 0.58f);
            case ManningLaneItemKind.GoldenQuarterZip: return new Vector2(0.56f, 0.65f);
            case ManningLaneItemKind.SandwichChicken: return new Vector2(0.62f, 0.34f);
            case ManningLaneItemKind.SandwichJersey: return new Vector2(0.54f, 0.28f);
            case ManningLaneItemKind.Remote: return new Vector2(0.32f, 0.54f);
            default: return new Vector2(0.49f, 0.3f);
        }
    }

    private float LeftX => leftBoundary != null ? leftBoundary.position.x : -7.8f;
    private float RightX => rightBoundary != null ? rightBoundary.position.x : 10.5f;

    private void PopulateLane(int lane)
    {
        int direction = lane % 2 == 0 ? 1 : -1;
        float min = LeftX + 1.1f;
        float max = RightX - 1.1f;
        for (int slot = 0; slot < initialItemsPerLane; slot++)
        {
            float t = (slot + 0.5f) / initialItemsPerLane;
            float x = Mathf.Lerp(min, max, direction > 0 ? t : 1f - t);
            Spawn(lane, InitialKind(lane, slot), x);
        }
    }

    private void Spawn(int lane, ManningLaneItemKind kind, float x)
    {
        if (activeItems.Count >= maximumActiveItems || lane < 0 || lane >= LaneCount) return;

        int direction = lane % 2 == 0 ? 1 : -1;
        ManningLaneItem template = FindTemplate(kind);
        ManningLaneItem item;

        if (template != null)
        {
            GameObject clone = Instantiate(template.gameObject, laneRoots[lane]);
            clone.name = $"{kind} (Runtime)";
            item = clone.GetComponent<ManningLaneItem>();
            item.Initialize(this, direction, firstLaneSpeed + lane * speedAddedPerLane, lane);
            clone.SetActive(true);
        }
        else
        {
            GameObject itemObject = new GameObject($"{kind} (Runtime Fallback)");
            itemObject.transform.SetParent(laneRoots[lane], false);
            item = itemObject.AddComponent<ManningLaneItem>();
            item.Initialize(this, kind, direction, firstLaneSpeed + lane * speedAddedPerLane, lane);
        }

        item.transform.position = new Vector3(x, laneRoots[lane].position.y, 0f);
        activeItems.Add(item);
    }

    private ManningLaneItem FindTemplate(ManningLaneItemKind kind)
    {
        if (obstacleTemplates == null) return null;
        foreach (ManningLaneItem template in obstacleTemplates)
        {
            if (template != null && template.Kind == kind) return template;
        }
        return null;
    }

    private ManningLaneItemKind PickWeightedKind()
    {
        if (obstacleTemplates == null || obstacleTemplates.Length == 0)
            return (ManningLaneItemKind)Random.Range(0, 10);

        float total = 0f;
        foreach (ManningLaneItem template in obstacleTemplates)
            if (template != null) total += template.SpawnWeight;

        if (total <= 0f) return ManningLaneItemKind.AthleteGray;
        float roll = Random.value * total;
        foreach (ManningLaneItem template in obstacleTemplates)
        {
            if (template == null) continue;
            roll -= template.SpawnWeight;
            if (roll <= 0f) return template.Kind;
        }

        return ManningLaneItemKind.AthleteGray;
    }

    private static ManningLaneItemKind InitialKind(int lane, int slot)
    {
        if (slot == 0) return lane % 2 == 0 ? ManningLaneItemKind.AthleteGray : ManningLaneItemKind.AthleteRed;
        if (slot == 1) return lane % 3 == 0 ? ManningLaneItemKind.ChallengeFlag : ManningLaneItemKind.SandwichChicken;
        if (slot == 2) return lane % 3 == 0 ? ManningLaneItemKind.Recliner
            : lane % 3 == 1 ? ManningLaneItemKind.BlueQuarterZip
            : ManningLaneItemKind.Remote;
        return lane % 2 == 0 ? ManningLaneItemKind.Football : ManningLaneItemKind.GoldenQuarterZip;
    }

    private void EnsureLaneRoots()
    {
        bool valid = laneRoots != null && laneRoots.Length == RequiredLaneCount;
        if (valid)
        {
            for (int i = 0; i < laneRoots.Length; i++) valid &= laneRoots[i] != null;
        }
        if (valid) return;

        laneRoots = new Transform[RequiredLaneCount];
        for (int lane = 0; lane < RequiredLaneCount; lane++)
        {
            GameObject root = new GameObject($"Lane {lane + 1:00} (Runtime Fallback)");
            root.transform.SetParent(transform, false);
            root.transform.position = new Vector3(0f, -3.05f + lane, 0f);
            laneRoots[lane] = root.transform;
        }
    }
}
