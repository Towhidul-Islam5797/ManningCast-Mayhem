#region summary
/// <summary>
/// A simple object pool implementation for reusing GameObjects in Unity 
/// to improve performance by reducing the overhead of frequent instantiation and destruction.
/// </summary>
#endregion

#region Phase 1 Sprint 6 - Object Pooling

using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    #region Pool Settings
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 5;
    #endregion

    #region Pool Storage
    private readonly Queue<GameObject> pool = new Queue<GameObject>();
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        // The revised client build uses ManningLaneDirector instead of the legacy
        // prefab pools. This check must happen in Awake because scene objects wake
        // before ManningRuntimeBootstrap receives SceneManager.sceneLoaded.
        if (ManningRuntimeBootstrap.IsInstalled)
        {
            enabled = false;
            return;
        }

        if (prefab == null)
        {
            Debug.LogWarning($"{name}: ObjectPool has no prefab and was disabled.", this);
            enabled = false;
            return;
        }

        for (int i = 0; i < initialSize; i++)
        {
            GameObject instance = Instantiate(prefab, transform);
            instance.SetActive(false);
            pool.Enqueue(instance);
        }
    }
    #endregion

    #region Pool Access
    public GameObject Get()
    {
        GameObject instance = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab, transform);
        instance.SetActive(true);
        return instance;
    }

    public void Return(GameObject instance)
    {
        instance.SetActive(false);
        pool.Enqueue(instance);
    }
    #endregion
}
#endregion
