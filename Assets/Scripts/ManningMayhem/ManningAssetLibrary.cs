using System.Collections.Generic;
using UnityEngine;

/// <summary>Central Resource paths and runtime sprite slicing for the client art bundle.</summary>
public static class ManningAssetLibrary
{
    public const string TitleScreen = "ManningMayhem/Screens/Title";
    public const string RulesScreen = "ManningMayhem/Screens/Rules";
    public const string CharacterSelectScreen = "ManningMayhem/Screens/CharacterSelect";
    public const string WinScreen = "ManningMayhem/Screens/Win";
    public const string LoseScreen = "ManningMayhem/Screens/Lose";
    public const string Logo = "ManningMayhem/Screens/Logo";
    public const string PeytonSheet = "ManningMayhem/Characters/Peyton";
    public const string EliSheet = "ManningMayhem/Characters/Eli";
    public const string DisplayFont = "ManningMayhem/Fonts/Anton";

    private static readonly Dictionary<string, Sprite> WorldSprites = new Dictionary<string, Sprite>();

    public static Texture2D LoadTexture(string path) => Resources.Load<Texture2D>(path);
    public static AudioClip LoadAudio(string name) => Resources.Load<AudioClip>($"ManningMayhem/Audio/{name}");

    public static Sprite LoadWorldSprite(string name, float maxWorldSize = 1.35f)
    {
        string key = $"{name}:{maxWorldSize:0.00}";
        if (WorldSprites.TryGetValue(key, out Sprite cached) && cached != null) return cached;

        // The Omaha exports contain substantial transparent padding. Using the complete texture made
        // footballs/remotes tiny while differently padded sprites appeared oversized. Select the largest
        // imported (alpha-trimmed) sprite rectangle and normalize its visible bounds instead.
        Sprite[] importedSprites = Resources.LoadAll<Sprite>($"ManningMayhem/Elements/{name}");
        Sprite source = null;
        float largestArea = 0f;
        foreach (Sprite candidate in importedSprites)
        {
            float area = candidate.rect.width * candidate.rect.height;
            if (area <= largestArea) continue;
            source = candidate;
            largestArea = area;
        }

        Texture2D texture = source != null ? source.texture : LoadTexture($"ManningMayhem/Elements/{name}");
        if (texture == null) return null;

        Rect visibleRect = source != null ? source.rect : new Rect(0f, 0f, texture.width, texture.height);
        float pixelsPerUnit = Mathf.Max(visibleRect.width, visibleRect.height) / Mathf.Max(0.1f, maxWorldSize);
        Sprite sprite = Sprite.Create(texture, visibleRect, new Vector2(0.5f, 0.5f), pixelsPerUnit, 0, SpriteMeshType.FullRect);
        sprite.name = name;
        WorldSprites[key] = sprite;
        return sprite;
    }

    public static Sprite[,] SliceCharacter(Texture2D sheet, float worldHeight = 1.65f)
    {
        if (sheet == null) return null;

        const int columns = 4;
        const int rows = 3;
        float cellWidth = sheet.width / (float)columns;
        float cellHeight = sheet.height / (float)rows;
        float pixelsPerUnit = cellHeight / worldHeight;
        Sprite[,] result = new Sprite[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Rect rect = new Rect(column * cellWidth, (rows - row - 1) * cellHeight, cellWidth, cellHeight);
                Sprite sprite = Sprite.Create(sheet, rect, new Vector2(0.5f, 0.42f), pixelsPerUnit, 0, SpriteMeshType.FullRect);
                sprite.name = $"{sheet.name}_{row}_{column}";
                result[row, column] = sprite;
            }
        }

        return result;
    }
}
