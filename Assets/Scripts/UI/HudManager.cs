#region Phase 1 Sprint 8 - HUDManager.cs
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class HUDManager : MonoBehaviour
//{
//    #region Singleton
//    public static HUDManager Instance { get; private set; }

//    private void Awake()
//    {
//        Instance = this;
//    }
//    #endregion

//    #region UI References
//    [SerializeField] private TMP_Text scoreText;
//    [SerializeField] private Image[] heartIcons;
//    #endregion

//    #region Unity Lifecycle
//    private void Update()
//    {
//        UpdateScore();
//        UpdateLives();
//    }
//    #endregion

//    #region HUD Updates
//    private void UpdateScore()
//    {
//        scoreText.text = "Score: " + GameManager.Instance.CurrentScore;
//    }

//    private void UpdateLives()
//    {
//        int currentLives = GameManager.Instance.CurrentLives;

//        for (int i = 0; i < heartIcons.Length; i++)
//        {
//            heartIcons[i].enabled = i < currentLives;
//        }
//    }
//    #endregion
//}
#endregion

#region Phase 3 Sprint 2 - HUDManager.cs
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    #region Singleton
    public static HUDManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region UI References
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Image[] heartIcons;
    #endregion

    #region Unity Lifecycle
    private void Update()
    {
        UpdateScore();
        UpdateTime();
        UpdateLives();
    }
    #endregion

    #region HUD Updates
    private void UpdateScore()
    {
        scoreText.text = "Score: " + GameManager.Instance.CurrentScore;
    }

    private void UpdateTime()
    {
        float elapsed = GameManager.Instance.ElapsedTime;
        int minutes = Mathf.FloorToInt(elapsed / 60f);
        int seconds = Mathf.FloorToInt(elapsed % 60f);
        timeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateLives()
    {
        int currentLives = GameManager.Instance.CurrentLives;

        for (int i = 0; i < heartIcons.Length; i++)
        {
            heartIcons[i].enabled = i < currentLives;
        }
    }
    #endregion
}
#endregion