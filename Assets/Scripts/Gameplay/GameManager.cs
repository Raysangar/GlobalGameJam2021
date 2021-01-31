using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public System.Action<System.Action> OnGameAboutToStart;
    public System.Action OnGameStarted;
    public System.Action OnGameOver;
    public System.Action<bool> OnGamePuasedStateChanged;

    public float SecondsLeft { get; private set; }
    public int CurrentLevel { get; private set; }

    public void Initialize(PlayerController player, ScenesManager scenesManager, PlayerInput input, IntroController introController)
    {
        this.input = input;
        this.player = player;
        this.scenesManager = scenesManager;
        this.introController = introController;
        player.OnFacemaskFound += OnPlayerFoundFacemask;
        enabled = false;
        input.enabled = false;
    }

    public void StartGame(bool playIntro)
    {
        CurrentLevel = 0;
        if (playIntro)
            introController.StartIntro(SetupLevel);
        else
            SetupLevel();
    }

    public void StartNextLevel()
    {
        playingEndGameAnim = false;
        ++CurrentLevel;
        SetupLevel();
    }

    public void OnStartButtonClicked(InputAction.CallbackContext context)
    {
        if (!playingEndGameAnim)
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            OnGamePuasedStateChanged(Time.timeScale == 0);
        }
    }

    private void Update()
    {
        if (!playingEndGameAnim && CurrentLevel > 0)
        {
            SecondsLeft -= Time.deltaTime;
            if (SecondsLeft <= 0)
            {
                SecondsLeft = 0;
                GameOver();
            }
        }
    }

    private void SetupLevel()
    {
        OnGameAboutToStart(OnLevelReadyToShow);
    }

    private void OnLevelReadyToShow()
    {
        var levelDifficulty = difficultySettings.GetFacemasksSetupForLevel(CurrentLevel);
        SecondsLeft = levelDifficulty.TimerDuration;
        scenesManager.StartGame(levelDifficulty.FacemasksSetup, CurrentLevel);
        enabled = true;
        input.enabled = true;
        player.transform.position = Vector3.zero;
        player.Move(Vector2.zero);
        player.Reset(false);
        introController.HideIntro();
        SoundManager.Instance.PlayTransitionSound();
        OnGameStarted();
    }

    private void OnPlayerFoundFacemask()
    {
        playingEndGameAnim = true;
        input.enabled = false;
        StartCoroutine(NextLevelCoroutine());
    }

    private IEnumerator NextLevelCoroutine()
    {
        yield return new WaitForSeconds(3);
        StartNextLevel();
    }

    private void GameOver()
    {
        playingEndGameAnim = true;
        input.enabled = false;
        OnGameOver();
    }

    [SerializeField] DifficultySettings difficultySettings;

    private PlayerController player;
    private PlayerInput input;
    private ScenesManager scenesManager;
    private IntroController introController;
    private bool playingEndGameAnim;
}
