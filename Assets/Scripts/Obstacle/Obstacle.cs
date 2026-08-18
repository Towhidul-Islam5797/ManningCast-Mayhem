#region Summary
/// <summary>
/// Obstacle class is responsible for detecting collisions with the player and notifying the player of the hit.
/// </summary>
#endregion

#region Phase 1 Sprint 2 - Obstacle Implementation
//using UnityEngine;

//public class Obstacle : MonoBehaviour
//{
//    #region Collision Detection
//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        PlayerMovement player = other.GetComponent<PlayerMovement>();

//        if (player != null)
//        {
//            player.HandleObstacleHit();
//        }
//    }
//    #endregion
//}
#endregion


#region Phase 2 Sprint 4 - Obstacle Types
//using UnityEngine;

//public class Obstacle : MonoBehaviour
//{
//    #region Obstacle Type
//    public enum ObstacleType
//    {
//        Football,
//        Sandwich,
//        Flag
//    }

//    [SerializeField] private ObstacleType obstacleType;
//    [SerializeField] private int scorePenalty;
//    #endregion

//    #region Collision Detection
//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        PlayerMovement player = other.GetComponent<PlayerMovement>();

//        if (player == null) return;

//        switch (obstacleType)
//        {
//            case ObstacleType.Football:
//            case ObstacleType.Flag:
//                player.HandleObstacleHit(scorePenalty);
//                break;

//            case ObstacleType.Sandwich:
//                player.HandleSandwichHit(scorePenalty);
//                break;
//        }
//    }
//    #endregion
//}
#endregion

#region Phase 3 Sprint 3 - Obstacle Types + Time Penalty
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    #region Obstacle Type
    public enum ObstacleType
    {
        Football,
        Sandwich,
        Flag
    }

    [SerializeField] private ObstacleType obstacleType;
    [SerializeField] private int scorePenalty;
    [SerializeField] private float timePenaltySeconds;
    #endregion

    #region Collision Detection
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player == null) return;

        switch (obstacleType)
        {
            case ObstacleType.Football:
            case ObstacleType.Flag:
                player.HandleObstacleHit(scorePenalty);
                break;

            case ObstacleType.Sandwich:
                player.HandleSandwichHit(timePenaltySeconds);
                break;
        }
    }
    #endregion
}
#endregion