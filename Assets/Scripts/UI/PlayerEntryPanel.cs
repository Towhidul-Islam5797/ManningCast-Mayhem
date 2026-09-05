#region Summary
/// <summary>
/// PlayerEntryPanel controls the LoginPanel (name/email/phone entry) shown before
/// Character Select. Entry is optional - Skip proceeds without saving details.
/// Data is stored locally only until Omaha confirms where contest entries should go.
/// </summary>
#endregion

#region Phase 3 Sprint 4 - Player Entry Panel
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerEntryPanel : MonoBehaviour
{
    #region Scene Settings
    [SerializeField] private string characterSelectSceneName = "CharacterSelectScene";
    #endregion

    #region Field References
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField phoneField;
    [SerializeField] private TMP_Text validationText;
    #endregion

    #region Player Prefs Keys
    private const string NameKey = "Manning.Entry.Name";
    private const string EmailKey = "Manning.Entry.Email";
    private const string PhoneKey = "Manning.Entry.Phone";
    #endregion

    #region Unity Lifecycle
    private void OnEnable()
    {
        if (validationText != null)
        {
            validationText.text = string.Empty;
        }
    }
    #endregion

    #region Button Actions
    public void Submit()
    {
        string playerName = nameField != null ? nameField.text.Trim() : string.Empty;
        string email = emailField != null ? emailField.text.Trim() : string.Empty;
        string phone = phoneField != null ? phoneField.text.Trim() : string.Empty;

        if (!IsValid(playerName, email, phone))
        {
            return;
        }

        PlayerPrefs.SetString(NameKey, playerName);
        PlayerPrefs.SetString(EmailKey, email);
        PlayerPrefs.SetString(PhoneKey, phone);
        PlayerPrefs.Save();

        SceneManager.LoadScene(characterSelectSceneName);
    }

    // Dev/testing convenience only - lets us skip past entry while building the game.
    // Confirm with the client whether this should remain in the shipped build.
    public void Skip()
    {
        SceneManager.LoadScene(characterSelectSceneName);
    }
    #endregion

    #region Validation
    private bool IsValid(string playerName, string email, string phone)
    {
        if (string.IsNullOrEmpty(playerName))
        {
            ShowValidationError("Please enter your name.");
            return false;
        }

        if (!IsValidEmail(email))
        {
            ShowValidationError("Please enter a valid email address.");
            return false;
        }

        if (string.IsNullOrEmpty(phone))
        {
            ShowValidationError("Please enter your phone number.");
            return false;
        }

        return true;
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }

        int atIndex = email.IndexOf('@');
        int lastDotIndex = email.LastIndexOf('.');
        return atIndex > 0 && lastDotIndex > atIndex + 1 && lastDotIndex < email.Length - 1;
    }

    private void ShowValidationError(string message)
    {
        if (validationText != null)
        {
            validationText.text = message;
        }
    }
    #endregion
}
#endregion