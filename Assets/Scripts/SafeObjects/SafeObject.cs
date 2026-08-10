#region Summary
/// <summary>
/// SafeObject is a moving prop (couch, TV, recliner) the player can stand on.
/// While riding, the player is parented to it and moves along with it, but can
/// still steer freely. If the player is still riding when it reaches the lane's
/// end, they lose a life.
/// </summary>
#endregion

#region Phase 2 Sprint 5 - Safe Object
using UnityEngine;

public class SafeObject : MonoBehaviour
{
    #region Settings
    [SerializeField] private int edgeScorePenalty;
    #endregion

    #region Private State
    private ObstacleMover mover;
    private PlayerMovement rider;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        mover = GetComponent<ObstacleMover>();
        mover.OnReachedEnd += HandleReachedEnd;
    }

    private void OnDestroy()
    {
        mover.OnReachedEnd -= HandleReachedEnd;
    }
    #endregion

    #region Rider Handling
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player == null) return;

        rider = player;
        rider.transform.SetParent(transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player == null || player != rider) return;

        rider.transform.SetParent(null, true);
        rider = null;
    }
    #endregion

    #region Reached End
    private void HandleReachedEnd()
    {
        if (rider == null) return;

        rider.transform.SetParent(null, true);
        rider.HandleObstacleHit(edgeScorePenalty);
        rider = null;
    }
    #endregion
}
#endregion