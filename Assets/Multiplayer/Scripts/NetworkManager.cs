using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {
	public Camera StandByCamera;
	SpawnSpots[] spawnSpots;
	public bool connecting = false;
	public float respawnTimer = 0;
	public int isSceneLoaded=0;
	public bool offlineMode = false;
//	public bool PlayingMode;
	public string PlayerName;
	int variable;
	GameObject MainMenu;
	public bool isDead;
	public Rect windowRect = new Rect(0,0,0,0);
	public GUISkin guiSkin;
	public GUISkin guiTitle;
	public GUISkin guiPlayerList;
	public GUIStyle myStyle;
	public GUIStyle Title;
	public Text text;
	MainMenu MMObject;
	// Use this for initialization
	void Start () {
		MainMenu = GameObject.FindGameObjectWithTag ("Menu");
		spawnSpots = GameObject.FindObjectsOfType<SpawnSpots> ();
		PhotonNetwork.player.name = PlayerPrefs.GetString("Username", "ok");
		isDead = false;
		PhotonNetwork.player.name = PlayerName;
		isSceneLoaded = 1;
		MMObject =MainMenu.GetComponent<MainMenu> ();
		PhotonNetwork.player.name = MMObject.Username_text;
		if (MMObject.PlayMode == "SinglePlayer") {
			connecting = true;
			PhotonNetwork.offlineMode = true;
			OnJoinedLobby();
		}
		if (MMObject.PlayMode == "MultiPlayer") {
			connecting = true;
			Connect ();			
		}
	}
	void Awake () {
		DontDestroyOnLoad (this.gameObject);
	}
	public void Connect(){
		PhotonNetwork.ConnectUsingSettings ("MultiFPS v01");
	}
	void OnDestroy() {
		PlayerPrefs.SetString("Username", PhotonNetwork.player.name);
	}
	void OnGUI(){
//		backgroundMusic.enabled = true;
//		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
//		if(PhotonNetwork.connected == false && connecting == false ) {
//			// We have not yet connected, so ask the player for online vs offline mode.
//			GUILayout.BeginArea( new Rect(0, 0, Screen.width, Screen.height) );
//			GUILayout.BeginHorizontal();
//			GUILayout.FlexibleSpace();
//			GUILayout.BeginVertical();
//			GUILayout.FlexibleSpace();
//
//			GUILayout.BeginHorizontal();
//			GUILayout.Label("Username: ");
//			PhotonNetwork.player.name = GUILayout.TextField(PhotonNetwork.player.name);
//			GUILayout.EndHorizontal();
//
//			if( GUILayout.Button("Single Player") ) {
//				connecting = true;
//				PhotonNetwork.offlineMode = true;
//				OnJoinedLobby();
//			}
//
//			if( GUILayout.Button("Multi Player") ) {
//				connecting = true;
//				Connect ();
//			}
//			GUILayout.FlexibleSpace();
//			GUILayout.EndVertical();
//			GUILayout.FlexibleSpace();
//			GUILayout.EndHorizontal();
//			GUILayout.EndArea();
//		}
		if (isDead == true) {
//			GUILayout.BeginArea( new Rect(Screen.width/2-250, Screen.height/2, 500, 500) );
//			GUILayout.BeginHorizontal();
//			GUILayout.BeginVertical();
//			GUILayout.Box ("Player"+" | "+"Score");
//			GUILayout.EndHorizontal();
//			GUILayout.EndVertical();
//
//			GUILayout.BeginHorizontal();
//			GUILayout.BeginVertical();
//			GUILayout.Label("Score ");
//			foreach (PhotonPlayer pl in PhotonNetwork.playerList) {
//				GUILayout.BeginHorizontal();
//				GUILayout.Box (pl.name + " | " + pl.GetScore ());
//				GUILayout.EndHorizontal();
//			}
//			GUILayout.EndHorizontal();
//			GUILayout.EndVertical();
//			GUILayout.EndArea();

//			GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
//			GUILayout.FlexibleSpace();
//			GUILayout.BeginHorizontal();
//			GUILayout.FlexibleSpace();
//
//			// Now you can finally put in your GUI, such as:
//			GUILayout.BeginVertical("box");
			GUI.skin=guiSkin;
			GUI.depth = 1;
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();


			windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "My Window");
			windowRect.x = (int) ( Screen.width * 0.5f - windowRect.width * 0.5f );
			windowRect.y = (int) ( Screen.height * 0.5f - windowRect.height * 0.5f );
//			GUI.skin.window.stretchHeight = true;
//			GUI.skin.window.stretchWidth = true;
			windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "My Window",Title);

			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

//			GUILayout.EndVertical();
//
//			GUILayout.FlexibleSpace();
//			GUILayout.EndHorizontal();
//			GUILayout.FlexibleSpace();
//			GUILayout.EndArea();

		}
	}
	void DoMyWindow(int windowID) {
//		if (GUILayout.Button("Hello World"))
//			print("Got a click");
		GUI.skin=guiTitle;
		GUILayout.BeginHorizontal();
		GUILayout.Label ("High Score");
		GUILayout.EndHorizontal();
		GUI.skin=guiSkin;

		GUILayout.BeginHorizontal();
		GUILayout.Label("Player");
		GUILayout.Label("Score");
//		GUILayout.Label("Dead");
		GUILayout.EndHorizontal();
		GUI.skin = guiPlayerList;
//		for (int i = 0; i < 3; i++) {
//			GUILayout.BeginHorizontal();
//			GUILayout.Label("Player");
//			GUILayout.Label("Kill");
//			GUILayout.Label("Dead");
//			GUILayout.EndHorizontal();
//		}
		string score,name,kill;
		foreach (PhotonPlayer pl in PhotonNetwork.playerList) {
			GUILayout.BeginHorizontal();
//			GUILayout.Box (pl.name + " | " + pl.GetScore ());
			GUILayout.Label(pl.name.ToString());
			GUILayout.Label(pl.GetScore().ToString());
//			GUILayout.Label("0");
			GUILayout.EndHorizontal();
		}

	}

	public void OnJoinedLobby(){
		Debug.Log ("OnJoinedLobby");
		PhotonNetwork.JoinRandomRoom ();
	}

	void OnPhotonRandomJoinFailed() {
		Debug.Log ("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom (null);
	}

	void OnJoinedRoom() {
		MMObject.backgroundMusic.Pause ();
		Debug.Log ("OnJoinedRoom");
		connecting = false;
		SpawnPlayer ();
	}

	void SpawnPlayer(){
		if (spawnSpots == null) {
			return;
		}
		SpawnSpots mySpawnSpot = spawnSpots [ Random.Range (0, spawnSpots.Length) ];

		GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate ("Player", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);
		StandByCamera.enabled = false;
		isDead = false;
		((MonoBehaviour)myPlayerGO.GetComponent ("PlayerController")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent ("PlayerShooting")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent ("Health")).enabled = true;
		//GameObject.FindGameObjectWithTag ("PlayerList").gameObject.SetActive (true);
		myPlayerGO.transform.FindChild ("Main Camera").gameObject.SetActive(true);	
//		Health h=myPlayerGO.GetComponent<Health> ();
//		h.currentHitPoints = 100f;
		if (myPlayerGO.transform.FindChild ("Main Camera").gameObject != null) {
			CameraController followScript = myPlayerGO.GetComponentInChildren<CameraController> ();
			if (followScript != null){
				followScript.followXformf = myPlayerGO.transform.Find("follow").transform;
			}
		}
	}
	void Update() {
		if(respawnTimer > 0) {
			GUI.depth = 0;
			text.text = ""+Mathf.Round(respawnTimer);
			respawnTimer -= Time.deltaTime;

			if(respawnTimer <= 0) {
				// Time to respawn the player!
				text.text = "";
				SpawnPlayer();
			}
		}
	}
}
