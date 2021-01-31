using UnityEngine;

public class CharacterDialog : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DialogController.Instance.ShowDialog(dialog);
        newCollider.enabled = false;
    }

    [SerializeField] string[] dialog;
    [SerializeField] Collider2D newCollider;
}
