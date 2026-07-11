#region Summary
/// PauseManager.cs
/// This script manages the pause state of the game. It allows the player to pause and unpause the game using the Escape key. When the game is paused, 
///     the time scale is set to 0, effectively freezing all gameplay, and a "Paused" text is displayed on the screen. When unpaused, the time scale is restored to 1, resuming normal gameplay.
/// Note: This script assumes that there is a GameManager class with an IsGameOver property to check if the game is over, and a UI element (pausedText) to display when the game is paused.
/// 
#endregion

#region Phase 1 Sprint 5 - Pause Functionality
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    #region Pause State
    public static bool IsPaused { get; private set; }
    #endregion

    #region UI Reference
    [SerializeField] private GameObject pausedText;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        IsPaused = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }
    #endregion

    #region Pause Logic
    private void TogglePause()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f : 1f;
        pausedText.SetActive(IsPaused);
    }
    #endregion
}
#endregion