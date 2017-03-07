using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerName : MonoBehaviour {
	public Text nameTag;
	void Awake()
	{
		GetComponent<PhotonView> ().RPC ("updateName",PhotonTargets.AllBuffered,PhotonNetwork.playerName);
	}
	[PunRPC]
	public void updateName(string name)
	{
		foreach (PhotonPlayer pl in PhotonNetwork.playerList) {
			if (GetComponent<PhotonView> ().isMine) {
				nameTag.text = pl.name;
			}
			if( GetComponent<PhotonView>().instantiationId==0 ) {
				if (gameObject.tag == "Player") {
					nameTag.text=pl.name;
				}
			}
		}
	}
}
