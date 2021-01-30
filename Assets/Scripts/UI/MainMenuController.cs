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

    private void Update()
    {
        var position = selectionImage.position;
        position.y = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.position.y;
        selectionImage.position = position;
    }

    [SerializeField] Button startGameButton;
    [SerializeField] Button quitButton;
    [SerializeField] Transform selectionImage;

    private System.Action startNewGameCallback;
}
