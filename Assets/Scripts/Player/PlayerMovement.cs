#region Summary
/// <summary>
/// This script handles the player's movement in a grid-based manner. The player can move up, down, left, or right by pressing the corresponding keys (WASD or arrow keys). 
///     The movement is restricted to a grid defined by the tileSize variable.
/// </summary>
/// 
/// <remarks>
/// This script is designed for a Unity project and utilizes the new Input System package. It allows for basic grid movement, where the player moves one tile at a time based on the specified tile size. 
///     The movement is instantaneous, and the player cannot move while already in motion.
/// </remarks>
#endregion

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Sprint 1 - Basic Grid Movement

    [SerializeField] private float tileSize = 1f;

    private bool isMoving = false;

    private void Update()
    {
        if (isMoving)
            return;

        Vector2 direction = GetInputDirection();

        if (direction != Vector2.zero)
        {
            Vector3 targetPosition = transform.position + (Vector3)(direction * tileSize);
            MoveToPosition(targetPosition);
        }
    }

    private Vector2 GetInputDirection()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return Vector2.zero;

        if (keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame)
            return Vector2.up;
        if (keyboard.sKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame)
            return Vector2.down;
        if (keyboard.aKey.wasPressedThisFrame || keyboard.leftArrowKey.wasPressedThisFrame)
            return Vector2.left;
        if (keyboard.dKey.wasPressedThisFrame || keyboard.rightArrowKey.wasPressedThisFrame)
            return Vector2.right;

        return Vector2.zero;
    }

    private void MoveToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    #endregion
}