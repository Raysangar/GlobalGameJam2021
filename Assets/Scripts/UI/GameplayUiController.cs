using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayUiController : MonoBehaviour
{
    public void Initialize(GameManager gameManager, System.Action startNewGameCallback, System.Action showMainMenuCallback)
    {
        this.gameManager = gameManager;
        this.showMainMenuCallback = showMainMenuCallback;
        this.startNewGameCallback = startNewGameCallback;
        this.gameManager.OnGameOver += OnGameOver;
        playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        popupParent.SetActive(false);
    }

    private void Update()
    {
        int seconds = (int)gameManager.SecondsLeft;
        timer.text = seconds / 60 + ":" + (seconds % 60).ToString("N2");
    }

    private void OnGameOver()
    {
        popupParent.SetActive(true);
    }

    private void OnMainMenuButtonClicked()
    {
        showMainMenuCallback();
    }

    private void OnPlayAgainButtonClicked()
    {
        startNewGameCallback();
    }

    [Header("HUD")]
    [SerializeField] TMP_Text timer;
    [SerializeField] TMP_Text levelText;

    [Header("End Game Popup")]
    [SerializeField] GameObject popupParent;
    [SerializeField] TMP_Text popupMessage;
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button playAgainButton;

    private GameManager gameManager;
    private System.Action startNewGameCallback;
    private System.Action showMainMenuCallback;
}
