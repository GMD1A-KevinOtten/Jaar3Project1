using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;
	public int teamCount;
	public float loadingProgress;
	private AsyncOperation async;


	void Start()
	{
		if (Instance == null)
		{
		Instance = this;
		}
		else if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
        DontDestroyOnLoad(this.gameObject);
	}

	public void ChangeScene(int i)
	{
		async = SceneManager.LoadSceneAsync(i);	
		async.allowSceneActivation = false;
		StartCoroutine(LoadingScreen());
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

	public void GameOverEvent()
	{
		// UIManager gameover function met variable input voor welk team/player wint (back to menu button, restart game button, quit game button)
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
