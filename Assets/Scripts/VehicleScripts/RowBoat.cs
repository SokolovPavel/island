using UnityEngine;
using System.Collections;

public class RowBoat : MonoBehaviour {
	public Transform sitPoint;
	public Transform exitPoint;

	private bool occupied;
	private GameObject ply;
	private Vector3 lastPos;
	// Use this for initialization
	void Start () {
	
	}

	void Use(GameObject user)
	{
		if (!occupied) {
			user.collider.enabled = false;
			user.GetComponent<PlayerController> ().enabled = false;

			user.SendMessage ("HideTool", SendMessageOptions.DontRequireReceiver);
			//user.SendMessage ("lockControl", SendMessageOptions.DontRequireReceiver);
			ply = user;
			lastPos = user.transform.position;
			user.transform.position = sitPoint.position;
			user.transform.parent = sitPoint;

			occupied = true;

		} else {


		}
	}


	
	// Update is called once per frame
	void FixedUpdate() {

		if (occupied) {
			if (Input.GetKey ("w")) {
			rigidbody.AddRelativeForce (new Vector3 (0, -200, 0));

			}


	
		if (Input.GetKey ("s")) {
			rigidbody.AddRelativeForce (new Vector3 (0, 200, 0));

			}

			if (Input.GetKey ("a")) {
				rigidbody.AddRelativeTorque(new Vector3 (0, 0, -100));

			}
			if (Input.GetKey ("d")) {
				rigidbody.AddRelativeTorque(new Vector3 (0, 0, 100));

			}

		}
	}


	void Update () {
				if (Input.GetButtonDown ("Use")) {
			ply.transform.parent = null;
			ply.transform.position = exitPoint.position;
			ply.collider.enabled = true;
			ply.GetComponent<PlayerController> ().enabled = true;
			occupied = false;
		}
	}
}
