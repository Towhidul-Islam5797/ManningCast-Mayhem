using UnityEngine;

/// <summary>Drives the approved front/back character poses without depending on old animator sheets.</summary>
public sealed class ManningCharacterSpriteAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite[,] frames;
    private Vector2Int direction = Vector2Int.up;
    private bool moving;
    private float nextFrameTime;
    private int frameIndex;

    public void Initialize(CharacterSelection.Character character)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Texture2D texture = ManningAssetLibrary.LoadTexture(character == CharacterSelection.Character.Peyton
            ? ManningAssetLibrary.PeytonSheet
            : ManningAssetLibrary.EliSheet);
        frames = ManningAssetLibrary.SliceCharacter(texture);
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 120;
            spriteRenderer.color = Color.white;
            spriteRenderer.sprite = frames?[1, 0];
        }
    }

    private void Update()
    {
        if (!moving || frames == null || Time.time < nextFrameTime) return;
        frameIndex = (frameIndex + 1) % 4;
        nextFrameTime = Time.time + 0.09f;
        ApplyFrame();
    }

    public void SetDirection(Vector2Int newDirection)
    {
        direction = newDirection;
        ApplyFrame();
    }

    public void SetMoving(bool value)
    {
        moving = value;
        if (!value)
        {
            frameIndex = 0;
            ApplyFrame();
        }
    }

    private void ApplyFrame()
    {
        if (frames == null || spriteRenderer == null) return;
        int row = direction.y > 0 ? 1 : 0;
        spriteRenderer.flipX = direction.x < 0;
        spriteRenderer.sprite = frames[row, moving ? frameIndex : 0];
    }
}
