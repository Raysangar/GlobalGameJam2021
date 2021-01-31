using UnityEngine;
using TMPro;
using System.Collections;

public class DialogController : MonoBehaviour
{
    public static DialogController Instance { get; private set; }

    public void Initialize(GameManager gameManager, PlayerController player)
    {
        Instance = this;
        this.gameManager = gameManager;
        gameManager.OnGamePuasedStateChanged += OnGamePausedStateChanged;
        gameManager.OnGameStarted += OnGameStarted;
        player.OnFacemaskFound += OnPlayerFoundFacemask;
        gameManager.OnGameOver += OnGameOver;
        dialogParent.alpha = 0;
    }

    public void ShowWrongFacemaskDialog(string message)
    {
        if (string.IsNullOrEmpty(message))
            ShowDialog(WrongFacemaskDialog[Random.Range(0, WrongFacemaskDialog.Length)]);
        else
            ShowDialog(message);
    }

    public void ShowDialog(string message)
    {
        CancelHideDialogIfNecessary();
        dialogParent.alpha = 1;
        dialog.text = message;
        dialogCoroutine = StartCoroutine(HideDialogCoroutine());
    }

    public void ShowDialog(string[] messages)
    {
        CancelHideDialogIfNecessary();
        dialogParent.alpha = 1;
        dialogCoroutine = StartCoroutine(ShowDialogCoroutine(messages));
    }

    private void OnGameOver()
    {
        dialogParent.alpha = 0;
    }

    private void OnPlayerFoundFacemask()
    {
        dialogParent.alpha = 1;
        dialog.text = "FINALLY! I FOUND IT!";
    }


    private void OnGameStarted()
    {
        HideDialog();
        if (gameManager.CurrentLevel > 0)
            ShowDialog("Level " + gameManager.CurrentLevel);
    }

    private void OnGamePausedStateChanged(bool paused)
    {
        if (paused)
        {
            CancelHideDialogIfNecessary();
            dialogParent.alpha = 1;
            dialog.text = "PAUSE";
        }
        else
        {
            dialogParent.alpha = 0;
        }
    }


    private void HideDialog()
    {
        CancelHideDialogIfNecessary();
        dialogParent.alpha = 0;
    }

    private void CancelHideDialogIfNecessary()
    {
        if (dialogCoroutine != null)
        {
            StopCoroutine(dialogCoroutine);
            dialogCoroutine = null;
        }
    }

    private IEnumerator ShowDialogCoroutine(string[] messages)
    {
        int i = 0;
        do
        {
            dialog.text = messages[i++];
            yield return WaitForSeconds;
        } while (i < messages.Length);
        HideDialog();
    }

    private IEnumerator HideDialogCoroutine()
    {
        yield return WaitForSeconds;
        HideDialog();
    }

    [Header("Dialog")]
    [SerializeField] CanvasGroup dialogParent;
    [SerializeField] TMP_Text dialog;

    private GameManager gameManager;
    private Coroutine dialogCoroutine;
    private WaitForSeconds WaitForSeconds = new WaitForSeconds(3);

    private static readonly string[] WrongFacemaskDialog = new string[]
    {
        "This one is my father's facemask!",
        "DIRTY! Who leaves dirty facemasks arround the house?!",
        "DO WE COLLECT BROKEN FACEMASKS OR WHAT?",
        "USELESS! ARGGG!!"
    };
}
