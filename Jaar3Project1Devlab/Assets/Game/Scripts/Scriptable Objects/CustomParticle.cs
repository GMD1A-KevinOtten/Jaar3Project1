using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Custom Particle", menuName = "Custom Particle", order = 1201)]
public class CustomParticle : ScriptableObject
{
    public string particleName;
    public GameObject particlePrefab;
    public Vector3 defaultScaling;
}