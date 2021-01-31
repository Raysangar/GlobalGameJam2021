using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class IntroController : MonoBehaviour
{
    public void Initialize(PlayerController player, PlayerInput input)
    {
        this.player = player;
        this.input = input;
        ResetValues();
    }

    public void StartIntro(System.Action introFinishedCallback)
    {
        ResetValues();
        StartCoroutine(IntroCoroutine(introFinishedCallback));
    }

    public void HideIntro()
    {
        introScene.SetActive(false);
    }

    private void ResetValues()
    {
        input.enabled = false;
        introScene.SetActive(true);
        player.transform.position = playerInitialPosition.position;
        introAnimator.SetBool(IntroTrigger, true);
        player.Reset(true);
    }


    private IEnumerator IntroCoroutine(System.Action introFinishedCallback)
    {
        player.Move(Vector2.right);
        SoundManager.Instance.PlaySound(backgroundSounds);
        yield return new WaitUntil(() => Vector2.Distance(player.transform.position, playerFinalPosition.position) < .01f);
        player.Move(Vector2.zero);
        player.GetUngry();
        introAnimator.SetBool(IntroTrigger, false);
        DialogController.Instance.ShowDialog(Dialog);
        yield return new WaitForSeconds(2);
        player.Move(Vector2.left);
        yield return new WaitUntil(() => Vector2.Distance(player.transform.position, playerInitialPosition.position) < .01f);
        SoundManager.Instance.StopSound(backgroundSounds);
        introFinishedCallback();
    }

    [SerializeField] GameObject introScene;
    [SerializeField] Transform playerInitialPosition;
    [SerializeField] Transform playerFinalPosition;
    [SerializeField] Animator introAnimator;
    [SerializeField] AudioClip backgroundSounds;

    private PlayerController player;
    private PlayerInput input;

    private readonly int IntroTrigger = Animator.StringToHash("happy");

    private static readonly string[] Dialog = new string[]
    {
        "ARGGGGGG! I FORGOT MY FACEMASK AGAIN!!",
        "I HAVE TO GO BACK HOME AND GET IT BEFORE THE SUPERMARKET CLOSES"
    };
}
