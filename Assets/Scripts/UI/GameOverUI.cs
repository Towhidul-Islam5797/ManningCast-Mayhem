#region Summary
/// <summary>
/// GameOverUI watches GameManager's state and shows a shared panel with a
/// win or lose message once the game ends. The panel itself holds Restart
/// and Main Menu buttons, wired to MainMenuManager in the Inspector.
/// </summary>
#endregion

#region Phase 2 Sprint 8 - Game Over UI
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    #region UI References
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text messageText;
    #endregion

    #region Settings
    [SerializeField] private string winMessage = "You Win!";
    [SerializeField] private string loseMessage = "Game Over";
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Won)
        {
            ShowPanel(winMessage);
        }
        else if (GameManager.Instance.CurrentState == GameManager.GameState.Lost)
        {
            ShowPanel(loseMessage);
        }
    }
    #endregion

    #region Panel Display
    private void ShowPanel(string message)
    {
        gameOverPanel.SetActive(true);
        messageText.text = message;
    }
    #endregion
}
#endregion