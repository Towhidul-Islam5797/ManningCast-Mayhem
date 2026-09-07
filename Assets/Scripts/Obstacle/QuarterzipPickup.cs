#region Summary
/// <summary>
/// QuarterzipPickup sits on the Quarterzip and Golden Quarterzip prefabs.
/// Touching it is safe - it awards a score bonus (and a time bonus for the
/// Golden variant) and removes itself immediately, instead of waiting to
/// reach the end of the lane like a hazard would.
/// </summary>
#endregion

#region Phase 3 Sprint 4 - Quarterzip Pickup
using UnityEngine;

public class QuarterzipPickup : MonoBehaviour
{
    #region Settings
    [SerializeField] private int bonusScore;
    [SerializeField] private float bonusTimeSeconds;
    #endregion

    #region Collision Detection
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player == null) return;

        GameManager.Instance.CollectQuarterzip(bonusScore, bonusTimeSeconds);

        ObstacleMover mover = GetComponent<ObstacleMover>();
        if (mover != null)
        {
            mover.ReturnEarly();
        }
    }
    #endregion
}
#endregion