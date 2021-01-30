using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public void Initialize(bool hasFacemask)
    {
        this.hasFacemask = hasFacemask;
        spriteRenderer.sprite = closedSprite;
        keyboardInteraction.SetActive(false);
        playstationGamepadInteraction.SetActive(false);
        genericGamepadInteraction.SetActive(false);
    }

    public bool Open()
    {
        nearCollider.enabled = false;
        spriteRenderer.sprite = openedSprite;
        return hasFacemask;
    }

    private void Awake()
    {
        nearCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        keyboardInteraction.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        keyboardInteraction.SetActive(false);
    }

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite closedSprite;
    [SerializeField] Sprite openedSprite;

    [Header("Interaction Feedback")]
    [SerializeField] GameObject keyboardInteraction;
    [SerializeField] GameObject playstationGamepadInteraction;
    [SerializeField] GameObject genericGamepadInteraction;

    private bool hasFacemask;
    private Collider2D nearCollider;
}
