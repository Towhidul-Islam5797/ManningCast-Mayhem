#region Summary
/// <summary>
/// GameManager class is responsible for managing the game state, including player lives and game over conditions.
/// </summary>
#endregion

#region Phase 1 Sprint 3 - GameManager Implementation with Game State
using UnityEngine;

public class GoalArea : MonoBehaviour
{
    #region Collision Detection
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player != null)
        {
            player.HandleGoalReached();
        }
    }
    #endregion
}
#endregion