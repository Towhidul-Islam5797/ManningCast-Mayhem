using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>Single pause authority shared by keyboard and the on-screen control.</summary>
public sealed class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    public static bool IsPaused { get; private set; }
    public static event Action<bool> PauseChanged;

    [SerializeField] private GameObject settingsPanel;

    private void Awake()
    {
        Instance = this;
        SetPaused(false);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SetPaused(false);
            Instance = null;
        }
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) TogglePause();
    }

    public void TogglePause() => SetPaused(!IsPaused);
    public void PauseGame() => SetPaused(true);
    public void ResumeGame() => SetPaused(false);

    private void SetPaused(bool paused)
    {
        IsPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
        if (settingsPanel != null) settingsPanel.SetActive(paused);
        PauseChanged?.Invoke(paused);
    }
}
