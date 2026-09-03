using System.Collections;
using UnityEngine;

/// <summary>The non-selected brother sits at the goal and reacts to gameplay.</summary>
public sealed class ManningCouchSpectator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite[,] reactions;
    private Coroutine resetRoutine;

    public void Initialize(CharacterSelection.Character character)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 112;
        Texture2D texture = ManningAssetLibrary.LoadTexture(character == CharacterSelection.Character.Peyton
            ? ManningAssetLibrary.PeytonSheet
            : ManningAssetLibrary.EliSheet);
        reactions = ManningAssetLibrary.SliceCharacter(texture, 1.35f);
        SetReaction(0);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PositiveFeedback += OnPositive;
            GameManager.Instance.NegativeFeedback += OnNegative;
            GameManager.Instance.StateChanged += OnStateChanged;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.PositiveFeedback -= OnPositive;
        GameManager.Instance.NegativeFeedback -= OnNegative;
        GameManager.Instance.StateChanged -= OnStateChanged;
    }

    private void OnPositive(string message) => React(1);
    private void OnNegative(string message) => React(2);

    private void OnStateChanged(GameManager.GameState state)
    {
        SetReaction(state == GameManager.GameState.Won ? 1 : 2);
    }

    private void React(int column)
    {
        SetReaction(column);
        if (resetRoutine != null) StopCoroutine(resetRoutine);
        resetRoutine = StartCoroutine(ResetReaction());
    }

    private IEnumerator ResetReaction()
    {
        yield return new WaitForSecondsRealtime(1.2f);
        if (GameManager.Instance != null && !GameManager.Instance.IsGameOver) SetReaction(0);
    }

    private void SetReaction(int column)
    {
        if (spriteRenderer != null && reactions != null)
            spriteRenderer.sprite = reactions[2, Mathf.Clamp(column, 0, 3)];
    }
}
