using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitioner : MonoBehaviour
{
    public System.Action<Scene.ID> OnPlayerGoesToNewScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnPlayerGoesToNewScene(targetScene);
    }

    [SerializeField] Scene.ID targetScene;
}
