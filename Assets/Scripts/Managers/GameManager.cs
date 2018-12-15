using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using InControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private static GameManager instance = null;
	public static GameManager Instance => instance;

	private GameObject ball;
	public GameObject Ball => ball;

	private PlayerMove[] players;
	public PlayerMove[] Players => players;

	private AudioManager myAudioManager;
	private UIManager myUIManager;
	public MatchManager MyMatchManager { get; private set; }

	private bool inLevel = false;
	public bool InLevel => inLevel;

	private Dictionary<playerNumber, int> score = new Dictionary<playerNumber, int>()
	{
		{playerNumber.Player1, 0},
		{playerNumber.Player2, 0}
	};

	public Dictionary<playerNumber, int> Score
	{
		get { return score; }
		set
		{
			score = value;
		}
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
		GameObject[] playersObject = GameObject.FindGameObjectsWithTag("Player");
		players = new PlayerMove[playersObject.Length];
		for (int i = 0; i < playersObject.Length; i++)
		{
			players[i] = playersObject[i].GetComponent<PlayerMove>();
		}

		myAudioManager = GetComponent<AudioManager>();
		MyMatchManager = GetComponent<MatchManager>();
		myUIManager = GetComponent<UIManager>();

		//checks if inlevel through the presence of a player
		if (players.Length > 0)
		{
			inLevel = true;
			StartCoroutine(AddPlayers());
		}

		SortPlayers();
	}

	private void Start()
	{
		UpdateUI();
	}

	private IEnumerator AddPlayers()
	{
		var inputDevices = InputManager.Devices;

		//we have to check periodically wether all the requested devices have been recognised, or not
		while (inputDevices == null || inputDevices.Count < 2)
		{
			yield return new WaitForSeconds(0.5f);
			inputDevices = InputManager.Devices;
		}

		for (int i = 0; i < players.Length; i++)
		{
			players[i].GetComponent<PlayerMove>().AssignController(inputDevices[i]);
		}
	}

	private void Update()
	{
		if (inLevel)
		{
			//TODO Something only in level
		}

		//TODO remove this test
		/*if (Input.GetButtonDown("Fire1"))
		{
			ChangeTimeScale(0.0f);
			MyMatchManager.AddPointTo(playerNumber.Player1);
			MyMatchManager.AddPointTo(playerNumber.Player1);
			MyMatchManager.AddPointTo(playerNumber.Player2);
		}
		else if (Input.GetButtonUp("Fire1"))
		{
			ChangeTimeScale(1.0f);
		}*/
	}

	//check if players are not ordered correctly and order them if so
	//this is not the perfect way to code it because I'm assuming we have two players in the level, not one or three.
	void SortPlayers()
	{
		if (inLevel && players[0].transform.position.x > players[1].transform.position.x)
		{
			var tmp = players[0];
			players[0] = players[1];
			players[1] = tmp;
		}
	}

	public PlayerMove GetPlayer(playerNumber player)
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

	public void UpdateUI()
	{
		myUIManager.UpdateUI();
	}

	public void ChangeTimeScale(float timeScale)
	{
		Time.timeScale = timeScale;
		myAudioManager.UpdateAudio();
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