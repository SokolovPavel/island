using UnityEngine;
using System.Collections;

public class CursorScript : MonoBehaviour {

	public Texture2D menuCursor;
	public Vector2 menuCursorOffset;
	public Texture2D inventoryCursor;
	public Vector2 inventoryCursorOffset;
	public Texture2D gameCursor;
	public Vector2 gameCursorOffset;

	private Vector2 cursorOffset;
	public bool lockCursor = true;

	public enum CursorType {game, menu, inventory };
	void Start () {
		guiTexture.enabled = true;
		if (gameCursor) {
			guiTexture.texture = gameCursor;
			cursorOffset = gameCursorOffset;
			Screen.showCursor = false;
			Screen.lockCursor = true;
			guiTexture.pixelInset = new Rect (Screen.width/2 - cursorOffset.x, Screen.height/2 - cursorOffset.y, guiTexture.texture.width, guiTexture.texture.height);
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (!lockCursor) {
			Vector2 mousePos = Input.mousePosition;
			Texture mouseTex = guiTexture.texture;
			guiTexture.pixelInset = new Rect (mousePos.x - cursorOffset.x, mousePos.y - cursorOffset.y, mouseTex.width, mouseTex.height);
		}
	}

	public void SetCursor (CursorType type) {
		switch (type) {
		case CursorType.game:
			guiTexture.texture = gameCursor;
			cursorOffset = gameCursorOffset;
			lockCursor = true;
			break;
		case CursorType.menu:
			guiTexture.texture = menuCursor;
			cursorOffset = menuCursorOffset;
			lockCursor = false;
			break;
		case CursorType.inventory:
			guiTexture.texture = inventoryCursor;
			cursorOffset = inventoryCursorOffset;
			lockCursor = false;
			break;
		}
		if (lockCursor) {
			guiTexture.pixelInset = new Rect (Screen.width/2 - cursorOffset.x, Screen.height/2 - cursorOffset.y, guiTexture.texture.width, guiTexture.texture.height);
		}
	}
}
