#region Phase 1 Sprint 6 - Obstacle Spawner
//using UnityEngine;

//public class ObstacleSpawner : MonoBehaviour
//{
//    #region Spawn Settings
//    [SerializeField] private ObjectPool pool;
//    [SerializeField] private Transform spawnPoint;
//    [SerializeField] private float laneEndX = 10f;
//    [SerializeField] private float moveSpeed = 2f;
//    [SerializeField] private float spawnInterval = 2f;
//    #endregion

//    #region Private State
//    private float spawnTimer;
//    #endregion

//    #region Unity Lifecycle
//    private void Update()
//    {
//        spawnTimer += Time.deltaTime;

//        if (spawnTimer >= spawnInterval)
//        {
//            spawnTimer = 0f;
//            SpawnObstacle();
//        }
//    }
//    #endregion

//    #region Spawning
//    private void SpawnObstacle()
//    {
//        GameObject obstacle = pool.Get();
//        obstacle.transform.position = spawnPoint.position;

//        ObstacleMover mover = obstacle.GetComponent<ObstacleMover>();
//        mover.Setup(moveSpeed, laneEndX, pool);
//    }
//    #endregion
//}
#endregion


#region Phase 2 Sprint 1 - Obstacle Spawner
//using System.Collections;
//using UnityEngine;

//public class ObstacleSpawner : MonoBehaviour
//{
//    #region Spawn Settings
//    [SerializeField] private ObjectPool pool;
//    [SerializeField] private Transform spawnPoint;
//    [SerializeField] private float laneEndX = 10f;
//    [SerializeField] private float moveSpeed = 2f;
//    [SerializeField] private int obstaclesPerBurst = 1;
//    [SerializeField] private float delayBetweenObstaclesInBurst = 0.3f;
//    [SerializeField] private float gapBetweenBursts = 2f;
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        StartCoroutine(SpawnPatternLoop());
//    }
//    #endregion

//    #region Spawning
//    private IEnumerator SpawnPatternLoop()
//    {
//        while (true)
//        {
//            for (int i = 0; i < obstaclesPerBurst; i++)
//            {
//                SpawnObstacle();
//                yield return new WaitForSeconds(delayBetweenObstaclesInBurst);
//            }

//            yield return new WaitForSeconds(gapBetweenBursts);
//        }
//    }

//    private void SpawnObstacle()
//    {
//        GameObject obstacle = pool.Get();
//        obstacle.transform.position = spawnPoint.position;

//        ObstacleMover mover = obstacle.GetComponent<ObstacleMover>();
//        mover.Setup(moveSpeed, laneEndX, pool);
//    }
//    #endregion
//}
#endregion


#region Phase 3 Sprint 1 - Obstacle Spawner (Mixed Pool) v1
//using System.Collections;
//using UnityEngine;

//public class ObstacleSpawner : MonoBehaviour
//{
//    #region Spawn Settings
//    [SerializeField] private ObjectPool[] pools;
//    [SerializeField] private Transform spawnPoint;
//    [SerializeField] private float laneEndX = 10f;
//    [SerializeField] private float moveSpeed = 2f;
//    [SerializeField] private int obstaclesPerBurst = 1;
//    [SerializeField] private float delayBetweenObstaclesInBurst = 0.3f;
//    [SerializeField] private float gapBetweenBursts = 2f;
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        StartCoroutine(SpawnPatternLoop());
//    }
//    #endregion

//    #region Spawning
//    private IEnumerator SpawnPatternLoop()
//    {
//        while (true)
//        {
//            for (int i = 0; i < obstaclesPerBurst; i++)
//            {
//                SpawnObstacle();
//                yield return new WaitForSeconds(delayBetweenObstaclesInBurst);
//            }

//            yield return new WaitForSeconds(gapBetweenBursts);
//        }
//    }

//    private void SpawnObstacle()
//    {
//        ObjectPool pool = pools[Random.Range(0, pools.Length)];

//        GameObject obstacle = pool.Get();
//        obstacle.transform.position = spawnPoint.position;

//        ObstacleMover mover = obstacle.GetComponent<ObstacleMover>();
//        mover.Setup(moveSpeed, laneEndX, pool);
//    }
//    #endregion
//}
#endregion

#region Phase 3 Sprint 1 - Obstacle Spawner (Independent Bursts, No Overlap) v2
//using System.Collections;
//using UnityEngine;

//public class ObstacleSpawner : MonoBehaviour
//{
//    #region Spawn Entry
//    [System.Serializable]
//    private class SpawnEntry
//    {
//        public ObjectPool pool;
//        public int obstaclesPerBurst = 1;
//        public float delayBetweenObstaclesInBurst = 0.3f;
//        public float gapBetweenBursts = 2f;
//    }
//    #endregion

//    #region Spawn Settings
//    [SerializeField] private SpawnEntry[] spawnEntries;
//    [SerializeField] private Transform spawnPoint;
//    [SerializeField] private float laneEndX = 10f;
//    [SerializeField] private float moveSpeed = 2f;
//    [SerializeField] private float minimumSpawnGap = 0.5f;
//    #endregion

//    #region Private State
//    private float lastSpawnTime = -Mathf.Infinity;
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        foreach (SpawnEntry entry in spawnEntries)
//        {
//            StartCoroutine(SpawnEntryLoop(entry));
//        }
//    }
//    #endregion

//    #region Spawning
//    private IEnumerator SpawnEntryLoop(SpawnEntry entry)
//    {
//        while (true)
//        {
//            for (int i = 0; i < entry.obstaclesPerBurst; i++)
//            {
//                yield return StartCoroutine(WaitForSpawnSlot());
//                SpawnObstacle(entry.pool);
//                yield return new WaitForSeconds(entry.delayBetweenObstaclesInBurst);
//            }

//            yield return new WaitForSeconds(entry.gapBetweenBursts);
//        }
//    }

//    private IEnumerator WaitForSpawnSlot()
//    {
//        float timeSinceLastSpawn = Time.time - lastSpawnTime;

//        if (timeSinceLastSpawn < minimumSpawnGap)
//        {
//            yield return new WaitForSeconds(minimumSpawnGap - timeSinceLastSpawn);
//        }
//    }

//    private void SpawnObstacle(ObjectPool pool)
//    {
//        lastSpawnTime = Time.time;

//        GameObject obstacle = pool.Get();
//        obstacle.transform.position = spawnPoint.position;

//        ObstacleMover mover = obstacle.GetComponent<ObstacleMover>();
//        mover.Setup(moveSpeed, laneEndX, pool);
//    }
//    #endregion
//}
//using System.Collections;
//using UnityEngine;

//public class ObstacleSpawner : MonoBehaviour
//{
//    #region Spawn Entry
//    [System.Serializable]
//    private class SpawnEntry
//    {
//        public ObjectPool pool;
//        public int obstaclesPerBurst = 1;
//        public float delayBetweenObstaclesInBurst = 0.3f;
//        public float gapBetweenBursts = 2f;
//    }
//    #endregion

//    #region Spawn Settings
//    [SerializeField] private SpawnEntry[] spawnEntries;
//    [SerializeField] private Transform spawnPoint;
//    [SerializeField] private float laneEndX = 10f;
//    [SerializeField] private float moveSpeed = 2f;
//    [SerializeField] private float minimumSpawnGap = 0.5f;
//    #endregion

//    #region Private State
//    private float lastSpawnTime = -Mathf.Infinity;
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        foreach (SpawnEntry entry in spawnEntries)
//        {
//            StartCoroutine(SpawnEntryLoop(entry));
//        }
//    }
//    #endregion

//    #region Spawning
//    private IEnumerator SpawnEntryLoop(SpawnEntry entry)
//    {
//        while (true)
//        {
//            for (int i = 0; i < entry.obstaclesPerBurst; i++)
//            {
//                yield return StartCoroutine(WaitForSpawnSlot());
//                SpawnObstacle(entry.pool);
//                yield return new WaitForSeconds(entry.delayBetweenObstaclesInBurst);
//            }

//            yield return new WaitForSeconds(entry.gapBetweenBursts);
//        }
//    }

//    private IEnumerator WaitForSpawnSlot()
//    {
//        float timeSinceLastSpawn = Time.time - lastSpawnTime;

//        if (timeSinceLastSpawn < minimumSpawnGap)
//        {
//            yield return new WaitForSeconds(minimumSpawnGap - timeSinceLastSpawn);
//        }
//    }

//    private void SpawnObstacle(ObjectPool pool)
//    {
//        lastSpawnTime = Time.time;

//        GameObject obstacle = pool.Get();
//        obstacle.transform.position = spawnPoint.position;

//        ObstacleMover mover = obstacle.GetComponent<ObstacleMover>();
//        mover.Setup(moveSpeed, laneEndX, pool);
//    }
//    #endregion
//}
#endregion

#region Phase 3 Sprint 1 - Obstacle Spawner (Independent Bursts, Collider-Checked Clearance) v3
//using System.Collections;
//using UnityEngine;

//public class ObstacleSpawner : MonoBehaviour
//{
//    #region Spawn Entry
//    [System.Serializable]
//    private class SpawnEntry
//    {
//        public ObjectPool pool;
//        public int obstaclesPerBurst = 1;
//        public float delayBetweenObstaclesInBurst = 0.3f;
//        public float gapBetweenBursts = 2f;
//    }
//    #endregion

//    #region Spawn Settings
//    [SerializeField] private SpawnEntry[] spawnEntries;
//    [SerializeField] private Transform spawnPoint;
//    [SerializeField] private float laneEndX = 10f;
//    [SerializeField] private float moveSpeed = 2f;
//    #endregion

//    #region Clearance Check
//    [SerializeField] private LayerMask laneItemsLayer;
//    [SerializeField] private Vector2 spawnCheckSize = new Vector2(1f, 1f);
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        foreach (SpawnEntry entry in spawnEntries)
//        {
//            StartCoroutine(SpawnEntryLoop(entry));
//        }
//    }
//    #endregion

//    #region Spawning
//    private IEnumerator SpawnEntryLoop(SpawnEntry entry)
//    {
//        while (true)
//        {
//            for (int i = 0; i < entry.obstaclesPerBurst; i++)
//            {
//                yield return StartCoroutine(WaitForClearSpawnPoint());
//                SpawnObstacle(entry.pool);
//                yield return new WaitForSeconds(entry.delayBetweenObstaclesInBurst);
//            }

//            yield return new WaitForSeconds(entry.gapBetweenBursts);
//        }
//    }

//    private IEnumerator WaitForClearSpawnPoint()
//    {
//        while (Physics2D.OverlapBox(spawnPoint.position, spawnCheckSize, 0f, laneItemsLayer))
//        {
//            yield return null;
//        }
//    }

//    private void SpawnObstacle(ObjectPool pool)
//    {
//        GameObject obstacle = pool.Get();
//        obstacle.transform.position = spawnPoint.position;

//        ObstacleMover mover = obstacle.GetComponent<ObstacleMover>();
//        mover.Setup(moveSpeed, laneEndX, pool);
//    }
//    #endregion
//}
#endregion

#region Phase 3 Sprint 1 - Obstacle Spawner (Independent Bursts, Collider-Checked Clearance) V4
//using System.Collections;
//using UnityEngine;

//public class ObstacleSpawner : MonoBehaviour
//{
//    #region Spawn Entry
//    [System.Serializable]
//    private class SpawnEntry
//    {
//        public ObjectPool pool;
//        public int obstaclesPerBurst = 1;
//        public float delayBetweenObstaclesInBurst = 0.3f;
//        public float gapBetweenBursts = 2f;
//        public Vector2 spawnCheckSize = new Vector2(1f, 1f);
//    }
//    #endregion

//    #region Spawn Settings
//    [SerializeField] private SpawnEntry[] spawnEntries;
//    [SerializeField] private Transform spawnPoint;
//    [SerializeField] private float laneEndX = 10f;
//    [SerializeField] private float moveSpeed = 2f;
//    #endregion

//    #region Clearance Check
//    [SerializeField] private LayerMask laneItemsLayer;
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        foreach (SpawnEntry entry in spawnEntries)
//        {
//            StartCoroutine(SpawnEntryLoop(entry));
//        }
//    }
//    #endregion

//    #region Spawning
//    private IEnumerator SpawnEntryLoop(SpawnEntry entry)
//    {
//        while (true)
//        {
//            for (int i = 0; i < entry.obstaclesPerBurst; i++)
//            {
//                yield return StartCoroutine(WaitForClearSpawnPoint(entry.spawnCheckSize));
//                SpawnObstacle(entry.pool);
//                yield return new WaitForSeconds(entry.delayBetweenObstaclesInBurst);
//            }

//            yield return new WaitForSeconds(entry.gapBetweenBursts);
//        }
//    }

//    private IEnumerator WaitForClearSpawnPoint(Vector2 checkSize)
//    {
//        while (Physics2D.OverlapBox(spawnPoint.position, checkSize, 0f, laneItemsLayer))
//        {
//            yield return null;
//        }
//    }

//    private void SpawnObstacle(ObjectPool pool)
//    {
//        GameObject obstacle = pool.Get();
//        obstacle.transform.position = spawnPoint.position;

//        ObstacleMover mover = obstacle.GetComponent<ObstacleMover>();
//        mover.Setup(moveSpeed, laneEndX, pool);
//    }
//    #endregion
//}
#endregion

#region Phase 3 Sprint 1 - Obstacle Spawner (Independent Bursts, Collider-Checked Clearance) V5
//using System.Collections;
//using UnityEngine;

//public class ObstacleSpawner : MonoBehaviour
//{
//    #region Spawn Entry
//    [System.Serializable]
//    private class SpawnEntry
//    {
//        public ObjectPool pool;
//        public int obstaclesPerBurst = 1;
//        public float delayBetweenObstaclesInBurst = 0.3f;
//        public float gapBetweenBursts = 2f;
//        public Vector2 spawnCheckSize = new Vector2(1f, 1f);
//    }
//    #endregion

//    #region Spawn Settings
//    [SerializeField] private SpawnEntry[] spawnEntries;
//    [SerializeField] private Transform spawnPoint;
//    [SerializeField] private float laneEndX = 10f;
//    [SerializeField] private float moveSpeed = 2f;
//    #endregion

//    #region Clearance Check
//    [SerializeField] private LayerMask laneItemsLayer;
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        foreach (SpawnEntry entry in spawnEntries)
//        {
//            StartCoroutine(SpawnEntryLoop(entry));
//        }
//    }
//    #endregion

//    #region Spawning
//    private IEnumerator SpawnEntryLoop(SpawnEntry entry)
//    {
//        while (true)
//        {
//            for (int i = 0; i < entry.obstaclesPerBurst; i++)
//            {
//                yield return StartCoroutine(WaitForClearSpawnPoint(entry.spawnCheckSize));
//                SpawnObstacle(entry.pool);
//                yield return new WaitForSeconds(entry.delayBetweenObstaclesInBurst);
//            }

//            yield return new WaitForSeconds(entry.gapBetweenBursts);
//        }
//    }

//    private IEnumerator WaitForClearSpawnPoint(Vector2 checkSize)
//    {
//        while (Physics2D.OverlapBox(spawnPoint.position, checkSize, 0f, laneItemsLayer))
//        {
//            yield return null;
//        }
//    }

//    private void SpawnObstacle(ObjectPool pool)
//    {
//        GameObject obstacle = pool.Get();
//        obstacle.transform.position = spawnPoint.position;
//        Physics2D.SyncTransforms();

//        ObstacleMover mover = obstacle.GetComponent<ObstacleMover>();
//        mover.Setup(moveSpeed, laneEndX, pool);
//    }
//    #endregion
//}
#endregion


#region Phase 3 Sprint 1 - Obstacle Spawner (Independent Bursts, Collider-Checked Clearance) v6
//using System.Collections;
//using UnityEngine;

//public class ObstacleSpawner : MonoBehaviour
//{
//    #region Spawn Entry
//    [System.Serializable]
//    private class SpawnEntry
//    {
//        public ObjectPool pool;
//        public int obstaclesPerBurst = 1;
//        public float delayBetweenObstaclesInBurst = 0.3f;
//        public float gapBetweenBursts = 2f;
//        public Vector2 spawnCheckSize = new Vector2(1f, 1f);
//    }
//    #endregion

//    #region Spawn Settings
//    [SerializeField] private SpawnEntry[] spawnEntries;
//    [SerializeField] private Transform spawnPoint;
//    [SerializeField] private float laneEndX = 10f;
//    [SerializeField] private float moveSpeed = 2f;
//    #endregion

//    #region Clearance Check
//    [SerializeField] private LayerMask laneItemsLayer;
//    [SerializeField] private float maxSpawnWaitSeconds = 5f;
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        for (int i = 0; i < spawnEntries.Length; i++)
//        {
//            StartCoroutine(SpawnEntryLoop(spawnEntries[i], i));
//        }
//    }
//    #endregion

//    #region Spawning
//    private IEnumerator SpawnEntryLoop(SpawnEntry entry, int entryIndex)
//    {
//        yield return new WaitForSeconds(entryIndex * 0.1f);

//        while (true)
//        {
//            for (int i = 0; i < entry.obstaclesPerBurst; i++)
//            {
//                float waited = 0f;

//                while (Physics2D.OverlapBox(spawnPoint.position, entry.spawnCheckSize, 0f, laneItemsLayer))
//                {
//                    waited += Time.deltaTime;
//                    if (waited >= maxSpawnWaitSeconds) break;

//                    yield return null;
//                }

//                if (waited < maxSpawnWaitSeconds)
//                {
//                    SpawnObstacle(entry.pool);
//                }

//                yield return new WaitForSeconds(entry.delayBetweenObstaclesInBurst);
//            }

//            yield return new WaitForSeconds(entry.gapBetweenBursts);
//        }
//    }

//    private void SpawnObstacle(ObjectPool pool)
//    {
//        GameObject obstacle = pool.Get();
//        obstacle.transform.position = spawnPoint.position;
//        Physics2D.SyncTransforms();

//        ObstacleMover mover = obstacle.GetComponent<ObstacleMover>();
//        mover.Setup(moveSpeed, laneEndX, pool);
//    }
//    #endregion
//}
#endregion

#region Phase 3 Sprint 1 - Obstacle Spawner (Independent Bursts, Collider-Checked Clearance) v6
using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    #region Spawn Entry
    [System.Serializable]
    private class SpawnEntry
    {
        public ObjectPool pool;
        public int obstaclesPerBurst = 1;
        public float delayBetweenObstaclesInBurstMin = 0.2f;
        public float delayBetweenObstaclesInBurstMax = 0.4f;
        public float gapBetweenBurstsMin = 1.5f;
        public float gapBetweenBurstsMax = 2.5f;
        public Vector2 spawnCheckSize = new Vector2(1f, 1f);
        public int initialPopulationCount = 2;
    }
    #endregion

    #region Spawn Settings
    [SerializeField] private SpawnEntry[] spawnEntries;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float laneEndX = 10f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float maxInitialDelay = 1f;
    #endregion

    #region Clearance Check
    [SerializeField] private LayerMask laneItemsLayer;
    [SerializeField] private float maxSpawnWaitSeconds = 5f;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        PrePopulateLane();

        for (int i = 0; i < spawnEntries.Length; i++)
        {
            StartCoroutine(SpawnEntryLoop(spawnEntries[i]));
        }
    }
    #endregion

    #region Pre-Population
    private void PrePopulateLane()
    {
        float laneMinX = Mathf.Min(spawnPoint.position.x, laneEndX);
        float laneMaxX = Mathf.Max(spawnPoint.position.x, laneEndX);

        foreach (SpawnEntry entry in spawnEntries)
        {
            float segmentWidth = (laneMaxX - laneMinX) / entry.initialPopulationCount;

            for (int i = 0; i < entry.initialPopulationCount; i++)
            {
                float segmentStart = laneMinX + segmentWidth * i;
                float startX = Random.Range(segmentStart, segmentStart + segmentWidth);

                SpawnObstacle(entry.pool, startX);
            }
        }
    }
    #endregion

    #region Spawning
    private IEnumerator SpawnEntryLoop(SpawnEntry entry)
    {
        yield return new WaitForSeconds(Random.Range(0f, maxInitialDelay));

        while (true)
        {
            for (int i = 0; i < entry.obstaclesPerBurst; i++)
            {
                float waited = 0f;

                while (Physics2D.OverlapBox(spawnPoint.position, entry.spawnCheckSize, 0f, laneItemsLayer))
                {
                    waited += Time.deltaTime;
                    if (waited >= maxSpawnWaitSeconds) break;

                    yield return null;
                }

                if (waited < maxSpawnWaitSeconds)
                {
                    SpawnObstacle(entry.pool, spawnPoint.position.x);
                }

                yield return new WaitForSeconds(Random.Range(entry.delayBetweenObstaclesInBurstMin, entry.delayBetweenObstaclesInBurstMax));
            }

            yield return new WaitForSeconds(Random.Range(entry.gapBetweenBurstsMin, entry.gapBetweenBurstsMax));
        }
    }

    private void SpawnObstacle(ObjectPool pool, float startX)
    {
        GameObject obstacle = pool.Get();
        obstacle.transform.position = new Vector3(startX, spawnPoint.position.y, spawnPoint.position.z);
        Physics2D.SyncTransforms();

        ObstacleMover mover = obstacle.GetComponent<ObstacleMover>();
        mover.Setup(moveSpeed, laneEndX, pool);
    }
    #endregion
}
#endregion