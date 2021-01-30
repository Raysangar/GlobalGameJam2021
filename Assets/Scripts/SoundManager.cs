using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public void PlaySound()
    {
        var source = GetAvailableSource();
    }

    private AudioSource GetAvailableSource()
    {
        int i = 0;
        while (i < sources.Count && sources[i].isPlaying)
            ++i;
        if (i == sources.Count)
            sources.Add(gameObject.AddComponent<AudioSource>());
        return sources[i];
    }

    private void Awake()
    {
        Instance = this;
        sources = new List<AudioSource>();
        for (int i = 0; i < 5; ++i)
            sources.Add(gameObject.AddComponent<AudioSource>());
    }

    public List<AudioSource> sources;
}
