using UnityEngine;
using System.Collections;

public class WoodenBucketScript : toolBaseScript {

	private bool busy;
	private float delay1=3.0f;
	private float delay2=3.0f;
	private float timer;
	private float delayTime;

	public bool Filled;
	public byte delayN;
	public AudioClip hydrSound;
	animController animCont;

	// Use this for initialization
	void Start () {
		toolName = "WoodenBucket";
		scriptName = "WoodenBucketScript";
		toolObj = Resources.Load (toolName, typeof(GameObject)) as GameObject;
		hydrSound = Resources.Load ("hydrating") as AudioClip;
		Init ();
		//animCont = cam.transform.Find("arm_anim").GetComponent<animController>();


	}




	// Update is called once per frame
	void Update () {


		if ((!busy)&&(!controls.locked)) {
			if (Input.GetMouseButtonDown (0)) {
				//	Debug.Log ("LMB Pressed");
				delayTime = delay1;
				delayN = 1;
				busy = true;
				//	animCont.SetAnim("Chop");
				StartCoroutine ("Action1");
				return;
			}

			if (Input.GetMouseButtonDown (1)) {
				if (Filled) {
					//Debug.Log ("RMB Pressed");
					delayTime = delay2;
					busy = true;
					delayN = 2;
					StartCoroutine ("Action2");
				}
				return;
			}

		} else {
			timer += Time.deltaTime;
			if (timer > delayTime) {
				busy = false;
				timer = 0.0f;
			} else if (Input.anyKeyDown) {
				if (delayN == 1) {
					StopCoroutine ("Action1");
					busy = false;
					timer = 0.0f;
				}
				if (delayN == 2) {
					StopCoroutine ("Action2");
					busy = false;
					timer = 0.0f;
				}
			}
		}

	}



	void FixedUpdate (){
		Validate();
	}

	void OnGUI() {
		if (busy) {
			DrawBar (timer / delayTime);
		}

	}

	IEnumerator Action1() {
		float range = 10f;
		Vector3 direction = cam.transform.TransformDirection (Vector3.forward);
		RaycastHit hit;
		if (Physics.Raycast (cam.transform.position, direction, out hit, range)) {	

			if (hit.collider.tag == "Water") {
				yield return new WaitForSeconds (delay1);
				//	inv.items [toolIndex].durability -= 1;
			    //hit.transform.gameObject.SendMessage ("Chop", new ToolParams(this.gameObject,1.0f), SendMessageOptions.DontRequireReceiver);
				Filled = true;
			}
		} else {
			yield return null;
		}

	}

	IEnumerator Action2() {
		float range = 10f;
		Vector3 direction = cam.transform.TransformDirection (Vector3.forward);
		RaycastHit hit;
		if (Physics.Raycast (cam.transform.position, direction, out hit, range)) {	

			if ((hit.rigidbody) && (hit.rigidbody.gameObject.name != "Player")) {
				audio.PlayOneShot (hydrSound);
				yield return new WaitForSeconds (delay2);
				inv.items [toolIndex].durability -= 1;
				hit.transform.gameObject.SendMessage ("Hydrate", this.transform.gameObject, SendMessageOptions.DontRequireReceiver);
				Filled = false;
			} else if (hit.collider.tag == "Usable") {
				audio.PlayOneShot (hydrSound);
				yield return new WaitForSeconds (delay2);
				inv.items [toolIndex].durability -= 1;
				hit.transform.gameObject.SendMessage ("Hydrate", this.transform.gameObject, SendMessageOptions.DontRequireReceiver);
				Filled = false;
			}
		}
	}
}
