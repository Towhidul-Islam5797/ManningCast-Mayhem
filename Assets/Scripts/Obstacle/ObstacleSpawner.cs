#region Phase 1 Sprint 6 - Obstacle Spawner
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    #region Spawn Settings
    [SerializeField] private ObjectPool pool;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float laneEndX = 10f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float spawnInterval = 2f;
    #endregion

    #region Private State
    private float spawnTimer;
    #endregion

    #region Unity Lifecycle
    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnObstacle();
        }
    }
    #endregion

    #region Spawning
    private void SpawnObstacle()
    {
        GameObject obstacle = pool.Get();
        obstacle.transform.position = spawnPoint.position;

        ObstacleMover mover = obstacle.GetComponent<ObstacleMover>();
        mover.Setup(moveSpeed, laneEndX, pool);
    }
    #endregion
}
#endregion