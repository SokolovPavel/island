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
	private Material buildMat;
	private Material objMat;
	private Texture objTex;

	private Renderer[] renderers;
	private Material[] renMat = new Material[16];
	private Texture[] renTex = new Texture[16];

	private float angle;
	// Use this for initialization
	void Start () {
		//placing = true;
		menu = this.gameObject.GetComponent<Menu>();
		buildMat = Resources.Load ("Materials\\BuildPlace", typeof(Material)) as Material;
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
						if (renderer != null) {
							building.renderer.material = objMat;
						}

						if (renderers [0] != null) {
							for (int i = 0; i < renderers.Length; i++) {
								renderers [i].material = renMat [i];

							}
						}
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

		buildMat = Resources.Load ("Materials/BuildPlace", typeof(Material)) as Material;
		blueprint = print;

		SendMessage ("Unequip", SendMessageOptions.DontRequireReceiver);

		blueprint.quantities [0] -= 1;
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
		GameObject tmp = Resources.Load (print.result, typeof(GameObject)) as GameObject;
		building =  Instantiate (tmp, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		building.collider.enabled = false;
		renderers = building.GetComponentsInChildren<Renderer> ();
		if (renderers [0] != null) {
			for (int i = 0; i < renderers.Length; i++) {
				renMat [i] = renderers [i].material;
				//renTex [i] = renderers [i].material.mainTexture;
				renderers [i].material = buildMat;
				//	renderers [i].material.mainTexture = renTex [i];
				//	Color bldCol = renderers [i].material.color;
				//	renderers [i].material.color = new Color (bldCol.r, bldCol.g, bldCol.b, 95);

			}
		}
		if (renderer != null) {
			objMat = building.renderer.material;
			objTex = building.renderer.material.mainTexture;
			building.renderer.material = buildMat;
			//building.renderer.material.mainTexture = objTex;
			//	Color bldCl = building.renderer.material.color;
			//	bldCl = new Color (bldCl.r, bldCl.g, bldCl.b, 95);
		}
		Debug.Log (buildMat.name);
		builder = building.GetComponent<Builder> ();
		builder.enabled = true;
		builder.built = false;
		builder.blueprint = print;
		placing = true;

	}


}
