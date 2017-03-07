using UnityEngine;
using System.Collections;
using CnControls;
public class PlayerController : MonoBehaviour {
	private Animator anim;
	public float speed;
	Vector3 movement;
	private Rigidbody rb;
	//public Vector3 moveDirection = Vector3.zero;
	// Update is called once per frame
	void Start(){
		rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
		//rb.velocity = Vector3.zero;
	}
	void FixedUpdate () {
//		float x = CnInputManager.GetAxis ("Horizontal")*Time.deltaTime*100.0f;
//		float z = CnInputManager.GetAxis ("Vertical")*Time.deltaTime;
		float x = CnInputManager.GetAxis ("Horizontal");
		float z = CnInputManager.GetAxis ("Vertical");
		///movement.Set (x, 0f, z);
		//movement=movement.normalized*speed*Time.deltaTime;
		//rb.MovePosition (transform.forward+movement);
//		transform.Rotate(0,x,0);
		z *= speed;
		rb.MovePosition (rb.position + (transform.forward * z) * Time.deltaTime);
		x *= speed;
		rb.MovePosition (rb.position + (transform.right * x) * Time.deltaTime);

		Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, 120f * (x < 0f ? -1f : 1f), 0f), Mathf.Abs(x));
		Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
		rb.transform.rotation = (rb.transform.rotation * deltaRotation);
		//transform.Translate (0, 0, z);
		Animating (x, z);
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit(); 
		}
	}

	void Animating(float x, float z)
	{
		bool walking = x != 0f || z != 0f;
		anim.SetBool ("IsWalking", walking);
	}
}
