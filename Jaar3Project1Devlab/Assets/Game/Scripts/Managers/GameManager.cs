using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;
	public int teamCount;
	public float loadingProgress;
	private AsyncOperation async;


	void Awake()
	{
		if (Instance == null)
		{
		Instance = this;
		}
		else if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
        DontDestroyOnLoad(this);
	}

	private void Update() 
	{
		if (Input.GetKeyDown("k"))
        {
            if(Cursor.lockState != CursorLockMode.Locked && Cursor.visible == true)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
	}
	
	public void ChangeScene(int i)
	{
        SceneManager.LoadScene(i);

        //Doesn't work
		//async = SceneManager.LoadSceneAsync(i);	
		//async.allowSceneActivation = false;
		//StartCoroutine(LoadingScreen());
	}

	public void ToggleTimeScale()
	{
		if(Time.timeScale == 1)
		{
			Time.timeScale = 0;
		}
		else
		{
			Time.timeScale = 1;
		}
	}

	public IEnumerator LoadingScreen()
	{
		while (async.isDone == false)
		{
			loadingProgress = async.progress;
			if(async.progress == 0.9f)
			{
				loadingProgress = 1;
				async.allowSceneActivation = true;
			}
			yield return null;
		}
	}

	//victoriouse team is the index + 1
	public void GameOverEvent(int victoriouseTeam)
	{
		if(victoriouseTeam > 0)
		{
            // UIManager gameover function met variable input voor welk team/player wint en UI element popup voor back to menu button, restart game button, quit game button
            UIManager.instance.GameOver(victoriouseTeam);
		}
		else
		{
            // gelijk spel scherm
            UIManager.instance.GameOver(404);
        }
	}

	public void QuitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
