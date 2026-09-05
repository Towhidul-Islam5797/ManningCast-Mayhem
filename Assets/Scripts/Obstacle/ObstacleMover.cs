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
//using UnityEngine;

//public class ObstacleMover : MonoBehaviour
//{
//    #region Movement State
//    private float speed;
//    private float laneEndX;
//    private ObjectPool sourcePool;
//    private bool isSetUp;
//    #endregion

//    #region Setup
//    public void Setup(float moveSpeed, float endX, ObjectPool pool)
//    {
//        speed = moveSpeed;
//        laneEndX = endX;
//        sourcePool = pool;
//        isSetUp = true;
//    }
//    #endregion

//    #region Unity Lifecycle
//    private void Update()
//    {
//        if (!isSetUp) return;

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

#region Phase 2 Sprint 5 - Obstacle Mover (Reached End Event)
//using System;
//using UnityEngine;

//public class ObstacleMover : MonoBehaviour
//{
//    #region Movement State
//    private float speed;
//    private float laneEndX;
//    private ObjectPool sourcePool;
//    private bool isSetUp;
//    #endregion

//    #region Events
//    public event Action OnReachedEnd;
//    #endregion

//    #region Setup
//    public void Setup(float moveSpeed, float endX, ObjectPool pool)
//    {
//        speed = moveSpeed;
//        laneEndX = endX;
//        sourcePool = pool;
//        isSetUp = true;
//    }
//    #endregion

//    #region Unity Lifecycle
//    private void Update()
//    {
//        if (!isSetUp) return;

//        transform.position += Vector3.right * speed * Time.deltaTime;

//        bool movingRight = speed > 0f;
//        bool reachedEnd = movingRight ? transform.position.x >= laneEndX : transform.position.x <= laneEndX;

//        if (reachedEnd)
//        {
//            OnReachedEnd?.Invoke();
//            sourcePool.Return(gameObject);
//        }
//    }
//    #endregion
//}
#endregion

#region Phase 3 Sprint 4 - Obstacle Mover (Reached End Event)
using System;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    #region Movement State
    private float speed;
    private float laneEndX;
    private ObjectPool sourcePool;
    private bool isSetUp;
    #endregion

    #region Events
    public event Action OnReachedEnd;
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
            OnReachedEnd?.Invoke();
            sourcePool.Return(gameObject);
        }
    }
    #endregion

    #region Early Return
    public void ReturnEarly()
    {
        sourcePool.Return(gameObject);
    }
    #endregion
}
#endregion