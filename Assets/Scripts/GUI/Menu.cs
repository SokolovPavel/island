using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	public GameObject mapCamera;

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
