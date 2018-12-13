using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private static GameManager instance = null;
	public static GameManager Instance => instance;

	private GameObject ball;
	public GameObject Ball => ball;

	private bool inLevel = false;


	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoadingScene;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoadingScene;
	}

	//Function that is called once a new scene has been loaded
	private void OnLevelFinishedLoadingScene(Scene scene, LoadSceneMode mode)
	{
		//Add code here
	}

	private void CheckEscape()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Quit();
		}
	}

	private void Awake()
	{
		//checks if another instance of GameManager exists, if so, destroy it, ensuring that only one GameManager exists at all time.
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}

		ball = GameObject.FindGameObjectWithTag("Ball");

		//checks if inlevel
		//TODO change method of check by player presence or something else
		if (ball)
		{
			inLevel = true;
		}
	}

	private void Update()
	{
		if (inLevel)
		{
			//DoSomething only in level
		}
	}

	public void LoadLevel(string nameLevel)
	{
		SceneManager.LoadScene(nameLevel);
	}

	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}