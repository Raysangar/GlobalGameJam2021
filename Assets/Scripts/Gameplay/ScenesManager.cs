using System.Collections.Generic;
using UnityEngine;

public class ScenesManager : MonoBehaviour
{
    public Dictionary<Scene.ID, Scene> Map { get; private set; }

    public void Initialize(PlayerController player)
    {
        this.player = player;
        Map = new Dictionary<Scene.ID, Scene>();
        foreach (var scene in scenes)
        {
            scene.gameObject.SetActive(false);
            Map.Add(scene.Id, scene);
            scene.Initialize(OnPlayerGoesToNewScene);
        }
    }

    public void StartGame(Dictionary<Scene.ID, int> facemasksInMap)
    {
        foreach(var scene in scenes)
        {
            int numberOfFacemask;
            facemasksInMap.TryGetValue(scene.Id, out numberOfFacemask);
            scene.ResetScene(numberOfFacemask);
            scene.gameObject.SetActive(scene.Id == initialScene);
        }
    }

    private void OnPlayerGoesToNewScene(Scene.ID newSceneId)
    {
        Map[currentSceneId].gameObject.SetActive(false);
        Map[newSceneId].gameObject.SetActive(true);
        player.transform.position = Map[newSceneId].GetPlayerInitialPositionComingFrom(currentSceneId);
        currentSceneId = newSceneId;
    }

    [SerializeField] Scene[] scenes;
    [SerializeField] Scene.ID initialScene;

    private Scene.ID currentSceneId;
    private PlayerController player;
}
