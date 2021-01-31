using UnityEngine;

public class UiManager : MonoBehaviour
{
    public void Initialize(GameManager gameManager, ScenesManager scenesManager, PlayerController player)
    {
        this.gameManager = gameManager;
        gameManager.OnGameAboutToStart += OnGameAboutToStart;
        mainMenu.Initialize(StartNewGame);
        gameplayUi.Initialize(gameManager, scenesManager, PlayAgainCallback, ShowMainMenu);
        dialogController.Initialize(gameManager, player);
        ShowMainMenu();
    }

    private void StartNewGame()
    {
        mainMenu.gameObject.SetActive(false);
        gameManager.StartGame(true);
    }

    private void PlayAgainCallback()
    {
        gameManager.StartGame(false);
    }

    private void OnGameAboutToStart(System.Action _)
    {
        gameplayUi.gameObject.SetActive(true);
    }

    private void ShowMainMenu()
    {
        gameplayUi.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

    [SerializeField] GameplayUiController gameplayUi;
    [SerializeField] MainMenuController mainMenu;
    [SerializeField] DialogController dialogController;

    private GameManager gameManager;
}
