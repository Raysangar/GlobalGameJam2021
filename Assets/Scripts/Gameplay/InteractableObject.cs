using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class InteractableObject : MonoBehaviour
{
    public bool HasFacemask { get; private set; }

    public bool CanBeGoodFacemask(int currentLevel)
    {
        return canBeGoodFacemask && (currentLevel > 0 || shouldAppearOnFirstLevel);
    }

    public void Initialize(PlayerInput input)
    {
        this.input = input;
        nearCollider = GetComponent<Collider2D>();
    }

    public void SetupForLevel(bool hasFacemask, bool isTutorial)
    {
        HasFacemask = hasFacemask;
        this.isTutorial = isTutorial;
        gameObject.SetActive(!isTutorial || shouldAppearOnFirstLevel);
        spriteRenderer.sprite = closedSprite;
        nearCollider.enabled = true;
        keyboardInteraction.SetActive(false);
        playstationGamepadInteraction.SetActive(false);
        genericGamepadInteraction.SetActive(false);
    }

    public bool Open()
    {
        nearCollider.enabled = false;
        spriteRenderer.sprite = openedSprite;
        SoundManager.Instance.PlayGrabObjectSound(openSound);
        if (!HasFacemask)
            DialogController.Instance.ShowWrongFacemaskDialog(isTutorial ? firstLevelCustomWrongDialog : customWrongDialog);
        return HasFacemask;
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
    [SerializeField] bool shouldAppearOnFirstLevel;
    [SerializeField] string firstLevelCustomWrongDialog;

    [Header("Interaction Feedback")]
    [SerializeField] GameObject keyboardInteraction;
    [SerializeField] GameObject playstationGamepadInteraction;
    [SerializeField] GameObject genericGamepadInteraction;
    private bool isTutorial;
    private Collider2D nearCollider;
    private PlayerInput input;
}
