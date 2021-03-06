﻿using UnityEngine;
using System.Collections;

public class DestroyThis : MonoBehaviour {

	public float selfDestructTime = 1.0f;

	void Update () {
		selfDestructTime -= Time.deltaTime;

		if(selfDestructTime <= 0) {

			PhotonView pv = GetComponent<PhotonView>();

			if(pv != null && pv.instantiationId!=0 ) {
				PhotonNetwork.Destroy(gameObject);
			}
			else {
				Destroy(gameObject);
			}

		}
	}
}
