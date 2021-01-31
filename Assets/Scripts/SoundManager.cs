using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public void PlaySound(AudioClip clip)
    {
        var source = GetAvailableSource();
        source.clip = clip;
        source.Play();
    }

    public void PlayCountdownMusic()
    {
        if (musicSource.clip != countdownMusic)
        {
            musicSource.Stop();
            musicSource.clip = countdownMusic;
            musicSource.Play();
        }
    }

    public void PlayMainMusic()
    {
        if (musicSource.clip != mainMusic)
        {
            musicSource.Stop();
            musicSource.clip = mainMusic;
            musicSource.Play();
        }
    }

    public void PlayTransitionSound()
    {
        PlaySound(transitionSounds[Random.Range(0, transitionSounds.Length)]);
    }

    public void PlayGrabObjectSound(AudioClip clip = null)
    {
        PlaySound(grabObjectSound);
        if (clip != null)
            PlaySound(clip);
    }

    public void StopSound(AudioClip clip)
    {
        foreach (var source in sources)
            if (source.clip == clip)
                source.Stop();
    }

    public void PlayPlayerSteps()
    {
        if (stepsCoroutine == null)
            stepsCoroutine = StartCoroutine(StepsCoroutine());
    }

    public void StopPlayerSteps()
    {
        if (stepsCoroutine != null)
        {
            StopCoroutine(stepsCoroutine);
            stepSource.Stop();
            stepsCoroutine = null;
        }
    }

    private IEnumerator StepsCoroutine()
    {
        while(true)
        {
            stepSource.Stop();
            stepSource.clip = stepsSounds[Random.Range(0, stepsSounds.Length)];
            stepSource.Play();
            yield return waitForStep;
        }
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

        stepSource = gameObject.AddComponent<AudioSource>();
        stepSource.loop = true;
        waitForStep = new WaitForSeconds(stepSoundPeriod);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = mainMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    [SerializeField] AudioClip[] stepsSounds;
    [SerializeField] float stepSoundPeriod;
    [SerializeField] AudioClip grabObjectSound;
    [SerializeField] AudioClip[] transitionSounds;
    [SerializeField] AudioClip mainMusic;
    [SerializeField] AudioClip countdownMusic;

    private AudioSource musicSource;
    private List<AudioSource> sources;
    private AudioSource stepSource;
    private WaitForSeconds waitForStep;
    private Coroutine stepsCoroutine;
}
