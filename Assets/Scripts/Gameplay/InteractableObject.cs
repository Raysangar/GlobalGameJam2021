using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;

public class InteractableObject : MonoBehaviour
{
    public bool CanBeGoodFacemask => canBeGoodFacemask;

    public void Initialize(PlayerInput input)
    {
        this.input = input;
    }

    public void SetupForLevel(bool hasFacemask)
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
        SoundManager.Instance.PlayGrabObjectSound(openSound);
        if (!hasFacemask)
            DialogController.Instance.ShowWrongFacemaskDialog(customWrongDialog);
        return hasFacemask;
    }

    private void Awake()
    {
        nearCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (input.currentControlScheme == "Gamepad")
        {
            int i = 0;
            Gamepad gamepad;
            do
            {
                gamepad = input.devices[i++] as Gamepad;
            }
            while (i < input.devices.Count && gamepad == null);
            if (gamepad != null)
            {
                if (gamepad is DualShockGamepad)
                    playstationGamepadInteraction.SetActive(true);
                else
                    genericGamepadInteraction.SetActive(true);
            }
            else
                keyboardInteraction.SetActive(true);
        }
        else
            keyboardInteraction.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        keyboardInteraction.SetActive(false);
        playstationGamepadInteraction.SetActive(false);
        genericGamepadInteraction.SetActive(false);
    }

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite closedSprite;
    [SerializeField] Sprite openedSprite;
    [SerializeField] AudioClip openSound;
    [SerializeField] bool canBeGoodFacemask = true;
    [SerializeField] string customWrongDialog;

    [Header("Interaction Feedback")]
    [SerializeField] GameObject keyboardInteraction;
    [SerializeField] GameObject playstationGamepadInteraction;
    [SerializeField] GameObject genericGamepadInteraction;

    private bool hasFacemask;
    private Collider2D nearCollider;
    private PlayerInput input;
}
