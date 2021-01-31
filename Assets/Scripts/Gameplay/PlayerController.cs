using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public System.Action OnFacemaskFound;

    public Collider2D Collider { get; private set; }

    public void Reset(bool playIntro)
    {
        animator.SetBool(FacemaskAnimHash, false);
        animator.SetBool(HappyAnimHash, playIntro);
        animator.SetInteger(HorizontalAnimHash, 0);
        animator.SetInteger(VerticalAnimHash, 0);
    }

    public void GetUngry()
    {
        animator.SetBool(HappyAnimHash, false);
    }

    public void OnPlayerMove(InputAction.CallbackContext context)
    {
        var movement = context.ReadValue<Vector2>();
        Move(movement);
    }

    public void Move(Vector2 movement)
    {
        if (movement.x != 0 && movement.x != direction.x)
            spriteRenderer.flipX = movement.x < 0;
        direction = movement;
        
        animator.SetInteger(HorizontalAnimHash, Mathf.RoundToInt(direction.x));
        animator.SetInteger(VerticalAnimHash, Mathf.RoundToInt(direction.y));

        if (movement == Vector2.zero)
            SoundManager.Instance.StopPlayerSteps();
        else
            SoundManager.Instance.PlayPlayerSteps();
    }

    public void OnPlayerTriedInteracting(InputAction.CallbackContext context)
    {
        if (nearInteractableObjects.Count > 0)
        {
            if (nearInteractableObjects[0].Open())
            {
                OnFacemaskFound();
                animator.SetBool(FacemaskAnimHash, true);
            }
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        nearInteractableObjects = new List<InteractableObject>();
        Collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var interactableObject = collision.GetComponent<InteractableObject>();
        if (interactableObject != null)
            nearInteractableObjects.Add(interactableObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var interactableObject = collision.GetComponent<InteractableObject>();
        if (interactableObject != null)
            nearInteractableObjects.Remove(interactableObject);
    }

    [SerializeField] float speed;
    
    private Vector2 direction;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private List<InteractableObject> nearInteractableObjects;
    private readonly int FacemaskAnimHash = Animator.StringToHash("facemask");
    private readonly int HappyAnimHash = Animator.StringToHash("happy");
    private readonly int VerticalAnimHash = Animator.StringToHash("vertical");
    private readonly int HorizontalAnimHash = Animator.StringToHash("horizontal");
}
