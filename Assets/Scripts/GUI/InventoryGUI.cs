using UnityEngine;
using System.Collections;

public class InventoryGUI : MonoBehaviour {

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
	private int _contexMenuY = 0;
	private int _contexMenuSize;
	private int _contexMenuIndex;
	int id;

	private Vector3 _dragPrevPos;
	private bool _dragSetup = false;
	private bool _dragOn = false;
	private int _dragFromIndex;
	private int _dragToIndex;
	private int _dragTempStorage = 126;
	// Use this for initialization

	private bool _showInventory = false;
	private int _windowYShift = (int)(Screen.height*0.05f);
	ImageLibrary library = new ImageLibrary();
	Inventory inventory;
	void Start () {
		_buttonSize = (int) (Screen.width * 0.4f - 9 * _buttonShift) / 10;
		_buttonX = (int) (Screen.width/2 - 5 * _buttonSize - 4.5f * _buttonShift);
		_buttonY = Screen.height - _buttonShift - _buttonSize;
		_contexMenuSize = (int)(0.9f * _buttonSize/2);
		inventory = gameObject.GetComponent<Inventory>(); 
		library.getImage ("NoImage");
	}

	void OnGUI () {
		GUI.skin = guiSkin;

		//Фон хотбара
		if (Event.current.type.Equals (EventType.Repaint)) {
			GUI.DrawTexture (new Rect (Screen.width * 0.26f, Screen.height - _buttonSize - 2 * _buttonShift - 10, Screen.width - Screen.width * 2 * 0.26f, _buttonSize + 2 * _buttonShift + 10), background);
		}
		for (int j = 0; j < 9; j++) {
			for (int i = 0; i < 10; i++) {
				int Y = _buttonY - j * (_buttonSize + _buttonShift);
				if (j > 0) {
					Y -= _windowYShift;
				}
				if (inventory.items [i + j*10] != null) {
					if (GUI.Button (new Rect (_buttonX + i * (_buttonSize + _buttonShift), Y, _buttonSize, _buttonSize), library.getImage (inventory.items [i + j*10].name))) {

						if (Event.current.button == 0) {
							_contexMenuShow = false;
							if (_dragOn) {
								if (inventory.items [_dragTempStorage].name == inventory.items [i + j*10].name) {
									inventory.SetQuantity (_dragTempStorage, inventory.AddQuantity (i + j*10, inventory.items [_dragTempStorage].quantity));
									if (inventory.items [_dragTempStorage] == null) {
										_dragOn = false;
									}
								} 

							} else {
								_dragOn = true;
								inventory.SwapItems (i + j*10, _dragTempStorage);
							}
						} else if (Event.current.button == 1) {
							if (_dragOn) {
								inventory.shareStack (_dragTempStorage, i + j*10);
								if (inventory.items [_dragTempStorage] == null)
									_dragOn = false;
							} else {
								showContexMenu (i,j);
							}
						}
					}
					if ((inventory.items [i + j*10] != null) && (inventory.items [i + j*10].quantity > 1))
						GUI.Label (new Rect (_buttonX + i * (_buttonSize + _buttonShift) + 4, Y, _buttonSize, _buttonSize), inventory.items [i + j*10].quantity.ToString ());
				} else {
					if (GUI.Button (new Rect (_buttonX + i * (_buttonSize + _buttonShift), Y, _buttonSize, _buttonSize), "")) {
						if (Event.current.button == 0) {
							_contexMenuShow = false;
							if (_dragOn) {
								inventory.SwapItems (_dragTempStorage, i + j*10);
								_dragOn = false;
							} 
						} else if (Event.current.button == 1) {
							if (_dragOn) {
								inventory.shareStack (_dragTempStorage, i + j*10);
								if (inventory.items [_dragTempStorage] == null)
									_dragOn = false;
							}
						}
					}
		
				}

			}
			if (!_showInventory) {
				break;
			}
		}

		//Рамка выделения
		if (Event.current.type.Equals (EventType.Repaint)) {
			GUI.DrawTexture (new Rect(_buttonX + _selectedIndex * (_buttonSize + _buttonShift), _buttonY, _buttonSize, _buttonSize), hotbarFrame);
		}
		if (_contexMenuShow) {
			if (GUI.Button (new Rect (_contexMenuX - 2 * _contexMenuSize, _contexMenuY - _contexMenuSize, 2 * _contexMenuSize, _contexMenuSize), "Eat")) {
				GameObject item = new GameObject();
				item = inventory.DropItem(_contexMenuIndex, 1);
				if (inventory.items [_contexMenuIndex] == null) {
					_contexMenuShow = false;
				}
				item.gameObject.SendMessage ("Use", this.transform.gameObject, SendMessageOptions.DontRequireReceiver);
				//Destroy (item);
			}
			GUI.Button (new Rect (_contexMenuX + _buttonSize/2 - _contexMenuSize,_contexMenuY - _contexMenuSize, 2 * _contexMenuSize, _contexMenuSize), "Equip");
			GUI.Button (new Rect (_contexMenuX + _buttonSize, _contexMenuY - _contexMenuSize, 2 * _contexMenuSize, _contexMenuSize), "Use");
		}

		//DragAndDrop
		/*if (Input.GetMouseButton (0)) {
			if (!_dragOn) {
				_dragFromIndex = checkHitSlot (Input.mousePosition);
				if (_dragFromIndex >= 0)
					_dragOn = true;
			} 
		} else if (_dragOn) {
			_dragToIndex = checkHitSlot (Input.mousePosition);
			if((_dragFromIndex < 10)&&(inventory.items [_dragToIndex] == null))
				inventory.SwapItems (_dragFromIndex, _dragToIndex);
			_dragOn = false;
		}*/

		//Иконка итема около курсора
		if (Event.current.type.Equals (EventType.Repaint)) {
			if ((_dragOn)&&(inventory.items [_dragTempStorage] != null)) {
				GUI.DrawTexture (new Rect (Input.mousePosition.x - _buttonSize / 2, Screen.height - Input.mousePosition.y - _buttonSize / 2, _buttonSize, _buttonSize), library.getImage (inventory.items [_dragTempStorage].name));
				if (inventory.items [_dragTempStorage].quantity > 1)
					GUI.Label (new Rect (Input.mousePosition.x - _buttonSize / 2, Screen.height - Input.mousePosition.y - _buttonSize / 2, _buttonSize, _buttonSize), inventory.items [_dragTempStorage].quantity.ToString ());
			}
		}
	}

	void pickUpItem(int index){
		Debug.Log ("Index : " + index);
		_contexMenuIndex = index;
	}

	void showContexMenu (int indexX, int indexY){
		if (!_contexMenuShow) {
			_contexMenuShow = true;
		} else {
			if ( (indexX + indexY*10) == _contexMenuIndex)
				_contexMenuShow = false;

		}
		_contexMenuIndex = indexX + indexY*10;
		_contexMenuX = _buttonX + indexX * (_buttonSize + _buttonShift);
		_contexMenuY = _buttonY - indexY * (_buttonSize + _buttonShift);
		if(indexY>0)
			_contexMenuY -= _windowYShift;
	}
	/*Texture getImage(string name){
		int index = 0;
		switch (name) {
		case "Apple":
			index = 1;
			break;
		case "AppleSeed":
			index = 2;
			break;
		case "StoneAxe":
			index = 3;
			break;
		case "WoodenShovel":
			index = 4;
			break;
		case "TreeLog":
			index = 5;
			break;
		case "TreeBranch":
			index = 6;
			break;
		case "PineTreeSeed":
			index = 7;
			break;
		}
		return image [index];
	}*/
		
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
					if (index < 10) {
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
		} else {
			return -1;
		}
	}

	public void HideWindow (){
		_showInventory = false;
	}
	public void ShowWindow (){
		_showInventory = true;
	}
}
