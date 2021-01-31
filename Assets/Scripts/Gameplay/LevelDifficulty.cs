using System.Collections.Generic;

public class LevelDifficulty
{
    public int TimerDuration;
    public Dictionary<Scene.ID, int> FacemasksSetup { get; private set; } = new Dictionary<Scene.ID, int>();
}
