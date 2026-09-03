using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// ManningCast Mayhem — Room Layout Setup Tool
/// Menu: Manning > Setup Room Layout
///
/// Places and positions all ManningCast broadcast studio props to create
/// a polished, client-ready game scene matching the ManningCast aesthetic.
///
/// HOW TO USE:
///   1. Open GameScene in Unity
///   2. Go to top menu: Manning > Setup Room Layout
///   3. Save the scene (Ctrl+S)
///
/// What it does:
///   - Creates a "StudioDecor" child under Environments with all studio props
///   - Enables and fixes the SOFA -98 object (was disabled with wrong sprite)
///   - Sets warm studio lighting
///
/// COORDINATE REFERENCE:
///   Environments parent is at world (-6.006, -3.595)
///   All localPosition values are relative to Environments.
///   Camera: orthographic size 4.7 at world (2.16, 1.54)
///   Visible local Y range: approx 0.4 to 9.8
///   Lane 1 (bottom) = local Y 0.545, Lane 7 (top) = local Y 6.545
/// </summary>
public static class RoomLayoutSetup
{
    [MenuItem("Manning/Setup Room Layout")]
    public static void SetupRoomLayout()
    {
        GameObject environmentsGO = GameObject.Find("Environments");
        if (environmentsGO == null)
        {
            Debug.LogError("[RoomLayoutSetup] 'Environments' not found. Make sure GameScene is open.");
            return;
        }

        Transform env = environmentsGO.transform;

        // ---- Fix the disabled SOFA object ----
        FixSofaObject(env);

        // ---- Remove old StudioDecor (for re-runs) ----
        Transform oldDecor = env.Find("StudioDecor");
        if (oldDecor != null)
            Undo.DestroyObjectImmediate(oldDecor.gameObject);

        // ---- Create StudioDecor parent ----
        GameObject decorGO = new GameObject("StudioDecor");
        Undo.RegisterCreatedObjectUndo(decorGO, "Setup Room Layout");
        decorGO.transform.SetParent(env, false);
        decorGO.transform.localPosition = Vector3.zero;
        Transform decor = decorGO.transform;

        // ---- Load sprites ----
        string mainPath = "Assets/Imported/Environment/LocationMAnningCast.png";
        var sprites = LoadSprites(mainPath);
        Debug.Log($"[RoomLayoutSetup] Loaded {sprites.Count} sprites from LocationMAnningCast.png");

        // ---- Place studio props ----
        // Format: spriteName, localX, localY, scaleX, scaleY, sortingOrder, objectName
        //
        // SPRITE GUIDE (LocationMAnningCast.png):
        //   _0  Manning bros posed photo (wide frame)     _1  Eli eating photo (wide frame)
        //   _2  Peyton pointing photo (wide frame)        _3  Cartoon Mannings (wide frame)
        //   _4  Eli Most Punchable Face (tall cover)      _5  Football field TV screen (wide)
        //   _6  Donkey/mascot photo (square frame)        _7  PLAYBILL yellow theater poster
        //   _8  Brown studio sofa/couch (2-seat)          _9  Brown recliner (single seat)
        //   _10 Audio equalizer bars (colored)            _11 Therese figure
        //   _13 MNF neon "MONDAY NIGHT FOOTBALL" (gold)   _14 MNF neon (red alt)
        //   _15 Studio spotlight lamp (small, angled)     _16 Red buzzer button
        //   _17 Broncos helmet                            _18 Colts helmet
        //   _19 Giants helmet                             _20 Large green plant (tall)
        //   _21 Small green plant (compact)               _22 Dark/purple plant (tall)
        //   _23 Dog bowl "PEYTON"                         _24 Football (brown)
        //   _25 Full bookcase (wood shelves, left style)  _29 Stacked books (blue row)
        //   _30 Stacked books (red/multi row)             _31 Tall stacked books (blue)
        //   _32 Tall stacked books (red)                  _33 Wide books (blue/white row)
        //   _34 Wide books (orange/yellow row)            _35 Shelf board (horizontal plank)
        //   _37 Black video wall (wide monitor)           _38 Video wall red stripe
        //   _40 ManningCast marquee - Eli version         _41 ManningCast marquee - Peyton version
        //   _42 "ONE NIGHT ONLY Sep 3 2024" sign         _44 Light wood wall panel
        //   _52 Broadcast camera on tripod               _53 Christmas tree (full, with balls)
        //   _54 Whiteboard/canvas easel                   _55 Large bookcase (alternate style)
        //   _56 ManningCast smaller - Peyton             _57 ManningCast smaller - Eli
        //   _58 Vintage jukebox/record player             _59 Emmy Award statue (gold)
        //   _60 Slim Christmas tree (tall)

        var layout = new (string s, float lx, float ly, float sx, float sy, int order, string name)[]
        {
            // ─────────────────────────────────────────────────────────────
            // BACK WALL (local Y 7.5–9.8) — visible at camera top
            // World Y equivalent: ~3.9 to ~6.2 (top of visible area)
            // ─────────────────────────────────────────────────────────────

            // Large video wall / broadcast TV screen (center-left, dominant)
            ("LocationMAnningCast_37", 3.5f,  8.8f, 5.5f, 2.0f, -100, "VideoWall_Screen"),
            ("LocationMAnningCast_38", 3.5f,  8.0f, 5.5f, 0.9f, -100, "VideoWall_RedStripe"),

            // Monday Night Football neon signs (iconic, above video wall)
            ("LocationMAnningCast_13", 0.8f,  9.5f, 3.5f, 1.8f, -98, "MNF_Sign_Gold"),
            ("LocationMAnningCast_14", 4.8f,  9.5f, 3.5f, 1.8f, -98, "MNF_Sign_Red"),

            // ManningCast The Musical marquees (right side)
            ("LocationMAnningCast_40", 10.0f, 9.5f, 3.8f, 2.8f, -98, "Marquee_ManningCast_Eli"),
            ("LocationMAnningCast_41", 14.0f, 9.5f, 3.8f, 2.8f, -98, "Marquee_ManningCast_Peyton"),

            // "ONE NIGHT ONLY" event sign (under marquee)
            ("LocationMAnningCast_42", 13.0f, 8.0f, 3.0f, 1.2f, -97, "Sign_OneNightOnly"),

            // PLAYBILL theater poster (far right wall)
            ("LocationMAnningCast_7",  16.0f, 9.0f, 1.8f, 2.8f, -98, "Poster_Playbill"),

            // Wall photo frames (left side of back wall)
            ("LocationMAnningCast_0",  0.3f,  8.8f, 2.5f, 1.8f, -98, "Photo_ManningsTogether"),
            ("LocationMAnningCast_1",  3.0f,  8.8f, 2.5f, 1.8f, -98, "Photo_Eli_Candid"),

            // Center wall photos
            ("LocationMAnningCast_5",  7.0f,  8.2f, 2.8f, 2.2f, -98, "Photo_FootballField_Screen"),
            ("LocationMAnningCast_4",  9.5f,  8.5f, 2.0f, 3.0f, -97, "Photo_Eli_PunchableFace"),
            ("LocationMAnningCast_6",  9.0f,  9.5f, 2.2f, 2.2f, -98, "Photo_Donkey"),

            // Studio spotlights on back wall
            ("LocationMAnningCast_15", 0.0f,  9.2f, 1.5f, 2.2f, -97, "StudioLight_FarLeft"),
            ("LocationMAnningCast_15", 8.5f,  9.0f, 1.5f, 2.2f, -97, "StudioLight_Center"),

            // ─────────────────────────────────────────────────────────────
            // COUCH SAFE ZONE (local Y 6.8–7.5) — just above Lane 7
            // This is the ManningCast broadcast desk / couch area
            // ─────────────────────────────────────────────────────────────

            // Left studio sofa (large, main seating)
            ("LocationMAnningCast_8",  0.5f,  7.2f, 4.2f, 2.8f, -97, "StudioSofa_Left"),
            // Left recliner (next to main sofa)
            ("LocationMAnningCast_9",  4.8f,  7.1f, 2.5f, 2.2f, -97, "StudioRecliner_Left"),

            // Right studio sofa (mirrored)
            ("LocationMAnningCast_8",  12.0f, 7.2f, 4.2f, 2.8f, -97, "StudioSofa_Right"),
            // Right recliner
            ("LocationMAnningCast_9",  10.0f, 7.1f, 2.5f, 2.2f, -97, "StudioRecliner_Right"),

            // Audio equalizer (broadcast desk center piece)
            ("LocationMAnningCast_10", 7.0f,  7.5f, 3.0f, 1.5f, -97, "AudioEqualizer_Panel"),

            // NFL Helmets displayed on shelf above couches
            ("LocationMAnningCast_17", 5.4f,  7.7f, 1.1f, 1.1f, -96, "Helmet_Broncos"),
            ("LocationMAnningCast_18", 6.7f,  7.7f, 1.1f, 1.1f, -96, "Helmet_Colts"),
            ("LocationMAnningCast_19", 8.0f,  7.7f, 1.1f, 1.1f, -96, "Helmet_Giants"),

            // Plants (studio decor flanking the couch zone)
            ("LocationMAnningCast_20", -0.3f, 7.3f, 1.5f, 2.2f, -96, "Plant_Large_Left"),
            ("LocationMAnningCast_22", 9.5f,  7.0f, 1.2f, 1.8f, -96, "Plant_Dark_Center"),
            ("LocationMAnningCast_21", 16.0f, 7.2f, 1.2f, 1.8f, -96, "Plant_Small_Right"),

            // Dog bowl (Peyton's brand)
            ("LocationMAnningCast_23", 9.0f,  7.0f, 1.3f, 1.0f, -96, "DogBowl_Peyton"),

            // Emmy Award statue (prominent trophy, right of center)
            ("LocationMAnningCast_59", 13.0f, 7.5f, 1.8f, 3.5f, -95, "Emmy_Award"),

            // Christmas tree decoration (far right corner, festive)
            ("LocationMAnningCast_53", 15.8f, 6.8f, 2.5f, 5.0f, -96, "ChristmasTree_Full"),

            // ─────────────────────────────────────────────────────────────
            // BOOKSHELVES — flanking left and right walls
            // Partially visible during gameplay (mid-screen)
            // ─────────────────────────────────────────────────────────────

            // Left wall bookcase (tall, spans most of left side)
            ("LocationMAnningCast_25", -0.5f, 4.0f, 2.0f, 4.5f, -97, "Bookcase_Left_Main"),

            // Books on left bookcase shelves
            ("LocationMAnningCast_33", 0.5f,  6.1f, 2.0f, 0.7f, -96, "Books_BlueWhite_ShelfTop"),
            ("LocationMAnningCast_34", 0.5f,  5.4f, 2.0f, 0.7f, -96, "Books_Orange_ShelfMid"),
            ("LocationMAnningCast_29", 0.5f,  4.7f, 1.5f, 0.7f, -96, "Books_Blue_ShelfLow"),
            ("LocationMAnningCast_30", 2.1f,  4.7f, 1.5f, 0.7f, -96, "Books_Red_ShelfLow"),

            // Right wall bookcase
            ("LocationMAnningCast_55", 15.8f, 4.0f, 2.0f, 4.5f, -97, "Bookcase_Right_Main"),

            // Books on right bookcase
            ("LocationMAnningCast_31", 14.5f, 6.1f, 1.5f, 1.0f, -96, "Books_TallBlue_Right"),
            ("LocationMAnningCast_32", 16.0f, 6.1f, 1.5f, 1.0f, -96, "Books_TallRed_Right"),
            ("LocationMAnningCast_34", 14.5f, 5.2f, 2.2f, 0.7f, -96, "Books_Orange_Right"),

            // ─────────────────────────────────────────────────────────────
            // STUDIO EQUIPMENT (lower area, local Y 2–5)
            // Visible during gameplay in the side areas
            // ─────────────────────────────────────────────────────────────

            // Broadcast camera on tripod (left side, production element)
            ("LocationMAnningCast_52", 0.8f,  3.5f, 2.2f, 3.5f, -96, "BroadcastCamera_Tripod"),

            // Whiteboard / drawing easel (right side)
            ("LocationMAnningCast_54", 15.5f, 3.5f, 2.2f, 3.5f, -96, "Whiteboard_Easel"),

            // Vintage jukebox (right area near bookcase, nostalgic decor)
            ("LocationMAnningCast_58", 14.0f, 2.0f, 2.0f, 3.0f, -96, "Jukebox_Vintage"),
        };

        int created = 0, skipped = 0;
        foreach (var (spriteName, lx, ly, sx, sy, order, label) in layout)
        {
            if (!sprites.TryGetValue(spriteName, out Sprite sprite))
            {
                Debug.LogWarning($"[RoomLayoutSetup] '{spriteName}' not found — skipping '{label}'");
                skipped++;
                continue;
            }
            PlaceProp(decor, label, sprite, lx, ly, sx, sy, order);
            created++;
        }

        // ---- Warm studio lighting ----
        SetStudioLighting();

        // ---- Mark scene dirty ----
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log($"[RoomLayoutSetup] ✅ Complete! Created: {created} | Skipped: {skipped}");
        Debug.Log("[RoomLayoutSetup] Save scene with Ctrl+S to persist.");
    }

    static void FixSofaObject(Transform env)
    {
        // The SOFA -98 object exists in the scene hierarchy but was inactive (m_IsActive: 0).
        // Enable it and ensure its sprite and renderer are set properly.
        Transform sofaTransform = env.Find("SOFA -98");
        if (sofaTransform == null) return;

        // Load the couch sprite (LocationMAnningCast_8)
        string mainPath = "Assets/Imported/Environment/LocationMAnningCast.png";
        var sprites = LoadSprites(mainPath);
        if (!sprites.TryGetValue("LocationMAnningCast_8", out Sprite couchSprite)) return;

        GameObject sofaGO = sofaTransform.gameObject;
        Undo.RecordObject(sofaGO, "Enable SOFA");

        // Enable the object
        sofaGO.SetActive(true);

        // Fix the sprite
        var sr = sofaGO.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Undo.RecordObject(sr, "Fix SOFA sprite");
            sr.sprite = couchSprite;
            EditorUtility.SetDirty(sr);
        }

        Debug.Log("[RoomLayoutSetup] Fixed and enabled SOFA -98 object.");
    }

    static void SetStudioLighting()
    {
        var globalLight = GameObject.Find("Global Light 2D");
        if (globalLight == null) return;
        var light2D = globalLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        if (light2D == null) return;
        Undo.RecordObject(light2D, "Studio Warm Light");
        light2D.color = new Color(1.0f, 0.96f, 0.88f, 1f);
        light2D.intensity = 1.1f;
        EditorUtility.SetDirty(light2D);
    }

    static void PlaceProp(Transform parent, string name, Sprite sprite,
                           float lx, float ly, float sx, float sy, int sortOrder)
    {
        var go = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(go, $"Place {name}");
        go.transform.SetParent(parent, false);
        go.transform.localPosition = new Vector3(lx, ly, 0f);
        go.transform.localScale = new Vector3(sx, sy, 1f);
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = sortOrder;
    }

    static Dictionary<string, Sprite> LoadSprites(string assetPath)
    {
        var dict = new Dictionary<string, Sprite>();
        foreach (var obj in AssetDatabase.LoadAllAssetsAtPath(assetPath))
            if (obj is Sprite sp) dict[sp.name] = sp;
        return dict;
    }
}
