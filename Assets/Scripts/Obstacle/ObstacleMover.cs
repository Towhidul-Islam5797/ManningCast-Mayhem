#region Phase 1 Sprint 6 - Obstacle Mover
//using UnityEngine;

//public class ObstacleMover : MonoBehaviour
//{
//    #region Movement State
//    private float speed;
//    private float laneEndX;
//    private ObjectPool sourcePool;
//    #endregion

//    #region Setup
//    public void Setup(float moveSpeed, float endX, ObjectPool pool)
//    {
//        speed = moveSpeed;
//        laneEndX = endX;
//        sourcePool = pool;
//    }
//    #endregion

//    #region Unity Lifecycle
//    private void Update()
//    {
//        transform.position += Vector3.right * speed * Time.deltaTime;

//        bool movingRight = speed > 0f;
//        bool reachedEnd = movingRight ? transform.position.x >= laneEndX : transform.position.x <= laneEndX;

//        if (reachedEnd)
//        {
//            sourcePool.Return(gameObject);
//        }
//    }
//    #endregion
//}
#endregion

#region Phase 1 Sprint 6 - Obstacle Mover (Updated)
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    #region Movement State
    private float speed;
    private float laneEndX;
    private ObjectPool sourcePool;
    private bool isSetUp;
    #endregion

    #region Setup
    public void Setup(float moveSpeed, float endX, ObjectPool pool)
    {
        speed = moveSpeed;
        laneEndX = endX;
        sourcePool = pool;
        isSetUp = true;
    }
    #endregion

    #region Unity Lifecycle
    private void Update()
    {
        if (!isSetUp) return;

        transform.position += Vector3.right * speed * Time.deltaTime;

        bool movingRight = speed > 0f;
        bool reachedEnd = movingRight ? transform.position.x >= laneEndX : transform.position.x <= laneEndX;

        if (reachedEnd)
        {
            sourcePool.Return(gameObject);
        }
    }
    #endregion
}
#endregion