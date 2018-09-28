using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    public CustomAudioClip[] allAudioclips;
    public List<AudioSource> audioSources = new List<AudioSource>();
    public int defaultAudiosourcesAmount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < defaultAudiosourcesAmount; i++)
        {
            AudioSource source = CreateAudioSource();
            audioSources.Add(source);
        }
    }

    /// <summary>
    /// Plays an Audioclip in 2D.
    /// <para>Use FindAudioClip(string clipName) to overload the file you want to play.</para>
    /// </summary>
    /// <param name="toPlay"></param>
    public void PlayAudio2D(CustomAudioClip toPlay)
    {
        AudioSource source = GetEmptyAudiosource();
        source.clip = toPlay.clip;
        source.volume = toPlay.defaultVolume;
        source.transform.position = Vector3.zero;
        source.spatialBlend = 0;

        source.Play();
    }

    /// <summary>
    /// Plays an Audioclip in 3D.
    /// <para>Use FindAudioClip(string clipName) to overload the file you want to play.</para>
    /// </summary>
    /// <param name="toPlay"></param>
    /// <param name="playPos"></param>
    public void PlayAudio3D(CustomAudioClip toPlay, Vector3 playPos)
    {
        AudioSource source = GetEmptyAudiosource();
        source.clip = toPlay.clip;
        source.volume = toPlay.defaultVolume;
        source.transform.position = playPos;
        source.spatialBlend = 1;

        source.Play();
    }

    /// <summary>
    /// Stops the overloaded AudioClip.
    /// <para>Use FindAudioClip(string clipName) to overload the file you want to stop.</para>
    /// </summary>
    /// <param name="toStop"></param>
    public void StopAudio(CustomAudioClip toStop)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].clip.name == toStop.clip.name)
            {
                audioSources[i].Stop();
            }
        }
    }

    /// <summary>
    /// Finds the audioclip with the overloaded name. Audioclips are stored in AudioManager.
    /// </summary>
    /// <param name="clipName"></param>
    /// <returns></returns>
    public CustomAudioClip FindAudioClip(string clipName)
    {
        for (int i = 0; i < allAudioclips.Length; i++)
        {
            if (allAudioclips[i].clipName == clipName)
            {
                return allAudioclips[i];
            }
        }

        Debug.LogError("There is no audioclip named: " + clipName);
        return new CustomAudioClip();
    }

    /// <summary>
    /// Finds an Audiosource which is not being used. If there are no empty audiosources, a new one is created.
    /// </summary>
    /// <returns></returns>
    private AudioSource GetEmptyAudiosource()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                return audioSources[i];
            }
        }

        AudioSource source = CreateAudioSource();
        audioSources.Add(source);

        return source;
    }

    private AudioSource CreateAudioSource()
    {
        GameObject newObject = new GameObject();
        newObject.transform.position = transform.position;
        newObject.transform.SetParent(transform);
        AudioSource source = newObject.AddComponent<AudioSource>();

        return source;
    }
}
