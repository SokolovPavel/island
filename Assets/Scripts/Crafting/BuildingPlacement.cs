using UnityEngine;
using System.Collections;

public class BuildingPlacement : MonoBehaviour {
	public string buildingName;
	public GameObject building;
	public Blueprint blueprint;


	private bool busy;
	private float timer;
	private GameObject cam;
	private bool placing;
	private Builder builder;
	private Menu menu;

	private float angle;
	// Use this for initialization
	void Start () {
		//placing = true;
		menu = this.gameObject.GetComponent<Menu>();
	}
	
	// Update is called once per frame
	void Update () {

		if (placing) {
			float range = 8.0f;
			Vector3 direction = cam.transform.TransformDirection (Vector3.forward);
			RaycastHit hit;
			if (Physics.Raycast (cam.transform.position, direction, out hit, range)) {	

				if (hit.transform.gameObject.name == "Ground") {
					building.transform.position = hit.point;
					building.transform.rotation = Quaternion.LookRotation (hit.normal) * Quaternion.Euler (90, 0, 0);
					building.transform.rotation *= Quaternion.Euler (0, angle, 0);
					if(Input.GetKey("y")) {
						angle += 1f;
					}

					if(Input.GetKey("u")) {
						angle -= 1f;
					}

					if (Input.GetMouseButtonDown (0)) {
						placing = false;
						building.collider.enabled = true;
						menu.unlock ();
						DestroyObject (this);
					}
				}

			}

		}
	}

	public void Activate(Blueprint print) {
		blueprint = print;

		blueprint.quantities [0] -= 1;
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
		GameObject tmp = Resources.Load (print.result, typeof(GameObject)) as GameObject;
		building =  Instantiate (tmp, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		building.collider.enabled = false;
		builder = building.GetComponent<Builder> ();
		builder.enabled = true;
		builder.built = false;
		builder.blueprint = print;
		placing = true;
	}


}
