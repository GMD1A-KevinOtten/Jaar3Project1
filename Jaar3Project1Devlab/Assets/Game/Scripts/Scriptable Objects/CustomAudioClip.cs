using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Custom Audio Clip", menuName = "Custom Audio Clip", order = 1200)]
public class CustomAudioClip : ScriptableObject
{
    public string clipName;
    public AudioClip clip;
    [Range(0, 1)]
    public float defaultVolume;
}