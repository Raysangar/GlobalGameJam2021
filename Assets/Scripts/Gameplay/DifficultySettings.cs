using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultySettings", menuName = "Game/Difficulty Settings")]
public class DifficultySettings : ScriptableObject
{
    public LevelDifficulty GetFacemasksSetupForLevel(int level)
    {
        var levelSetup = levels[Mathf.Clamp(level, 0, levels.Length - 1)];
        cachedLevelDifficulty.TimerDuration = levelSetup.TimerDurationInSeconds;
        cachedLevelDifficulty.FacemasksSetup.Clear();

        int facemasksCount = Random.Range(levelSetup.MinFacemasks, levelSetup.MaxFacemasks + 1);

        int totalWeight = 0;
        foreach (var weightSetup in levelSetup.FacemaskProbabilityWeightForEachScene)
        {
            cachedSceneFacemaskWeights.Add(weightSetup.weight);
            totalWeight += weightSetup.weight;
        }

        for (int i = 0; i <  facemasksCount; ++i)
        {
            int outcome = Random.Range(0, totalWeight);
            int j = -1;
            do
                outcome -= cachedSceneFacemaskWeights[++j];
            while (outcome > 0);

            Scene.ID sceneId = levelSetup.FacemaskProbabilityWeightForEachScene[j].id;

            if (cachedLevelDifficulty.FacemasksSetup.ContainsKey(sceneId))
                cachedLevelDifficulty.FacemasksSetup[sceneId]++;
            else
                cachedLevelDifficulty.FacemasksSetup.Add(sceneId, 1);
        }

        return cachedLevelDifficulty;
    }

    [System.Serializable]
    private class LevelDifficultySettings
    {
        public int MinFacemasks;
        public int MaxFacemasks;
        public int TimerDurationInSeconds;
        public SceneFacemaskWeight[] FacemaskProbabilityWeightForEachScene;
    }

    [System.Serializable]
    private class SceneFacemaskWeight
    {
        public Scene.ID id;
        public int weight;
    }

    [SerializeField] LevelDifficultySettings[] levels;

    private LevelDifficulty cachedLevelDifficulty = new LevelDifficulty();
    private List<int> cachedSceneFacemaskWeights = new List<int>();
}
