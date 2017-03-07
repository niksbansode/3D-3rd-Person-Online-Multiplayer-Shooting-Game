using UnityEngine;
using System.Collections;
using CnControls;
public class PlayerShooting : MonoBehaviour {

	public float firerate = 0.05f;
	float cooldown = 0;
	public float damage = 25f;
	FxManager fxManager;
	Animator anim;
	// Use this for initialization
	void Start () {
		fxManager = GameObject.FindObjectOfType<FxManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		cooldown -= Time.deltaTime;
		if (CnInputManager.GetButton ("Jump"))
			Fire ();
	}

	void Fire()
	{
		if (cooldown > 0)
			return;
		Debug.Log ("Fireing is on");

		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		Transform hitTransform;
		Vector3   hitPoint;

		hitTransform = FindClosestHitObject(ray, out hitPoint);

		if (hitTransform != null) {
			Debug.Log ("We hit: " + hitTransform.name);
			// We could do a special effect at the hit location
			// DoRicochetEffectAt( hitPoint );

			Health h = hitTransform.GetComponent<Health> ();

			while (h == null && hitTransform.parent) {
				hitTransform = hitTransform.parent;
				h = hitTransform.GetComponent<Health> ();
			}

			// Once we reach here, hitTransform may not be the hitTransform we started with!

			if (h != null) {
				// This next line is the equivalent of calling:
				//    				h.TakeDamage( damage );
				// Except more "networky"
				PhotonView pv = h.GetComponent<PhotonView> ();
				if (pv == null) {
					Debug.LogError ("Freak out!");

				} else {
//
//					TeamMember tm = hitTransform.GetComponent<TeamMember>();
//					TeamMember myTm = this.GetComponent<TeamMember>();
//
//					if(tm==null || tm.teamID==0 || myTm==null || myTm.teamID==0 || tm.teamID != myTm.teamID ) {
					h.GetComponent<PhotonView> ().RPC ("TakeDamage", PhotonTargets.All, damage,PhotonNetwork.player);
//					}
//				}
				}
//
//			if(fxManager != null) {
//
//				DoGunFX(hitPoint);
//			}
			}
//		else {
//			// We didn't hit anything (except empty space), but let's do a visual FX anyway
//			if(fxManager != null) {
//				hitPoint = Camera.main.transform.position + (Camera.main.transform.forward*100f);
//				DoGunFX(hitPoint);
//			}
//		
			if (fxManager != null) {
				GunEffect (hitPoint);
			}
		} else {
			if (fxManager != null) {
				hitPoint=Camera.main.transform.position+(Camera.main.transform.forward*100f);
				GunEffect (hitPoint);
			}
		}
		cooldown = firerate;
	}
	void GunEffect(Vector3 hitPoint)
	{
		Weapon w = gameObject.GetComponentInChildren<Weapon> ();
		fxManager.GetComponent<PhotonView> ().RPC ("SniperBulletFX", PhotonTargets.All, w.transform.position, hitPoint);
	}
	Transform FindClosestHitObject(Ray ray, out Vector3 hitPoint) {

		RaycastHit[] hits = Physics.RaycastAll(ray);

		Transform closestHit = null;
		float distance = 0;
		hitPoint = Vector3.zero;

		foreach(RaycastHit hit in hits) {
			if(hit.transform != this.transform && ( closestHit==null || hit.distance < distance ) ) {
				// We have hit something that is:
				// a) not us
				// b) the first thing we hit (that is not us)
				// c) or, if not b, is at least closer than the previous closest thing

				closestHit = hit.transform;
				distance = hit.distance;
				hitPoint = hit.point;
			}
		}

		// closestHit is now either still null (i.e. we hit nothing) OR it contains the closest thing that is a valid thing to hit

		return closestHit;

	}
}
