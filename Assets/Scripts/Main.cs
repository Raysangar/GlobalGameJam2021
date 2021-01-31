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
        introController = GetComponent<IntroController>();

        cameraController.Initialize(player);
        introController.Initialize(player, input);
        gameManager.Initialize(player, scenesManager, input, introController);
        scenesManager.Initialize(player, cameraController);
        uiManager.Initialize(gameManager, scenesManager, player);
    }

    [SerializeField] PlayerController player;
    [SerializeField] CameraController cameraController;

    private GameManager gameManager;
    private ScenesManager scenesManager;
    private PlayerInput input;
    private UiManager uiManager;
    private IntroController introController;
}
