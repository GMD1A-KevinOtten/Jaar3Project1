using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{

    public Vector2 intensityMin_Max;
    public Vector2 flickerDurationMin_Max;

    private float intensity;
    private float duration;
    private float timer;
    private bool ready;
    private Light lightSource;

    private void Awake()
    {
        lightSource = GetComponent<Light>();
        ready = true;
    }

    private void Update()
    {
        if (ready)
        {
            intensity = Random.Range(intensityMin_Max.x, intensityMin_Max.y);
            duration = Random.Range(flickerDurationMin_Max.x, flickerDurationMin_Max.y);

            if (intensity > lightSource.intensity)
                StartCoroutine(FlickerUp());
            else
                StartCoroutine(FlickerDown());
        }
    }

    public IEnumerator FlickerUp()
    {
        ready = false;

        while (lightSource.intensity < intensity)
        {
            lightSource.intensity += Time.deltaTime * duration;

            yield return null;
        }

         ready = true;
    }
    public IEnumerator FlickerDown()
    {
        ready = false;

        while (lightSource.intensity > intensity)
        {
            lightSource.intensity -= Time.deltaTime * duration;

            yield return null;
        }

          ready = true;
    }


}
