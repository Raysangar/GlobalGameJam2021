using UnityEngine;

public class SceneTransitioner : MonoBehaviour
{
    public System.Action<Scene.ID> OnPlayerGoesToNewScene;

    public Scene.ID TargetScene => targetScene;
    public Vector3 PlayerPosition => playerPositionReference.position;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnPlayerGoesToNewScene(targetScene);
    }

    [SerializeField] Scene.ID targetScene;
    [SerializeField] Transform playerPositionReference;
}
