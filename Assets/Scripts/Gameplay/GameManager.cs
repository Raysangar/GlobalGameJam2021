using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public float SecondsLeft { get; private set; }

    private void Awake()
    {
        player.OnPlayerFoundFacemask += OnPlayerFoundFacemask;
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

    private void OnPlayerFoundFacemask()
    {
        playingEndGameAnim = true;
        input.enabled = false;
    }

    private void GameOver()
    {
        playingEndGameAnim = true;
        input.enabled = false;
    }

    [SerializeField] PlayerController player;
    [SerializeField] PlayerInput input;

    private bool playingEndGameAnim;
}
