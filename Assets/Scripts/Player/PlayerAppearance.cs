using UnityEngine;

public class PlayerAppearance : MonoBehaviour
{
    #region Placeholder Colors
    [SerializeField] private Color peytonColor = new Color(1f, 0.5f, 0f);
    [SerializeField] private Color eliColor = new Color(0.3f, 0.5f, 1f);
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (CharacterSelection.SelectedCharacter == CharacterSelection.Character.Peyton)
        {
            spriteRenderer.color = peytonColor;
        }
        else
        {
            spriteRenderer.color = eliColor;
        }
    }
    #endregion
}