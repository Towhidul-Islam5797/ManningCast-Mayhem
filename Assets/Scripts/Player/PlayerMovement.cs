#region Summary
/// <summary>
/// This script handles the player's movement on a grid-based system. It listens for input actions and moves the player character to adjacent tiles based on the input direction.
/// </summary>
/// <remarks>
/// The movement is smooth and interpolated over a specified duration, ensuring that the player character transitions between tiles in a visually appealing manner. The script uses Unity's new Input System for handling player input.
/// </remarks>
#endregion
#region Phase 1 Sprint 1 - Player Movement
//using System.Collections;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerMovement : MonoBehaviour
//{
//    #region Movement Settings
//    [SerializeField] private float moveDuration = 0.15f;
//    [SerializeField] private float gridSize = 1f;
//    #endregion

//    #region Private State
//    private ManningCastControls controls;
//    private bool isMoving;
//    #endregion

//    #region Unity Lifecycle
//    private void Awake()
//    {
//        controls = new ManningCastControls();
//    }

//    private void OnEnable()
//    {
//        controls.Player.Enable();
//        controls.Player.Move.performed += OnMovePerformed;
//    }

//    private void OnDisable()
//    {
//        controls.Player.Move.performed -= OnMovePerformed;
//        controls.Player.Disable();
//    }
//    #endregion

//    #region Input Handling
//    private void OnMovePerformed(InputAction.CallbackContext context)
//    {
//        if (isMoving) return;

//        Vector2 input = context.ReadValue<Vector2>();
//        Vector2Int direction = GetCardinalDirection(input);

//        if (direction != Vector2Int.zero)
//        {
//            StartCoroutine(MoveToTile(direction));
//        }
//    }

//    private Vector2Int GetCardinalDirection(Vector2 input)
//    {
//        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
//        {
//            return input.x > 0 ? Vector2Int.right : Vector2Int.left;
//        }

//        if (input.y != 0)
//        {
//            return input.y > 0 ? Vector2Int.up : Vector2Int.down;
//        }

//        return Vector2Int.zero;
//    }
//    #endregion

//    #region Grid Movement
//    private IEnumerator MoveToTile(Vector2Int direction)
//    {
//        isMoving = true;

//        Vector3 startPosition = transform.position;
//        Vector3 endPosition = startPosition + new Vector3(direction.x, direction.y, 0f) * gridSize;

//        float elapsed = 0f;
//        while (elapsed < moveDuration)
//        {
//            elapsed += Time.deltaTime;
//            transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / moveDuration);
//            yield return null;
//        }

//        transform.position = endPosition;
//        isMoving = false;
//    }
//    #endregion
//}
#endregion

#region Phase 1 Sprint 2 - Player Movement with Obstacle Handling
//using System.Collections;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerMovement : MonoBehaviour
//{
//    #region Movement Settings
//    [SerializeField] private float moveDuration = 0.15f;
//    [SerializeField] private float gridSize = 1f;
//    #endregion

//    #region Private State
//    private ManningCastControls controls;
//    private bool isMoving;
//    private Vector3 startPosition;
//    #endregion

//    #region Unity Lifecycle
//    private void Awake()
//    {
//        controls = new ManningCastControls();
//        startPosition = transform.position;
//    }

//    private void OnEnable()
//    {
//        controls.Player.Enable();
//        controls.Player.Move.performed += OnMovePerformed;
//    }

//    private void OnDisable()
//    {
//        controls.Player.Move.performed -= OnMovePerformed;
//        controls.Player.Disable();
//    }
//    #endregion

//    #region Input Handling
//    private void OnMovePerformed(InputAction.CallbackContext context)
//    {
//        if (isMoving) return;

//        Vector2 input = context.ReadValue<Vector2>();
//        Vector2Int direction = GetCardinalDirection(input);

//        if (direction != Vector2Int.zero)
//        {
//            StartCoroutine(MoveToTile(direction));
//        }
//    }

//    private Vector2Int GetCardinalDirection(Vector2 input)
//    {
//        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
//        {
//            return input.x > 0 ? Vector2Int.right : Vector2Int.left;
//        }

//        if (input.y != 0)
//        {
//            return input.y > 0 ? Vector2Int.up : Vector2Int.down;
//        }

//        return Vector2Int.zero;
//    }
//    #endregion

//    #region Grid Movement
//    private IEnumerator MoveToTile(Vector2Int direction)
//    {
//        isMoving = true;

//        Vector3 startPos = transform.position;
//        Vector3 endPosition = startPos + new Vector3(direction.x, direction.y, 0f) * gridSize;

//        float elapsed = 0f;
//        while (elapsed < moveDuration)
//        {
//            elapsed += Time.deltaTime;
//            transform.position = Vector3.Lerp(startPos, endPosition, elapsed / moveDuration);
//            yield return null;
//        }

//        transform.position = endPosition;
//        isMoving = false;
//    }
//    #endregion

//    #region Obstacle Collision
//    public void HandleObstacleHit()
//    {
//        StopAllCoroutines();
//        isMoving = false;

//        GameManager.Instance.PlayerHitObstacle();

//        if (GameManager.Instance.IsGameOver)
//        {
//            controls.Player.Disable();
//        }
//        else
//        {
//            transform.position = startPosition;
//        }
//    }
//    #endregion
//}
#endregion

#region Phase 1 Sprint 3 - Player Movement with Goal Handling
//using System.Collections;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerMovement : MonoBehaviour
//{
//    #region Movement Settings
//    [SerializeField] private float moveDuration = 0.15f;
//    [SerializeField] private float gridSize = 1f;
//    #endregion

//    #region Private State
//    private ManningCastControls controls;
//    private bool isMoving;
//    private Vector3 startPosition;
//    #endregion

//    #region Unity Lifecycle
//    private void Awake()
//    {
//        controls = new ManningCastControls();
//        startPosition = transform.position;
//    }

//    private void OnEnable()
//    {
//        controls.Player.Enable();
//        controls.Player.Move.performed += OnMovePerformed;
//    }

//    private void OnDisable()
//    {
//        controls.Player.Move.performed -= OnMovePerformed;
//        controls.Player.Disable();
//    }
//    #endregion

//    #region Input Handling
//    private void OnMovePerformed(InputAction.CallbackContext context)
//    {
//        if (isMoving) return;

//        Vector2 input = context.ReadValue<Vector2>();
//        Vector2Int direction = GetCardinalDirection(input);

//        if (direction != Vector2Int.zero)
//        {
//            StartCoroutine(MoveToTile(direction));
//        }
//    }

//    private Vector2Int GetCardinalDirection(Vector2 input)
//    {
//        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
//        {
//            return input.x > 0 ? Vector2Int.right : Vector2Int.left;
//        }

//        if (input.y != 0)
//        {
//            return input.y > 0 ? Vector2Int.up : Vector2Int.down;
//        }

//        return Vector2Int.zero;
//    }
//    #endregion

//    #region Grid Movement
//    private IEnumerator MoveToTile(Vector2Int direction)
//    {
//        isMoving = true;

//        Vector3 startPos = transform.position;
//        Vector3 endPosition = startPos + new Vector3(direction.x, direction.y, 0f) * gridSize;

//        float elapsed = 0f;
//        while (elapsed < moveDuration)
//        {
//            elapsed += Time.deltaTime;
//            transform.position = Vector3.Lerp(startPos, endPosition, elapsed / moveDuration);
//            yield return null;
//        }

//        transform.position = endPosition;
//        isMoving = false;
//    }
//    #endregion

//    #region Obstacle Collision
//    public void HandleObstacleHit()
//    {
//        StopAllCoroutines();
//        isMoving = false;

//        GameManager.Instance.PlayerHitObstacle();

//        if (GameManager.Instance.IsGameOver)
//        {
//            controls.Player.Disable();
//        }
//        else
//        {
//            transform.position = startPosition;
//        }
//    }
//    #endregion

//    #region Goal Handling
//    public void HandleGoalReached()
//    {
//        StopAllCoroutines();
//        isMoving = false;

//        GameManager.Instance.PlayerReachedGoal();
//        controls.Player.Disable();
//    }
//    #endregion
//}
#endregion

#region Phase 1 Sprint 5 - Player Movement with Pause Handling
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Movement Settings
    [SerializeField] private float moveDuration = 0.15f;
    [SerializeField] private float gridSize = 1f;
    #endregion

    #region Private State
    private ManningCastControls controls;
    private bool isMoving;
    private Vector3 startPosition;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        controls = new ManningCastControls();
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += OnMovePerformed;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Disable();
    }
    #endregion

    #region Input Handling
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (isMoving || PauseManager.IsPaused) return;

        Vector2 input = context.ReadValue<Vector2>();
        Vector2Int direction = GetCardinalDirection(input);

        if (direction != Vector2Int.zero)
        {
            StartCoroutine(MoveToTile(direction));
        }
    }

    private Vector2Int GetCardinalDirection(Vector2 input)
    {
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            return input.x > 0 ? Vector2Int.right : Vector2Int.left;
        }

        if (input.y != 0)
        {
            return input.y > 0 ? Vector2Int.up : Vector2Int.down;
        }

        return Vector2Int.zero;
    }
    #endregion

    #region Grid Movement
    private IEnumerator MoveToTile(Vector2Int direction)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 endPosition = startPos + new Vector3(direction.x, direction.y, 0f) * gridSize;

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPosition, elapsed / moveDuration);
            yield return null;
        }

        transform.position = endPosition;
        isMoving = false;

        GameManager.Instance.AddMoveScore();
    }
    #endregion

    #region Obstacle Collision
    public void HandleObstacleHit()
    {
        StopAllCoroutines();
        isMoving = false;

        GameManager.Instance.PlayerHitObstacle();

        if (GameManager.Instance.IsGameOver)
        {
            controls.Player.Disable();
        }
        else
        {
            transform.position = startPosition;
        }
    }
    #endregion

    #region Goal Handling
    public void HandleGoalReached()
    {
        StopAllCoroutines();
        isMoving = false;

        GameManager.Instance.PlayerReachedGoal();
        controls.Player.Disable();
    }
    #endregion
}
#endregion