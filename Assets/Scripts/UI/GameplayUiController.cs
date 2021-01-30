using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameplayUiController : MonoBehaviour
{
    public void Initialize(GameManager gameManager, ScenesManager scenesManager, System.Action startNewGameCallback, System.Action showMainMenuCallback)
    {
        scenesManager.OnPlayerGoesToNewScene += OnPlayerGoesToNewScene;
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
        timer.text = seconds / 60 + ":" + (seconds % 60).ToString("D2");
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

    private void OnPlayerGoesToNewScene(System.Action transitionTimeCallback)
    {
        sceneTransitionAnimator.SetTrigger(TransitionTrigger);
        StartCoroutine(WaitForCoroutine(transitionTimeCallback));
    }

    private IEnumerator WaitForCoroutine(System.Action callback)
    {
        yield return WaitForTransition;
        callback();
    }

    [Header("HUD")]
    [SerializeField] TMP_Text timer;
    [SerializeField] TMP_Text levelText;

    [Header("End Game Popup")]
    [SerializeField] GameObject popupParent;
    [SerializeField] TMP_Text popupMessage;
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button playAgainButton;

    [Header("Screen Transition")]
    [SerializeField] Animator sceneTransitionAnimator;

    private GameManager gameManager;
    private System.Action startNewGameCallback;
    private System.Action showMainMenuCallback;

    private readonly int TransitionTrigger = Animator.StringToHash("transition");
    private readonly WaitForSeconds WaitForTransition = new WaitForSeconds(.5f);
}
