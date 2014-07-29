using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	public GameObject mapCamera;
	public GameObject HeadCamera;
	private GUIt GUIHolder;
	private MiniMap map;
	private MouseLook look;
	//private FPSInputController controller;
	private PlayerController controller;
	private bool locked = false;
	private bool equipped;
	InventoryGUI invGUI;
	void Start () {
		look = gameObject.GetComponent<MouseLook> ();
		GUIHolder = GetComponent<GUIt> ();
		//controller = gameObject.GetComponent<FPSInputController> ();
		controller = gameObject.GetComponent<PlayerController> ();
		map = mapCamera.GetComponent<MiniMap> ();
		equipped = false;
		invGUI = gameObject.GetComponent<InventoryGUI> ();
	}

	void Update () {
		if (Input.GetAxis("Mouse ScrollWheel") < 0) {
			invGUI.changeSelectedIndex(1);
			StartCoroutine( "EquipTool",invGUI.selectedIndex);
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0) {
			invGUI.changeSelectedIndex(-1);
			StartCoroutine( "EquipTool",invGUI.selectedIndex);
		}
		if (Input.GetButtonDown("Inventory")) {
			if (locked){
				map.setMiniMap();
				unlockControl();
				GUIHolder.SetCursor(GUIt.CursorType.game);
			} else {
				lockControl();
				GUIHolder.SetCursor(GUIt.CursorType.inventory);
			}
			locked =!locked;
		}

		if (Input.GetButtonDown("Map")) {
			if (locked){
				unlockControl();
				GUIHolder.SetCursor(GUIt.CursorType.game);
				map.setMiniMap();
			} else {
				lockControl();
				GUIHolder.SetCursor(GUIt.CursorType.inventory);
				map.setMap();
			}
			locked =!locked;
		}


		if (!locked) {
			if (Input.GetButtonDown ("Take")) {
				TakeItem ();
			}

			if (Input.GetButtonDown ("Drop")) {
				DropItem ();
				//Inventory inv = GetComponent<Inventory>();
				//inv.DropItem (invGUI.selectedIndex, 1);
			}

			if (Input.GetButtonDown ("Use")) {
				UseObject ();
			}

			if (Input.GetKeyDown ("1")) { //Equip tool/weapon in inventory slot1
				//UseObject ();
				StartCoroutine( "EquipTool",0);
			}

			if (Input.GetKeyDown ("2")) { //Equip tool/weapon in inventory slot2
				//UseObject ();
				StartCoroutine( "EquipTool",(int)1);
			}

			if (Input.GetKeyDown ("3")) { //Equip tool/weapon in inventory slot3
				//UseObject ();
				StartCoroutine( "EquipTool",(int)2);
			}
			if (Input.GetKeyDown ("4")) { //Equip tool/weapon in inventory slot4
				//UseObject ();
				StartCoroutine( "EquipTool",(int)3);
			}

		}

	}

	void unlockControl () {
		look.enabled = true;
		controller.enabled = true;
		Screen.lockCursor = true;
		//Screen.showCursor = false;
	}

	void lockControl () {
		look.enabled = false;
		controller.enabled = false;
		Screen.lockCursor = false;
		//Screen.showCursor = false;
	}




	void DropItem()
	{
		Vector3 direction = HeadCamera.transform.TransformDirection (Vector3.forward);
		float range = 7.0f;
		RaycastHit hit;
		if (Physics.Raycast (HeadCamera.transform.position, direction, out hit, range)) {

			Inventory inv = GetComponent<Inventory>();
			//GameObject obj = inv.DropLastItem();
			GameObject obj = inv.DropItem (invGUI.selectedIndex, 1);

			Component[] allComponents = obj.GetComponents<Component>();
			if (allComponents.Length == 1) { // Contains only Transform?
				Destroy (obj);
				return;
			}
			obj.transform.position = hit.point;
			obj.transform.rotation = Quaternion.identity;

		} else {
			return;
		}

	}

	void TakeItem()
	{
		Inventory inv = GetComponent<Inventory>();
		float range = 10f;
		Vector3 direction = HeadCamera.transform.TransformDirection (Vector3.forward);
		RaycastHit hit;
		if (Physics.Raycast (HeadCamera.transform.position, direction, out hit, range)) {	
			if ((hit.rigidbody)&&(hit.rigidbody.gameObject.name!="Player"))
			{
				GameObject nz= hit.transform.gameObject;
				if (inv.AddItem (nz) >= 0) {
					Destroy (nz);
				}
			}
		}
	}

	void UseObject(){
		float range = 10f;
		Vector3 direction = HeadCamera.transform.TransformDirection (Vector3.forward);
		RaycastHit hit;
		if (Physics.Raycast (HeadCamera.transform.position, direction, out hit, range)) {	
			if(hit.collider.tag=="Water")
			{
				hit.transform.gameObject.SendMessage("Drink",this.transform.gameObject, SendMessageOptions.DontRequireReceiver);
			}
			if ((hit.rigidbody)&&(hit.rigidbody.gameObject.name!="Player"))
			{hit.transform.gameObject.SendMessage("Use",this.transform.gameObject,SendMessageOptions.DontRequireReceiver);Debug.Log("Use Rigidbody");	}
			else if (hit.collider.tag=="Usable")
			{hit.transform.gameObject.SendMessage("Use",this.transform.gameObject,SendMessageOptions.DontRequireReceiver);Debug.Log("Use object");}
		}


	}

	IEnumerator EquipTool(int index){
		Inventory inv = GetComponent<Inventory> ();
		if (inv.items [index] != null) {
			if ((inv.items [index].type == Item.enType.Tool) || (inv.items [index].type == Item.enType.Weapon)) {
				if (equipped) {
					SendMessage ("Unequip");
					yield return new WaitForSeconds (0.02f);
				}
				Debug.Log (inv.items [index].name);
				string tmp = inv.items [index].name +"Script";
				this.gameObject.AddComponent (tmp);

				yield return new WaitForSeconds (0.02f);

				SendMessage ("Equip", index);
				equipped = true;
			} else {
				yield return null;
			}
		} else {
			yield return null;
		}
	}
}
