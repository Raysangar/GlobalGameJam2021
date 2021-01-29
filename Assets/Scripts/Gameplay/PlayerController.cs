using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public System.Action OnPlayerFoundFacemask;

    public void Initialize(bool playIntro)
    {
        animator.SetBool(FacemaskAnimHash, false);
        animator.SetBool(HappyAnimHash, playIntro);
        animator.SetInteger(HorizontalAnimHash, 0);
        animator.SetInteger(VerticalAnimHash, 0);
    }

    public void OnPlayerMove(InputAction.CallbackContext context)
    {
        var newDirection = context.ReadValue<Vector2>();
        if (newDirection.x != 0 && newDirection.x != direction.x)
            spriteRenderer.flipX = newDirection.x < 0;
        direction = newDirection;
        animator.SetInteger(HorizontalAnimHash, Mathf.RoundToInt(direction.x));
        animator.SetInteger(VerticalAnimHash, Mathf.RoundToInt(direction.y));
    }

    public void OnPlayerTriedInteracting(InputAction.CallbackContext context)
    {
        if (nearInteractableObject != null)
        {
            if (nearInteractableObject.Open())
            {
                OnPlayerFoundFacemask();
                animator.SetBool(FacemaskAnimHash, true);
            }
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        nearInteractableObject = collision.GetComponent<InteractableObject>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        nearInteractableObject = null;
    }

    [SerializeField] float speed;
    
    private Vector2 direction;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private InteractableObject nearInteractableObject;

    private readonly int FacemaskAnimHash = Animator.StringToHash("facemask");
    private readonly int HappyAnimHash = Animator.StringToHash("happy");
    private readonly int VerticalAnimHash = Animator.StringToHash("vertical");
    private readonly int HorizontalAnimHash = Animator.StringToHash("horizontal");
}
