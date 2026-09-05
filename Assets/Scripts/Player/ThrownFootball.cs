#region Summary
/// <summary>
/// ThrownFootball is the projectile spawned when the player throws a collected
/// football. Travels in a fixed direction and destroys any GameObject tagged
/// "Athlete" it touches. Self-destructs after a set lifetime so it never
/// flies forever if it misses everything.
/// </summary>
#endregion

#region Phase 3 Sprint 5 - Thrown Football
using UnityEngine;

public class ThrownFootball : MonoBehaviour
{
    #region Settings
    [SerializeField] private float lifetimeSeconds = 2f;
    #endregion

    #region Travel State
    private Vector2 travelDirection;
    private float travelSpeed;
    #endregion

    #region Setup
    public void Launch(Vector2 direction, float speed)
    {
        travelDirection = direction.normalized;
        travelSpeed = speed;
        Destroy(gameObject, lifetimeSeconds);
    }
    #endregion

    #region Unity Lifecycle
    private void Update()
    {
        transform.position += (Vector3)(travelDirection * travelSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Athlete"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
    #endregion
}
#endregion