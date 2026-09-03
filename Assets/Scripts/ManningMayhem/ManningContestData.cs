using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Safe local persistence for the prototype. It deliberately performs no network
/// submission until Omaha supplies its official endpoint, consent copy, and terms URL.
/// </summary>
public static class ManningContestData
{
    private const string NameKey = "Manning.Entry.Name";
    private const string EmailKey = "Manning.Entry.Email";
    private const string PhoneKey = "Manning.Entry.Phone";
    private const string ScoresKey = "Manning.LocalScores";
    private const string TermsUrlKey = "Manning.Config.TermsUrl";
    private const string WebsiteUrlKey = "Manning.Config.WebsiteUrl";

    [Serializable]
    private sealed class ScoreRecord
    {
        public string player;
        public int score;
        public string character;
    }

    [Serializable]
    private sealed class ScoreCollection
    {
        public List<ScoreRecord> scores = new List<ScoreRecord>();
    }

    public static string PlayerName => PlayerPrefs.GetString(NameKey, "Guest");
    public static string TermsUrl => PlayerPrefs.GetString(TermsUrlKey, string.Empty);
    public static string WebsiteUrl => PlayerPrefs.GetString(WebsiteUrlKey, "https://omahaproductions.com");

    public static void SaveEntry(string playerName, string email, string phone)
    {
        PlayerPrefs.SetString(NameKey, string.IsNullOrWhiteSpace(playerName) ? "Guest" : playerName.Trim());
        PlayerPrefs.SetString(EmailKey, email?.Trim() ?? string.Empty);
        PlayerPrefs.SetString(PhoneKey, phone?.Trim() ?? string.Empty);
        PlayerPrefs.Save();
    }

    public static void SkipEntry()
    {
        if (!PlayerPrefs.HasKey(NameKey)) PlayerPrefs.SetString(NameKey, "Guest");
        PlayerPrefs.Save();
    }

    public static void RecordScore(int score, CharacterSelection.Character character)
    {
        ScoreCollection collection = LoadScores();
        collection.scores.Add(new ScoreRecord
        {
            player = PlayerName,
            score = Mathf.Max(0, score),
            character = character.ToString()
        });
        collection.scores.Sort((left, right) => right.score.CompareTo(left.score));
        if (collection.scores.Count > 10) collection.scores.RemoveRange(10, collection.scores.Count - 10);
        PlayerPrefs.SetString(ScoresKey, JsonUtility.ToJson(collection));
        PlayerPrefs.Save();
    }

    public static string GetLeaderboardText(int count = 5)
    {
        ScoreCollection collection = LoadScores();
        if (collection.scores.Count == 0) return "No completed runs yet";

        StringBuilder builder = new StringBuilder();
        int maximum = Mathf.Min(Mathf.Max(1, count), collection.scores.Count);
        for (int i = 0; i < maximum; i++)
        {
            ScoreRecord record = collection.scores[i];
            builder.Append(i + 1).Append(". ").Append(record.player).Append("  ").Append(record.score.ToString("0000"));
            if (i < maximum - 1) builder.AppendLine();
        }
        return builder.ToString();
    }

    private static ScoreCollection LoadScores()
    {
        string json = PlayerPrefs.GetString(ScoresKey, string.Empty);
        if (string.IsNullOrWhiteSpace(json)) return new ScoreCollection();
        ScoreCollection collection = JsonUtility.FromJson<ScoreCollection>(json);
        return collection ?? new ScoreCollection();
    }
}
