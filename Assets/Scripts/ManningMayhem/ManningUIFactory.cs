using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

/// <summary>
/// Shared UI styling helpers. The production screens are authored into the scene hierarchy;
/// these helpers remain available for safe runtime fallbacks and dynamically populated content.
/// </summary>
public static class ManningUIFactory
{
    public static readonly Color Navy = new Color32(4, 22, 55, 245);
    public static readonly Color Blue = new Color32(15, 91, 171, 255);
    public static readonly Color Orange = new Color32(242, 113, 33, 255);
    public static readonly Color Gold = new Color32(255, 195, 32, 255);
    public static readonly Color Cream = new Color32(247, 244, 232, 255);

    /// <summary>Matches the neutral grey menu plates painted into the supplied title mock-up.</summary>
    public static readonly Color MenuPlate = new Color32(104, 104, 106, 240);

    /// <summary>Source resolution of the supplied full-screen artwork. Art-space helpers use these units.</summary>
    public const float ArtWidth = 1920f;
    public const float ArtHeight = 1080f;

    private const int RoundedCornerRadius = 10;

    private static Font displayFont;
    private static Font bodyFont;
    private static Sprite roundedSprite;

    /// <summary>
    /// Applies the client-supplied Eight-Bit Madness family to every generated label. Scene-authored
    /// screens call this during Awake so direct GameScene/CharacterSelect play uses the same family.
    /// </summary>
    public static void SetFontFamily(Font font)
    {
        if (font == null) return;
        displayFont = font;
        bodyFont = font;
    }

    /// <summary>Condensed display face used for headings and button labels.</summary>
    public static Font DisplayFont
    {
        get
        {
            if (displayFont == null)
            {
                displayFont = Resources.Load<Font>(ManningAssetLibrary.DisplayFont);
                if (displayFont == null) displayFont = BodyFont;
            }
            return displayFont;
        }
    }

    /// <summary>Readable face used for paragraphs and HUD readouts.</summary>
    public static Font BodyFont
    {
        get
        {
            if (bodyFont == null)
            {
                bodyFont = displayFont != null ? displayFont : Resources.Load<Font>(ManningAssetLibrary.DisplayFont);
                if (bodyFont == null) bodyFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            }
            return bodyFont;
        }
    }

    public static Canvas CreateCanvas(string name, int sortingOrder = 500)
    {
        EnsureEventSystem();
        GameObject canvasObject = new GameObject(name, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = sortingOrder;
        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(ArtWidth, ArtHeight);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
        return canvas;
    }

    public static GameObject CreatePanel(Transform parent, string name, Color color)
    {
        GameObject panel = new GameObject(name, typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(parent, false);
        Stretch(panel.GetComponent<RectTransform>());
        panel.GetComponent<Image>().color = color;
        return panel;
    }

    /// <summary>
    /// Letterboxes a full-screen artwork inside its parent. Children of the returned graphic can be
    /// positioned with the art-space helpers below and stay locked to the painted artwork at every aspect ratio.
    /// </summary>
    public static RawImage CreateScreen(Transform parent, string resourcePath, Color tint)
    {
        CreatePanel(parent, "ScreenBackdrop", Color.black);
        GameObject imageObject = new GameObject("ScreenArtwork", typeof(RectTransform), typeof(RawImage), typeof(AspectRatioFitter));
        imageObject.transform.SetParent(parent, false);
        Stretch(imageObject.GetComponent<RectTransform>());
        RawImage rawImage = imageObject.GetComponent<RawImage>();
        rawImage.texture = ManningAssetLibrary.LoadTexture(resourcePath);
        rawImage.color = tint;
        AspectRatioFitter fitter = imageObject.GetComponent<AspectRatioFitter>();
        fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        fitter.aspectRatio = rawImage.texture != null ? rawImage.texture.width / (float)rawImage.texture.height : ArtWidth / ArtHeight;
        return rawImage;
    }

    /// <summary>
    /// Anchors a child of a fitted artwork to a pixel rectangle of the 1920x1080 source art,
    /// measuring Y downwards from the top of the art so values can be read straight off the mock-up.
    /// </summary>
    public static void AnchorToArt(RectTransform rect, float centerX, float centerY, float width, float height)
    {
        float halfWidth = width * 0.5f;
        float halfHeight = height * 0.5f;
        rect.anchorMin = new Vector2((centerX - halfWidth) / ArtWidth, 1f - (centerY + halfHeight) / ArtHeight);
        rect.anchorMax = new Vector2((centerX + halfWidth) / ArtWidth, 1f - (centerY - halfHeight) / ArtHeight);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    public static Text CreateText(Transform parent, string name, string value, int fontSize, TextAnchor alignment,
        Vector2 anchor, Vector2 size, Vector2 position, Color color, Font font = null)
    {
        GameObject textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
        textObject.transform.SetParent(parent, false);
        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = size;
        rect.anchoredPosition = position;
        Text text = textObject.GetComponent<Text>();
        text.font = font != null ? font : BodyFont;
        text.text = value;
        text.fontSize = fontSize;
        text.fontStyle = font != null ? FontStyle.Normal : FontStyle.Bold;
        text.alignment = alignment;
        text.color = color;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        return text;
    }

    /// <summary>Display-face label pinned to a pixel rectangle of the source artwork.</summary>
    public static Text CreateArtText(Transform artParent, string name, string value, float centerX, float centerY,
        float width, float height, int fontSize, TextAnchor alignment, Color color)
    {
        GameObject textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
        textObject.transform.SetParent(artParent, false);
        AnchorToArt(textObject.GetComponent<RectTransform>(), centerX, centerY, width, height);
        Text text = textObject.GetComponent<Text>();
        text.font = DisplayFont;
        text.text = value;
        text.alignment = alignment;
        text.color = color;
        text.raycastTarget = false;
        ApplyBestFit(text, fontSize);
        return text;
    }

    public static Button CreateButton(Transform parent, string name, string label, Vector2 anchor, Vector2 size,
        Vector2 position, Color background, UnityAction action, int fontSize = 34)
    {
        GameObject buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(parent, false);
        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = size;
        rect.anchoredPosition = position;

        StyleButton(buttonObject, background, action);
        AddButtonLabel(buttonObject, label, fontSize);
        return buttonObject.GetComponent<Button>();
    }

    /// <summary>Menu plate pinned to a pixel rectangle of the source artwork, as laid out in the client mock-up.</summary>
    public static Button CreateArtButton(Transform artParent, string name, string label, float centerX, float centerY,
        float width, float height, Color background, UnityAction action, int fontSize = 34)
    {
        GameObject buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(artParent, false);
        AnchorToArt(buttonObject.GetComponent<RectTransform>(), centerX, centerY, width, height);

        StyleButton(buttonObject, background, action);
        Image image = buttonObject.GetComponent<Image>();
        image.sprite = GetRoundedSprite();
        image.type = Image.Type.Sliced;

        Shadow shadow = buttonObject.AddComponent<Shadow>();
        shadow.effectColor = new Color(0f, 0f, 0f, 0.45f);
        shadow.effectDistance = new Vector2(3f, -4f);

        AddButtonLabel(buttonObject, label, fontSize);
        return buttonObject.GetComponent<Button>();
    }

    /// <summary>Text-only link pinned to the artwork; the label itself is the clickable target.</summary>
    public static Button CreateArtLink(Transform artParent, string name, string label, float centerX, float centerY,
        float width, float height, int fontSize, TextAnchor alignment, Color color, Color highlight, UnityAction action)
    {
        Text text = CreateArtText(artParent, name, label, centerX, centerY, width, height, fontSize, alignment, color);
        text.raycastTarget = true;
        AddTextOutline(text, 0.65f);

        // Selectable tints the target graphic's canvas renderer, so the base colour stays on the Text
        // itself and normalColor must be white to avoid double-multiplying it.
        Button button = text.gameObject.AddComponent<Button>();
        button.targetGraphic = text;
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = highlight;
        colors.pressedColor = Color.Lerp(highlight, Color.black, 0.2f);
        colors.selectedColor = highlight;
        button.colors = colors;
        AddClickHandler(button, action);
        return button;
    }

    /// <summary>Labelled 0-1 slider with a live percentage readout, used by the settings screen.</summary>
    public static Slider CreateSlider(Transform parent, string name, string label, float value, Vector2 position,
        UnityAction<float> onChanged)
    {
        GameObject root = new GameObject(name, typeof(RectTransform), typeof(Slider));
        root.transform.SetParent(parent, false);
        RectTransform rect = root.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(720f, 96f);
        rect.anchoredPosition = position;

        CreateText(root.transform, "Label", label, 27, TextAnchor.MiddleLeft, new Vector2(0f, 1f),
            new Vector2(420f, 38f), new Vector2(220f, -24f), Cream, DisplayFont);
        Text readout = CreateText(root.transform, "Value", FormatPercent(value), 27, TextAnchor.MiddleRight,
            new Vector2(1f, 1f), new Vector2(160f, 38f), new Vector2(-90f, -24f), Gold, DisplayFont);

        CreateSliderTrack(root.transform, "Background", new Color(1f, 1f, 1f, 0.18f));
        RectTransform fillArea = CreateSliderTrack(root.transform, "Fill Area", Color.clear);
        RectTransform fill = CreateStretchedBar(fillArea, "Fill", Orange);
        RectTransform handleArea = CreateSliderTrack(root.transform, "Handle Slide Area", Color.clear);
        // Inset by half the handle width so the handle never overhangs the track ends.
        handleArea.offsetMin = new Vector2(handleArea.offsetMin.x + 15f, handleArea.offsetMin.y);
        handleArea.offsetMax = new Vector2(handleArea.offsetMax.x - 15f, handleArea.offsetMax.y);

        GameObject handleObject = new GameObject("Handle", typeof(RectTransform), typeof(Image));
        handleObject.transform.SetParent(handleArea, false);
        RectTransform handle = handleObject.GetComponent<RectTransform>();
        handle.anchorMin = new Vector2(0f, 0f);
        handle.anchorMax = new Vector2(0f, 1f);
        handle.pivot = new Vector2(0.5f, 0.5f);
        handle.sizeDelta = new Vector2(30f, 14f);
        Image handleImage = handleObject.GetComponent<Image>();
        handleImage.sprite = GetRoundedSprite();
        handleImage.type = Image.Type.Sliced;
        handleImage.color = Cream;

        Slider slider = root.GetComponent<Slider>();
        slider.direction = Slider.Direction.LeftToRight;
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.wholeNumbers = false;
        slider.fillRect = fill;
        slider.handleRect = handle;
        slider.targetGraphic = handleImage;
        slider.SetValueWithoutNotify(Mathf.Clamp01(value));
        slider.onValueChanged.AddListener(changed =>
        {
            readout.text = FormatPercent(changed);
            onChanged?.Invoke(changed);
        });
        return slider;
    }

    public static InputField CreateInputField(Transform parent, string name, string placeholder, Vector2 position,
        InputField.ContentType contentType = InputField.ContentType.Standard)
    {
        GameObject inputObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(InputField));
        inputObject.transform.SetParent(parent, false);
        RectTransform rect = inputObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(660f, 76f);
        rect.anchoredPosition = position;

        Image image = inputObject.GetComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 0.96f);

        Text valueText = CreateText(inputObject.transform, "Value", string.Empty, 29, TextAnchor.MiddleLeft,
            new Vector2(0.5f, 0.5f), new Vector2(610f, 70f), Vector2.zero, Navy);
        valueText.fontStyle = FontStyle.Normal;
        Text placeholderText = CreateText(inputObject.transform, "Placeholder", placeholder, 28, TextAnchor.MiddleLeft,
            new Vector2(0.5f, 0.5f), new Vector2(610f, 70f), Vector2.zero, new Color(0.15f, 0.2f, 0.3f, 0.55f));
        placeholderText.fontStyle = FontStyle.Italic;

        InputField input = inputObject.GetComponent<InputField>();
        input.textComponent = valueText;
        input.placeholder = placeholderText;
        input.contentType = contentType;
        input.lineType = InputField.LineType.SingleLine;
        input.characterLimit = 120;
        return input;
    }

    public static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    public static void Clear(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--) Object.Destroy(parent.GetChild(i).gameObject);
    }

    /// <summary>Nine-sliced rounded rectangle generated at runtime so no external UI atlas is required.</summary>
    public static Sprite GetRoundedSprite()
    {
        if (roundedSprite != null) return roundedSprite;

        const int radius = RoundedCornerRadius;
        const int size = radius * 2 + 4;
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false)
        {
            name = "ManningRoundedRect",
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Bilinear,
            hideFlags = HideFlags.HideAndDontSave
        };

        Color[] pixels = new Color[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                pixels[y * size + x] = new Color(1f, 1f, 1f, CornerCoverage(x, y, size, radius));
            }
        }
        texture.SetPixels(pixels);
        texture.Apply();

        roundedSprite = Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f), 100f, 0,
            SpriteMeshType.FullRect, new Vector4(radius, radius, radius, radius));
        roundedSprite.name = "ManningRoundedRect";
        roundedSprite.hideFlags = HideFlags.HideAndDontSave;
        return roundedSprite;
    }

    private static void StyleButton(GameObject buttonObject, Color background, UnityAction action)
    {
        Image image = buttonObject.GetComponent<Image>();
        image.color = background;
        Button button = buttonObject.GetComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = background;
        colors.highlightedColor = Color.Lerp(background, Color.white, 0.22f);
        colors.pressedColor = Color.Lerp(background, Color.black, 0.18f);
        colors.selectedColor = colors.highlightedColor;
        button.colors = colors;
        AddClickHandler(button, action);
    }

    private static void AddButtonLabel(GameObject buttonObject, string label, int fontSize)
    {
        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
        Text text = CreateText(buttonObject.transform, "Label", label, fontSize, TextAnchor.MiddleCenter,
            new Vector2(0.5f, 0.5f), buttonRect.sizeDelta, Vector2.zero, Color.white, DisplayFont);
        Stretch(text.rectTransform);
        text.raycastTarget = false;
        ApplyBestFit(text, fontSize);
        AddTextOutline(text, 0.55f);
    }

    private static void AddClickHandler(Button button, UnityAction action)
    {
        button.onClick.AddListener(() =>
        {
            ManningAudio.Instance?.PlayUi();
            action?.Invoke();
        });
    }

    private static void AddTextOutline(Text text, float alpha)
    {
        Outline outline = text.gameObject.AddComponent<Outline>();
        outline.effectColor = new Color(0f, 0f, 0f, alpha);
        outline.effectDistance = new Vector2(2f, -2f);
    }

    /// <summary>Keeps labels proportional to their plate when the artwork is letterboxed on off-16:9 displays.</summary>
    private static void ApplyBestFit(Text text, int fontSize)
    {
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        text.fontSize = fontSize;
        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = 6;
        text.resizeTextMaxSize = fontSize;
    }

    private static RectTransform CreateSliderTrack(Transform parent, string name, Color color)
    {
        GameObject bar = new GameObject(name, typeof(RectTransform), typeof(Image));
        bar.transform.SetParent(parent, false);
        RectTransform rect = bar.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 0f);
        rect.anchorMax = new Vector2(1f, 0f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.offsetMin = new Vector2(10f, 10f);
        rect.offsetMax = new Vector2(-10f, 28f);
        ApplyBarSkin(bar.GetComponent<Image>(), color);
        return rect;
    }

    private static RectTransform CreateStretchedBar(RectTransform parent, string name, Color color)
    {
        GameObject bar = new GameObject(name, typeof(RectTransform), typeof(Image));
        bar.transform.SetParent(parent, false);
        RectTransform rect = bar.GetComponent<RectTransform>();
        Stretch(rect);
        ApplyBarSkin(bar.GetComponent<Image>(), color);
        return rect;
    }

    private static void ApplyBarSkin(Image image, Color color)
    {
        image.sprite = GetRoundedSprite();
        image.type = Image.Type.Sliced;
        image.color = color;
        image.raycastTarget = color.a > 0f;
    }

    private static string FormatPercent(float value) => Mathf.RoundToInt(Mathf.Clamp01(value) * 100f) + "%";

    private static float CornerCoverage(int x, int y, int size, int radius)
    {
        const int samples = 4;
        int inside = 0;
        for (int sampleY = 0; sampleY < samples; sampleY++)
        {
            for (int sampleX = 0; sampleX < samples; sampleX++)
            {
                float pointX = x + (sampleX + 0.5f) / samples;
                float pointY = y + (sampleY + 0.5f) / samples;
                float nearestX = Mathf.Clamp(pointX, radius, size - radius);
                float nearestY = Mathf.Clamp(pointY, radius, size - radius);
                float deltaX = pointX - nearestX;
                float deltaY = pointY - nearestY;
                if (deltaX * deltaX + deltaY * deltaY <= radius * radius) inside++;
            }
        }
        return inside / (float)(samples * samples);
    }

    private static void EnsureEventSystem()
    {
        if (Object.FindAnyObjectByType<EventSystem>() != null) return;
        GameObject eventObject = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
        Object.DontDestroyOnLoad(eventObject);
    }
}
