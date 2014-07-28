using UnityEngine;
using System.Collections;

public class toolBaseScript : MonoBehaviour {
	public int toolIndex;
	public GameObject toolPlace;
	public Inventory inv;
	public GameObject cam;
	public GameObject toolModel;
	public string scriptName;
	public string toolName;
	public AudioClip useSound;
	public GameObject toolObj;
	void Start () {
		//	Init ();
	}
	
	// Update is called once per frame
	void FixedUpdate() {

		//	Validate ();

	}

	public void Equip(int invIndex) {
		toolIndex = invIndex;
		//toolModel = Instantiate (toolObj, toolPlace.transform.position, toolPlace.transform.rotation) as GameObject;
		toolName = inv.items [invIndex].name;
		toolModel = Instantiate (Resources.Load(toolName,typeof (GameObject)),toolPlace.transform.position, toolPlace.transform.rotation) as GameObject;
		toolModel.transform.parent = cam.transform;
		toolModel.collider.enabled = false;
		toolModel.rigidbody.isKinematic = true;
		toolModel.GetComponent<Item> ().enabled = false;

	}


	public void Unequip() {
		Destroy (toolModel);

		DestroyObject (this);
	}



	public void Validate() {
		if (inv.items [toolIndex] != null) {

			if (inv.items [toolIndex].name != toolName) {
				Unequip ();
				return;
			}
		} else {
			Unequip ();
			return;
		}
	}

	public void Init() {
		inv = this.gameObject.GetComponent<Inventory> ();
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
		toolPlace = GameObject.Find ("ToolPlace");
		scriptName = this.name;
	}
}
