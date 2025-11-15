using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public sealed class PlayerController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private InputActionAsset inputActions;

    [Header("Config")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 0.5f;

    // privates
    private Rigidbody _rigidbody;
    private Vector3 _velocity = Vector3.zero;
    private Vector2 _moveInput = Vector2.zero;

    private InputAction _moveAction;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Debug.Assert( _rigidbody != null, $"The game object with name {name}, requires a rigidbody" );

        Debug.Assert(inputActions != null, $"The game object with name {name}, requires an input action asset in order to work");
        _moveAction = inputActions.FindAction("Move");
        Debug.Assert(_moveAction != null);
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _moveAction.performed += HandleMovement;
        _moveAction.canceled += HandleMovement;
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _moveAction.performed -= HandleMovement;
        _moveAction.canceled -= HandleMovement;
    }

    private void FixedUpdate()
    {
        Vector3 targetDir = new Vector3(_moveInput.x, 0f, _moveInput.y);

        Vector3 targetVelocity =
            targetDir.sqrMagnitude > 0.01f
            ? targetDir.normalized * maxSpeed // accelerate
            : Vector3.zero; // slow down

        _velocity = Vector3.MoveTowards(
            _velocity,
            targetVelocity,
            acceleration * Time.fixedDeltaTime
        );

        _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.fixedDeltaTime);
    }

    private void HandleMovement(InputAction.CallbackContext callbackContext)
    {
        _moveInput = callbackContext.ReadValue<Vector2>();
    }
}
