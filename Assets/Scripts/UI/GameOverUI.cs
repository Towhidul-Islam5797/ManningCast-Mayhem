#region Summary
/// <summary>
/// GameOverUI watches GameManager's state and shows a shared panel with a
/// win or lose message once the game ends. The panel itself holds Restart
/// and Main Menu buttons, wired to MainMenuManager in the Inspector.
/// </summary>
#endregion

#region Phase 2 Sprint 8 - Game Over UI
//using TMPro;
//using UnityEngine;

//public class GameOverUI : MonoBehaviour
//{
//    #region UI References
//    [SerializeField] private GameObject gameOverPanel;
//    [SerializeField] private TMP_Text messageText;
//    #endregion

//    #region Settings
//    [SerializeField] private string winMessage = "You Win!";
//    [SerializeField] private string loseMessage = "Game Over";
//    #endregion

//    #region Unity Lifecycle
//    private void Awake()
//    {
//        gameOverPanel.SetActive(false);
//    }

//    private void Update()
//    {
//        if (GameManager.Instance.CurrentState == GameManager.GameState.Won)
//        {
//            ShowPanel(winMessage);
//        }
//        else if (GameManager.Instance.CurrentState == GameManager.GameState.Lost)
//        {
//            ShowPanel(loseMessage);
//        }
//    }
//    #endregion

//    #region Panel Display
//    private void ShowPanel(string message)
//    {
//        gameOverPanel.SetActive(true);
//        messageText.text = message;
//    }
//    #endregion
//}
#endregion

#region Phase 3 Sprint 8 - Game Over UI with Win Lose Art
//using UnityEngine;
//using UnityEngine.UI;

//public class GameOverUI : MonoBehaviour
//{
//    #region UI References
//    [SerializeField] private GameObject gameOverPanel;
//    [SerializeField] private Image resultImage;
//    #endregion

//    #region Result Art
//    [SerializeField] private Sprite winSprite;
//    [SerializeField] private Sprite loseSprite;
//    #endregion

//    #region Private State
//    private bool hasShownPanel;
//    #endregion

//    #region Unity Lifecycle
//    private void Awake()
//    {
//        gameOverPanel.SetActive(false);
//    }

//    private void Update()
//    {
//        if (hasShownPanel) return;

//        if (GameManager.Instance.CurrentState == GameManager.GameState.Won)
//        {
//            ShowPanel(winSprite);
//        }
//        else if (GameManager.Instance.CurrentState == GameManager.GameState.Lost)
//        {
//            ShowPanel(loseSprite);
//        }
//    }
//    #endregion

//    #region Panel Display
//    private void ShowPanel(Sprite artwork)
//    {
//        hasShownPanel = true;
//        resultImage.sprite = artwork;
//        gameOverPanel.SetActive(true);
//    }
//    #endregion
//}
#endregion

#region Phase 3 Sprint 8 - Game Over UI with Win Lose Art
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    #region UI References
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    #endregion

    #region Private State
    private bool hasShownPanel;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        gameOverPanel.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    private void Update()
    {
        if (hasShownPanel) return;

        if (GameManager.Instance.CurrentState == GameManager.GameState.Won)
        {
            ShowPanel(winScreen);
        }
        else if (GameManager.Instance.CurrentState == GameManager.GameState.Lost)
        {
            ShowPanel(loseScreen);
        }
    }
    #endregion

    #region Panel Display
    private void ShowPanel(GameObject screen)
    {
        hasShownPanel = true;
        screen.SetActive(true);
        gameOverPanel.SetActive(true);
    }
    #endregion
}
#endregion