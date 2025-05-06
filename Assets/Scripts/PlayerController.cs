using Assets.Scripts.GameEvents;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public delegate void LifeUpdate(int nuevaVida);
    public static event LifeUpdate OnLifeChanged;

    [Header("Configuración Básica")]
    [SerializeField] private int maxLife = 10;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Referencias")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private GameEvent winEvent;
    [SerializeField] private GameEvent lossEvent;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private int currentLife;
    private int currentPoints;
    private bool isGrounded;
    private int currentColorID;

    public int MaxLife => maxLife;
    public int CurrentLife => currentLife;
    public int CurrentPoints => currentPoints;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentLife = maxLife;
        OnLifeChanged?.Invoke(currentLife);
    }

    private void Update()
    {
        CheckGround();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float direction = context.ReadValue<float>();
            rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheckPoint.position,
            groundCheckRadius,
            groundLayer
        );
    }

    public void ChangeColor(Color newColor, int colorID)
    {
        spriteRenderer.color = newColor;
        currentColorID = colorID;
        Debug.Log($"Color cambiado a: {newColor}");
    }

    public void SetRed() => ChangeColor(Color.red, 1);
    public void SetBlue() => ChangeColor(Color.blue, 2);
    public void SetGreen() => ChangeColor(Color.green, 3);
    public void SetYellow() => ChangeColor(Color.yellow, 4);
    public void SetCyan() => ChangeColor(Color.cyan, 5);

    public void TakeDamage(int damage)
    {
        currentLife = Mathf.Max(currentLife - damage, 0);
        OnLifeChanged?.Invoke(currentLife);

        if (currentLife <= 0)
        {
            lossEvent?.Raise();
        }
    }

    public void Heal(int amount)
    {
        currentLife = Mathf.Min(currentLife + amount, maxLife);
        OnLifeChanged?.Invoke(currentLife);
    }

    public void AddPoints(int points)
    {
        currentPoints += points;
        PointsSystem.UpdatePoints(currentPoints);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Victoria"))
        {
            winEvent?.Raise();
            return;
        }

        if (collision.CompareTag("Moneda"))
        {
            AddPoints(10);
            Destroy(collision.gameObject);
            return;
        }

        if (collision.CompareTag("Corazon"))
        {
            Heal(3);
            Destroy(collision.gameObject);
            return;
        }

        if (collision.CompareTag("Red") || collision.CompareTag("Blue") || collision.CompareTag("Yellow"))
        {
            bool applyDamage = false;

            if (collision.CompareTag("Red") && spriteRenderer.color != Color.red)
                applyDamage = true;
            else if (collision.CompareTag("Blue") && spriteRenderer.color != Color.blue)
                applyDamage = true;
            else if (collision.CompareTag("Yellow") && spriteRenderer.color != Color.yellow)
                applyDamage = true;

            if (applyDamage)
            {
                TakeDamage(1);
            }
            else
            {
                Debug.Log("Colisión segura: el color del jugador coincide con el muro.");
            }
        }
    }
}
