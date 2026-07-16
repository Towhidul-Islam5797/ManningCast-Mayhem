#region Phase 1 Sprint 8 - HUDManager.cs
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
    [SerializeField] private Image[] heartIcons;
    #endregion

    #region Unity Lifecycle
    private void Update()
    {
        UpdateScore();
        UpdateLives();
    }
    #endregion

    #region HUD Updates
    private void UpdateScore()
    {
        scoreText.text = "Score: " + GameManager.Instance.CurrentScore;
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