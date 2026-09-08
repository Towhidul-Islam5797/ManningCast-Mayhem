#region Summary
/// <summary>
/// SpectatorReaction controls the non-selected brother sitting at the goal,
/// showing his neutral, positive, or negative pose based on gameplay events.
/// Only the brother the player did not choose at Character Select is shown.
/// </summary>
#endregion

#region Phase 3 Sprint 9 - Spectator Reaction
using UnityEngine;

public class SpectatorReaction : MonoBehaviour
{
    public static SpectatorReaction Instance { get; private set; }

    #region Spectator Objects
    [SerializeField] private GameObject peytonSpectator;
    [SerializeField] private GameObject eliSpectator;
    #endregion

    #region Pose Sprites
    [SerializeField] private Sprite peytonNeutral;
    [SerializeField] private Sprite peytonPositive;
    [SerializeField] private Sprite peytonNegative;
    [SerializeField] private Sprite eliNeutral;
    [SerializeField] private Sprite eliPositive;
    [SerializeField] private Sprite eliNegative;
    #endregion

    #region Private State
    private SpriteRenderer activeRenderer;
    private Sprite neutralSprite;
    private Sprite positiveSprite;
    private Sprite negativeSprite;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        Instance = this;

        bool isPeyton = CharacterSelection.SelectedCharacter == CharacterSelection.Character.Peyton;

        peytonSpectator.SetActive(!isPeyton);
        eliSpectator.SetActive(isPeyton);

        if (isPeyton)
        {
            activeRenderer = eliSpectator.GetComponent<SpriteRenderer>();
            neutralSprite = eliNeutral;
            positiveSprite = eliPositive;
            negativeSprite = eliNegative;
        }
        else
        {
            activeRenderer = peytonSpectator.GetComponent<SpriteRenderer>();
            neutralSprite = peytonNeutral;
            positiveSprite = peytonPositive;
            negativeSprite = peytonNegative;
        }

        ShowNeutral();
    }
    #endregion

    #region Reactions
    public void ShowNeutral()
    {
        activeRenderer.sprite = neutralSprite;
    }

    public void ShowPositive()
    {
        activeRenderer.sprite = positiveSprite;
    }

    public void ShowNegative()
    {
        activeRenderer.sprite = negativeSprite;
    }
    #endregion
}
#endregion