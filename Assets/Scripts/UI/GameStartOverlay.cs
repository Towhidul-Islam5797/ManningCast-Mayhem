#region Summary
/// <summary>
/// GameStartOverlay shows the rules panel the moment GameScene loads and
/// pauses the game until the player closes it and clicks Start Game.
/// </summary>
#endregion

#region Phase 3 Sprint 11 - Game Start Overlay
using UnityEngine;

public class GameStartOverlay : MonoBehaviour
{
    public static bool IsShowing { get; private set; }

    #region References
    [SerializeField] private GameObject rulesOverlayPanel;
    [SerializeField] private PauseManager pauseManager;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        IsShowing = true;
        rulesOverlayPanel.SetActive(true);
        pauseManager.PauseGame();
    }
    #endregion

    #region Button Actions
    public void StartGame()
    {
        IsShowing = false;
        rulesOverlayPanel.SetActive(false);
        pauseManager.ResumeGame();
    }
    #endregion
}
#endregion