using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EffectsManager : MonoBehaviour {

    public static EffectsManager instance;

    [System.Serializable]
    public struct BulletHoleColor
    {
        public string materialName;
        public Color bulletholeColor;
    }

    [Header("Audio")]
    public AudioMixer audioMixer;
    public CustomAudioClip[] allAudioclips;
    public List<AudioSource> audioSources = new List<AudioSource>();
    public int defaultAudiosourcesAmount;

    [Header("Particles")]
    public CustomParticle[] allParticles;

    [Header("BulletHoles")]
    public GameObject bulletHolePrefab;
    public BulletHoleColor[] bulletHoleColors;
    public List<Transform> activeBulletHoles = new List<Transform>();
    public int maxActiveBulletholes;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
           // DontDestroyOnLoad(this);
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

    //Audio functions:

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
        newObject.name = "Audio Source";
        newObject.transform.SetParent(transform);
        AudioSource source = newObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[0];

        return source;
    }


    //Particle Functions.

    public void PlayParticle(CustomParticle customParticle, Vector3 playPosition, Vector3 lookDirection)
    {
        GameObject newObject = Instantiate(customParticle.particlePrefab, playPosition, Quaternion.LookRotation(lookDirection));
        HierarchyHelp.ChangeScaleOfParentAndChildren(newObject.transform, customParticle.defaultScaling);
        ParticleSystem particle = newObject.GetComponent<ParticleSystem>();
        particle.Play();
        StartCoroutine(DeleteFinishedParticle(particle));
    }

    public CustomParticle FindParticle(string particleName)
    {
        for (int i = 0; i < allParticles.Length; i++)
        {
            if (allParticles[i].particleName == particleName)
            {
                return allParticles[i];
            }
        }

        Debug.LogError("There is no particle with the name " + particleName);
        return null;
    }

    public IEnumerator DeleteFinishedParticle(ParticleSystem toDelete)
    {
        yield return new WaitForSeconds(toDelete.main.duration);
        Destroy(toDelete.gameObject);
    }

    // BulletHole Functions

    public Transform CreateBulletHole(Sprite[] possibleSprites, Vector3 spawnPostion, Quaternion spawnRotation, string materialName)
    {
        GameObject newObject = Instantiate(bulletHolePrefab, spawnPostion, spawnRotation);
        print(newObject);
        newObject.name = "Bullethole " + (activeBulletHoles.Count).ToString();
        SpriteRenderer sr = newObject.GetComponent<SpriteRenderer>();
        sr.sprite = possibleSprites[Random.Range(0, possibleSprites.Length)];
        sr.color = FindBulletHoleColor(materialName).bulletholeColor;
        UpdateBulletHoleList(newObject.transform);
        print("Bullethole");
        return newObject.transform;
    }

    private void UpdateBulletHoleList(Transform bulletHole)
    {
        activeBulletHoles.Add(bulletHole);

        if(activeBulletHoles.Count > maxActiveBulletholes)
        {
            Destroy(activeBulletHoles[0].gameObject);
            activeBulletHoles.RemoveAt(0);
        }
    }
    private BulletHoleColor FindBulletHoleColor(string colorName)
    {
        for (int i = 0; i < bulletHoleColors.Length; i++)
        {
            if (bulletHoleColors[i].materialName == colorName)
            {
                return bulletHoleColors[i];
            }
        }

        return bulletHoleColors[0];
    }
}
