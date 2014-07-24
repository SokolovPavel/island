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

	void Start () {
		look = gameObject.GetComponent<MouseLook> ();
		GUIHolder = GetComponent<GUIt> ();
		//controller = gameObject.GetComponent<FPSInputController> ();
		controller = gameObject.GetComponent<PlayerController> ();
		map = mapCamera.GetComponent<MiniMap> ();

	}

	void Update () {
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
			GameObject obj = inv.DropLastItem();

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


}
