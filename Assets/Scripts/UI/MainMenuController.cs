using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public void Initialize(System.Action startNewGameCallback)
    {
        this.startNewGameCallback = startNewGameCallback;
        startGameButton.onClick.AddListener(OnStartGameButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
        creditsButton.onClick.AddListener(OnCreditsButtonClicked);

        WaitUntilCreditsReachTargetPositionForFadeIn = new WaitUntil(CreditsReachedTargetPositionForFadeIn);
    }

    private void OnStartGameButtonClicked()
    {
        SoundManager.Instance.PlayButtonClickedSound();
        SoundManager.Instance.PlayMainMusic();
        startNewGameCallback();
    }

    private void OnQuitButtonClicked()
    {
        SoundManager.Instance.PlayButtonClickedSound();
        Application.Quit();
    }

    private void OnCreditsButtonClicked()
    {
        SoundManager.Instance.PlayButtonClickedSound();
        StartCoroutine(MainMenuFadeCoroutine());
        StartCoroutine(CreditsMovementCoroutine());
    }

    private IEnumerator MainMenuFadeCoroutine()
    {
        mainMenuCanvasGroup.interactable = false;

        float time = 0;
        while (time < creditsFadeDuration)
        {
            mainMenuCanvasGroup.alpha = (creditsFadeDuration - time) / creditsFadeDuration;
            time += Time.deltaTime;
            yield return null;
        }
        mainMenuCanvasGroup.alpha = 0;

        yield return WaitUntilCreditsReachTargetPositionForFadeIn;

        time = 0;
        while (time < creditsFadeDuration)
        {
            mainMenuCanvasGroup.alpha = creditsFadeDuration - time;
            time += Time.deltaTime;
            yield return null;
        }
        mainMenuCanvasGroup.alpha = 1;
        mainMenuCanvasGroup.interactable = true;
    }

    private IEnumerator CreditsMovementCoroutine()
    {
        Vector2 pos = new Vector2(0, credtisInitialPosition);
        creditsParent.anchoredPosition = pos;
        while(creditsParent.anchoredPosition.y < credtisTargetPosition)
        {
            yield return null;
            pos.y += creditsSpeed * Time.deltaTime;
            creditsParent.anchoredPosition = pos;
        }
    }

    private bool CreditsReachedTargetPositionForFadeIn()
    {
        return creditsParent.anchoredPosition.y >= creditsTargetPositionForFadeIn;
    }

    private void OnEnable()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayMainMenuMusic();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(startGameButton.gameObject);
    }

    private void Update()
    {
        var position = selectionImage.position;
        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null)
            position.y = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.position.y;
        selectionImage.position = position;
    }

    [SerializeField] Button startGameButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button creditsButton;
    [SerializeField] Transform selectionImage;

    [Header("Credits Animation")]
    [SerializeField] CanvasGroup mainMenuCanvasGroup;
    [SerializeField] RectTransform creditsParent;
    [SerializeField] float creditsFadeDuration;
    [SerializeField] float credtisInitialPosition;
    [SerializeField] float credtisTargetPosition;
    [SerializeField] float creditsTargetPositionForFadeIn;
    [SerializeField] float creditsSpeed;

    private System.Action startNewGameCallback;

    private WaitUntil WaitUntilCreditsReachTargetPositionForFadeIn;
}
