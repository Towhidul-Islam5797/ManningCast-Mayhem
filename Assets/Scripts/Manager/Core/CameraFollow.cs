#region Summary
/// <summary>
/// Makes the camera follow the player on the Y axis only, since the room is taller
/// than the camera view but not wider. X and Z stay fixed at the camera's starting values.
/// </summary>
#endregion
#region Phase 2 Sprint 4 - Camera Follow
//using UnityEngine;

//public class CameraFollow : MonoBehaviour
//{
//    #region Follow Settings
//    [SerializeField] private Transform player;
//    [SerializeField] private float followSpeed = 5f;
//    [SerializeField] private float minY = -3f;
//    [SerializeField] private float maxY = 9f;
//    #endregion

//    #region Private State
//    private float fixedX;
//    private float fixedZ;
//    #endregion

//    #region Unity Lifecycle
//    private void Awake()
//    {
//        fixedX = transform.position.x;
//        fixedZ = transform.position.z;
//    }

//    private void LateUpdate()
//    {
//        float targetY = Mathf.Clamp(player.position.y, minY, maxY);
//        float newY = Mathf.Lerp(transform.position.y, targetY, followSpeed * Time.deltaTime);

//        transform.position = new Vector3(fixedX, newY, fixedZ);
//    }
//    #endregion
//}
#endregion

#region Phase 2 Sprint 2 - Camera Follow + Character Selection
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Follow Settings
    [SerializeField] private Transform peytonTransform;
    [SerializeField] private Transform eliTransform;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 9f;
    #endregion

    #region Private State
    private Transform player;
    private float fixedX;
    private float fixedZ;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        fixedX = transform.position.x;
        fixedZ = transform.position.z;

        bool isPeyton = CharacterSelection.SelectedCharacter == CharacterSelection.Character.Peyton;
        player = isPeyton ? peytonTransform : eliTransform;
    }

    private void LateUpdate()
    {
        float targetY = Mathf.Clamp(player.position.y, minY, maxY);
        float newY = Mathf.Lerp(transform.position.y, targetY, followSpeed * Time.deltaTime);

        transform.position = new Vector3(fixedX, newY, fixedZ);
    }
    #endregion
}
#endregion