using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public void Initialize(bool hasFacemask)
    {
        this.hasFacemask = hasFacemask;
        interactionCanvas.enabled = false;
        spriteRenderer.sprite = closedSprite;
    }

    public bool Open()
    {
        nearCollider.enabled = false;
        spriteRenderer.sprite = openedSprite;
        return hasFacemask;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactionCanvas.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactionCanvas.enabled = false;
    }

    [SerializeField] Collider2D nearCollider;
    [SerializeField] Canvas interactionCanvas;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite closedSprite;
    [SerializeField] Sprite openedSprite;

    private bool hasFacemask;
}
