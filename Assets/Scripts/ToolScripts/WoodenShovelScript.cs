using UnityEngine;
using System.Collections;

public class WoodenShovelScript :  toolBaseScript  {

	public GameObject hole;
	private TerrainTexture tex;

	private bool busy;
	private float delay1=3.0f;
	private float delay2=1.0f;
	private float timer;
	private float delayTime;
	public byte delayN;



	void Start () {

		toolName = "WoodenShovel";
		scriptName = "WoodenShovelScript";
		toolObj = Resources.Load (toolName, typeof(GameObject)) as GameObject;
		hole = Resources.Load ("SoilHole", typeof(GameObject)) as GameObject;
		Init ();
		tex =  new TerrainTexture ();
		tex.SetSelf (this.transform);

	}
	
	// Update is called once per frame
	void Update () {
		if ((!busy)&&(!controls.locked)) {
			if (Input.GetMouseButtonDown (0)) {
				//	Debug.Log ("LMB Pressed");
				delayTime = delay1;
				delayN = 1;
				busy = true;
				StartCoroutine ("Action1");
				return;
			}

			if (Input.GetMouseButtonDown (1)) {
				//Debug.Log ("RMB Pressed");
				delayTime = delay2;
				busy = true;
				delayN = 2;
				StartCoroutine ("Action2");
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
		float range = 8.0f;
		Vector3 direction = cam.transform.TransformDirection (Vector3.forward);
		RaycastHit hit;
		if (Physics.Raycast (cam.transform.position, direction, out hit, range)) {	

			if ((hit.collider.tag=="Usable")&&(hit.transform.gameObject.name == "Ground")){

				if (tex.TexMatch ("Grass")) {
					yield return new WaitForSeconds (delay1);
					inv.items [toolIndex].durability -= 1;
					GameObject tmp = Instantiate (hole, hit.point, Quaternion.LookRotation (hit.normal) * Quaternion.Euler (90, 0, 0)) as GameObject;
					tmp.name = hole.name; 
					tmp.name.Replace ("(Clone)", "");
				} else {
					Debug.Log ("Terrain type mismatch");
					busy = false;
					timer = 0.0f;
					yield return null;
				}

			}
		}
		yield return null;
	}

		IEnumerator Action2() {
		yield return null;
	}
}
