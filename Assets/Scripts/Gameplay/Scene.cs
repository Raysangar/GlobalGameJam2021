using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    public enum ID { Entrance, LivingRoom, Kitchen, Bedroom, Aisle, Bathroom, Garage }

    public System.Action<ID> OnPlayerGoesToScene;
    public InteractableObject[] InteractableObjects { get; private set; }
    public ID Id => id;
    public float LeftBound => maxLeftPosition;
    public float RightBound => maxRightPosition;

    public void Initialize(System.Action<ID> playerGoesToNewSceneCallback, UnityEngine.InputSystem.PlayerInput input)
    {
        InteractableObjects = GetComponentsInChildren<InteractableObject>();
        foreach (var interactableObject in InteractableObjects)
            interactableObject.Initialize(input);

        this.sceneTransitioners = new Dictionary<ID, SceneTransitioner>();
        var sceneTransitioners = GetComponentsInChildren<SceneTransitioner>();
        foreach (var transitioner in sceneTransitioners)
        {
            transitioner.OnPlayerGoesToNewScene += playerGoesToNewSceneCallback;
            this.sceneTransitioners.Add(transitioner.TargetScene, transitioner);
        }
    }

    public Vector3 GetPlayerInitialPositionComingFrom(ID scene)
    {
        return sceneTransitioners[scene].PlayerPosition;
    }

    public void ResetScene(int numberOfFaceMasks, int level)
    {
        for (int iObject = 0; iObject < InteractableObjects.Length; ++iObject)
        {
            bool hasMask = false;
            if (numberOfFaceMasks > 0 && InteractableObjects[iObject].CanBeGoodFacemask(level))
            {
                hasMask = Random.Range(iObject, InteractableObjects.Length) == iObject;
                if (hasMask)
                    --numberOfFaceMasks;
            }
            InteractableObjects[iObject].SetupForLevel(hasMask, level == 0);
        }

        int i = 0;
        while (i < InteractableObjects.Length && numberOfFaceMasks > 0)
        {
            if (InteractableObjects[i].CanBeGoodFacemask(level))
            {
                InteractableObjects[i].SetupForLevel(true, level == 0);
                --numberOfFaceMasks;
            }
        }
    }

    [SerializeField] ID id;
    [SerializeField] float maxLeftPosition;
    [SerializeField] float maxRightPosition;
 
    private Dictionary<ID, SceneTransitioner> sceneTransitioners;
}
