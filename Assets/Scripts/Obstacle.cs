#region Summary
/// <summary>
/// Obstacle class is responsible for detecting collisions with the player and notifying the player of the hit.
/// </summary>
#endregion

#region Phase 1 Sprint 2 - Obstacle Implementation
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    #region Collision Detection
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player != null)
        {
            player.HandleObstacleHit();
        }
    }
    #endregion
}
#endregion