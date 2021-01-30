using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public System.Action<System.Action> OnGameAboutToStart;
    public System.Action OnGameOver;
    public float SecondsLeft { get; private set; }
    public int CurrentLevel;

    public void Initialize(PlayerController player, ScenesManager scenesManager, PlayerInput input, IntroController introController)
    {
        this.input = input;
        this.player = player;
        this.scenesManager = scenesManager;
        this.introController = introController;
        player.OnPlayerFoundFacemask += OnPlayerFoundFacemask;
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
        ++CurrentLevel;
        SetupLevel();
    }

    private void Update()
    {
        if (!playingEndGameAnim)
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
        SecondsLeft = 120;
        var sceneIds = System.Enum.GetValues(typeof(Scene.ID));
        var facemaskInMap = new Dictionary<Scene.ID, int>()
        {
            { Scene.ID.Entrance, 1 }
        };
        scenesManager.StartGame(facemaskInMap);
        enabled = true;
        input.enabled = true;
        player.transform.position = Vector3.zero;
        player.Move(Vector2.zero);
        player.Reset(false);
        introController.HideIntro();
    }

    private void OnPlayerFoundFacemask()
    {
        playingEndGameAnim = true;
        input.enabled = false;
        StartCoroutine(NextLevelCoroutine());
    }

    private IEnumerator NextLevelCoroutine()
    {
        yield return new WaitForSeconds(2);
        StartNextLevel();
    }

    private void GameOver()
    {
        playingEndGameAnim = true;
        input.enabled = false;
        OnGameOver();
    }

    private PlayerController player;
    private PlayerInput input;
    private ScenesManager scenesManager;
    private IntroController introController;
    private bool playingEndGameAnim;
}
