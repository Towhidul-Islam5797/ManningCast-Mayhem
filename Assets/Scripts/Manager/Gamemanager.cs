using System;
using UnityEngine;

/// <summary>
/// Authoritative round state for ManningCast Mayhem. The current client rules use
/// three lives and a hard thirty-second countdown that resets after each lost life.
/// </summary>
public sealed class GameManager : MonoBehaviour
{
    public enum GameState { Playing, Won, Lost }
    public enum LifeLossReason { Athlete, ChallengeFlag, Timeout, Other }

    public static GameManager Instance { get; private set; }

    [Header("Round")]
    [SerializeField, Min(1)] private int startingLives = 3;
    [SerializeField, Min(10f)] private float roundDuration = 30f;

    [Header("Hard Difficulty Ramp")]
    [SerializeField, Min(0.5f)] private float minimumDifficultyMultiplier = 1.1f;
    [SerializeField, Min(1f)] private float maximumDifficultyMultiplier = 2.35f;
    [SerializeField, Min(15f)] private float fullDifficultyAfterSeconds = 75f;

    [Header("Scoring")]
    [SerializeField, Min(0)] private int scorePerMove = 10;
    [SerializeField, Min(0)] private int scorePenaltyPerLifeLost = 75;
    [SerializeField, Min(0)] private int scorePerSecondRemaining = 5;

    private GameState currentState;
    private int currentLives;
    private int currentScore;
    private int footballCharges;
    private float remainingTime;
    private float elapsedTime;
    private float furthestProgress;

    public event Action<int> ScoreChanged;
    public event Action<int> LivesChanged;
    public event Action<float> TimeChanged;
    public event Action<int> FootballChanged;
    public event Action<GameState> StateChanged;
    public event Action<LifeLossReason> LifeLost;
    public event Action<string> PositiveFeedback;
    public event Action<string> NegativeFeedback;

    public GameState CurrentState => currentState;
    public bool IsGameOver => currentState != GameState.Playing;
    public int CurrentLives => currentLives;
    public int CurrentScore => currentScore;
    public int FootballCharges => footballCharges;
    public float RemainingTime => remainingTime;
    public float ElapsedTime => elapsedTime;
    public float RoundDuration => roundDuration;
    public float FurthestProgress => furthestProgress;
    public float MinimumDifficultyMultiplier => minimumDifficultyMultiplier;
    public float MaximumDifficultyMultiplier => maximumDifficultyMultiplier;
    public float DifficultyMultiplier => Mathf.Lerp(minimumDifficultyMultiplier, maximumDifficultyMultiplier,
        Mathf.Clamp01(Mathf.Max(furthestProgress, elapsedTime / fullDifficultyAfterSeconds)));

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        ResetRound();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void OnValidate()
    {
        startingLives = Mathf.Max(1, startingLives);
        roundDuration = Mathf.Max(10f, roundDuration);
        minimumDifficultyMultiplier = Mathf.Max(0.5f, minimumDifficultyMultiplier);
        maximumDifficultyMultiplier = Mathf.Max(minimumDifficultyMultiplier, maximumDifficultyMultiplier);
        fullDifficultyAfterSeconds = Mathf.Max(15f, fullDifficultyAfterSeconds);
    }

    /// <summary>Applies the client-approved hard preset while keeping every value editable in Inspector.</summary>
    [ContextMenu("Apply Hard Gameplay Defaults")]
    public void ApplyHardGameplayDefaults()
    {
        startingLives = 3;
        roundDuration = 30f;
        minimumDifficultyMultiplier = 1.1f;
        maximumDifficultyMultiplier = 2.35f;
        fullDifficultyAfterSeconds = 75f;
        scorePerMove = 10;
        scorePenaltyPerLifeLost = 75;
        scorePerSecondRemaining = 5;
    }

    private void Update()
    {
        if (currentState != GameState.Playing || PauseManager.IsPaused) return;

        elapsedTime += Time.deltaTime;
        remainingTime = Mathf.Max(0f, remainingTime - Time.deltaTime);
        TimeChanged?.Invoke(remainingTime);
        if (remainingTime <= 0f) LoseLife(LifeLossReason.Timeout, 0);
    }

    public void ResetRound()
    {
        Time.timeScale = 1f;
        currentState = GameState.Playing;
        currentLives = startingLives;
        currentScore = 0;
        footballCharges = 0;
        remainingTime = roundDuration;
        elapsedTime = 0f;
        furthestProgress = 0f;
    }

    public void PlayerHitObstacle(int scorePenalty)
    {
        LoseLife(LifeLossReason.Other, scorePenalty > 0 ? scorePenalty : scorePenaltyPerLifeLost);
    }

    public void PlayerHitObstacle(LifeLossReason reason, int scorePenalty = -1)
    {
        LoseLife(reason, scorePenalty < 0 ? scorePenaltyPerLifeLost : scorePenalty);
    }

    public void PlayerHitSandwich(float timePenaltySeconds)
    {
        if (currentState != GameState.Playing) return;

        remainingTime = Mathf.Max(0f, remainingTime - Mathf.Max(0f, timePenaltySeconds));
        NegativeFeedback?.Invoke($"Sandwich! -{Mathf.CeilToInt(timePenaltySeconds)} seconds");
        TimeChanged?.Invoke(remainingTime);
        if (remainingTime <= 0f) LoseLife(LifeLossReason.Timeout, 0);
    }

    public void PlayerReachedGoal()
    {
        if (currentState != GameState.Playing) return;

        AddScore(Mathf.RoundToInt(remainingTime) * scorePerSecondRemaining);
        currentState = GameState.Won;
        PositiveFeedback?.Invoke("Touchdown! You made it to the couch!");
        StateChanged?.Invoke(currentState);
    }

    public void AddMoveScore() => AddScore(scorePerMove);

    public void AddBonusScore(int amount, string message = null)
    {
        AddScore(Mathf.Max(0, amount));
        if (!string.IsNullOrWhiteSpace(message)) PositiveFeedback?.Invoke(message);
    }

    public void AddFootball(int bonusScore = 100)
    {
        if (currentState != GameState.Playing) return;

        footballCharges = Mathf.Min(3, footballCharges + 1);
        AddScore(bonusScore);
        FootballChanged?.Invoke(footballCharges);
        PositiveFeedback?.Invoke("Football ready - press SPACE to distract an athlete!");
    }

    public bool SpendFootball()
    {
        if (currentState != GameState.Playing || footballCharges <= 0) return false;

        footballCharges--;
        FootballChanged?.Invoke(footballCharges);
        return true;
    }

    public void RegisterProgress(float normalizedProgress)
    {
        furthestProgress = Mathf.Max(furthestProgress, Mathf.Clamp01(normalizedProgress));
    }

    public void ReportPositive(string message)
    {
        if (currentState == GameState.Playing && !string.IsNullOrWhiteSpace(message)) PositiveFeedback?.Invoke(message);
    }

    public void ReportNegative(string message)
    {
        if (currentState == GameState.Playing && !string.IsNullOrWhiteSpace(message)) NegativeFeedback?.Invoke(message);
    }

    private void LoseLife(LifeLossReason reason, int scorePenalty)
    {
        if (currentState != GameState.Playing) return;

        currentLives = Mathf.Max(0, currentLives - 1);
        currentScore = Mathf.Max(0, currentScore - Mathf.Max(0, scorePenalty));
        remainingTime = roundDuration;
        LivesChanged?.Invoke(currentLives);
        ScoreChanged?.Invoke(currentScore);
        TimeChanged?.Invoke(remainingTime);

        if (currentLives <= 0)
        {
            currentState = GameState.Lost;
            NegativeFeedback?.Invoke("You've been sacked!");
        }
        else
        {
            NegativeFeedback?.Invoke(reason == LifeLossReason.Timeout
                ? "Time expired - life lost!"
                : "Life lost - back to the start!");
        }

        LifeLost?.Invoke(reason);
        if (currentState == GameState.Lost) StateChanged?.Invoke(currentState);
    }

    private void AddScore(int amount)
    {
        if (currentState != GameState.Playing || amount <= 0) return;
        currentScore += amount;
        ScoreChanged?.Invoke(currentScore);
    }
}
