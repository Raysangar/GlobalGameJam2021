using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UiManager), typeof(ScenesManager), typeof(GameManager))]
[RequireComponent(typeof(PlayerInput))]
public class Main : MonoBehaviour
{
    private void Awake()
    {
        uiManager = GetComponent<UiManager>();
        gameManager = GetComponent<GameManager>();
        input = GetComponent<PlayerInput>();
        scenesManager = GetComponent<ScenesManager>();

        gameManager.Initialize(player, scenesManager, input);
        scenesManager.Initialize(player);
        uiManager.Initialize(gameManager, scenesManager);
    }

    [SerializeField] PlayerController player;

    private GameManager gameManager;
    private ScenesManager scenesManager;
    private PlayerInput input;
    private UiManager uiManager;
}
