using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>Grid movement, collision recovery, and football activation.</summary>
public sealed class PlayerMovement : MonoBehaviour
{
    [SerializeField, Min(0.05f)] private float moveDuration = 0.15f;
    [SerializeField, Min(0.25f)] private float gridSize = 1f;
    [SerializeField] private LayerMask wallLayer;

    private Animator animator;
    private BoxCollider2D bodyCollider;
    private ManningCharacterSpriteAnimator spriteAnimator;
    private GameManager subscribedGame;
    private bool isMoving;
    private bool inputLocked;
    private Vector3 startPosition;
    private float invulnerableUntil;

    public Vector3 StartPosition => startPosition;
    public bool InputLocked => inputLocked;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<BoxCollider2D>();
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        BindGameManager();
    }

    private void Start()
    {
        BindGameManager();
        spriteAnimator = GetComponent<ManningCharacterSpriteAnimator>();
    }

    private void OnDisable()
    {
        UnbindGameManager();
    }

    private void OnDestroy()
    {
        UnbindGameManager();
    }

    public void BindGameManager()
    {
        GameManager game = GameManager.Instance;
        if (game == null || subscribedGame == game) return;
        UnbindGameManager();
        subscribedGame = game;
        subscribedGame.LifeLost += OnLifeLost;
    }

    private void UnbindGameManager()
    {
        if (subscribedGame != null) subscribedGame.LifeLost -= OnLifeLost;
        subscribedGame = null;
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null || inputLocked || isMoving || PauseManager.IsPaused) return;

        if (keyboard.spaceKey.wasPressedThisFrame || keyboard.enterKey.wasPressedThisFrame)
        {
            UseFootballPowerUp();
            return;
        }

        Vector2Int direction = Vector2Int.zero;
        if (keyboard.upArrowKey.wasPressedThisFrame || keyboard.wKey.wasPressedThisFrame) direction = Vector2Int.up;
        else if (keyboard.downArrowKey.wasPressedThisFrame || keyboard.sKey.wasPressedThisFrame) direction = Vector2Int.down;
        else if (keyboard.leftArrowKey.wasPressedThisFrame || keyboard.aKey.wasPressedThisFrame) direction = Vector2Int.left;
        else if (keyboard.rightArrowKey.wasPressedThisFrame || keyboard.dKey.wasPressedThisFrame) direction = Vector2Int.right;

        TryMove(direction);
    }

    public void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        TryMove(GetCardinalDirection(context.ReadValue<Vector2>()));
    }

    public void OnUsePerformed(InputAction.CallbackContext context)
    {
        if (context.performed) UseFootballPowerUp();
    }

    public void ConfigureRuntimeArt(ManningCharacterSpriteAnimator runtimeAnimator)
    {
        spriteAnimator = runtimeAnimator;
        if (animator != null) animator.enabled = false;
        if (bodyCollider != null)
        {
            bodyCollider.offset = new Vector2(0f, -0.12f);
            bodyCollider.size = new Vector2(0.62f, 1.18f);
        }
    }

    private void TryMove(Vector2Int direction)
    {
        if (direction == Vector2Int.zero || isMoving || inputLocked || PauseManager.IsPaused) return;
        if (GameManager.Instance == null || GameManager.Instance.IsGameOver) return;

        Vector3 targetPosition = transform.position + new Vector3(direction.x, direction.y, 0f) * gridSize;
        if (IsWallAt(targetPosition)) return;

        SetBlendDirection(direction);
        spriteAnimator?.SetDirection(direction);
        StartCoroutine(MoveToTile(direction));
    }

    private bool IsWallAt(Vector3 position)
    {
        if (bodyCollider == null || wallLayer.value == 0) return false;
        Vector2 checkPosition = (Vector2)position + bodyCollider.offset;
        return Physics2D.OverlapBox(checkPosition, bodyCollider.size, 0f, wallLayer) != null;
    }

    private static Vector2Int GetCardinalDirection(Vector2 input)
    {
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) return input.x > 0f ? Vector2Int.right : Vector2Int.left;
        if (Mathf.Abs(input.y) > 0.1f) return input.y > 0f ? Vector2Int.up : Vector2Int.down;
        return Vector2Int.zero;
    }

    private void SetBlendDirection(Vector2Int direction)
    {
        if (animator == null || !animator.enabled) return;
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }

    private void ResetToIdle()
    {
        if (animator != null && animator.enabled)
        {
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", 0f);
        }
        spriteAnimator?.SetMoving(false);
    }

    private IEnumerator MoveToTile(Vector2Int direction)
    {
        isMoving = true;
        spriteAnimator?.SetMoving(true);
        Vector3 start = transform.position;
        Vector3 end = start + new Vector3(direction.x, direction.y, 0f) * gridSize;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, Mathf.Clamp01(elapsed / moveDuration));
            yield return null;
        }

        transform.position = end;
        isMoving = false;
        Physics2D.SyncTransforms();
        ResetToIdle();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddMoveScore();
            GameManager.Instance.RegisterProgress((transform.position.y - startPosition.y) / 7f);
        }

        ManningAudio.Instance?.PlayMove();
    }

    public void HandleObstacleHit(int scorePenalty)
    {
        HandleObstacleHit(GameManager.LifeLossReason.Other, scorePenalty);
    }

    public void HandleObstacleHit(GameManager.LifeLossReason reason, int scorePenalty = 50)
    {
        if (Time.unscaledTime < invulnerableUntil || GameManager.Instance == null || GameManager.Instance.IsGameOver) return;
        StopAllCoroutines();
        isMoving = false;
        transform.SetParent(null, true);
        GameManager.Instance.PlayerHitObstacle(reason, scorePenalty);
    }

    public void HandleSandwichHit(float timePenaltySeconds)
    {
        if (GameManager.Instance != null) GameManager.Instance.PlayerHitSandwich(timePenaltySeconds);
    }

    public void HandleGoalReached()
    {
        if (inputLocked || GameManager.Instance == null || GameManager.Instance.IsGameOver) return;
        StopAllCoroutines();
        isMoving = false;
        inputLocked = true;
        ResetToIdle();
        GameManager.Instance.PlayerReachedGoal();
    }

    public void TeleportToStart()
    {
        StopAllCoroutines();
        isMoving = false;
        transform.SetParent(null, true);
        transform.position = startPosition;
        ResetToIdle();
        Physics2D.SyncTransforms();
    }

    private void OnLifeLost(GameManager.LifeLossReason reason)
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
        {
            TeleportToStart();
            invulnerableUntil = Time.unscaledTime + 0.65f;
        }
        else
        {
            inputLocked = true;
        }
    }

    private void UseFootballPowerUp()
    {
        if (inputLocked || PauseManager.IsPaused || GameManager.Instance == null) return;

        if (GameManager.Instance.FootballCharges <= 0)
        {
            GameManager.Instance.ReportNegative("Collect a football first!");
            return;
        }

        if (ManningLaneDirector.Instance == null || !ManningLaneDirector.Instance.TryDistractNearestAthlete(transform.position))
        {
            GameManager.Instance.ReportNegative("No athlete is close enough to distract.");
        }
    }
}
