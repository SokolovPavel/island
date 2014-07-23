using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {


	public GameObject cursorTextureHolder;
	public GameObject mapCamera;

	private CursorScript cursor;
	private MiniMap map;
	private MouseLook look;
	//private FPSInputController controller;
	private PlayerController controller;
	private bool locked = false;

	void Start () {
		look = gameObject.GetComponent<MouseLook> ();
		cursor = cursorTextureHolder.GetComponent<CursorScript> ();
		//controller = gameObject.GetComponent<FPSInputController> ();
		controller = gameObject.GetComponent<PlayerController> ();
		map = mapCamera.GetComponent<MiniMap> ();

	}

	void Update () {
		if (Input.GetButtonDown("Inventory")) {
			if (locked){
				map.setMiniMap();
				unlockControl();
				cursor.SetCursor(CursorScript.CursorType.game);
			} else {
				lockControl();
				cursor.SetCursor(CursorScript.CursorType.inventory);
			}
			locked =!locked;
		}

		if (Input.GetButtonDown("Map")) {
			if (locked){
				unlockControl();
				cursor.SetCursor(CursorScript.CursorType.game);
				map.setMiniMap();
			} else {
				lockControl();
				cursor.SetCursor(CursorScript.CursorType.inventory);
				map.setMap();
			}
			locked =!locked;
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
}
