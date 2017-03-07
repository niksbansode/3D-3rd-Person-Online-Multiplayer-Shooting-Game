using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	public InputField Username;
	public string Username_text;
	public Button Single_Player_Button;
	public Button MultiPlayer_Button;
	public Button Exit_Button;
	public string PlayMode;
	public AudioSource backgroundMusic;
	// Use this for initialization
	void Start () {
		backgroundMusic.Play ();
		Single_Player_Button.onClick.AddListener(delegate() { StartSinglePlayer(); });
		MultiPlayer_Button.onClick.AddListener(delegate() { StartMultiPlayer(); });
		Exit_Button.onClick.AddListener(delegate() { Exit(); });
	}
	void Awake () {
		DontDestroyOnLoad (this.gameObject);
	}

	public void StartSinglePlayer()
	{
		SceneManager.LoadScene ("Main2", LoadSceneMode.Single);
		Username_text = Username.text;
		PlayMode = "SinglePlayer";
	}
	public void StartMultiPlayer()
	{
		SceneManager.LoadScene ("Main2", LoadSceneMode.Single);
		PlayMode = "MultiPlayer";
	}
	public void Exit(){
		Application.Quit(); 
	}
}
