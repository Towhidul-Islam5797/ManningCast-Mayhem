#region Summary
/// <summary>
/// FootballPickup sits on the collectible football prefab. Touching it is
/// safe - it adds one football charge and removes itself immediately,
/// instead of waiting to reach the end of the lane like a hazard would.
/// </summary>
#endregion

#region Phase 3 Sprint 5 - Football Pickup
//using UnityEngine;

//public class FootballPickup : MonoBehaviour
//{
//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        PlayerMovement player = other.GetComponent<PlayerMovement>();
//        if (player == null) return;

//        GameManager.Instance.CollectFootball();

//        ObstacleMover mover = GetComponent<ObstacleMover>();
//        if (mover != null)
//        {
//            mover.ReturnEarly();
//        }
//    }
//}
#endregion

#region Phase 3 Sprint 6 - Football Pickup (Collected State)
using UnityEngine;

public class FootballPickup : MonoBehaviour
{
    #region Collected State
    private bool collected;

    private void OnEnable()
    {
        collected = false;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player == null) return;

        collected = true;

        GameManager.Instance.CollectFootball();

        ObstacleMover mover = GetComponent<ObstacleMover>();
        if (mover != null)
        {
            mover.ReturnEarly();
        }
    }
}
#endregion