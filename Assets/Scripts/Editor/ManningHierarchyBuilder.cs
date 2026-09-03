#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Creates the client-facing screens, hard-mode obstacle templates, lane transforms, and gameplay UI
/// as saved scene objects. Run from Manning > Rebuild Client Hierarchy after changing source artwork.
/// </summary>
public static class ManningHierarchyBuilder
{
    private const string MainMenuScenePath = "Assets/Scenes/MainMenu.unity";
    private const string CharacterScenePath = "Assets/Scenes/CharacterSelectScene.unity";
    private const string GameScenePath = "Assets/Scenes/GameScene.unity";

    private const string MainMenuAssetRoot = "Assets/Imported/Sprites/Main_Manu/Mayhem updated assets 9.2";
    private const string BrandFontPath = MainMenuAssetRoot + "/1/Eight-Bit Madness.ttf";
    private const string BlueBackgroundPath = MainMenuAssetRoot + "/1/mcm-blue background.png";
    private const string TelevisionPath = MainMenuAssetRoot + "/2/mcm-tv.png";
    private const string InputBackgroundPath = MainMenuAssetRoot + "/3/mcm-blackbox.png";
    private const string SubmitButtonPath = MainMenuAssetRoot + "/3/mcm-submitbutton.png";

    private static readonly Color Navy = new Color32(3, 20, 56, 250);
    private static readonly Color DeepNavy = new Color32(1, 10, 28, 242);
    private static readonly Color Blue = new Color32(12, 75, 174, 255);
    private static readonly Color Orange = new Color32(242, 105, 28, 255);
    private static readonly Color Gold = new Color32(255, 191, 28, 255);
    private static readonly Color Cream = new Color32(249, 246, 232, 255);
    private static readonly Color Danger = new Color32(255, 92, 67, 255);

    [MenuItem("Manning/Rebuild Client Hierarchy")]
    public static void RebuildAll()
    {
        Font font = AssetDatabase.LoadAssetAtPath<Font>(BrandFontPath);
        if (font == null) throw new InvalidOperationException("Eight-Bit Madness font is missing at " + BrandFontPath);

        BuildInScene(MainMenuScenePath, scene => BuildMainMenu(scene, font));
        BuildInScene(CharacterScenePath, scene => BuildCharacterSelect(scene, font));
        BuildInScene(GameScenePath, scene => BuildGameScene(scene, font));

        AssetDatabase.SaveAssets();
        Debug.Log("[ManningHierarchyBuilder] COMPLETE - MainMenu, CharacterSelectScene, and GameScene now use organized, hierarchy-authored production UI and hard-mode lanes.");
    }

    private static void BuildInScene(string path, Action<Scene> build)
    {
        Scene previous = SceneManager.GetActiveScene();
        Scene scene = SceneManager.GetSceneByPath(path);
        bool wasLoaded = scene.IsValid() && scene.isLoaded;
        if (!wasLoaded) scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

        SceneManager.SetActiveScene(scene);
        build(scene);
        EditorSceneManager.MarkSceneDirty(scene);
        if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Could not save " + path);

        if (previous.IsValid() && previous.isLoaded) SceneManager.SetActiveScene(previous);
        if (!wasLoaded) EditorSceneManager.CloseScene(scene, true);
    }

    private static void BuildMainMenu(Scene scene, Font font)
    {
        DestroyRoot(scene, "[00] MANNINGCAST FRONT END - EDIT HERE");
        GameObject legacy = EnsureRoot(scene, "[99] Legacy Main Menu (Disabled)");
        MoveRoot(scene, legacy.transform, "Canvas");
        MoveRoot(scene, legacy.transform, "MainMenuManager");
        MoveRoot(scene, legacy.transform, "Main Camera");
        MoveRoot(scene, legacy.transform, "EventSystem");
        legacy.SetActive(false);

        GameObject root = NewRoot(scene, "[00] MANNINGCAST FRONT END - EDIT HERE");
        ManningFrontEndFlow flow = root.AddComponent<ManningFrontEndFlow>();
        CreateEventSystem(root.transform);
        Canvas canvas = CreateCanvas(root.transform, "ManningFrontEndCanvas", 600);

        Sprite blueBackground = LoadLargestSprite(BlueBackgroundPath);
        Sprite tvSprite = LoadLargestSprite(TelevisionPath);
        Sprite inputSprite = LoadLargestSprite(InputBackgroundPath);
        Sprite submitSprite = LoadLargestSprite(SubmitButtonPath);
        Texture2D logo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/ManningMayhem/Screens/Logo.png");

        GameObject title = BuildTitleScreen(canvas.transform, font, flow);
        GameObject entry = BuildEntryScreen(canvas.transform, font, blueBackground, tvSprite, inputSprite, submitSprite,
            logo, flow, out InputField name, out InputField email, out InputField phone, out Text validation);
        GameObject overview = BuildOverviewScreen(canvas.transform, font, blueBackground, logo, flow);
        GameObject rules = BuildRulesScreen(canvas.transform, font, blueBackground, logo, flow);
        GameObject settings = BuildSettingsScreen(canvas.transform, font, blueBackground, logo, flow,
            out Slider music, out Slider sfx);
        GameObject terms = BuildTermsOverlay(canvas.transform, font, flow);

        flow.ConfigureHierarchy(font, title, entry, overview, rules, settings, terms, name, email, phone,
            validation, music, sfx);
        title.SetActive(true);
        entry.SetActive(false);
        overview.SetActive(false);
        rules.SetActive(false);
        settings.SetActive(false);
        terms.SetActive(false);

        Require(root.GetComponentsInChildren<InputField>(true).Length == 3, "MainMenu must contain exactly three player-entry fields.");
        Require(root.GetComponentsInChildren<Text>(true).All(text => text.font == font), "Every MainMenu label must use Eight-Bit Madness.");
        Debug.Log("[ManningHierarchyBuilder] MainMenu: 5 screens + terms overlay, 3 entry fields, and client art/font saved in Hierarchy.");
    }

    private static GameObject BuildTitleScreen(Transform parent, Font font, ManningFrontEndFlow flow)
    {
        GameObject screen = ScreenRoot(parent, "01 - MAIN TITLE");
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/ManningMayhem/Screens/Title.png");
        RawImage artwork = CreateScreenArtwork(screen.transform, "Client Title Artwork", texture);

        Button play = CreateArtButton(artwork.transform, "PLAY Button", "PLAY", font, 884f, 498f, 236f, 52f, new Color32(104, 104, 106, 242));
        Button settings = CreateArtButton(artwork.transform, "SETTINGS Button", "SETTINGS", font, 884f, 595f, 236f, 52f, new Color32(104, 104, 106, 242));
        Button rules = CreateArtButton(artwork.transform, "RULES Button", "RULES", font, 884f, 692f, 236f, 52f, new Color32(104, 104, 106, 242));
        Button exit = CreateArtButton(artwork.transform, "EXIT Button", "EXIT", font, 884f, 789f, 236f, 52f, new Color32(104, 104, 106, 242));
        Button website = CreateArtTextButton(artwork.transform, "Omaha Website Link", "WWW.OMAHAPRODUCTIONS.COM", font,
            1610f, 1038f, 560f, 48f, 29, TextAnchor.MiddleRight);

        Link(play, flow.ShowEntry);
        Link(settings, flow.ShowSettings);
        Link(rules, flow.ShowRules);
        Link(exit, flow.QuitGame);
        Link(website, flow.OpenWebsite);
        return screen;
    }

    private static GameObject BuildEntryScreen(Transform parent, Font font, Sprite backgroundSprite, Sprite tvSprite,
        Sprite inputSprite, Sprite submitSprite, Texture2D logo, ManningFrontEndFlow flow, out InputField nameField,
        out InputField emailField, out InputField phoneField, out Text validation)
    {
        GameObject screen = ScreenRoot(parent, "02 - PLAYER INFORMATION (LOGIN)");
        CreateFullScreenImage(screen.transform, "Blue Background - CLIENT ASSET", backgroundSprite, Color.white);
        RawImage logoImage = CreateRawImage(screen.transform, "ManningCast Mayhem Logo", logo, true);
        Place(logoImage.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(710f, 312f), new Vector2(0f, 388f));

        Image tv = CreateImage(screen.transform, "Television - CLIENT ASSET", tvSprite, Color.white);
        tv.preserveAspect = true;
        Place(tv.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(1030f, 759f), new Vector2(0f, -78f));

        Image shade = CreateImage(tv.transform, "CRT Content Shade", null, new Color(0f, 0f, 0f, 0.76f));
        AnchorFromTop(shade.rectTransform, 318f, 284f, 520f, 402f, 752f, 554f);

        Text heading = CreateText(tv.transform, "PLAYER INFORMATION Heading", "PLAYER INFORMATION", font, 31,
            TextAnchor.MiddleCenter, Gold);
        AnchorFromTop(heading.rectTransform, 318f, 125f, 470f, 46f, 752f, 554f);

        nameField = CreateTvInput(tv.transform, "Name Input", "NAME", font, inputSprite, 190f);
        emailField = CreateTvInput(tv.transform, "Email Input", "EMAIL", font, inputSprite, 258f);
        emailField.contentType = InputField.ContentType.EmailAddress;
        phoneField = CreateTvInput(tv.transform, "Phone Input", "PHONE", font, inputSprite, 326f);
        phoneField.contentType = InputField.ContentType.Standard;

        Button submit = CreateImageButton(tv.transform, "SUBMIT Button - CLIENT ASSET", submitSprite, Color.white);
        AnchorFromTop(submit.GetComponent<RectTransform>(), 355f, 392f, 245f, 55f, 752f, 554f);
        Link(submit, flow.SubmitEntry);

        Text legal = CreateText(tv.transform, "Contest Terms Link",
            "BY SUBMITTING, YOU ARE ENTERING THE\nOMAHA PRODUCTIONS CONTEST AND AGREE\nTO THE <color=#55A8FF><u>TERMS AND CONDITIONS</u></color>.",
            font, 17, TextAnchor.MiddleCenter, Color.white);
        legal.supportRichText = true;
        AnchorFromTop(legal.rectTransform, 318f, 458f, 500f, 76f, 752f, 554f);
        Button legalButton = legal.gameObject.AddComponent<Button>();
        legalButton.targetGraphic = legal;
        Link(legalButton, flow.ShowTerms);

        validation = CreateText(tv.transform, "Validation Message", string.Empty, font, 15, TextAnchor.MiddleCenter, Danger);
        AnchorFromTop(validation.rectTransform, 318f, 520f, 500f, 28f, 752f, 554f);

        Button skip = CreateButton(screen.transform, "SKIP Button", "SKIP", font, inputSprite, Color.white, 31);
        Place(skip.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(255f, 58f), new Vector2(0f, -485f));
        Link(skip, flow.SkipEntry);

        Button back = CreateButton(screen.transform, "BACK Button", "BACK", font, inputSprite, Color.white, 24);
        Place(back.GetComponent<RectTransform>(), new Vector2(0f, 1f), new Vector2(155f, 50f), new Vector2(98f, -52f));
        Link(back, flow.ShowTitle);
        return screen;
    }

    private static InputField CreateTvInput(Transform tv, string name, string label, Font font, Sprite inputSprite, float centerY)
    {
        Text rowLabel = CreateText(tv, label + " Label", label, font, 25, TextAnchor.MiddleRight, Color.white);
        AnchorFromTop(rowLabel.rectTransform, 160f, centerY, 145f, 48f, 752f, 554f);

        InputField input = CreateInputField(tv, name, label, font, inputSprite);
        AnchorFromTop(input.GetComponent<RectTransform>(), 386f, centerY, 315f, 50f, 752f, 554f);
        return input;
    }

    private static GameObject BuildOverviewScreen(Transform parent, Font font, Sprite background, Texture2D logo,
        ManningFrontEndFlow flow)
    {
        GameObject screen = BrandedPanelScreen(parent, "03 - GAME OVERVIEW", font, background, logo, "GAME OVERVIEW",
            out GameObject card);
        Text body = CreateText(card.transform, "Overview Copy",
            "THE MANNINGCAST IS ABOUT TO START!\n\nCROSS 7 MOVING STUDIO LANES AND REACH THE COUCH.\n\n" +
            "ATHLETES AND CHALLENGE FLAGS COST A LIFE. SANDWICHES COST TIME.\n\n" +
            "RECLINERS, REMOTES, AND BLUE QUARTER-ZIPS ARE SAFE.\n\n" +
            "COLLECT A FOOTBALL, THEN PRESS SPACE TO DISTRACT THE NEAREST ATHLETE.",
            font, 29, TextAnchor.MiddleCenter, Cream);
        Place(body.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(1040f, 490f), new Vector2(0f, 5f));

        Button next = CreateButton(card.transform, "NEXT Button", "NEXT", font, null, Orange, 30);
        Place(next.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(240f, 68f), new Vector2(-145f, 58f));
        Button skip = CreateButton(card.transform, "SKIP Button", "SKIP", font, null, Blue, 30);
        Place(skip.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(220f, 68f), new Vector2(145f, 58f));
        Link(next, flow.ShowRules);
        Link(skip, flow.LoadCharacterSelect);
        return screen;
    }

    private static GameObject BuildRulesScreen(Transform parent, Font font, Sprite background, Texture2D logo,
        ManningFrontEndFlow flow)
    {
        GameObject screen = BrandedPanelScreen(parent, "04 - RULES", font, background, logo, "KEYS TO VICTORY",
            out GameObject card);
        Text body = CreateText(card.transform, "Rules Copy",
            "<color=#65D6A6>SAFE ROUTE</color>\nRECLINERS, REMOTES, AND BLUE QUARTER-ZIPS ARE HARMLESS.\n\n" +
            "<color=#FFD34D>POWER PLAYS</color>\nGOLDEN QUARTER-ZIPS ADD 250. FOOTBALLS SEND THE NEAREST ATHLETE AWAY.\n\n" +
            "<color=#FF8069>WATCH OUT</color>\nATHLETES AND FLAGS COST 1 LIFE. SANDWICHES REMOVE 8 SECONDS.\n\n" +
            "<color=#7DB8FF>BEAT THE CLOCK</color>\nCROSS ALL 7 LANES IN 30 SECONDS. YOU HAVE 3 LIVES.",
            font, 27, TextAnchor.MiddleLeft, Color.white);
        body.supportRichText = true;
        Place(body.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(1070f, 515f), new Vector2(0f, 8f));

        Button back = CreateButton(card.transform, "BACK TO MENU Button", "BACK TO MENU", font, null, Blue, 26);
        Place(back.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(300f, 68f), new Vector2(-205f, 55f));
        Button pick = CreateButton(card.transform, "PICK YOUR MANNING Button", "PICK YOUR MANNING", font, null, Orange, 25);
        Place(pick.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(390f, 68f), new Vector2(185f, 55f));
        Link(back, flow.ShowTitle);
        Link(pick, flow.LoadCharacterSelect);
        return screen;
    }

    private static GameObject BuildSettingsScreen(Transform parent, Font font, Sprite background, Texture2D logo,
        ManningFrontEndFlow flow, out Slider music, out Slider sfx)
    {
        GameObject screen = BrandedPanelScreen(parent, "05 - SETTINGS", font, background, logo, "SETTINGS",
            out GameObject card);
        music = CreateSlider(card.transform, "Music Volume", "MUSIC", font, new Vector2(0f, 110f));
        sfx = CreateSlider(card.transform, "Sound Effects Volume", "SOUND EFFECTS", font, new Vector2(0f, -25f));
        UnityEventTools.AddPersistentListener(music.onValueChanged, flow.SetMusicVolume);
        UnityEventTools.AddPersistentListener(sfx.onValueChanged, flow.SetSfxVolume);
        Text controls = CreateText(card.transform, "Controls",
            "MOVE: ARROWS / WASD     USE FOOTBALL: SPACE / ENTER     PAUSE: ESC", font, 22,
            TextAnchor.MiddleCenter, Cream);
        Place(controls.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(1070f, 52f), new Vector2(0f, -165f));
        Button back = CreateButton(card.transform, "BACK Button", "BACK", font, null, Orange, 29);
        Place(back.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(240f, 68f), new Vector2(0f, 58f));
        Link(back, flow.ShowTitle);
        return screen;
    }

    private static GameObject BuildTermsOverlay(Transform parent, Font font, ManningFrontEndFlow flow)
    {
        GameObject overlay = CreatePanel(parent, "06 - TERMS OVERLAY", new Color(0f, 0f, 0f, 0.88f));
        GameObject card = CreatePanel(overlay.transform, "Terms Card", Navy);
        Place(card.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(940f, 470f), Vector2.zero);
        Text heading = CreateText(card.transform, "Heading", "OFFICIAL TERMS", font, 43, TextAnchor.MiddleCenter, Gold);
        Place(heading.rectTransform, new Vector2(0.5f, 1f), new Vector2(760f, 70f), new Vector2(0f, -65f));
        Text body = CreateText(card.transform, "Legal Status",
            "OMAHA'S APPROVED CONTEST TERMS AND PUBLIC URL WERE NOT INCLUDED WITH THE SOURCE FILES.\n\n" +
            "SET MANNING.CONFIG.TERMSURL WHEN THE FINAL LEGAL DESTINATION IS PROVIDED.",
            font, 25, TextAnchor.MiddleCenter, Color.white);
        Place(body.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(790f, 220f), new Vector2(0f, 15f));
        Button close = CreateButton(card.transform, "CLOSE Button", "CLOSE", font, null, Orange, 28);
        Place(close.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(230f, 68f), new Vector2(0f, 52f));
        Link(close, flow.CloseTerms);
        return overlay;
    }

    private static GameObject BrandedPanelScreen(Transform parent, string name, Font font, Sprite background,
        Texture2D logo, string heading, out GameObject card)
    {
        GameObject screen = ScreenRoot(parent, name);
        CreateFullScreenImage(screen.transform, "Blue Background - CLIENT ASSET", background, Color.white);
        RawImage logoImage = CreateRawImage(screen.transform, "ManningCast Mayhem Logo", logo, true);
        Place(logoImage.rectTransform, new Vector2(0.5f, 1f), new Vector2(430f, 189f), new Vector2(0f, -105f));
        card = CreatePanel(screen.transform, "Editable Content Card", DeepNavy);
        Place(card.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(1260f, 775f), new Vector2(0f, -95f));
        Outline outline = card.AddComponent<Outline>();
        outline.effectColor = Gold;
        outline.effectDistance = new Vector2(3f, -3f);
        Text title = CreateText(card.transform, "Heading", heading, font, 50, TextAnchor.MiddleCenter, Gold);
        Place(title.rectTransform, new Vector2(0.5f, 1f), new Vector2(980f, 78f), new Vector2(0f, -64f));
        return screen;
    }

    private static void BuildCharacterSelect(Scene scene, Font font)
    {
        DestroyRoot(scene, "[00] MANNINGCAST CHARACTER SELECT - EDIT HERE");
        GameObject legacy = EnsureRoot(scene, "[99] Legacy Character Select (Disabled)");
        MoveRoot(scene, legacy.transform, "Canvas");
        MoveRoot(scene, legacy.transform, "CharacterSelectManager");
        MoveRoot(scene, legacy.transform, "Main Camera");
        MoveRoot(scene, legacy.transform, "EventSystem");
        legacy.SetActive(false);

        GameObject root = NewRoot(scene, "[00] MANNINGCAST CHARACTER SELECT - EDIT HERE");
        ManningCharacterSelectFlow flow = root.AddComponent<ManningCharacterSelectFlow>();
        CreateEventSystem(root.transform);
        Canvas canvas = CreateCanvas(root.transform, "ManningCharacterSelectCanvas", 600);
        Texture2D artworkTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/ManningMayhem/Screens/CharacterSelect.png");
        CreateScreenArtwork(canvas.transform, "Character Select - CLIENT ARTWORK", artworkTexture);

        Button peyton = CreateButton(canvas.transform, "PLAY AS PEYTON Button", "PLAY AS PEYTON", font, null, Orange, 29);
        Place(peyton.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(380f, 76f), new Vector2(-315f, 70f));
        Button eli = CreateButton(canvas.transform, "PLAY AS ELI Button", "PLAY AS ELI", font, null, Blue, 29);
        Place(eli.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(380f, 76f), new Vector2(315f, 70f));
        Button back = CreateButton(canvas.transform, "BACK Button", "BACK", font, null, Navy, 24);
        Place(back.GetComponent<RectTransform>(), new Vector2(0f, 1f), new Vector2(170f, 56f), new Vector2(105f, -58f));
        Link(peyton, flow.SelectPeyton);
        Link(eli, flow.SelectEli);
        Link(back, flow.BackToMenu);
        flow.ConfigureHierarchy(font, canvas);

        Require(root.GetComponentsInChildren<Text>(true).All(text => text.font == font), "Every CharacterSelect label must use Eight-Bit Madness.");
        Debug.Log("[ManningHierarchyBuilder] CharacterSelectScene: artwork and all controls saved in Hierarchy.");
    }

    private static void BuildGameScene(Scene scene, Font font)
    {
        OrganizeGameScene(scene);
        DestroyRoot(scene, "[02] SEVEN LANE HARD MODE - EDIT HERE");
        DestroyRoot(scene, "[03] GAMEPLAY UI - EDIT HERE");

        GameObject core = EnsureRoot(scene, "[01] GAMEPLAY CORE");
        GameObject players = EnsureChild(core.transform, "Player Variants (Peyton + Eli)");
        MoveAnywhere(scene, players.transform, "P_Peyton 100");
        MoveAnywhere(scene, players.transform, "P_Eli 100");
        foreach (PlayerMovement player in players.GetComponentsInChildren<PlayerMovement>(true))
            if (player.GetComponent<ManningCharacterSpriteAnimator>() == null) player.gameObject.AddComponent<ManningCharacterSpriteAnimator>();

        Transform oldSpectator = core.transform.Find("Opposite Brother - Couch Spectator");
        if (oldSpectator != null) UnityEngine.Object.DestroyImmediate(oldSpectator.gameObject);
        GameObject spectator = new GameObject("Opposite Brother - Couch Spectator", typeof(SpriteRenderer), typeof(ManningCouchSpectator));
        spectator.transform.SetParent(core.transform, false);
        spectator.transform.position = new Vector3(0.48f, 3.2f, 0f);

        GameManager game = FindComponentInScene<GameManager>(scene);
        Require(game != null, "GameScene requires a GameManager.");
        game.ApplyHardGameplayDefaults();
        EditorUtility.SetDirty(game);
        PauseManager pause = FindComponentInScene<PauseManager>(scene);
        if (pause != null)
        {
            SerializedObject serializedPause = new SerializedObject(pause);
            SerializedProperty legacyPanel = serializedPause.FindProperty("settingsPanel");
            if (legacyPanel != null) legacyPanel.objectReferenceValue = null;
            serializedPause.ApplyModifiedPropertiesWithoutUndo();
        }

        GameObject laneRoot = NewRoot(scene, "[02] SEVEN LANE HARD MODE - EDIT HERE");
        ManningLaneDirector director = laneRoot.AddComponent<ManningLaneDirector>();
        GameObject bounds = EnsureChild(laneRoot.transform, "Course Bounds (Move Left + Right Handles)");
        Transform left = NewTransform(bounds.transform, "LEFT Spawn + Despawn", new Vector3(-7.8f, 0f, 0f));
        Transform right = NewTransform(bounds.transform, "RIGHT Spawn + Despawn", new Vector3(10.5f, 0f, 0f));

        GameObject lanesParent = EnsureChild(laneRoot.transform, "Seven Lane Runtime Contents");
        Transform[] lanes = new Transform[7];
        for (int i = 0; i < lanes.Length; i++)
            lanes[i] = NewTransform(lanesParent.transform, $"Lane {i + 1:00} - {(i % 2 == 0 ? "LEFT TO RIGHT" : "RIGHT TO LEFT")}",
                new Vector3(0f, -3.05f + i, 0f));

        GameObject templatesParent = EnsureChild(laneRoot.transform, "Obstacle Templates (EDIT VISUAL SCALE + HITBOX)");
        ManningLaneItem[] templates = BuildObstacleTemplates(templatesParent.transform);
        director.ConfigureHierarchy(left, right, lanes, templates);

        GameObject uiRoot = NewRoot(scene, "[03] GAMEPLAY UI - EDIT HERE");
        CreateEventSystem(uiRoot.transform);
        BuildGameUiHierarchy(uiRoot.transform, font);

        Require(lanes.Length == 7, "Exactly seven lane transforms are required.");
        Require(templates.Length == 10, "Exactly ten obstacle templates are required.");
        Require(templates.All(item => item.GetComponent<Collider2D>() != null && item.GetComponentInChildren<SpriteRenderer>(true) != null),
            "Every obstacle template requires a visible sprite and editable collider.");
        Debug.Log("[ManningHierarchyBuilder] GameScene: 7 lanes, 10 normalized obstacle templates, 30-second hard mode, and authored HUD saved in Hierarchy.");
    }

    private static void OrganizeGameScene(Scene scene)
    {
        GameObject environment = EnsureRoot(scene, "[00] STUDIO ENVIRONMENT");
        MoveRoot(scene, environment.transform, "World");
        MoveRoot(scene, environment.transform, "Environments");
        MoveRoot(scene, environment.transform, "Grid");

        GameObject core = EnsureRoot(scene, "[01] GAMEPLAY CORE");
        MoveRoot(scene, core.transform, "Managers");
        MoveRoot(scene, core.transform, "GoalArea");

        GameObject camera = EnsureRoot(scene, "[04] CAMERA + LIGHTING");
        MoveRoot(scene, camera.transform, "Main Camera");
        MoveRoot(scene, camera.transform, "Global Light 2D");

        GameObject legacy = EnsureRoot(scene, "[99] LEGACY GAMEPLAY (Disabled - Reference Only)");
        string[] legacyRoots = { "Pools", "ObstacleLanes", "SafeLane", "SafeLane_1", "SafeLane_2", "HazardZone_1", "HazardZone_2", "HUDCanvas", "EventSystem" };
        foreach (string name in legacyRoots) MoveRoot(scene, legacy.transform, name);
        legacy.SetActive(false);
    }

    private static ManningLaneItem[] BuildObstacleTemplates(Transform parent)
    {
        TemplateSpec[] specs =
        {
            new TemplateSpec(ManningLaneItemKind.AthleteGray, "AthleteGray", 1.20f, new Vector2(0.68f, 0.92f), 0.19f, 1.05f),
            new TemplateSpec(ManningLaneItemKind.AthleteRed, "AthleteRed", 1.20f, new Vector2(0.68f, 0.92f), 0.19f, 1.10f),
            new TemplateSpec(ManningLaneItemKind.ChallengeFlag, "ChallengeFlag", 0.90f, new Vector2(0.66f, 0.44f), 0.13f, 1.05f),
            new TemplateSpec(ManningLaneItemKind.SandwichChicken, "SandwichChicken", 0.78f, new Vector2(0.62f, 0.34f), 0.075f, 0.98f),
            new TemplateSpec(ManningLaneItemKind.SandwichJersey, "SandwichJersey", 0.68f, new Vector2(0.54f, 0.28f), 0.075f, 1.02f),
            new TemplateSpec(ManningLaneItemKind.Recliner, "Recliner", 1.05f, new Vector2(0.78f, 0.65f), 0.08f, 0.88f),
            new TemplateSpec(ManningLaneItemKind.Remote, "Remote", 0.68f, new Vector2(0.32f, 0.54f), 0.06f, 0.95f),
            new TemplateSpec(ManningLaneItemKind.BlueQuarterZip, "BlueQZip", 0.78f, new Vector2(0.50f, 0.58f), 0.06f, 0.95f),
            new TemplateSpec(ManningLaneItemKind.GoldenQuarterZip, "GoldenQZip", 0.84f, new Vector2(0.56f, 0.65f), 0.06f, 1.00f),
            new TemplateSpec(ManningLaneItemKind.Football, "Football", 0.62f, new Vector2(0.49f, 0.30f), 0.08f, 1.08f),
        };

        List<ManningLaneItem> result = new List<ManningLaneItem>();
        foreach (TemplateSpec spec in specs)
        {
            GameObject template = new GameObject("TEMPLATE - " + spec.Kind, typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(ManningLaneItem));
            template.transform.SetParent(parent, false);
            Rigidbody2D body = template.GetComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Kinematic;
            body.gravityScale = 0f;
            BoxCollider2D hitbox = template.GetComponent<BoxCollider2D>();
            hitbox.isTrigger = true;
            hitbox.size = spec.ColliderSize;

            Sprite sprite = LoadLargestSprite("Assets/Resources/ManningMayhem/Elements/" + spec.AssetName + ".png");
            Require(sprite != null, "Missing obstacle sprite: " + spec.AssetName);
            GameObject visualObject = new GameObject("Visual - scale this child", typeof(SpriteRenderer));
            visualObject.transform.SetParent(template.transform, false);
            SpriteRenderer visual = visualObject.GetComponent<SpriteRenderer>();
            visual.sprite = sprite;
            visual.sortingOrder = 82;
            float largestWorldDimension = Mathf.Max(sprite.bounds.size.x, sprite.bounds.size.y);
            float scale = largestWorldDimension > 0f ? spec.MaxVisualSize / largestWorldDimension : 1f;
            visualObject.transform.localScale = Vector3.one * scale;
            visualObject.transform.localPosition = -Vector3.Scale(sprite.bounds.center, visualObject.transform.localScale);

            ManningLaneItem item = template.GetComponent<ManningLaneItem>();
            item.ConfigureTemplate(spec.Kind, visual, hitbox, spec.Weight, spec.SpeedMultiplier);
            template.SetActive(false);
            result.Add(item);
        }
        return result.ToArray();
    }

    private static void BuildGameUiHierarchy(Transform parent, Font font)
    {
        ManningGameUI controller = parent.gameObject.AddComponent<ManningGameUI>();
        Canvas canvas = CreateCanvas(parent, "ManningGameCanvas", 700);
        GameObject hud = CreatePanel(canvas.transform, "01 - TOP HUD (Score / Time / Life)", new Color(0.01f, 0.05f, 0.14f, 0.94f));
        RectTransform hudRect = hud.GetComponent<RectTransform>();
        hudRect.anchorMin = new Vector2(0f, 1f);
        hudRect.anchorMax = new Vector2(1f, 1f);
        hudRect.pivot = new Vector2(0.5f, 1f);
        hudRect.sizeDelta = new Vector2(0f, 82f);
        hudRect.anchoredPosition = Vector2.zero;

        Text score = CreateText(hud.transform, "Score", "SCORE  0000", font, 27, TextAnchor.MiddleLeft, Color.white);
        Place(score.rectTransform, new Vector2(0f, 0.5f), new Vector2(300f, 62f), new Vector2(165f, 0f));
        Text time = CreateText(hud.transform, "Time", "TIME  00:30", font, 29, TextAnchor.MiddleCenter, Color.white);
        Place(time.rectTransform, new Vector2(0.36f, 0.5f), new Vector2(290f, 62f), Vector2.zero);
        Text lives = CreateText(hud.transform, "Lives", "LIVES  3", font, 27, TextAnchor.MiddleCenter, new Color32(255, 126, 71, 255));
        Place(lives.rectTransform, new Vector2(0.59f, 0.5f), new Vector2(270f, 62f), Vector2.zero);
        Text football = CreateText(hud.transform, "Football", "FOOTBALL  --", font, 22, TextAnchor.MiddleCenter, Gold);
        Place(football.rectTransform, new Vector2(0.79f, 0.5f), new Vector2(330f, 62f), Vector2.zero);
        Button pause = CreateButton(hud.transform, "PAUSE Button", "PAUSE", font, null, Blue, 22);
        Place(pause.GetComponent<RectTransform>(), new Vector2(1f, 0.5f), new Vector2(145f, 54f), new Vector2(-88f, 0f));

        Text toast = CreateText(canvas.transform, "02 - Gameplay Feedback", string.Empty, font, 27, TextAnchor.MiddleCenter, Gold);
        Place(toast.rectTransform, new Vector2(0.5f, 1f), new Vector2(1000f, 62f), new Vector2(0f, -116f));
        toast.gameObject.SetActive(false);
        Button website = CreateButton(canvas.transform, "Omaha Website", "OMAHAPRODUCTIONS.COM", font, null,
            new Color(0.02f, 0.08f, 0.17f, 0.86f), 17);
        Place(website.GetComponent<RectTransform>(), new Vector2(0f, 0f), new Vector2(350f, 44f), new Vector2(185f, 25f));

        GameObject pauseOverlay = CreatePanel(canvas.transform, "03 - PAUSE OVERLAY", new Color(0f, 0f, 0f, 0.84f));
        Text paused = CreateText(pauseOverlay.transform, "PAUSED Heading", "PAUSED", font, 76, TextAnchor.MiddleCenter, Gold);
        Place(paused.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(700f, 110f), new Vector2(0f, 160f));
        Text instructions = CreateText(pauseOverlay.transform, "Controls", "ARROWS / WASD TO MOVE     SPACE TO USE FOOTBALL", font, 25,
            TextAnchor.MiddleCenter, Color.white);
        Place(instructions.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(960f, 60f), new Vector2(0f, 70f));
        Button resume = CreateButton(pauseOverlay.transform, "RESUME Button", "RESUME", font, null, Orange, 30);
        Place(resume.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(300f, 76f), new Vector2(0f, -30f));
        Button pauseMenu = CreateButton(pauseOverlay.transform, "MAIN MENU Button", "MAIN MENU", font, null, Blue, 27);
        Place(pauseMenu.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(300f, 70f), new Vector2(0f, -132f));

        GameObject endOverlay = RectObject("04 - WIN / LOSE OVERLAY", canvas.transform);
        Stretch(endOverlay.GetComponent<RectTransform>());
        Texture2D winTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/ManningMayhem/Screens/Win.png");
        RawImage endArtwork = CreateScreenArtwork(endOverlay.transform, "Win or Lose - CLIENT ARTWORK", winTexture);
        Text finalScore = CreateText(endOverlay.transform, "Final Score", "FINAL SCORE  0000", font, 44, TextAnchor.MiddleCenter, Gold);
        Place(finalScore.rectTransform, new Vector2(0.5f, 0f), new Vector2(720f, 68f), new Vector2(0f, 205f));
        GameObject board = CreatePanel(endOverlay.transform, "Local Leaderboard", new Color(0.01f, 0.05f, 0.14f, 0.92f));
        Place(board.GetComponent<RectTransform>(), new Vector2(1f, 0.5f), new Vector2(420f, 300f), new Vector2(-50f, -35f));
        Text boardTitle = CreateText(board.transform, "Heading", "LOCAL TOP SCORES", font, 25, TextAnchor.MiddleCenter, Gold);
        Place(boardTitle.rectTransform, new Vector2(0.5f, 1f), new Vector2(370f, 48f), new Vector2(0f, -35f));
        Text leaderboard = CreateText(board.transform, "Scores", "NO COMPLETED RUNS YET", font, 23, TextAnchor.UpperLeft, Color.white);
        Place(leaderboard.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(340f, 205f), new Vector2(0f, -25f));
        Button again = CreateButton(endOverlay.transform, "PLAY AGAIN Button", "PLAY AGAIN", font, null, Orange, 25);
        Place(again.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(270f, 68f), new Vector2(-290f, 100f));
        Button change = CreateButton(endOverlay.transform, "CHANGE MANNING Button", "CHANGE MANNING", font, null, Blue, 23);
        Place(change.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(300f, 68f), new Vector2(0f, 100f));
        Button menu = CreateButton(endOverlay.transform, "MAIN MENU Button", "MAIN MENU", font, null, Navy, 24);
        Place(menu.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(250f, 68f), new Vector2(285f, 100f));

        Link(pause, controller.TogglePause);
        Link(website, controller.OpenWebsite);
        Link(resume, controller.ResumeGame);
        Link(pauseMenu, controller.LoadMainMenu);
        Link(again, controller.PlayAgain);
        Link(change, controller.ChangeCharacter);
        Link(menu, controller.LoadMainMenu);
        controller.ConfigureHierarchy(font, canvas, score, time, lives, football, toast, pauseOverlay, endOverlay,
            endArtwork, finalScore, leaderboard);
        pauseOverlay.SetActive(false);
        endOverlay.SetActive(false);
    }

    private static GameObject ScreenRoot(Transform parent, string name)
    {
        GameObject root = RectObject(name, parent);
        Stretch(root.GetComponent<RectTransform>());
        return root;
    }

    private static Canvas CreateCanvas(Transform parent, string name, int order)
    {
        GameObject canvasObject = RectObject(name, parent, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = order;
        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
        return canvas;
    }

    private static void CreateEventSystem(Transform parent)
    {
        GameObject eventSystem = new GameObject("Event System - Input System", typeof(EventSystem), typeof(InputSystemUIInputModule));
        eventSystem.transform.SetParent(parent, false);
    }

    private static RawImage CreateScreenArtwork(Transform parent, string name, Texture2D texture)
    {
        CreatePanel(parent, name + " Backdrop", Color.black);
        GameObject imageObject = RectObject(name, parent, typeof(RawImage), typeof(AspectRatioFitter));
        Stretch(imageObject.GetComponent<RectTransform>());
        RawImage image = imageObject.GetComponent<RawImage>();
        image.texture = texture;
        image.color = Color.white;
        AspectRatioFitter fitter = imageObject.GetComponent<AspectRatioFitter>();
        fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        fitter.aspectRatio = texture != null ? texture.width / (float)texture.height : 16f / 9f;
        return image;
    }

    private static Image CreateFullScreenImage(Transform parent, string name, Sprite sprite, Color color)
    {
        Image image = CreateImage(parent, name, sprite, color);
        Stretch(image.rectTransform);
        return image;
    }

    private static GameObject CreatePanel(Transform parent, string name, Color color)
    {
        Image image = CreateImage(parent, name, null, color);
        Stretch(image.rectTransform);
        return image.gameObject;
    }

    private static Image CreateImage(Transform parent, string name, Sprite sprite, Color color)
    {
        GameObject imageObject = RectObject(name, parent, typeof(Image));
        Image image = imageObject.GetComponent<Image>();
        image.sprite = sprite;
        image.color = color;
        image.raycastTarget = false;
        return image;
    }

    private static RawImage CreateRawImage(Transform parent, string name, Texture texture, bool preserveAspect)
    {
        GameObject imageObject = RectObject(name, parent, typeof(RawImage));
        RawImage image = imageObject.GetComponent<RawImage>();
        image.texture = texture;
        image.color = Color.white;
        image.raycastTarget = false;
        if (preserveAspect && texture != null)
        {
            AspectRatioFitter fitter = imageObject.AddComponent<AspectRatioFitter>();
            // Keep the hierarchy-authored width instead of expanding the logo to its parent.
            fitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
            fitter.aspectRatio = texture.width / (float)texture.height;
        }
        return image;
    }

    private static Text CreateText(Transform parent, string name, string value, Font font, int size,
        TextAnchor alignment, Color color)
    {
        GameObject textObject = RectObject(name, parent, typeof(Text));
        Text text = textObject.GetComponent<Text>();
        text.font = font;
        text.text = value;
        text.fontSize = size;
        text.fontStyle = FontStyle.Normal;
        text.alignment = alignment;
        text.color = color;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        text.lineSpacing = 1f;
        return text;
    }

    private static Button CreateArtButton(Transform parent, string name, string label, Font font, float centerX,
        float centerY, float width, float height, Color color)
    {
        Button button = CreateButton(parent, name, label, font, null, color, 30);
        AnchorFromTop(button.GetComponent<RectTransform>(), centerX, centerY, width, height, 1920f, 1080f);
        Shadow shadow = button.gameObject.AddComponent<Shadow>();
        shadow.effectColor = new Color(0f, 0f, 0f, 0.55f);
        shadow.effectDistance = new Vector2(3f, -4f);
        return button;
    }

    private static Button CreateArtTextButton(Transform parent, string name, string label, Font font, float centerX,
        float centerY, float width, float height, int size, TextAnchor anchor)
    {
        Text text = CreateText(parent, name, label, font, size, anchor, Color.white);
        AnchorFromTop(text.rectTransform, centerX, centerY, width, height, 1920f, 1080f);
        Outline outline = text.gameObject.AddComponent<Outline>();
        outline.effectColor = new Color(0f, 0f, 0f, 0.8f);
        outline.effectDistance = new Vector2(2f, -2f);
        Button button = text.gameObject.AddComponent<Button>();
        button.targetGraphic = text;
        SetButtonColors(button, Color.white);
        return button;
    }

    private static Button CreateButton(Transform parent, string name, string label, Font font, Sprite sprite,
        Color background, int fontSize)
    {
        Button button = CreateImageButton(parent, name, sprite, background);
        Text text = CreateText(button.transform, "Label", label, font, fontSize, TextAnchor.MiddleCenter, Color.white);
        Stretch(text.rectTransform);
        text.raycastTarget = false;
        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = 10;
        text.resizeTextMaxSize = fontSize;
        Outline outline = text.gameObject.AddComponent<Outline>();
        outline.effectColor = new Color(0f, 0f, 0f, 0.9f);
        outline.effectDistance = new Vector2(2f, -2f);
        return button;
    }

    private static Button CreateImageButton(Transform parent, string name, Sprite sprite, Color color)
    {
        GameObject buttonObject = RectObject(name, parent, typeof(Image), typeof(Button));
        Image image = buttonObject.GetComponent<Image>();
        image.sprite = sprite;
        image.color = color;
        image.raycastTarget = true;
        Button button = buttonObject.GetComponent<Button>();
        button.targetGraphic = image;
        SetButtonColors(button, sprite != null ? Color.white : color);
        return button;
    }

    private static InputField CreateInputField(Transform parent, string name, string placeholder, Font font, Sprite sprite)
    {
        GameObject inputObject = RectObject(name, parent, typeof(Image), typeof(InputField));
        Image image = inputObject.GetComponent<Image>();
        image.sprite = sprite;
        image.color = Color.white;
        image.raycastTarget = true;

        Text value = CreateText(inputObject.transform, "Value", string.Empty, font, 22, TextAnchor.MiddleLeft, Color.white);
        value.supportRichText = false;
        StretchWithInsets(value.rectTransform, 18f, 18f, 5f, 5f);
        Text hint = CreateText(inputObject.transform, "Placeholder", placeholder, font, 20, TextAnchor.MiddleLeft,
            new Color(1f, 1f, 1f, 0.42f));
        StretchWithInsets(hint.rectTransform, 18f, 18f, 5f, 5f);

        InputField input = inputObject.GetComponent<InputField>();
        input.textComponent = value;
        input.placeholder = hint;
        input.lineType = InputField.LineType.SingleLine;
        input.contentType = InputField.ContentType.Standard;
        input.characterLimit = 120;
        input.caretColor = Gold;
        input.selectionColor = new Color(0.2f, 0.55f, 1f, 0.45f);
        return input;
    }

    private static Slider CreateSlider(Transform parent, string name, string label, Font font, Vector2 position)
    {
        GameObject root = RectObject(name, parent, typeof(Slider));
        Place(root.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(820f, 100f), position);
        Text title = CreateText(root.transform, "Label", label, font, 28, TextAnchor.MiddleLeft, Cream);
        Place(title.rectTransform, new Vector2(0f, 1f), new Vector2(500f, 40f), new Vector2(250f, -22f));

        GameObject background = CreatePanel(root.transform, "Track", new Color(1f, 1f, 1f, 0.20f));
        RectTransform backgroundRect = background.GetComponent<RectTransform>();
        backgroundRect.anchorMin = new Vector2(0f, 0f);
        backgroundRect.anchorMax = new Vector2(1f, 0f);
        backgroundRect.offsetMin = new Vector2(10f, 14f);
        backgroundRect.offsetMax = new Vector2(-10f, 32f);

        GameObject fillAreaObject = RectObject("Fill Area", root.transform);
        RectTransform fillArea = fillAreaObject.GetComponent<RectTransform>();
        fillArea.anchorMin = new Vector2(0f, 0f);
        fillArea.anchorMax = new Vector2(1f, 0f);
        fillArea.offsetMin = new Vector2(10f, 14f);
        fillArea.offsetMax = new Vector2(-10f, 32f);
        GameObject fillObject = CreatePanel(fillArea, "Fill", Orange);
        RectTransform fill = fillObject.GetComponent<RectTransform>();

        GameObject handleAreaObject = RectObject("Handle Slide Area", root.transform);
        RectTransform handleArea = handleAreaObject.GetComponent<RectTransform>();
        handleArea.anchorMin = new Vector2(0f, 0f);
        handleArea.anchorMax = new Vector2(1f, 0f);
        handleArea.offsetMin = new Vector2(18f, 6f);
        handleArea.offsetMax = new Vector2(-18f, 40f);
        Image handleImage = CreateImage(handleArea, "Handle", null, Gold);
        RectTransform handle = handleImage.rectTransform;
        handle.anchorMin = handle.anchorMax = new Vector2(0f, 0.5f);
        handle.sizeDelta = new Vector2(30f, 34f);

        Slider slider = root.GetComponent<Slider>();
        slider.direction = Slider.Direction.LeftToRight;
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 0.8f;
        slider.fillRect = fill;
        slider.handleRect = handle;
        slider.targetGraphic = handleImage;
        return slider;
    }

    private static void SetButtonColors(Button button, Color normal)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = normal;
        colors.highlightedColor = Color.Lerp(normal, Color.white, 0.22f);
        colors.pressedColor = Color.Lerp(normal, Color.black, 0.22f);
        colors.selectedColor = colors.highlightedColor;
        colors.disabledColor = new Color(normal.r, normal.g, normal.b, 0.45f);
        colors.colorMultiplier = 1f;
        button.colors = colors;
    }

    private static void Link(Button button, UnityEngine.Events.UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        UnityEventTools.AddPersistentListener(button.onClick, action);
    }

    private static void Place(RectTransform rect, Vector2 anchor, Vector2 size, Vector2 position)
    {
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = size;
        rect.anchoredPosition = position;
    }

    private static void AnchorFromTop(RectTransform rect, float centerX, float centerY, float width, float height,
        float sourceWidth, float sourceHeight)
    {
        float halfWidth = width * 0.5f;
        float halfHeight = height * 0.5f;
        rect.anchorMin = new Vector2((centerX - halfWidth) / sourceWidth, 1f - (centerY + halfHeight) / sourceHeight);
        rect.anchorMax = new Vector2((centerX + halfWidth) / sourceWidth, 1f - (centerY - halfHeight) / sourceHeight);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static void StretchWithInsets(RectTransform rect, float left, float right, float bottom, float top)
    {
        Stretch(rect);
        rect.offsetMin = new Vector2(left, bottom);
        rect.offsetMax = new Vector2(-right, -top);
    }

    private static GameObject RectObject(string name, Transform parent, params Type[] components)
    {
        List<Type> types = new List<Type> { typeof(RectTransform) };
        types.AddRange(components.Where(type => type != typeof(RectTransform)));
        GameObject gameObject = new GameObject(name, types.ToArray());
        gameObject.layer = LayerMask.NameToLayer("UI");
        gameObject.transform.SetParent(parent, false);
        return gameObject;
    }

    private static Sprite LoadLargestSprite(string path)
    {
        return AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>()
            .OrderByDescending(sprite => sprite.rect.width * sprite.rect.height).FirstOrDefault();
    }

    private static GameObject NewRoot(Scene scene, string name)
    {
        GameObject root = new GameObject(name);
        SceneManager.MoveGameObjectToScene(root, scene);
        return root;
    }

    private static GameObject EnsureRoot(Scene scene, string name)
    {
        GameObject existing = scene.GetRootGameObjects().FirstOrDefault(root => root.name == name);
        return existing != null ? existing : NewRoot(scene, name);
    }

    private static GameObject EnsureChild(Transform parent, string name)
    {
        Transform existing = parent.Find(name);
        if (existing != null) return existing.gameObject;
        GameObject child = new GameObject(name);
        child.transform.SetParent(parent, false);
        return child;
    }

    private static Transform NewTransform(Transform parent, string name, Vector3 worldPosition)
    {
        GameObject child = new GameObject(name);
        child.transform.SetParent(parent, false);
        child.transform.position = worldPosition;
        return child.transform;
    }

    private static void DestroyRoot(Scene scene, string name)
    {
        GameObject root = scene.GetRootGameObjects().FirstOrDefault(candidate => candidate.name == name);
        if (root != null) UnityEngine.Object.DestroyImmediate(root);
    }

    private static void MoveRoot(Scene scene, Transform parent, string name)
    {
        GameObject root = scene.GetRootGameObjects().FirstOrDefault(candidate => candidate.name == name);
        if (root != null && root.transform != parent) root.transform.SetParent(parent, true);
    }

    private static void MoveAnywhere(Scene scene, Transform parent, string name)
    {
        GameObject target = FindAnywhere(scene, name);
        if (target != null && target.transform != parent) target.transform.SetParent(parent, true);
    }

    private static GameObject FindAnywhere(Scene scene, string name)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            Transform found = root.GetComponentsInChildren<Transform>(true).FirstOrDefault(child => child.name == name);
            if (found != null) return found.gameObject;
        }
        return null;
    }

    private static T FindComponentInScene<T>(Scene scene) where T : Component
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            T result = root.GetComponentInChildren<T>(true);
            if (result != null) return result;
        }
        return null;
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException("[ManningHierarchyBuilder] " + message);
    }

    private readonly struct TemplateSpec
    {
        public readonly ManningLaneItemKind Kind;
        public readonly string AssetName;
        public readonly float MaxVisualSize;
        public readonly Vector2 ColliderSize;
        public readonly float Weight;
        public readonly float SpeedMultiplier;

        public TemplateSpec(ManningLaneItemKind kind, string assetName, float maxVisualSize, Vector2 colliderSize,
            float weight, float speedMultiplier)
        {
            Kind = kind;
            AssetName = assetName;
            MaxVisualSize = maxVisualSize;
            ColliderSize = colliderSize;
            Weight = weight;
            SpeedMultiplier = speedMultiplier;
        }
    }
}
#endif
