using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public System.Action OnGameOver;
    public float SecondsLeft { get; private set; }
    public int CurrentLevel;

    public void Initialize(PlayerController player, ScenesManager scenesManager, PlayerInput input)
    {
        this.input = input;
        this.player = player;
        this.scenesManager = scenesManager;
        player.OnPlayerFoundFacemask += OnPlayerFoundFacemask;
        enabled = false;
        input.enabled = false;
    }

    public void StartGame()
    {
        CurrentLevel = 0;
        SetupLevel();
    }

    public void StartNextLevel()
    {
        ++CurrentLevel;
        SetupLevel();
    }

    private void Start()
    {
        StartGame();
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
        SecondsLeft = 120;
        var sceneIds = System.Enum.GetValues(typeof(Scene.ID));
        var facemaskInMap = new Dictionary<Scene.ID, int>()
        {
            { (Scene.ID)sceneIds.GetValue(Random.Range(0, sceneIds.Length)), 1 }
        };
        scenesManager.StartGame(facemaskInMap);
        enabled = true;
        input.enabled = true;
        player.transform.position = Vector3.zero;
    }

    private void OnPlayerFoundFacemask()
    {
        playingEndGameAnim = true;
        input.enabled = false;
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
    private bool playingEndGameAnim;
}
