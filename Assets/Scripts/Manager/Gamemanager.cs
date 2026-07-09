#region Summary
/// <summary>
/// GameManager class is responsible for managing the game state, including player lives and game over conditions.
/// </summary>
#endregion

#region Phase 1 Sprint 2 - GameManager Implementation
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

    #region Lives Settings
    [SerializeField] private int startingLives = 3;
    private int currentLives;
    private bool isGameOver;
    #endregion

    #region Public Properties
    public bool IsGameOver => isGameOver;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        currentLives = startingLives;
    }
    #endregion

    #region Game State
    public void PlayerHitObstacle()
    {
        if (isGameOver) return;

        currentLives--;
        Debug.Log("Life lost. Lives remaining: " + currentLives);

        if (currentLives <= 0)
        {
            isGameOver = true;
            Debug.Log("Game Over");
        }
    }
    #endregion
}
#endregion