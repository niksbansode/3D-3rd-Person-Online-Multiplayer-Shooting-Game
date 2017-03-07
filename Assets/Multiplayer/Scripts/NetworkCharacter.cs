using UnityEngine;
using System.Collections;
public class NetworkCharacter : Photon.MonoBehaviour
{
	Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
	Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this
	// Update is called once per frame
	Animator anim;
	void Start()
	{
		CacheComponents ();
	}

	void CacheComponents() {
		if (anim == null) {
			anim = GetComponent<Animator> ();
			if (anim == null) {
				Debug.LogError ("ZOMG, you forgot to put an Animator component on this character prefab!");
			}
		}
	}
	void Update()
	{
		if (!photonView.isMine)
		{
			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, 0.1f);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		CacheComponents ();
		if (stream.isWriting)
		{
			// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext (anim.GetBool ("IsWalking"));
//			stream.SendNext (anim.GetBool("Death"));
		}
		else
		{
			// Network player, receive data
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
			anim.SetBool("IsWalking", (bool)stream.ReceiveNext());
//			anim.SetBool ("Death", (bool)stream.ReceiveNext());
		}
	}
}