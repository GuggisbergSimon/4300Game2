using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private static GameManager instance = null;
	public static GameManager Instance => instance;

	private GameObject ball;
	public GameObject Ball => ball;

	public GameObject[] players;

	private bool inLevel = false;

	//TODO delete that code if not needed
	//That code is not being needed for the moment
	/*private void OnEnable()
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
	}*/

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
		players = GameObject.FindGameObjectsWithTag("Player");

		//checks if inlevel
		//TODO change method of check by player presence or something else
		if (players.Length>0)
		{
			inLevel = true;
		}

		SortPlayers();
	}

	private void Update()
	{
		if (inLevel)
		{
			//TODO Something only in level
		}
	}

	//check if players are not ordered correctly and order them if so
	void SortPlayers()
	{
		if (inLevel && players[0].transform.position.x > players[1].transform.position.x)
		{
			var tmp = players[0];
			players[0] = players[1];
			players[1] = tmp;
		}
	}

	public GameObject GetPlayer(playerNumber player)
	{
		if (player == playerNumber.Player1)
		{
			return players[0];
		}
		else
		{
			return players[1];
		}
	}

	public void ChangeTimeScale(float timeScale)
	{
		Time.timeScale = timeScale;
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