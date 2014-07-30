using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIt : MonoBehaviour {

	public bool ShowPlayerStats = true;
	public bool ShowMaps = true;
	public GameObject mapHolder;
	public Texture2D menuCursor;
	public Vector2 menuCursorOffset;
	public Texture2D inventoryCursor;
	public Vector2 inventoryCursorOffset;
	public Texture2D gameCursor;
	public Vector2 gameCursorOffset;

	public enum CursorType {game, menu, inventory };
	public bool lockCursor = true;

	[System.NonSerialized]
	public Texture2D currentCursor;
	[System.NonSerialized]
	public Vector2 currentOffset;
	MiniMap map;
	void Start () {
		map = mapHolder.GetComponent<MiniMap> ();
		if (gameCursor) {
			currentCursor = gameCursor;
			currentOffset = gameCursorOffset;
			Screen.showCursor = false;
			Screen.lockCursor = true;
		}

	}

	void OnGUI () {
		map.Draw ();
		GUI.depth = 0;
		if (lockCursor) {
			GUI.DrawTexture (new Rect (Screen.width/2 - currentOffset.x/2, Screen.height/2 - currentOffset.y/2, currentCursor.width, currentCursor.height), currentCursor);
		} else {
			GUI.DrawTexture (new Rect (Input.mousePosition.x - currentOffset.x, Screen.height - Input.mousePosition.y - currentOffset.y, currentCursor.width, currentCursor.height), currentCursor);
		}
	}



	public void SetCursor (CursorType type) {
		switch (type) {
		case CursorType.game:
			currentCursor = gameCursor;
			currentOffset = gameCursorOffset;
			lockCursor = true;
			break;
		case CursorType.menu:
			currentCursor = menuCursor;
			currentOffset = menuCursorOffset;
			lockCursor = false;
			break;
		case CursorType.inventory:
			currentCursor = inventoryCursor;
			currentOffset = inventoryCursorOffset;
			lockCursor = false;
			break;
		}
	}
}
