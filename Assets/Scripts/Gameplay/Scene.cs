using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    public enum ID { LivingRoom }

    public System.Action<ID> OnPlayerGoesToScene;
    public InteractableObject[] InteractableObjects { get; private set; }

    public ID Id { get; }

    public void Initialize(System.Action<ID> playerGoesToNewSceneCallback)
    {
        foreach (var transitioner in GetComponentsInChildren<SceneTransitioner>())
            transitioner.OnPlayerGoesToNewScene += playerGoesToNewSceneCallback;
    }

    public void ResetScene(int numberOfFaceMasks)
    {
        for (int iObject = 0; iObject < InteractableObjects.Length; ++iObject)
        {
            bool hasMask = false;
            if (numberOfFaceMasks > 0)
            {
                hasMask = Random.Range(iObject, InteractableObjects.Length) == iObject;
                if (hasMask)
                    --numberOfFaceMasks;
            }
            InteractableObjects[iObject].Initialize(hasMask);
        }
    }

    private void Awake()
    {
        InteractableObjects = GetComponentsInChildren<InteractableObject>();
    }

    [SerializeField] Transform playerInitialPosition;
}
