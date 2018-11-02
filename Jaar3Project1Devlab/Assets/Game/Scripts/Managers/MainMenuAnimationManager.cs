using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimationManager : MonoBehaviour {

    public Animator mainCamAnim;
    public Animator tankAnim;

    public GameObject optionsCanvas;
    private bool quitting;

    public ParticleSystem sandStormParticle;
    private void Awake()
    {
        mainCamAnim = Camera.main.GetComponent<Animator>();
        if(tankAnim == null)
        {
            Debug.LogError("Please assign tankAnim in the inspector");
        }

        sandStormParticle.Pause(true);
    }

    public void OptionsToggle()
    {
        if (!mainCamAnim.GetBool("MoveToOptions"))
        {
            mainCamAnim.SetBool("MoveToOptions", true);
        }
        else if (mainCamAnim.GetBool("MoveToOptions"))
        {
            mainCamAnim.SetBool("MoveToOptions", false);
        }
    }

    public void QuitGameCamAnimation()
    {
        if (!quitting)
        {
            quitting = true;
            optionsCanvas.SetActive(false);
            mainCamAnim.SetBool("QuitGame", true);
            StartCoroutine(QuitGameTankAnimation());
        }
      
    }
    public IEnumerator QuitGameTankAnimation()
    {
        yield return new WaitForSeconds(1);
        tankAnim.SetBool("QuitGame", true);
        StartCoroutine(QuitGame());
    }

    public IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(1.3F);
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

#endif
        Application.Quit();

    }
}
