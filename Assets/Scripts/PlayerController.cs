using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public delegate void LifeUpdate(int newLife);
    public delegate void PointsUpdate(int newPoints);
    public delegate void GameState();
    
    public static event LifeUpdate OnLifeChanged;
    public static event PointsUpdate OnPointsChanged;
    public static event GameState OnPlayerDeath;
    public static event GameState OnPlayerWin;

    [Header("Configuración Básica")]
    [SerializeField] private int maxLife = 10;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Estado del Jugador")]
    [SerializeField] private int _currentLife;
    [SerializeField] private int currentPoints = 0;

    private Rigidbody2D rb;
    public int CurrentLife => _currentLife;
    public int MaxLife => maxLife;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _currentLife = maxLife;
    }

    public void TakeDamage(int damage)
    {
        _currentLife = Mathf.Max(_currentLife - damage, 0);
        OnLifeChanged?.Invoke(_currentLife);
        
        if(_currentLife <= 0) HandleDefeat();
    }

    public void Heal(int amount)
    {
        _currentLife = Mathf.Min(_currentLife + amount, maxLife);
        OnLifeChanged?.Invoke(_currentLife);
    }

    public void AddPoints(int points)
    {
        currentPoints += points;
        OnPointsChanged?.Invoke(currentPoints);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            rb.velocity = new Vector2(input.x * moveSpeed, rb.velocity.y);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed && CheckGround())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private bool CheckGround()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Muerte")) HandleDefeat();
        if(collision.CompareTag("Victoria")) HandleVictory();
    }

    private void HandleDefeat()
    {
        OnPlayerDeath?.Invoke();
        SceneManager.LoadScene("YouLoss");
    }

    private void HandleVictory()
    {
        OnPlayerWin?.Invoke();
        SceneManager.LoadScene("YouWin");
    }
}