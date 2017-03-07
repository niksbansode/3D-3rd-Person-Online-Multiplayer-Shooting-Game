using UnityEngine;


using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private Vector3 firstpoint; //change type on Vector3
	private Vector3 secondpoint;
	public Camera camera;
	private float xAngle = 0.0f; //angle for axes x for rotation
	private float yAngle = 0.0f;
	private float xAngTemp = 0.0f; //temp variable for angle
	private float yAngTemp = 0.0f;
	private Vector3 offset;
	[SerializeField]
	private float DistanceAway=1.25f;
	[SerializeField]
	private float DistanceUp=0.0f;
	[SerializeField]
	public Transform followXformf;
	private Vector3 velocityCamSmooth=Vector3.zero;
	[SerializeField]
	private float camSmoothDampTime=0.1f;

	private Vector3 lookDir;
	private Vector3 targetPosition;
	// Use this for initialization
	void Start () {
		//offset = transform.position - player.transform.position;
		//followXformf=GameObject.Find("follow").transform;
//		if (followXformf == null) {
//			followXformf=GameObject.FindWithTag("followIt").transform;
//		}
//		if (this.transform.position == null) {
//			this.transform.position = this.transform.position;
//		}
		lookDir = followXformf.forward;
		//offset=new Vector3(-5,2,0);
		xAngle = 0.0f;
		yAngle = 0.0f;
		camera = GetComponent<Camera> ();
		if (camera == null)
			Debug.Log ("wrong");
		Debug.Log (camera);
		//camera.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
	}
	void  LateUpdate (){
		Vector3 characterOffset= followXformf.position+new Vector3(0f,DistanceUp,0f);
		//characterOffset.y = 0.475f;
		//transform.position = player.transform.position + offset;
		lookDir = characterOffset - this.transform.position;

		lookDir.y = 0;
		lookDir.Normalize ();
		Debug.DrawRay (this.transform.position,lookDir,Color.green);
		targetPosition = characterOffset + followXformf.up * DistanceUp - lookDir * DistanceAway;
		Debug.DrawLine (followXformf.position, targetPosition, Color.magenta);
		compensateForWallls (characterOffset,ref targetPosition);
		//targetPosition.y=0.75f;

		smoothPosition (this.transform.position,targetPosition);
		//		transform.LookAt (characterOffset);
		transform.LookAt (followXformf);

//		//Check count touches
//		if(Input.touchCount > 0) {
//			//Touch began, save position
//			if(Input.GetTouch(0).phase == TouchPhase.Began) {
//				firstpoint = Input.GetTouch(0).position;
//				xAngTemp = xAngle;
//				yAngTemp = yAngle;
//			}
//			//Move finger by screen
//			if(Input.GetTouch(0).phase==TouchPhase.Moved) {
//				secondpoint = Input.GetTouch(0).position;
//				//Mainly, about rotate camera. For example, for Screen.width rotate on 180 degree
//				xAngle = xAngTemp + (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;
//				yAngle = yAngTemp - (secondpoint.y - firstpoint.y) *90.0f / Screen.height;
//				//Rotate camera
//				camera.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
//				followXformf = camera.transform.rotation;
//			}
//		}
	}
	private void smoothPosition(Vector3 fromPos,Vector3 toPos)
	{
		this.transform.position = Vector3.SmoothDamp (fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
	}

	private void compensateForWallls(Vector3 fromObject,ref Vector3 toTarget)
	{
		Debug.DrawLine (fromObject,toTarget,Color.cyan);
		RaycastHit wallHit = new RaycastHit ();
		if (Physics.Linecast (fromObject, toTarget, out wallHit)) {
			Debug.DrawRay(wallHit.point,Vector3.left,Color.red);
			toTarget = new Vector3 (wallHit.point.x, toTarget.y, wallHit.point.z);

		}
	}
}
