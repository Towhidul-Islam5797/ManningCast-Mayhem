using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Inspector-based HUD retained for scene compatibility.</summary>
public sealed class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Image[] heartIcons;

    private void Awake() => Instance = this;

    private void Update()
    {
        GameManager game = GameManager.Instance;
        if (game == null) return;

        if (scoreText != null) scoreText.text = $"SCORE  {game.CurrentScore:0000}";
        if (timeText != null)
        {
            int total = Mathf.CeilToInt(game.RemainingTime);
            timeText.text = $"TIME  {total / 60:00}:{total % 60:00}";
        }

        if (heartIcons == null) return;
        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (heartIcons[i] != null) heartIcons[i].enabled = i < game.CurrentLives;
        }
    }
}
