#region Phase 1 Sprint 5 - Player Appearance
//using UnityEngine;

//public class PlayerAppearance : MonoBehaviour
//{
//    #region Placeholder Colors
//    [SerializeField] private Color peytonColor = new Color(1f, 0.5f, 0f);
//    [SerializeField] private Color eliColor = new Color(0.3f, 0.5f, 1f);
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

//        if (CharacterSelection.SelectedCharacter == CharacterSelection.Character.Peyton)
//        {
//            spriteRenderer.color = peytonColor;
//        }
//        else
//        {
//            spriteRenderer.color = eliColor;
//        }
//    }
//    #endregion
//}
#endregion

#region Phase 1 Sprint 6 - Player Appearance (Updated) 
using UnityEngine;

public class PlayerAppearance : MonoBehaviour
{
    #region Visual References
    [SerializeField] private GameObject peytonVisual;
    [SerializeField] private GameObject eliVisual;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        bool isPeyton = CharacterSelection.SelectedCharacter == CharacterSelection.Character.Peyton;

        peytonVisual.SetActive(isPeyton);
        eliVisual.SetActive(!isPeyton);
    }
    #endregion
}
#endregion