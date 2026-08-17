#region Summary
/// <summary>
/// MainMenuManager handles the Main Menu's button actions: starting the game
/// flow by loading Character Select, and quitting the application.
/// </summary>
#endregion

#region Phase 2 Sprint 7 - Main Menu
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class MainMenuManager : MonoBehaviour
//{
//    #region Scene Settings
//    [SerializeField] private string mainMenuSceneName = "MainMenu";
//    [SerializeField] private string characterSelectSceneName = "CharacterSelectScene";
//    [SerializeField] private string LeaderboardScene = "LeaderboardScene";
//    [SerializeField] private string CreditsScene = "CreditsScene";
//    #endregion

//    #region Panel References
//    [SerializeField] private GameObject settingsPanel;
//    #endregion

//    #region Unity Lifecycle
//    private void Awake()
//    {
//        settingsPanel.SetActive(false);
//    }
//    #endregion

//    #region Button Actions
//    public void PlayGame()
//    {
//        SceneManager.LoadScene(characterSelectSceneName);
//    }

//    public void SettingsPanel()
//    {
//        settingsPanel.SetActive(true);
//    }

//    public void Credits()
//    {
//        SceneManager.LoadScene(CreditsScene);
//    }

//    public void Leaderboard()
//    {
//        SceneManager.LoadScene(LeaderboardScene);
//    }

//    public void QuitGame()
//    {
//        Application.Quit();
//    }

//    public void BackToMainMenu()
//    {
//        SceneManager.LoadScene(mainMenuSceneName);
//    }
//    #endregion
//}
//#endregion

#endregion

#region Phase 2 Sprint 8 - Main Menu + Restart Game
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    #region Scene Settings
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string characterSelectSceneName = "CharacterSelectScene";
    [SerializeField] private string LeaderboardScene = "LeaderboardScene";
    [SerializeField] private string CreditsScene = "CreditsScene";
    #endregion

    #region Panel References
    [SerializeField] private GameObject settingsPanel;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
    #endregion

    #region Button Actions
    public void PlayGame()
    {
        SceneManager.LoadScene(characterSelectSceneName);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SettingsPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
    #endregion
}
#endregion