using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerList : MonoBehaviour {

	public GameObject  playerScorePrefab;
	public Text nameTag;
	public Text kill;
	public Text dead;
	void Start () {
//		for (int i = 0; i < 3; i++) {
//			GameObject go = (GameObject)Instantiate (playerScorePrefab);
//			go.transform.SetParent (this.transform,false);
//		}
		GetComponent<PhotonView> ().RPC ("updateName",PhotonTargets.AllBuffered,PhotonNetwork.playerName);
	}
	
	[PunRPC]
	public void updateName(string name)
	{
		foreach (PhotonPlayer pl in PhotonNetwork.playerList) {
			GameObject go = (GameObject)Instantiate (playerScorePrefab);
			go.transform.SetParent (this.transform,false);

			if (GetComponent<PhotonView> ().isMine) {
				nameTag.text = pl.name;
				kill.text = "0";
				dead.text = "0";
				Debug.Log (pl.name);
			}
			if( GetComponent<PhotonView>().instantiationId==0 ) {
				if (gameObject.tag == "Player") {
					nameTag.text=pl.name;
					kill.text = "0";
					dead.text = "0";
					Debug.Log (pl.name);
				}
			}
		}
	}
}
