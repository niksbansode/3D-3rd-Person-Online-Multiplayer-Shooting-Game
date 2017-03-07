using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour {
	public float hitPoints = 100f;
	public float currentHitPoints;
	public Slider healthSlider;
	public Image damageImage;
	PhotonView PView;
	public float flashSpeed = 5f;
	Animator anim;
	public Color flashColor = new Color (1f, 0f, 0f, 0.5f);
	bool damaged;
	// Use this for initialization
	void Start () {
		PView = GetComponent<PhotonView> ();
		if (PView == null)
			Debug.Log ("null");
		anim = GetComponent<Animator> ();
		healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
		damageImage= GameObject.Find("DamageImage").GetComponent<Image>();
		healthSlider.value = hitPoints;
		currentHitPoints = hitPoints;
		if (anim != null)
			Debug.Log ("anim not null");
	}
	void Update()
	{
		if (damaged)
			damageImage.color = flashColor;
		else
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		damaged = false;
	}
	[PunRPC]
	public void TakeDamage(float amt,PhotonPlayer player) {
		damaged = true;
		currentHitPoints -= amt;
		if(healthSlider!=null)
			healthSlider.value = currentHitPoints;
		if(currentHitPoints <= 0) {
			player.AddScore (1);
			StartCoroutine(Die ());
//			Die();
		}
	}
	void OnGUI() {
		if( GetComponent<PhotonView>().isMine && gameObject.tag == "Player" ) {
			if( GUI.Button(new Rect (Screen.width-100, 0, 100, 40), "Suicide!") ) {
				StartCoroutine(Die ());
//				Die ();
			}
		}
	}
//
//	IEnumerator Wait() {
//		print(Time.time);
//		yield return new WaitForSeconds(5);
//		print(Time.time);
//	}
	IEnumerator Die() {
//		PlayerName plname = GetComponent<PlayerName> ();
		//isDead = true;
		if( GetComponent<PhotonView>().instantiationId==0 ) {
//			anim.SetBool ("Death",true);
//			anim.SetTrigger("Dead");
			this.transform.position=this.transform.position+new Vector3(0,0.05f,0);
			PView.RPC("SetTrigger", PhotonTargets.AllBuffered,"Dead");
			yield return new WaitForSeconds(5);
			Destroy(gameObject);
		}
		else {
			if( GetComponent<PhotonView>().isMine ) {
				this.transform.position=this.transform.position+new Vector3(0,0.05f,0);
				PView.RPC("SetTrigger", PhotonTargets.AllBuffered,"Dead");
				yield return new WaitForSeconds(5);
				if( gameObject.tag == "Player") {		// This is my actual PLAYER object, then initiate the respawn process
					NetworkManager nm = GameObject.FindObjectOfType<NetworkManager>();
					nm.StandByCamera.enabled = true;
					nm.isDead = true;
					nm.respawnTimer = 5f;
				}
//				anim.SetBool ("Death",true);
//				anim.SetTrigger("Dead");
				PhotonNetwork.Destroy(gameObject);
			}
		}
	}
	[PunRPC]
	void SetTrigger( string triggerName )
	{
		anim.SetTrigger("Dead");
	}
}

