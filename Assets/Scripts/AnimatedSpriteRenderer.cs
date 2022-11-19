using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSpriteRenderer : MonoBehaviour
{
    public bool idle = true;

    [SerializeField] private float animationTime = 0.25f;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite[] animationSprites;
    [SerializeField] private bool loop = true;

    private SpriteRenderer _spriteRenderer;
    private int _animationFrame;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        _spriteRenderer.enabled = false;
    }

    private void Start()
    {
        InvokeRepeating(nameof(NextFrame), animationTime, animationTime);
    }

    private void NextFrame()
    {
        _animationFrame++;

        if (loop && _animationFrame >= animationSprites.Length)
            _animationFrame = 0;

        if (idle)
            _spriteRenderer.sprite = idleSprite;
        else if (_animationFrame >= 0 && _animationFrame < animationSprites.Length)
            _spriteRenderer.sprite = animationSprites[_animationFrame];
    }
}