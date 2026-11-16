using UnityEngine;

public class PlayerCharacterMovement : MonoBehaviour
{
    private Vector3 _movementDirection;

    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 10f;
    private float _currentSpeed;

    private Vector3 _velocityXZ;

    [SerializeField] private CharacterController _characterController;

    // Status Sprint & Property IsSprint untuk dibaca oleh PlayerCharacterStamina
    private bool _isSprint;
    public bool IsSprint => _isSprint;

    private void Awake()
    {
        _currentSpeed = _walkSpeed;
    }

    // Function untuk mengubah status sprint dan kecepatannya
    public void SetSprint(bool isSprint)
    {
        _isSprint = isSprint;
        _currentSpeed = _isSprint ? _sprintSpeed : _walkSpeed;
    }

    public void SetMoveDirection(Vector2 inputDirection)
    {
        _movementDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
    }

    private void CalculateVelocityXZ()
    {
        Transform cameraTransform = Camera.main.transform;

        Vector3 xDirection = _movementDirection.x * cameraTransform.right;
        Vector3 zDirection = _movementDirection.z * cameraTransform.forward;

        Vector3 direction = xDirection + zDirection;
        direction.y = 0;

        if (_movementDirection.magnitude > 0.01f)
        {
            _velocityXZ = direction.normalized * _currentSpeed * Time.deltaTime;
        }
        else
        {
            _velocityXZ = Vector3.zero;
        }
    }

    public void Move()
    {
        CalculateVelocityXZ();

        if (_characterController != null)
        {
            _characterController.Move(_velocityXZ);
        }
    }

    private void Update()
    {
        Move();
    }
}