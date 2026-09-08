#region Summary
/// <summary>
/// PlayerDebugOverlay shows a live on-screen panel for testing player input
/// in WebGL builds, where the browser console is harder to check. It listens
/// to the same Move and Interact actions the player already uses, without
/// changing PlayerMovement.cs. Only compiles into WebGL builds.
/// </summary>
#endregion

#region Phase 3 Sprint 6 - Player Debug Overlay (WebGL Only)
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDebugOverlay : MonoBehaviour
{
    #region Settings
    [SerializeField] private float pressResetSeconds = 0.5f;
    #endregion

    #region Private State
    private PlayerInput playerInput;
    private Vector2 lastMoveInput;
    private int moveFireCount;
    private float lastMoveFireTime;
    private int interactFireCount;
    private float lastInteractFireTime;
    #endregion

#if UNITY_WEBGL && !UNITY_EDITOR
    #region Unity Lifecycle
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Interact"].performed += OnInteract;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Interact"].performed -= OnInteract;
    }
    #endregion

    #region Input Tracking
    private void OnMove(InputAction.CallbackContext context)
    {
        lastMoveInput = context.ReadValue<Vector2>();
        moveFireCount = Time.time - lastMoveFireTime > pressResetSeconds ? 1 : moveFireCount + 1;
        lastMoveFireTime = Time.time;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        interactFireCount = Time.time - lastInteractFireTime > pressResetSeconds ? 1 : interactFireCount + 1;
        lastInteractFireTime = Time.time;
    }
    #endregion

    #region On Screen Display
    private void OnGUI()
    {
        string deviceName = playerInput.devices.Count > 0 ? playerInput.devices[0].displayName : "None";

        GUI.Box(new Rect(10, 10, 320, 130), "");
        GUI.Label(new Rect(20, 15, 300, 20), "Move Input: " + lastMoveInput);
        GUI.Label(new Rect(20, 35, 300, 20), "Move Fires Last Press: " + moveFireCount);
        GUI.Label(new Rect(20, 55, 300, 20), "Interact Fires Last Press: " + interactFireCount);
        GUI.Label(new Rect(20, 75, 300, 20), "Position: " + transform.position);
        GUI.Label(new Rect(20, 95, 300, 20), "Paired Device: " + deviceName);
        GUI.Label(new Rect(20, 115, 300, 20), "Control Scheme: " + playerInput.currentControlScheme);
    }
    #endregion
#endif
}
#endregion