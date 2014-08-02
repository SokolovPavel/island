using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	public GameObject mapCamera;
	public GameObject HeadCamera;
	private GUIt GUIHolder;
	private MiniMap map;
	private MouseLook look;
	private RecipeList craft;
	private BuildingCrafting build;
	//private FPSInputController controller;
	private PlayerController controller;
	public bool locked = false;
	private bool equipped;
	private bool _dragMap = false;
	private Vector3 oldCamPos;
	private Vector3 oldMousePos;
	InventoryGUI invGUI;
	void Start () {
		craft = gameObject.GetComponent<RecipeList> ();
		build = gameObject.GetComponent<BuildingCrafting> ();
		look = gameObject.GetComponent<MouseLook> ();
		GUIHolder = GetComponent<GUIt> ();
		//controller = gameObject.GetComponent<FPSInputController> ();
		controller = gameObject.GetComponent<PlayerController> ();
		map = mapCamera.GetComponent<MiniMap> ();
		equipped = false;
		invGUI = gameObject.GetComponent<InventoryGUI> ();
	}

	void Update () {
		if (!map.isMinimap) {
			GUI.Label (new Rect (20, Screen.height / 2, 40, 40), "Size: 0" + map.camera.orthographicSize);
			if (Input.GetMouseButton (0)) {
				if (_dragMap) {
					Vector3 diff = oldMousePos - map.camera.ScreenToWorldPoint (Input.mousePosition);
					diff.y = 0;
					map.gameObject.transform.position = oldCamPos + diff*3;
					_dragMap = false;
				} else {
					_dragMap = true;
					oldCamPos = map.gameObject.transform.position;
					oldMousePos = map.camera.ScreenToWorldPoint (Input.mousePosition);
				}
			} else {
				_dragMap = false;
			}
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0) {
			if (map.isMinimap) {
				invGUI.changeSelectedIndex (1);
				StartCoroutine ("EquipTool", invGUI.selectedIndex);
			} else {
				map.changeCamMapSize (10);
			}

		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0) {
			if (map.isMinimap) {
				invGUI.changeSelectedIndex(-1);
				StartCoroutine( "EquipTool",invGUI.selectedIndex);
			} else {
				map.changeCamMapSize (-10);
			}

		}
		if (Input.GetButtonDown("Inventory")) {
			CloseWindows ();
			if (locked){
				map.setMiniMap();
				unlockControl();
				GUIHolder.SetCursor(GUIt.CursorType.game);
				invGUI.HideWindow ();
			} else {
				lockControl();
				GUIHolder.SetCursor(GUIt.CursorType.inventory);
				invGUI.ShowWindow ();
			}
			locked =!locked;
		}

		if (Input.GetButtonDown("Map")) {
			CloseWindows ();
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

		if (Input.GetButtonDown("Craft")) {
			CloseWindows ();
			if (locked){
				craft.HideWindow (gameObject);
				unlockControl();
				GUIHolder.SetCursor(GUIt.CursorType.game);
			} else {
				craft.ShowWindow (gameObject);
				lockControl();
				GUIHolder.SetCursor(GUIt.CursorType.inventory);
			}
			locked =!locked;
		}

		if (Input.GetButtonDown("Build")) {
			CloseWindows ();
			if (locked){
				build.HideWindow (gameObject);
				unlockControl();
				GUIHolder.SetCursor(GUIt.CursorType.game);
			} else {
				build.ShowWindow (gameObject);
				lockControl();
				GUIHolder.SetCursor(GUIt.CursorType.inventory);
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

			if (Input.GetKeyDown ("1")) { //Equip tool/weapon in inventory slot1. Лучше сделать циклом наверн.
				//UseObject ();
				StartCoroutine( "EquipTool",0);
				invGUI.selectedIndex = 0;
			}

			if (Input.GetKeyDown ("2")) { //Equip tool/weapon in inventory slot2
				//UseObject ();
				StartCoroutine( "EquipTool",(int)1);
				invGUI.selectedIndex = 1;
			}

			if (Input.GetKeyDown ("3")) { //Equip tool/weapon in inventory slot3
				//UseObject ();
				StartCoroutine( "EquipTool",(int)2);
				invGUI.selectedIndex = 2;
			}
			if (Input.GetKeyDown ("4")) { //Equip tool/weapon in inventory slot4
				//UseObject ();
				StartCoroutine( "EquipTool",(int)3);
				invGUI.selectedIndex = 3;
			}

		}

	}

	public void CloseWindows() {
		map.setMiniMap();
		craft.HideWindow (gameObject);
		build.HideWindow (gameObject);
		invGUI.HideWindow ();
	}

	public void unlockControl () {

		look.enabled = true;
		controller.enable = true;
		Screen.lockCursor = true;
		//Screen.showCursor = false;
		GUIHolder.SetCursor(GUIt.CursorType.game);
	}

	public void unlock () {
		CloseWindows ();
		locked = false;
		look.enabled = true;
		controller.enable = true;
		Screen.lockCursor = true;
		//Screen.showCursor = false;
		GUIHolder.SetCursor(GUIt.CursorType.game);
	}

	public void lockControl () {
		look.enabled = false;
		controller.enable = false;
		Screen.lockCursor = false;
		//Screen.showCursor = false;
	}

	public void showCursor() {
		GUIHolder.SetCursor(GUIt.CursorType.inventory);
	}

	public void showCrosshair() {
		GUIHolder.SetCursor(GUIt.CursorType.game);
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
				Item it = nz.GetComponent<Item> ();
				if (it != null) {
					int index = inv.AddItem (nz);

					if (index>=0) {
						if ((!equipped)&&(index == invGUI.selectedIndex)) {
							StartCoroutine( "EquipTool",index);
						}
						Destroy (nz);
					}
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
					yield return new WaitForSeconds (0.01f);
				}
				Debug.Log (inv.items [index].name);
				string tmp = inv.items [index].name +"Script";
				this.gameObject.AddComponent (tmp);

				yield return new WaitForSeconds (0.01f);

				SendMessage ("Equip", index);
				equipped = true;
			} else {
				if (equipped) {
					SendMessage ("Unequip");
					equipped = false;
				}
				yield return null;
			}
		} else {
			if (equipped) {
				SendMessage ("Unequip");
				equipped = false;
			}
			yield return null;
		}
	}

	void Unequipped() {
		equipped = false;
	}
}
