using UnityEngine;
using System.Collections;

public class FxManager : MonoBehaviour {
	
	public GameObject BulletFXPrefab;
	[PunRPC]
	void SniperBulletFX( Vector3 startPos, Vector3 endPos ) {
		Debug.Log ("SniperBulletFX");
		if(BulletFXPrefab != null) {
			GameObject sniperFX = (GameObject)Instantiate(BulletFXPrefab, startPos, Quaternion.LookRotation( endPos - startPos ) );

			LineRenderer lr = sniperFX.transform.Find("Line").GetComponent<LineRenderer>();
			if(lr != null) {
				lr.SetPosition(0, startPos);
				lr.SetPosition(1, endPos);
			}
			else {
				Debug.LogError("BulletFXPrefab's linerenderer is missing.");
			}
		}
		else {
			Debug.LogError("BulletFXPrefab is missing!");
		}
	}
}
