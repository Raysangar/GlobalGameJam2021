using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ScenesManager : MonoBehaviour
{
    public System.Action<System.Action> OnPlayerGoesToNewScene;
    public Dictionary<Scene.ID, Scene> Map { get; private set; }


    public void Initialize(PlayerController player, CameraController cameraController, PlayerInput input)
    {
        this.player = player;
        this.input = input;
        this.cameraController = cameraController;
        Map = new Dictionary<Scene.ID, Scene>();
        foreach (var scene in scenes)
        {
            scene.gameObject.SetActive(false);
            Map.Add(scene.Id, scene);
            scene.Initialize(OnPlayerGoesToNewSceneCallback, input);
        }
    }

    public void StartGame(Dictionary<Scene.ID, int> facemasksInMap, int level)
    {
        currentSceneId = initialScene;
        foreach(var scene in scenes)
        {
            int numberOfFacemask;
            facemasksInMap.TryGetValue(scene.Id, out numberOfFacemask);
            scene.ResetScene(numberOfFacemask, level);
            if (scene.Id == initialScene)
            {
                scene.gameObject.SetActive(true);
                cameraController.SetBounds(scene.LeftBound, scene.RightBound);
            }
            else
                scene.gameObject.SetActive(false);
        }
    }

    private void OnPlayerGoesToNewSceneCallback(Scene.ID newSceneId)
    {
        nextSceneId = newSceneId;
        player.Collider.enabled = false;
        input.enabled = false;
        OnPlayerGoesToNewScene(MakeSceneTransition);
    }

    private void MakeSceneTransition()
    {
        Map[currentSceneId].gameObject.SetActive(false);
        var scene = Map[nextSceneId];
        scene.gameObject.SetActive(true);
        cameraController.SetBounds(scene.LeftBound, scene.RightBound);
        player.transform.position = Map[nextSceneId].GetPlayerInitialPositionComingFrom(currentSceneId);
        player.Collider.enabled = true;
        input.enabled = true;
        currentSceneId = nextSceneId;
    }

    [SerializeField] Scene[] scenes;
    [SerializeField] Scene.ID initialScene;

    private Scene.ID currentSceneId;
    private Scene.ID nextSceneId;
    private PlayerController player;
    private CameraController cameraController;
    private PlayerInput input;
}
