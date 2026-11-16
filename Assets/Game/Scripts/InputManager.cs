using UnityEngine; 
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static GameInputAction; 
public class InputManager : MonoBehaviour, IPlayerActions  
{ 
    // Variable untuk menyimpan reference object input action 
    private GameInputAction _inputAction;
    // Property untuk menyimpan nilai pergerakan mouse (Delta)
    public Vector2 LookInput { get; private set; }
    // Membuat event OnMoveInput 
    public UnityEvent<Vector2> OnMoveInput; 
    public UnityEvent<bool> OnSprintInput;
    private void Awake() 
    { 
        // Membuat object GameInputAction dan menyimpan reference nya 
        // ke variable _inputAction 
        _inputAction = new GameInputAction(); 
        // Mengaktifkan input action 
        _inputAction.Enable(); 
        // Mengaktifkan action map Player 
        _inputAction.Player.Enable(); 
        // Memberi tahu bahwa kelas ini akan mendeteksi input dari 
        // action map Player 
        _inputAction.Player.SetCallbacks(this); 
    } 
    public void OnInteract(InputAction.CallbackContext context) 
    { 
        // Menulis code yang akan dieksekusi 
        // ketika tombol interact ditekan 
        // contect.performed digunakan untuk mengecek apakah input ditekan 
        if (context.performed) 
        { 
            // Memunculkan log interact di console  
            // ketika input interact ditekan 
            Debug.Log("Interact"); 
        } 
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        // Memanggil on move input ketika input move ditekan dan dilepas 
        // Event akan mengirimkan data arah input 
        OnMoveInput?.Invoke(context.ReadValue<Vector2>()); 
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed) OnSprintInput?.Invoke(true);
        else if (context.canceled) OnSprintInput?.Invoke(false);
    }
} 