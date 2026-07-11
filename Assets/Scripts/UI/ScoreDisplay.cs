using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    #region UI Reference
    [SerializeField] private TMP_Text scoreText;
    #endregion

    #region Unity Lifecycle
    private void Update()
    {
        scoreText.text = "Score: " + GameManager.Instance.CurrentScore;
    }
    #endregion
}