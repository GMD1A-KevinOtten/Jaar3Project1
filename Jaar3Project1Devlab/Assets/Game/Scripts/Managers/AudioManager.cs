using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioManager instance;

    [System.Serializable]
    public struct Clip
    {
        public string clipName;
        public AudioClip clip;
        [Range(0, 1)]
        public float defaultVolume;
    }

    public Clip[] allAudioclips;
    public List<AudioSource> audioSources = new List<AudioSource>();


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
    }

    /// <summary>
    /// Plays an Audioclip in 2D.
    /// <para>Use FindAudioClip(string clipName) to overload the file you want to play.</para>
    /// </summary>
    /// <param name="toPlay"></param>
    public void PlayAudio2D(Clip toPlay)
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
    public void PlayAudio3D(Clip toPlay, Vector3 playPos)
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
    public void StopAudio(Clip toStop)
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
    public Clip FindAudioClip(string clipName)
    {
        for (int i = 0; i < allAudioclips.Length; i++)
        {
            if (allAudioclips[i].clipName == clipName)
            {
                return allAudioclips[i];
            }
        }

        Debug.LogError("There is no audioclip named: " + clipName);
        return new Clip();
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

        GameObject newObject = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        newObject.transform.SetParent(transform);
        AudioSource source = newObject.AddComponent<AudioSource>();
        audioSources.Add(source);

        return source;
    }
}
