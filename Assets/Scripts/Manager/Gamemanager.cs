#region Summary
/// <summary>
/// GameManager class is responsible for managing the game state, including player lives and game over conditions.
/// </summary>
#endregion

#region Phase 1 Sprint 2 - GameManager Implementation
//using UnityEngine;

//public class GameManager : MonoBehaviour
//{
//    #region Singleton
//    public static GameManager Instance { get; private set; }

//    private void Awake()
//    {
//        Instance = this;
//    }
//    #endregion

//    #region Lives Settings
//    [SerializeField] private int startingLives = 3;
//    private int currentLives;
//    private bool isGameOver;
//    #endregion

//    #region Public Properties
//    public bool IsGameOver => isGameOver;
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        currentLives = startingLives;
//    }
//    #endregion

//    #region Game State
//    public void PlayerHitObstacle()
//    {
//        if (isGameOver) return;

//        currentLives--;
//        Debug.Log("Life lost. Lives remaining: " + currentLives);

//        if (currentLives <= 0)
//        {
//            isGameOver = true;
//            Debug.Log("Game Over");
//        }
//    }
//    #endregion
//}
#endregion

#region Phase 1 Sprint 3 - GameManager Implementation with Game State
//using UnityEngine;

//public class GameManager : MonoBehaviour
//{
//    #region Singleton
//    public static GameManager Instance { get; private set; }

//    private void Awake()
//    {
//        Instance = this;
//    }
//    #endregion

//    #region Game State
//    public enum GameState
//    {
//        Playing,
//        Won,
//        Lost
//    }

//    private GameState currentState;
//    public GameState CurrentState => currentState;
//    public bool IsGameOver => currentState != GameState.Playing;
//    #endregion

//    #region Lives Settings
//    [SerializeField] private int startingLives = 3;
//    private int currentLives;
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        currentLives = startingLives;
//        currentState = GameState.Playing;
//    }
//    #endregion

//    #region Game State Changes
//    public void PlayerHitObstacle()
//    {
//        if (currentState != GameState.Playing) return;

//        currentLives--;
//        Debug.Log("Life lost. Lives remaining: " + currentLives);

//        if (currentLives <= 0)
//        {
//            currentState = GameState.Lost;
//            Debug.Log("Game Over");
//        }
//    }

//    public void PlayerReachedGoal()
//    {
//        if (currentState != GameState.Playing) return;

//        currentState = GameState.Won;
//        Debug.Log("You Win");
//    }
//    #endregion
//}
#endregion

#region Phase 1 Sprint 5 - GameManager Implementation with Score
//using UnityEngine;

//public class GameManager : MonoBehaviour
//{
//    #region Singleton
//    public static GameManager Instance { get; private set; }

//    private void Awake()
//    {
//        Instance = this;
//    }
//    #endregion

//    #region Game State
//    public enum GameState
//    {
//        Playing,
//        Won,
//        Lost
//    }

//    private GameState currentState;
//    public GameState CurrentState => currentState;
//    public bool IsGameOver => currentState != GameState.Playing;
//    #endregion

//    #region Lives Settings
//    [SerializeField] private int startingLives = 3;
//    private int currentLives;
//    #endregion

//    #region Score Settings
//    [SerializeField] private int scorePerMove = 10;
//    [SerializeField] private int scorePenaltyPerHit = 20;
//    private int currentScore;
//    public int CurrentScore => currentScore;
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        currentLives = startingLives;
//        currentScore = 0;
//        currentState = GameState.Playing;
//    }
//    #endregion

//    #region Game State Changes
//    public void PlayerHitObstacle()
//    {
//        if (currentState != GameState.Playing) return;

//        currentLives--;
//        currentScore = Mathf.Max(0, currentScore - scorePenaltyPerHit);
//        Debug.Log("Life lost. Lives remaining: " + currentLives);

//        if (currentLives <= 0)
//        {
//            currentState = GameState.Lost;
//            Debug.Log("Game Over");
//        }
//    }

//    public void PlayerReachedGoal()
//    {
//        if (currentState != GameState.Playing) return;

//        currentState = GameState.Won;
//        Debug.Log("You Win");
//    }

//    public void AddMoveScore()
//    {
//        if (currentState != GameState.Playing) return;

//        currentScore += scorePerMove;
//    }
//    #endregion
//}
//using UnityEngine;

//public class GameManager : MonoBehaviour
//{
//    #region Singleton
//    public static GameManager Instance { get; private set; }

//    private void Awake()
//    {
//        Instance = this;
//    }
//    #endregion

//    #region Game State
//    public enum GameState
//    {
//        Playing,
//        Won,
//        Lost
//    }

//    private GameState currentState;
//    public GameState CurrentState => currentState;
//    public bool IsGameOver => currentState != GameState.Playing;
//    #endregion

//    #region Lives Settings
//    [SerializeField] private int startingLives = 3;
//    private int currentLives;
//    #endregion

//    #region Score Settings
//    [SerializeField] private int scorePerMove = 10;
//    [SerializeField] private int scorePenaltyPerHit = 20;
//    private int currentScore;
//    public int CurrentScore => currentScore;
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        currentLives = startingLives;
//        currentScore = 0;
//        currentState = GameState.Playing;
//    }
//    #endregion

//    #region Game State Changes
//    public void PlayerHitObstacle()
//    {
//        if (currentState != GameState.Playing) return;

//        currentLives--;
//        currentScore = Mathf.Max(0, currentScore - scorePenaltyPerHit);
//        Debug.Log("Life lost. Lives remaining: " + currentLives);

//        if (currentLives <= 0)
//        {
//            currentState = GameState.Lost;
//            Debug.Log("Game Over");
//        }
//    }

//    public void PlayerReachedGoal()
//    {
//        if (currentState != GameState.Playing) return;

//        currentState = GameState.Won;
//        Debug.Log("You Win");
//    }

//    public void AddMoveScore()
//    {
//        if (currentState != GameState.Playing) return;

//        currentScore += scorePerMove;
//    }
//    #endregion
//}
#endregion

#region Phase 1 Sprint 8 - GameManager Implementation with Score and Lives
//using UnityEngine;

//public class GameManager : MonoBehaviour
//{
//    #region Singleton
//    public static GameManager Instance { get; private set; }

//    private void Awake()
//    {
//        Instance = this;
//    }
//    #endregion

//    #region Game State
//    public enum GameState
//    {
//        Playing,
//        Won,
//        Lost
//    }

//    private GameState currentState;
//    public GameState CurrentState => currentState;
//    public bool IsGameOver => currentState != GameState.Playing;
//    #endregion

//    #region Lives Settings
//    [SerializeField] private int startingLives = 3;
//    private int currentLives;
//    public int CurrentLives => currentLives;
//    #endregion

//    #region Score Settings
//    [SerializeField] private int scorePerMove = 10;
//    [SerializeField] private int scorePenaltyPerHit = 20;
//    private int currentScore;
//    public int CurrentScore => currentScore;
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        currentLives = startingLives;
//        currentScore = 0;
//        currentState = GameState.Playing;
//    }
//    #endregion

//    #region Game State Changes
//    public void PlayerHitObstacle()
//    {
//        if (currentState != GameState.Playing) return;

//        currentLives--;
//        currentScore = Mathf.Max(0, currentScore - scorePenaltyPerHit);
//        Debug.Log("Life lost. Lives remaining: " + currentLives);

//        if (currentLives <= 0)
//        {
//            currentState = GameState.Lost;
//            Debug.Log("Game Over");
//        }
//    }

//    public void PlayerReachedGoal()
//    {
//        if (currentState != GameState.Playing) return;

//        currentState = GameState.Won;
//        Debug.Log("You Win");
//    }

//    public void AddMoveScore()
//    {
//        if (currentState != GameState.Playing) return;

//        currentScore += scorePerMove;
//    }
//    #endregion
//}
#endregion

#region Phase 2 Sprint 4 - GameManager Implementation with Score and Lives
//using UnityEngine;

//public class GameManager : MonoBehaviour
//{
//    #region Singleton
//    public static GameManager Instance { get; private set; }

//    private void Awake()
//    {
//        Instance = this;
//    }
//    #endregion

//    #region Game State
//    public enum GameState
//    {
//        Playing,
//        Won,
//        Lost
//    }

//    private GameState currentState;
//    public GameState CurrentState => currentState;
//    public bool IsGameOver => currentState != GameState.Playing;
//    #endregion

//    #region Lives Settings
//    [SerializeField] private int startingLives = 3;
//    private int currentLives;
//    public int CurrentLives => currentLives;
//    #endregion

//    #region Score Settings
//    [SerializeField] private int scorePerMove = 10;
//    private int currentScore;
//    public int CurrentScore => currentScore;
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        currentLives = startingLives;
//        currentScore = 0;
//        currentState = GameState.Playing;
//    }
//    #endregion

//    #region Game State Changes
//    public void PlayerHitObstacle(int scorePenalty)
//    {
//        if (currentState != GameState.Playing) return;

//        currentLives--;
//        currentScore = Mathf.Max(0, currentScore - scorePenalty);
//        Debug.Log("Life lost. Lives remaining: " + currentLives);

//        if (currentLives <= 0)
//        {
//            currentState = GameState.Lost;
//            Debug.Log("Game Over");
//        }
//    }

//    public void PlayerHitSandwich(int scorePenalty)
//    {
//        if (currentState != GameState.Playing) return;

//        currentScore = Mathf.Max(0, currentScore - scorePenalty);
//        Debug.Log("Sandwich hit. Score penalty: " + scorePenalty);
//    }

//    public void PlayerReachedGoal()
//    {
//        if (currentState != GameState.Playing) return;

//        currentState = GameState.Won;
//        Debug.Log("You Win");
//    }

//    public void AddMoveScore()
//    {
//        if (currentState != GameState.Playing) return;

//        currentScore += scorePerMove;
//    }
//    #endregion
//}
#endregion


#region Phase 3 Sprint 2 - GameManager Implementation with Score, Lives, and Elapsed Time
//using UnityEngine;

//public class GameManager : MonoBehaviour
//{
//    #region Singleton
//    public static GameManager Instance { get; private set; }

//    private void Awake()
//    {
//        Instance = this;
//    }
//    #endregion

//    #region Game State
//    public enum GameState
//    {
//        Playing,
//        Won,
//        Lost
//    }

//    private GameState currentState;
//    public GameState CurrentState => currentState;
//    public bool IsGameOver => currentState != GameState.Playing;
//    #endregion

//    #region Lives Settings
//    [SerializeField] private int startingLives = 3;
//    private int currentLives;
//    public int CurrentLives => currentLives;
//    #endregion

//    #region Score Settings
//    [SerializeField] private int scorePerMove = 10;
//    private int currentScore;
//    public int CurrentScore => currentScore;
//    #endregion

//    #region Time Settings
//    [SerializeField] private float parTime = 60f;
//    [SerializeField] private int scorePenaltyPerSecondOverPar = 1;
//    [SerializeField] private int scorePenaltyPerLifeLost = 50;
//    private float elapsedTime;
//    public float ElapsedTime => elapsedTime;
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        currentLives = startingLives;
//        currentScore = 0;
//        elapsedTime = 0f;
//        currentState = GameState.Playing;
//    }

//    private void Update()
//    {
//        if (currentState == GameState.Playing)
//        {
//            elapsedTime += Time.deltaTime;
//        }
//    }
//    #endregion

//    #region Game State Changes
//    public void PlayerHitObstacle(int scorePenalty)
//    {
//        if (currentState != GameState.Playing) return;

//        currentLives--;
//        currentScore = Mathf.Max(0, currentScore - scorePenalty);
//        Debug.Log("Life lost. Lives remaining: " + currentLives);

//        if (currentLives <= 0)
//        {
//            currentState = GameState.Lost;
//            Debug.Log("Game Over");
//        }
//    }

//    public void PlayerHitSandwich(int scorePenalty)
//    {
//        if (currentState != GameState.Playing) return;

//        currentScore = Mathf.Max(0, currentScore - scorePenalty);
//        Debug.Log("Sandwich hit. Score penalty: " + scorePenalty);
//    }

//    public void PlayerReachedGoal()
//    {
//        if (currentState != GameState.Playing) return;

//        int triesUsed = startingLives - currentLives;
//        float secondsOverPar = Mathf.Max(0f, elapsedTime - parTime);

//        int triesPenalty = triesUsed * scorePenaltyPerLifeLost;
//        int timePenalty = Mathf.RoundToInt(secondsOverPar * scorePenaltyPerSecondOverPar);

//        currentScore = Mathf.Max(0, currentScore - triesPenalty - timePenalty);

//        currentState = GameState.Won;
//        Debug.Log("You Win. Tries used: " + triesUsed + ", Seconds over par: " + secondsOverPar);
//    }

//    public void AddMoveScore()
//    {
//        if (currentState != GameState.Playing) return;

//        currentScore += scorePerMove;
//    }
//    #endregion
//}
#endregion

#region Phase 3 Sprint 3 - GameManager Implementation with Score and Lives, Elapsed Time, and Time Penalty for Sandwich
//using UnityEngine;

//public class GameManager : MonoBehaviour
//{
//    #region Singleton
//    public static GameManager Instance { get; private set; }

//    private void Awake()
//    {
//        Instance = this;
//    }
//    #endregion

//    #region Game State
//    public enum GameState
//    {
//        Playing,
//        Won,
//        Lost
//    }

//    private GameState currentState;
//    public GameState CurrentState => currentState;
//    public bool IsGameOver => currentState != GameState.Playing;
//    #endregion

//    #region Lives Settings
//    [SerializeField] private int startingLives = 3;
//    private int currentLives;
//    public int CurrentLives => currentLives;
//    #endregion

//    #region Score Settings
//    [SerializeField] private int scorePerMove = 10;
//    private int currentScore;
//    public int CurrentScore => currentScore;
//    #endregion

//    #region Time Settings
//    [SerializeField] private float parTime = 60f;
//    [SerializeField] private int scorePenaltyPerSecondOverPar = 1;
//    [SerializeField] private int scorePenaltyPerLifeLost = 50;
//    private float elapsedTime;
//    public float ElapsedTime => elapsedTime;
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        currentLives = startingLives;
//        currentScore = 0;
//        elapsedTime = 0f;
//        currentState = GameState.Playing;
//    }

//    private void Update()
//    {
//        if (currentState == GameState.Playing)
//        {
//            elapsedTime += Time.deltaTime;
//        }
//    }
//    #endregion

//    #region Game State Changes
//    public void PlayerHitObstacle(int scorePenalty)
//    {
//        if (currentState != GameState.Playing) return;

//        currentLives--;
//        currentScore = Mathf.Max(0, currentScore - scorePenalty);
//        Debug.Log("Life lost. Lives remaining: " + currentLives);

//        if (currentLives <= 0)
//        {
//            currentState = GameState.Lost;
//            Debug.Log("Game Over");
//        }
//    }

//    public void PlayerHitSandwich(float timePenaltySeconds)
//    {
//        if (currentState != GameState.Playing) return;

//        elapsedTime += timePenaltySeconds;
//        Debug.Log("Sandwich hit. Time penalty: " + timePenaltySeconds + "s");
//    }

//    public void PlayerReachedGoal()
//    {
//        if (currentState != GameState.Playing) return;

//        int triesUsed = startingLives - currentLives;
//        float secondsOverPar = Mathf.Max(0f, elapsedTime - parTime);

//        int triesPenalty = triesUsed * scorePenaltyPerLifeLost;
//        int timePenalty = Mathf.RoundToInt(secondsOverPar * scorePenaltyPerSecondOverPar);

//        currentScore = Mathf.Max(0, currentScore - triesPenalty - timePenalty);

//        currentState = GameState.Won;
//        Debug.Log("You Win. Tries used: " + triesUsed + ", Seconds over par: " + secondsOverPar);
//    }

//    public void AddMoveScore()
//    {
//        if (currentState != GameState.Playing) return;

//        currentScore += scorePerMove;
//    }
//    #endregion
//}
#endregion

#region Phase 3 Sprint 3 - GameManager Implementation with Score and Lives, Elapsed Time, and Time Penalty for Sandwich
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region Game State
    public enum GameState
    {
        Playing,
        Won,
        Lost
    }

    private GameState currentState;
    public GameState CurrentState => currentState;
    public bool IsGameOver => currentState != GameState.Playing;
    #endregion

    #region Lives Settings
    [SerializeField] private int startingLives = 3;
    private int currentLives;
    public int CurrentLives => currentLives;
    #endregion

    #region Score Settings
    [SerializeField] private int scorePerMove = 10;
    private int currentScore;
    public int CurrentScore => currentScore;
    #endregion

    #region Time Settings
    [SerializeField] private float parTime = 60f;
    [SerializeField] private int scorePenaltyPerSecondOverPar = 1;
    [SerializeField] private int scorePenaltyPerLifeLost = 50;
    private float elapsedTime;
    public float ElapsedTime => elapsedTime;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        currentLives = startingLives;
        currentScore = 0;
        elapsedTime = 0f;
        footballCharges = 0;
        currentState = GameState.Playing;
    }

    private void Update()
    {
        if (currentState == GameState.Playing)
        {
            elapsedTime += Time.deltaTime;
        }
    }
    #endregion

    #region Game State Changes
    public void PlayerHitObstacle(int scorePenalty)
    {
        if (currentState != GameState.Playing) return;

        currentLives--;
        currentScore = Mathf.Max(0, currentScore - scorePenalty);
        Debug.Log("Life lost. Lives remaining: " + currentLives);

        if (currentLives <= 0)
        {
            currentState = GameState.Lost;
            Debug.Log("Game Over");
        }
    }

    public void PlayerHitSandwich(float timePenaltySeconds)
    {
        if (currentState != GameState.Playing) return;

        elapsedTime += timePenaltySeconds;
        Debug.Log("Sandwich hit. Time penalty: " + timePenaltySeconds + "s");
    }

    public void PlayerReachedGoal()
    {
        if (currentState != GameState.Playing) return;

        int triesUsed = startingLives - currentLives;
        float secondsOverPar = Mathf.Max(0f, elapsedTime - parTime);

        int triesPenalty = triesUsed * scorePenaltyPerLifeLost;
        int timePenalty = Mathf.RoundToInt(secondsOverPar * scorePenaltyPerSecondOverPar);

        currentScore = Mathf.Max(0, currentScore - triesPenalty - timePenalty);

        currentState = GameState.Won;
        Debug.Log("You Win. Tries used: " + triesUsed + ", Seconds over par: " + secondsOverPar);
    }

    public void AddMoveScore()
    {
        if (currentState != GameState.Playing) return;

        currentScore += scorePerMove;
    }
    #endregion

    #region Football Charges
    [SerializeField] private int maxFootballCharges = 3;
    private int footballCharges;
    public int FootballCharges => footballCharges;

    public void CollectFootball()
    {
        footballCharges = Mathf.Min(footballCharges + 1, maxFootballCharges);
    }

    public bool SpendFootball()
    {
        if (footballCharges <= 0) return false;

        footballCharges--;
        return true;
    }
    #endregion
}
#endregion