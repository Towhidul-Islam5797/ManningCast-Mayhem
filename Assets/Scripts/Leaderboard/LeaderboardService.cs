#region Summary
/// <summary>
/// LeaderboardService sends contest entries and win scores to the Google
/// Sheets leaderboard backend. Reuses the same locally saved identity that
/// PlayerEntryPanel stores, so a returning player's score updates without
/// them re-entering their details. Lives on its own dedicated GameObject
/// and persists across scenes, same pattern as AudioManager.
/// </summary>
#endregion

#region Phase 3 Sprint 10 - Leaderboard Service
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardService : MonoBehaviour
{
    public static LeaderboardService Instance { get; private set; }

    #region Backend Settings
    [SerializeField] private string webAppUrl;
    [SerializeField] private string sharedSecret;
    #endregion

    #region Player Prefs Keys
    private const string NameKey = "Manning.Entry.Name";
    private const string EmailKey = "Manning.Entry.Email";
    private const string PhoneKey = "Manning.Entry.Phone";
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region Identity
    public bool HasSavedIdentity()
    {
        return PlayerPrefs.HasKey(NameKey) && PlayerPrefs.HasKey(EmailKey) && PlayerPrefs.HasKey(PhoneKey);
    }
    #endregion

    #region Submission
    public void SubmitContestEntry(string playerName, string email, string phone)
    {
        StartCoroutine(SubmitRoutine(playerName, email, phone, -1));
    }

    public void SubmitScore(int score)
    {
        if (!HasSavedIdentity()) return;

        string playerName = PlayerPrefs.GetString(NameKey);
        string email = PlayerPrefs.GetString(EmailKey);
        string phone = PlayerPrefs.GetString(PhoneKey);

        StartCoroutine(SubmitRoutine(playerName, email, phone, score));
    }

    private IEnumerator SubmitRoutine(string playerName, string email, string phone, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("secret", sharedSecret);
        form.AddField("name", playerName);
        form.AddField("email", email);
        form.AddField("phone", phone);

        if (score >= 0)
        {
            form.AddField("score", score);
        }

        using (UnityWebRequest request = UnityWebRequest.Post(webAppUrl, form))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("Leaderboard submission failed: " + request.error);
            }
        }
    }
    #endregion
}
#endregion