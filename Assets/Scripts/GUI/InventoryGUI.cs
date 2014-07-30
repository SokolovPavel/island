using UnityEngine;
using System.Collections;

public class InventoryGUI : MonoBehaviour {

	public Texture2D[] image;
	public Texture2D background;
	public Texture2D hotbarFrame;
	public GUISkin guiSkin;
	private int _selectedIndex = 0;
	private int _buttonSize = 30;
	private int _buttonShift = 5;
	private int _buttonX = 50;
	private int _buttonY = Screen.height - 50;

	private bool _contexMenuShow = false;
	private int _contexMenuX = 0;
	private int _contexMenuSize;
	private int _contexMenuIndex;
	int id;

	private Vector3 _dragPrevPos;
	private bool _dragSetup = false;
	private bool _dragOn = false;
	private int _dragFromIndex;
	// Use this for initialization
	Inventory inventory;
	void Start () {
		_buttonSize = (int) (Screen.width * 0.4f - 9 * _buttonShift) / 10;
		_buttonX = (int) (Screen.width/2 - 5 * _buttonSize - 4.5f * _buttonShift);
		_buttonY = Screen.height - _buttonShift - _buttonSize;
		_contexMenuSize = (int)(0.9f * _buttonSize/2);
		inventory = gameObject.GetComponent<Inventory>(); 
	}

	void OnGUI () {

		if (Input.GetMouseButton (0)) {
			if (_dragOn) {
				GUI.DrawTexture (new Rect (Input.mousePosition.x, Input.mousePosition.y, _buttonSize, _buttonSize), getImage (inventory.items [_dragFromIndex].name));
			} else {
				if (_dragSetup) {
					Vector3 diff = Input.mousePosition - _dragPrevPos;
					if (diff.sqrMagnitude > 25) {
						_dragOn = true;
						Debug.Log ("DragOn");
					} else {
						_dragSetup = false;
					}
				} else {
					_dragFromIndex = checkHitSlot (Input.mousePosition);
					if (_dragFromIndex >= 0) {
						_dragPrevPos = Input.mousePosition;
						_dragSetup = true;
						Debug.Log ("DragSetup");
					}
				}
			}
		} else {
			_dragOn = false;
			_dragSetup = false;
		}
			//if (_dragOn)
		//	findNearestAndSwipe ();

		GUI.skin = guiSkin;
		if (Event.current.type.Equals (EventType.Repaint)) {
			GUI.DrawTexture (new Rect (Screen.width * 0.26f, Screen.height - _buttonSize - 2 * _buttonShift - 10, Screen.width - Screen.width * 2 * 0.26f, _buttonSize + 2 * _buttonShift + 10), background);
		}
		for (int i = 0; i < 10; i++) {
			if (inventory.items [i] != null) {
				if (GUI.Button (new Rect (_buttonX + i * (_buttonSize + _buttonShift), _buttonY, _buttonSize, _buttonSize), new GUIContent (getImage(inventory.items[i].name), "inventory slot" + i))) {
					if (Event.current.button == 0) {
						Debug.Log ("Rigth click");
					}else if (Event.current.button == 1) {
						showContexMenu (i);
					}
				}
				if(inventory.items [i].quantity>1)GUI.Label (new Rect (_buttonX + i * (_buttonSize + _buttonShift)+4, _buttonY, _buttonSize, _buttonSize), inventory.items [i].quantity.ToString());
			} else {
				GUI.Button (new Rect (_buttonX + i * (_buttonSize + _buttonShift), _buttonY, _buttonSize, _buttonSize),"");
			}

		}
		if (Event.current.type.Equals (EventType.Repaint)) {
			GUI.DrawTexture (new Rect(_buttonX + _selectedIndex * (_buttonSize + _buttonShift), _buttonY, _buttonSize, _buttonSize), hotbarFrame);
		}
		if (_contexMenuShow) {
			GUI.Button (new Rect (_contexMenuX - 2 * _contexMenuSize , _buttonY - _contexMenuSize, 2 * _contexMenuSize, _contexMenuSize), "Eat");
			GUI.Button (new Rect (_contexMenuX + _buttonSize/2 - _contexMenuSize, _buttonY - _contexMenuSize, 2 * _contexMenuSize, _contexMenuSize), "Equip");
			GUI.Button (new Rect (_contexMenuX + _buttonSize, _buttonY - _contexMenuSize, 2 * _contexMenuSize, _contexMenuSize), "Use");
		}
	}
	void pickUpItem(int index){
		Debug.Log ("Index : " + index);
		_contexMenuIndex = index;
	}
	void showContexMenu (int index){
		if (!_contexMenuShow) {
			_contexMenuShow = true;
		} else {
			if (index == _contexMenuIndex)
				_contexMenuShow = false;

		}
		_contexMenuIndex = index;
		_contexMenuX = _buttonX + index * (_buttonSize + _buttonShift);
	}
	Texture getImage(string name){
		if (name == "Apple")
			return image[0];
		else if (name == "AppleSeed")
			return image[1];
		else if (name == "StoneAxe")
			return image[2];
		else if (name == "WoodenShovel")
			return image[3];
		else
			return image[4];
	}
	public void changeSelectedIndex(int diff){
		_selectedIndex = (_selectedIndex + diff + 10) % 10;
	}
	public int selectedIndex {
		get  { return _selectedIndex; }
		set  { if ((value <= 9) && (value >= 0))
				_selectedIndex = value;
			else
				Debug.Log ("Wrong selected index for hotbar!");
		}
	}

	int checkHitSlot(Vector2 pos){
		if( (pos.y <=  (_buttonSize + _buttonShift) )&& (pos.y >= _buttonShift)) {
			if (pos.x > _buttonX) {
				int index = (int)((pos.x - _buttonX) / (_buttonSize + _buttonShift));
				if (((pos.x - _buttonX) - index * (_buttonSize + _buttonShift)) < _buttonSize) {
					return index;
				} else {
					return -1;
				}
			} else {
				return -1;
			}
		} else {
			return -1;
		}
	}
}
