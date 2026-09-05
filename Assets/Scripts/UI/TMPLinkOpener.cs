#region Summary
/// <summary>
/// TMPLinkOpener detects clicks on TextMeshPro <link> tags and opens them.
/// Used for mailto: links inside long-form text like the Terms and Conditions.
/// </summary>
#endregion

#region Phase 3 Sprint 4 - TMP Link Opener
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TMPLinkOpener : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text linkText;

    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(linkText, eventData.position, null);

        if (linkIndex == -1)
        {
            return;
        }

        TMP_LinkInfo linkInfo = linkText.textInfo.linkInfo[linkIndex];
        Application.OpenURL(linkInfo.GetLinkID());
    }
}
#endregion