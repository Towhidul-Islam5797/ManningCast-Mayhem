using System.Collections;
using UnityEngine;

public enum ManningLaneItemKind
{
    AthleteGray,
    AthleteRed,
    ChallengeFlag,
    SandwichChicken,
    SandwichJersey,
    Recliner,
    Remote,
    BlueQuarterZip,
    GoldenQuarterZip,
    Football
}

/// <summary>
/// A moving lane object with client-approved danger, safe, bonus, or penalty behavior.
/// Production instances are cloned from inactive, hierarchy-authored templates so artists can
/// change each sprite scale, collider, spawn weight, and speed multiplier without editing code.
/// </summary>
public sealed class ManningLaneItem : MonoBehaviour
{
    [Header("Obstacle Identity")]
    [SerializeField] private ManningLaneItemKind kind;

    [Header("Hierarchy References")]
    [SerializeField] private SpriteRenderer visual;
    [SerializeField] private Collider2D hitbox;

    [Header("Hard Mode Tuning")]
    [SerializeField, Range(0.001f, 1f)] private float spawnWeight = 0.1f;
    [SerializeField, Min(0.25f)] private float speedMultiplier = 1f;

    private ManningLaneDirector director;
    private float speed;
    private int direction;
    private bool consumed;
    private bool distracted;
    private Vector3 initialVisualScale = Vector3.one;

    public ManningLaneItemKind Kind => kind;
    public float SpawnWeight => Mathf.Max(0.001f, spawnWeight);
    public float SpeedMultiplier => Mathf.Max(0.25f, speedMultiplier);
    public bool IsAthlete => kind == ManningLaneItemKind.AthleteGray || kind == ManningLaneItemKind.AthleteRed;

    /// <summary>Called by the hierarchy builder when creating the editable source templates.</summary>
    public void ConfigureTemplate(ManningLaneItemKind itemKind, SpriteRenderer itemVisual, Collider2D itemHitbox,
        float weight, float itemSpeedMultiplier)
    {
        kind = itemKind;
        visual = itemVisual;
        hitbox = itemHitbox;
        spawnWeight = Mathf.Max(0.001f, weight);
        speedMultiplier = Mathf.Max(0.25f, itemSpeedMultiplier);
    }

    /// <summary>Initializes a clone of an authored hierarchy template.</summary>
    public void Initialize(ManningLaneDirector owner, int moveDirection, float moveSpeed, int laneIndex)
    {
        director = owner;
        direction = moveDirection >= 0 ? 1 : -1;
        speed = Mathf.Max(0.1f, moveSpeed) * SpeedMultiplier;
        consumed = false;
        distracted = false;

        if (visual == null) visual = GetComponentInChildren<SpriteRenderer>(true);
        if (hitbox == null) hitbox = GetComponent<Collider2D>();
        if (hitbox != null)
        {
            hitbox.enabled = true;
            hitbox.isTrigger = true;
        }

        Rigidbody2D body = GetComponent<Rigidbody2D>();
        if (body == null) body = gameObject.AddComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Kinematic;
        body.gravityScale = 0f;
        body.simulated = true;

        if (visual != null)
        {
            initialVisualScale = visual.transform.localScale;
            visual.sortingOrder = 82 + laneIndex;
            visual.flipX = direction < 0 && IsAthlete;
            visual.color = Color.white;
        }
    }

    /// <summary>Runtime fallback used only when a scene has not yet been upgraded.</summary>
    public void Initialize(ManningLaneDirector owner, ManningLaneItemKind itemKind, int moveDirection,
        float moveSpeed, int laneIndex)
    {
        kind = itemKind;
        EnsureFallbackComponents(owner);
        Initialize(owner, moveDirection, moveSpeed, laneIndex);
    }

    private void Update()
    {
        if (director == null || GameManager.Instance == null ||
            GameManager.Instance.CurrentState != GameManager.GameState.Playing || PauseManager.IsPaused) return;

        float multiplier = distracted ? 3.4f : GameManager.Instance.DifficultyMultiplier;
        transform.position += Vector3.right * (direction * speed * multiplier * Time.deltaTime);
        if (distracted && visual != null)
        {
            visual.transform.localScale = Vector3.MoveTowards(
                visual.transform.localScale, initialVisualScale * 0.18f, Time.deltaTime * 1.7f);
            Color color = visual.color;
            color.a = Mathf.MoveTowards(color.a, 0f, Time.deltaTime * 1.8f);
            visual.color = color;
        }

        if (director.IsOutsideLane(transform.position.x)) director.Despawn(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (consumed) return;
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player == null || GameManager.Instance == null || GameManager.Instance.IsGameOver) return;

        switch (kind)
        {
            case ManningLaneItemKind.AthleteGray:
            case ManningLaneItemKind.AthleteRed:
                Consume();
                player.HandleObstacleHit(GameManager.LifeLossReason.Athlete, 75);
                ManningAudio.Instance?.PlayHit();
                break;

            case ManningLaneItemKind.ChallengeFlag:
                Consume();
                player.HandleObstacleHit(GameManager.LifeLossReason.ChallengeFlag, 75);
                ManningAudio.Instance?.PlayHit();
                break;

            case ManningLaneItemKind.SandwichChicken:
            case ManningLaneItemKind.SandwichJersey:
                Consume();
                player.HandleSandwichHit(8f);
                ManningAudio.Instance?.PlayHit();
                break;

            case ManningLaneItemKind.Football:
                Consume();
                GameManager.Instance.AddFootball(100);
                ManningAudio.Instance?.PlayPickup();
                break;

            case ManningLaneItemKind.GoldenQuarterZip:
                Consume();
                GameManager.Instance.AddBonusScore(250, "Golden quarter-zip! +250");
                ManningAudio.Instance?.PlayBonus();
                break;

            case ManningLaneItemKind.BlueQuarterZip:
            case ManningLaneItemKind.Recliner:
            case ManningLaneItemKind.Remote:
                GameManager.Instance.ReportPositive("Safe route!");
                break;
        }
    }

    public void Distract()
    {
        if (!IsAthlete || consumed) return;
        consumed = true;
        distracted = true;
        if (hitbox != null) hitbox.enabled = false;
        StartCoroutine(LeaveLane());
    }

    private IEnumerator LeaveLane()
    {
        yield return new WaitForSeconds(0.75f);
        if (director != null) director.Despawn(this);
    }

    private void Consume()
    {
        consumed = true;
        if (hitbox != null) hitbox.enabled = false;
        if (director != null) director.Despawn(this);
    }

    private void EnsureFallbackComponents(ManningLaneDirector owner)
    {
        if (visual == null)
        {
            GameObject visualObject = new GameObject("Visual");
            visualObject.transform.SetParent(transform, false);
            visual = visualObject.AddComponent<SpriteRenderer>();
            visual.sprite = ManningAssetLibrary.LoadWorldSprite(GetAssetName(kind), owner.GetFallbackVisualSize(kind));
        }

        if (hitbox == null)
        {
            BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
            box.size = owner.GetFallbackColliderSize(kind);
            hitbox = box;
        }
    }

    private static string GetAssetName(ManningLaneItemKind itemKind)
    {
        switch (itemKind)
        {
            case ManningLaneItemKind.AthleteGray: return "AthleteGray";
            case ManningLaneItemKind.AthleteRed: return "AthleteRed";
            case ManningLaneItemKind.ChallengeFlag: return "ChallengeFlag";
            case ManningLaneItemKind.SandwichChicken: return "SandwichChicken";
            case ManningLaneItemKind.SandwichJersey: return "SandwichJersey";
            case ManningLaneItemKind.Recliner: return "Recliner";
            case ManningLaneItemKind.Remote: return "Remote";
            case ManningLaneItemKind.BlueQuarterZip: return "BlueQZip";
            case ManningLaneItemKind.GoldenQuarterZip: return "GoldenQZip";
            default: return "Football";
        }
    }
}
