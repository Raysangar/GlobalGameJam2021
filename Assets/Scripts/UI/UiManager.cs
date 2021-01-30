using UnityEngine;

public class UiManager : MonoBehaviour
{
    public void Initialize(GameManager gameManager, ScenesManager scenesManager)
    {
        this.gameManager = gameManager;
        mainMenu.Initialize(StartNewGame);
        gameplayUi.Initialize(gameManager, scenesManager, StartNewGame, ShowMainMenu);
        ShowMainMenu();
    }

    private void StartNewGame()
    {
        gameplayUi.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
        gameManager.StartGame();
    }

    private void ShowMainMenu()
    {
        gameplayUi.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

    [SerializeField] GameplayUiController gameplayUi;
    [SerializeField] MainMenuController mainMenu;

    private GameManager gameManager;
}
