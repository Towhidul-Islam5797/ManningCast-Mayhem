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
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        currentLives = startingLives;
        currentState = GameState.Playing;
    }
    #endregion

    #region Game State Changes
    public void PlayerHitObstacle()
    {
        if (currentState != GameState.Playing) return;

        currentLives--;
        Debug.Log("Life lost. Lives remaining: " + currentLives);

        if (currentLives <= 0)
        {
            currentState = GameState.Lost;
            Debug.Log("Game Over");
        }
    }

    public void PlayerReachedGoal()
    {
        if (currentState != GameState.Playing) return;

        currentState = GameState.Won;
        Debug.Log("You Win");
    }
    #endregion
}
#endregion