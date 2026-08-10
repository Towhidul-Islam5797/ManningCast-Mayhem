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
//        if (isMoving || PauseManager.IsPaused) return;

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

//        GameManager.Instance.AddMoveScore();
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

#region Phase 1 Sprint 6 - Player Movement with Animation and Pause Handling
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
//    private Animator animator;
//    private bool isMoving;
//    private Vector3 startPosition;
//    #endregion

//    #region Unity Lifecycle
//    private void Awake()
//    {
//        controls = new ManningCastControls();
//        animator = GetComponent<Animator>();
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
//        if (isMoving || PauseManager.IsPaused) return;

//        Vector2 input = context.ReadValue<Vector2>();
//        Vector2Int direction = GetCardinalDirection(input);

//        if (direction != Vector2Int.zero)
//        {
//            SetBlendDirection(direction);
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

//    #region Animation
//    private void SetBlendDirection(Vector2Int direction)
//    {
//        if (animator == null) return;

//        animator.SetFloat("MoveX", direction.x);
//        animator.SetFloat("MoveY", direction.y);
//    }

//    private void ResetToIdle()
//    {
//        if (animator == null) return;

//        animator.SetFloat("MoveX", 0f);
//        animator.SetFloat("MoveY", 0f);
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

//        ResetToIdle();
//        GameManager.Instance.AddMoveScore();
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
//            ResetToIdle();
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

#region Phase 1 Sprint 7 - Player Movement with Animation, Pause Handling, and Score Tracking
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
//    private Animator animator;
//    private bool isMoving;
//    private bool inputLocked;
//    private Vector3 startPosition;
//    #endregion

//    #region Unity Lifecycle
//    private void Awake()
//    {
//        animator = GetComponent<Animator>();
//        startPosition = transform.position;
//    }
//    #endregion

//    #region Input Handling
//    public void OnMovePerformed(InputAction.CallbackContext context)
//    {
//        if (isMoving || inputLocked || PauseManager.IsPaused) return;

//        Vector2 input = context.ReadValue<Vector2>();
//        Vector2Int direction = GetCardinalDirection(input);

//        if (direction != Vector2Int.zero)
//        {
//            SetBlendDirection(direction);
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

//    #region Animation
//    private void SetBlendDirection(Vector2Int direction)
//    {
//        if (animator == null) return;

//        animator.SetFloat("MoveX", direction.x);
//        animator.SetFloat("MoveY", direction.y);
//    }

//    private void ResetToIdle()
//    {
//        if (animator == null) return;

//        animator.SetFloat("MoveX", 0f);
//        animator.SetFloat("MoveY", 0f);
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

//        ResetToIdle();
//        GameManager.Instance.AddMoveScore();
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
//            inputLocked = true;
//        }
//        else
//        {
//            transform.position = startPosition;
//            ResetToIdle();
//        }
//    }
//    #endregion

//    #region Goal Handling
//    public void HandleGoalReached()
//    {
//        StopAllCoroutines();
//        isMoving = false;

//        GameManager.Instance.PlayerReachedGoal();
//        inputLocked = true;
//    }
//    #endregion
//}
#endregion

#region Phase 2 Sprint 3 - Player Movement with Animation, Pause Handling, Score Tracking, and Wall Collision
//using System.Collections;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerMovement : MonoBehaviour
//{
//    #region Movement Settings
//    [SerializeField] private float moveDuration = 0.15f;
//    [SerializeField] private float gridSize = 1f;
//    [SerializeField] private LayerMask wallLayer;
//    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.8f, 0.8f);
//    #endregion

//    #region Private State
//    private Animator animator;
//    private bool isMoving;
//    private bool inputLocked;
//    private Vector3 startPosition;
//    #endregion

//    #region Unity Lifecycle
//    private void Awake()
//    {
//        animator = GetComponent<Animator>();
//        startPosition = transform.position;
//    }
//    #endregion

//    #region Input Handling
//    public void OnMovePerformed(InputAction.CallbackContext context)
//    {
//        if (isMoving || inputLocked || PauseManager.IsPaused) return;

//        Vector2 input = context.ReadValue<Vector2>();
//        Vector2Int direction = GetCardinalDirection(input);

//        if (direction != Vector2Int.zero)
//        {
//            Vector3 targetPosition = transform.position + new Vector3(direction.x, direction.y, 0f) * gridSize;

//            if (IsWallAt(targetPosition)) return;

//            SetBlendDirection(direction);
//            StartCoroutine(MoveToTile(direction));
//        }
//    }

//    private bool IsWallAt(Vector3 position)
//    {
//        return Physics2D.OverlapBox(position, wallCheckSize, 0f, wallLayer);
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

//    #region Animation
//    private void SetBlendDirection(Vector2Int direction)
//    {
//        if (animator == null) return;

//        animator.SetFloat("MoveX", direction.x);
//        animator.SetFloat("MoveY", direction.y);
//    }

//    private void ResetToIdle()
//    {
//        if (animator == null) return;

//        animator.SetFloat("MoveX", 0f);
//        animator.SetFloat("MoveY", 0f);
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

//        ResetToIdle();
//        GameManager.Instance.AddMoveScore();
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
//            inputLocked = true;
//        }
//        else
//        {
//            transform.position = startPosition;
//            ResetToIdle();
//        }
//    }
//    #endregion

//    #region Goal Handling
//    public void HandleGoalReached()
//    {
//        StopAllCoroutines();
//        isMoving = false;

//        GameManager.Instance.PlayerReachedGoal();
//        inputLocked = true;
//    }
//    #endregion
//}
//using System.Collections;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerMovement : MonoBehaviour
//{
//    #region Movement Settings
//    [SerializeField] private float moveDuration = 0.15f;
//    [SerializeField] private float gridSize = 1f;
//    [SerializeField] private LayerMask wallLayer;
//    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.8f, 0.8f);
//    #endregion

//    #region Private State
//    private Animator animator;
//    private bool isMoving;
//    private bool inputLocked;
//    private Vector3 startPosition;
//    #endregion

//    #region Unity Lifecycle
//    private void Awake()
//    {
//        animator = GetComponent<Animator>();
//        startPosition = transform.position;
//    }
//    #endregion

//    #region Input Handling
//    public void OnMovePerformed(InputAction.CallbackContext context)
//    {
//        if (isMoving || inputLocked || PauseManager.IsPaused) return;

//        Vector2 input = context.ReadValue<Vector2>();
//        Vector2Int direction = GetCardinalDirection(input);

//        if (direction != Vector2Int.zero)
//        {
//            Vector3 targetPosition = transform.position + new Vector3(direction.x, direction.y, 0f) * gridSize;

//            if (IsWallAt(targetPosition)) return;

//            SetBlendDirection(direction);
//            StartCoroutine(MoveToTile(direction));
//        }
//    }

//    private bool IsWallAt(Vector3 position)
//    {
//        return Physics2D.OverlapBox(position, wallCheckSize, 0f, wallLayer);
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

//    #region Animation
//    private void SetBlendDirection(Vector2Int direction)
//    {
//        if (animator == null) return;

//        animator.SetFloat("MoveX", direction.x);
//        animator.SetFloat("MoveY", direction.y);
//    }

//    private void ResetToIdle()
//    {
//        if (animator == null) return;

//        animator.SetFloat("MoveX", 0f);
//        animator.SetFloat("MoveY", 0f);
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

//        ResetToIdle();
//        GameManager.Instance.AddMoveScore();
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
//            inputLocked = true;
//        }
//        else
//        {
//            transform.position = startPosition;
//            ResetToIdle();
//        }
//    }
//    #endregion

//    #region Goal Handling
//    public void HandleGoalReached()
//    {
//        StopAllCoroutines();
//        isMoving = false;

//        GameManager.Instance.PlayerReachedGoal();
//        inputLocked = true;
//    }
//    #endregion
//}
#endregion

#region Phase 2 Sprint 3 - Player Movement with Animation, Pause Handling, Score Tracking, and Wall Collision
//using System.Collections;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerMovement : MonoBehaviour
//{
//    #region Movement Settings
//    [SerializeField] private float moveDuration = 0.15f;
//    [SerializeField] private float gridSize = 1f;
//    [SerializeField] private LayerMask wallLayer;
//    #endregion

//    #region Private State
//    private Animator animator;
//    private BoxCollider2D bodyCollider;
//    private bool isMoving;
//    private bool inputLocked;
//    private Vector3 startPosition;
//    #endregion

//    #region Unity Lifecycle
//    private void Awake()
//    {
//        animator = GetComponent<Animator>();
//        bodyCollider = GetComponent<BoxCollider2D>();
//        startPosition = transform.position;
//    }
//    #endregion

//    #region Input Handling
//    public void OnMovePerformed(InputAction.CallbackContext context)
//    {
//        if (isMoving || inputLocked || PauseManager.IsPaused) return;

//        Vector2 input = context.ReadValue<Vector2>();
//        Vector2Int direction = GetCardinalDirection(input);

//        if (direction != Vector2Int.zero)
//        {
//            Vector3 targetPosition = transform.position + new Vector3(direction.x, direction.y, 0f) * gridSize;

//            if (IsWallAt(targetPosition)) return;

//            SetBlendDirection(direction);
//            StartCoroutine(MoveToTile(direction));
//        }
//    }

//    private bool IsWallAt(Vector3 position)
//    {
//        Vector2 checkPosition = (Vector2)position + bodyCollider.offset;
//        return Physics2D.OverlapBox(checkPosition, bodyCollider.size, 0f, wallLayer);
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

//    #region Animation
//    private void SetBlendDirection(Vector2Int direction)
//    {
//        if (animator == null) return;

//        animator.SetFloat("MoveX", direction.x);
//        animator.SetFloat("MoveY", direction.y);
//    }

//    private void ResetToIdle()
//    {
//        if (animator == null) return;

//        animator.SetFloat("MoveX", 0f);
//        animator.SetFloat("MoveY", 0f);
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

//        ResetToIdle();
//        GameManager.Instance.AddMoveScore();
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
//            inputLocked = true;
//        }
//        else
//        {
//            transform.position = startPosition;
//            ResetToIdle();
//        }
//    }
//    #endregion

//    #region Goal Handling
//    public void HandleGoalReached()
//    {
//        StopAllCoroutines();
//        isMoving = false;

//        GameManager.Instance.PlayerReachedGoal();
//        inputLocked = true;
//    }
//    #endregion
//}
#endregion

#region Phase 2 Sprint 4 - Player Movement with Animation, Pause Handling, Score Tracking, and Wall Collision
//using System.Collections;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerMovement : MonoBehaviour
//{
//    #region Movement Settings
//    [SerializeField] private float moveDuration = 0.15f;
//    [SerializeField] private float gridSize = 1f;
//    [SerializeField] private LayerMask wallLayer;
//    #endregion

//    #region Private State
//    private Animator animator;
//    private BoxCollider2D bodyCollider;
//    private bool isMoving;
//    private bool inputLocked;
//    private Vector3 startPosition;
//    #endregion

//    #region Unity Lifecycle
//    private void Awake()
//    {
//        animator = GetComponent<Animator>();
//        bodyCollider = GetComponent<BoxCollider2D>();
//        startPosition = transform.position;
//    }
//    #endregion

//    #region Input Handling
//    public void OnMovePerformed(InputAction.CallbackContext context)
//    {
//        if (isMoving || inputLocked || PauseManager.IsPaused) return;

//        Vector2 input = context.ReadValue<Vector2>();
//        Vector2Int direction = GetCardinalDirection(input);

//        if (direction != Vector2Int.zero)
//        {
//            Vector3 targetPosition = transform.position + new Vector3(direction.x, direction.y, 0f) * gridSize;

//            if (IsWallAt(targetPosition)) return;

//            SetBlendDirection(direction);
//            StartCoroutine(MoveToTile(direction));
//        }
//    }

//    private bool IsWallAt(Vector3 position)
//    {
//        Vector2 checkPosition = (Vector2)position + bodyCollider.offset;
//        return Physics2D.OverlapBox(checkPosition, bodyCollider.size, 0f, wallLayer);
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

//    #region Animation
//    private void SetBlendDirection(Vector2Int direction)
//    {
//        if (animator == null) return;

//        animator.SetFloat("MoveX", direction.x);
//        animator.SetFloat("MoveY", direction.y);
//    }

//    private void ResetToIdle()
//    {
//        if (animator == null) return;

//        animator.SetFloat("MoveX", 0f);
//        animator.SetFloat("MoveY", 0f);
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

//        ResetToIdle();
//        GameManager.Instance.AddMoveScore();
//    }
//    #endregion

//    #region Obstacle Collision
//    public void HandleObstacleHit(int scorePenalty)
//    {
//        StopAllCoroutines();
//        isMoving = false;

//        GameManager.Instance.PlayerHitObstacle(scorePenalty);

//        if (GameManager.Instance.IsGameOver)
//        {
//            inputLocked = true;
//        }
//        else
//        {
//            transform.position = startPosition;
//            ResetToIdle();
//        }
//    }

//    public void HandleSandwichHit(int scorePenalty)
//    {
//        GameManager.Instance.PlayerHitSandwich(scorePenalty);
//    }
//    #endregion

//    #region Goal Handling
//    public void HandleGoalReached()
//    {
//        StopAllCoroutines();
//        isMoving = false;

//        GameManager.Instance.PlayerReachedGoal();
//        inputLocked = true;
//    }
//    #endregion
//}
#endregion

#region Phase 2 Sprint 6 - Player Movement with Animation, Pause Handling, Score Tracking, Wall Collision, and Hazard Collision
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Movement Settings
    [SerializeField] private float moveDuration = 0.15f;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask hazardLayer;
    [SerializeField] private int hazardScorePenalty;
    #endregion

    #region Private State
    private Animator animator;
    private BoxCollider2D bodyCollider;
    private bool isMoving;
    private bool inputLocked;
    private Vector3 startPosition;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<BoxCollider2D>();
        startPosition = transform.position;
    }
    #endregion

    #region Input Handling
    public void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (isMoving || inputLocked || PauseManager.IsPaused) return;

        Vector2 input = context.ReadValue<Vector2>();
        Vector2Int direction = GetCardinalDirection(input);

        if (direction != Vector2Int.zero)
        {
            Vector3 targetPosition = transform.position + new Vector3(direction.x, direction.y, 0f) * gridSize;

            if (IsWallAt(targetPosition)) return;

            SetBlendDirection(direction);
            StartCoroutine(MoveToTile(direction));
        }
    }

    private bool IsWallAt(Vector3 position)
    {
        Vector2 checkPosition = (Vector2)position + bodyCollider.offset;
        return Physics2D.OverlapBox(checkPosition, bodyCollider.size, 0f, wallLayer);
    }

    private bool IsHazardAt(Vector3 position)
    {
        Vector2 checkPosition = (Vector2)position + bodyCollider.offset;
        return Physics2D.OverlapBox(checkPosition, bodyCollider.size, 0f, hazardLayer);
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

    #region Animation
    private void SetBlendDirection(Vector2Int direction)
    {
        if (animator == null) return;

        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }

    private void ResetToIdle()
    {
        if (animator == null) return;

        animator.SetFloat("MoveX", 0f);
        animator.SetFloat("MoveY", 0f);
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
        Physics2D.SyncTransforms();

        if (transform.parent == null && IsHazardAt(transform.position))
        {
            HandleObstacleHit(hazardScorePenalty);
            yield break;
        }

        ResetToIdle();
        GameManager.Instance.AddMoveScore();
    }
    #endregion

    #region Obstacle Collision
    public void HandleObstacleHit(int scorePenalty)
    {
        StopAllCoroutines();
        isMoving = false;

        GameManager.Instance.PlayerHitObstacle(scorePenalty);

        if (GameManager.Instance.IsGameOver)
        {
            inputLocked = true;
        }
        else
        {
            transform.position = startPosition;
            ResetToIdle();
        }
    }

    public void HandleSandwichHit(int scorePenalty)
    {
        GameManager.Instance.PlayerHitSandwich(scorePenalty);
    }
    #endregion

    #region Goal Handling
    public void HandleGoalReached()
    {
        StopAllCoroutines();
        isMoving = false;

        GameManager.Instance.PlayerReachedGoal();
        inputLocked = true;
    }
    #endregion
}
#endregion