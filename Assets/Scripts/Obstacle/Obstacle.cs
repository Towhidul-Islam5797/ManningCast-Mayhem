using UnityEngine;

/// <summary>Compatibility behavior for any legacy obstacle prefab left in the project.</summary>
public sealed class Obstacle : MonoBehaviour
{
    public enum ObstacleType { Football, Sandwich, Flag }

    [SerializeField] private ObstacleType obstacleType;
    [SerializeField] private int scorePenalty = 50;
    [SerializeField] private float timePenaltySeconds = 8f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player == null || GameManager.Instance == null) return;

        switch (obstacleType)
        {
            case ObstacleType.Football:
                GameManager.Instance.AddFootball();
                gameObject.SetActive(false);
                break;
            case ObstacleType.Sandwich:
                player.HandleSandwichHit(timePenaltySeconds);
                gameObject.SetActive(false);
                break;
            case ObstacleType.Flag:
                player.HandleObstacleHit(GameManager.LifeLossReason.ChallengeFlag, scorePenalty);
                break;
        }
    }
}
