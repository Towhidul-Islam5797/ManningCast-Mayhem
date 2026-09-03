using UnityEngine;

/// <summary>Activates only the character selected on the preceding screen.</summary>
public sealed class PlayerAppearance : MonoBehaviour
{
    [SerializeField] private GameObject peytonVisual;
    [SerializeField] private GameObject eliVisual;

    private void Awake()
    {
        bool isPeyton = CharacterSelection.SelectedCharacter == CharacterSelection.Character.Peyton;
        if (peytonVisual != null) peytonVisual.SetActive(isPeyton);
        if (eliVisual != null) eliVisual.SetActive(!isPeyton);
    }
}
