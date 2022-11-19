using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _direction = Vector2.down;

    [SerializeField] private KeyCode _inputUp = KeyCode.W;
    [SerializeField] private KeyCode _inputDown = KeyCode.S;
    [SerializeField] private KeyCode _inputLeft = KeyCode.A;
    [SerializeField] private KeyCode _inputRight = KeyCode.D;

    [Header("Sprite Renderers")] 
    [SerializeField] private AnimatedSpriteRenderer spriteRendererUp;
    [SerializeField] private AnimatedSpriteRenderer spriteRendererDown;
    [SerializeField] private AnimatedSpriteRenderer spriteRendererLeft;
    [SerializeField] private AnimatedSpriteRenderer spriteRendererRight;
    [SerializeField] private AnimatedSpriteRenderer spriteRendererDeath;

    private AnimatedSpriteRenderer _activeSpriteRenderer;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _activeSpriteRenderer = spriteRendererDown;
    }

    private void Update()
    {
        if (Input.GetKey(_inputUp))
        {
            SetDirection(Vector2.up, spriteRendererUp);
        }
        else if (Input.GetKey(_inputDown))
        {
            SetDirection(Vector2.down, spriteRendererDown);
        }
        else if (Input.GetKey(_inputLeft))
        {
            SetDirection(Vector2.left, spriteRendererLeft);
        }
        else if (Input.GetKey(_inputRight))
        {
            SetDirection(Vector2.right, spriteRendererRight);
        }
        else
        {
            SetDirection(Vector2.zero, _activeSpriteRenderer);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
            DeathSequence();
    }

    private void DeathSequence()
    {
        enabled = false;
        GetComponent<BombController>().enabled = false;
        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDeath.enabled = true;
        
        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
        FindObjectOfType<GameManager>().CheckWinState();
    }


    private void FixedUpdate()
    {
        Vector2 position = _rigidbody2D.position;
        Vector2 translation = _direction * speed * Time.fixedDeltaTime;
        _rigidbody2D.MovePosition(position + translation);
    }

    private void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        _direction = newDirection;

        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        _activeSpriteRenderer = spriteRenderer;
        _activeSpriteRenderer.idle = _direction == Vector2.zero;
    }
}