using UnityEngine;
using TMPro;
using System.Collections;

public class DialogController : MonoBehaviour
{
    public static DialogController Instance { get; private set; }

    public void Initialize(GameManager gameManager, PlayerController player)
    {
        Instance = this;
        gameManager.OnGamePuasedStateChanged += OnGamePausedStateChanged;
        gameManager.OnGameAboutToStart += OnGameAboutToStart;
        player.OnFacemaskFound += OnPlayerFoundFacemask;
        gameManager.OnGameOver += OnGameOver;
        dialogParent.alpha = 0;
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

    private void OnGameAboutToStart(System.Action _)
    {
        HideDialog();
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

    private Coroutine dialogCoroutine;
    private WaitForSeconds WaitForSeconds = new WaitForSeconds(2);
}
