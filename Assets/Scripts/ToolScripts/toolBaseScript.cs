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

	public Texture texture;
	public Material material;
	public int size=50;

	private bool equipped;
	public Menu controls;
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
		equipped = true;
	}


	public void Unequip() {
		Destroy (toolModel);
		SendMessage ("Unequipped");
		DestroyObject (this);
	}



	public void Validate() {
		if (equipped) {
			if (inv.items [toolIndex] != null) {

				if (inv.items [toolIndex].name != toolName) {
					Unequip ();
					return;
				}

				if (inv.items [toolIndex].durability <= 0) {
					inv.DestroyItem (toolIndex);
					Unequip ();
					return;
				}
			} else {
				Unequip ();
				return;
			}
		}
	}

	public void Init() {
		inv = this.gameObject.GetComponent<Inventory> ();
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
		toolPlace = GameObject.Find ("ToolPlace");
		scriptName = this.name;
		prBar b = this.gameObject.GetComponent<prBar> ();
		texture = b.texture;
		material = b.material;
		controls = this.gameObject.GetComponent<Menu> ();
	}

	public void DrawBar(float percent) {
		material.SetFloat("_Progress",percent);
		if (Event.current.type.Equals (EventType.Repaint))
			Graphics.DrawTexture (new Rect (Screen.width/2 - texture.width/2*0.01f*size, Screen.height/2 - texture.height*0.01f*size, texture.width * size * 0.01f, texture.height * 0.01f * size), texture, material);

	}
}
