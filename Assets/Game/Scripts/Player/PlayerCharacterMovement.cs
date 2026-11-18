using UnityEngine;

public class PlayerCharacterMovement : MonoBehaviour
{
    private Vector3 _movementDirection;

    [SerializeField] private float _walkSpeed = 1;
    [SerializeField] private float _sprintSpeed = 2;
    [SerializeField] private float _acceleration = 0.5f; 
    private float _currentSpeed;

    private Vector3 _velocityXZ;

    [SerializeField] private CharacterController _characterController;

    [SerializeField] private float _gravityScale = 1; 
    private float _velocityY; 
    private bool _isGrounded;

    private bool _isSprint;
    public bool IsSprint => _isSprint;

    private void Awake()
    {
        //_currentSpeed = _walkSpeed;
    }

    // Function untuk mengubah status sprint dan kecepatannya
    public void SetSprint(bool isSprint)
    {
        _isSprint = isSprint;
        //_currentSpeed = _isSprint ? _sprintSpeed : _walkSpeed;
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
        CalculateVelocityY();
        Vector3 velocity = new Vector3(_velocityXZ.x, _velocityY, _velocityXZ.z);
        _characterController.Move(velocity); 

        //if (_characterController != null)
        //{
        //    _characterController.Move(_velocityXZ);
        //}
    }

    private void CalculateVelocityY() 
    { 
        // Menghitung kecepatan gerakan charater di sumbu y 
        _velocityY = _velocityY + Physics.gravity.y * _gravityScale * Time.deltaTime; 
    }

    private void CheckIsGrounded() 
    { 
        // Mendapatkan layer Ground 
        LayerMask groundLayer = LayerMask.GetMask("Ground"); 
        // Membuat pendeteksi berbentuk bola  
        // Posisi pendeteksi nya di transform.position(posisi kaki character) 
        // Radius = 0.5 
        // Layer object yang dicek adalah layer ground 
        // Jika terdeteksi _isGrounded bernilai true,  
       // jika tidak _isGrounded bernilai false 
        _isGrounded = Physics.CheckSphere(transform.position, 0.5f, groundLayer); 
    } 

    private void ResetVelocityY() 
    { 
        // Mengecek apakah character sudah ada di ground 
        if (_isGrounded == true && _velocityY < 0) 
        { 
            // Jika iya maka reset velocity ke -2 
            // Sebenarnya bisa di-reset ke 0 
            // Tapi jika masih ingin menarik karakter nya sedikit,  
            // reset velocity ke -2 atau -1 
            _velocityY = -2; 
        } 
    }

    private void CalculateAcceleration() 
    { 
        // Mengecek apakah player character bergerak atau tidak 
        if (_movementDirection.magnitude > 0.01) 
        { 
            // Jika karakter bergerak maka akan mengecek 
            // Apakah character sedang sprint atau tidak 
            if (_isSprint) 
            { 
                // Jika sedang sprint maka kecepatan akan bertambah  
                // sebesar nilai acceleration setiap detik 
                _currentSpeed = _currentSpeed + _acceleration * Time.deltaTime; 
            } 
            else 
            { 
                // Jika berhenti sprint maka kecepatan akan berkurang  
                // sebesar nilai acceleration setiap detik 
                _currentSpeed = _currentSpeed - _acceleration * Time.deltaTime; 
            } 
            // Membatasi kecepatan, minimum: walk speed maksimum: sprint speed  
            _currentSpeed = Mathf.Clamp(_currentSpeed, _walkSpeed, _sprintSpeed); 
        } 
        else 
        { 
            // Jika tidak bergerak kecepatan diset nol,  
           // agar character tidak bisa bergerak 
            _currentSpeed = 0; 
        } 
    }
    
    private void Update()
    {
        CheckIsGrounded();
        CalculateAcceleration(); 
        ResetVelocityY();  
        Move();
    }

}