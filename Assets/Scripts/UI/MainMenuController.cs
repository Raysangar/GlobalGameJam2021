using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public void Initialize(System.Action startNewGameCallback)
    {
        this.startNewGameCallback = startNewGameCallback;
        startGameButton.onClick.AddListener(OnStartGameButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnStartGameButtonClicked()
    {
        startNewGameCallback();
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    [SerializeField] Button startGameButton;
    [SerializeField] Button quitButton;

    private System.Action startNewGameCallback;
}
