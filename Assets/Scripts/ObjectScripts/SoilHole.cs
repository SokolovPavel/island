using UnityEngine;
using System.Collections;

public class SoilHole : MonoBehaviour {
	public GameObject secondStage;
	public bool hydrated;
	public bool haveSeed;
	private int stage;
	public float timeToGrow;
	public int waterAmount;
	private int neededWater;
	private float timer;
	public GameObject seedObj;

	private bool showWindow;
	private int seedIndex = 0;
	private Menu menu;
	private Inventory inv;
	// Use this for initialization
	void Start () {
		hydrated = false;
		stage = 0;
		//menu = this.gameObject.GetComponent<Menu> ();
	}

	void Update() {
		if (showWindow) {
			if ((Input.GetButtonDown ("Inventory")) || (Input.GetButtonDown ("Use"))) {
				showWindow = false;
				menu.enabled = true;
				menu.locked = false;
				menu.unlockControl();
				menu.showCrosshair();

			}
		}
	}

	void Use(GameObject user) //Если растение полито, то открываем окно посадки
	{
		if ((hydrated)&&(stage==0)) {

			//Open Plant Dialog
			menu =  user.gameObject.GetComponent<Menu> ();
			inv =  user.GetComponent<Inventory> ();
			showWindow = true;
			menu.locked = true;
			menu.lockControl();
			menu.showCursor ();
			menu.enabled = false;



		} else	if ((hydrated) && (stage == 1)) { //Если юзаем уже посаженную семку, то она уничтожается и лунка снова становится пустой
			haveSeed = false;
			timer = 0.0f;
			waterAmount = 1;
			stage = 0;
			this.gameObject.GetComponent<MeshRenderer> ().enabled = true;
			secondStage.SetActive (false);
			Destroy (seedObj);
			seedObj = null;
		}


	}

	void SwitchStage(){
		stage++;
		secondStage.SetActive (true);
		this.gameObject.GetComponent<MeshRenderer> ().enabled = false;

	}

	void FixedUpdate(){
		if (haveSeed) {
			timer += Time.fixedDeltaTime;
			if ((timer > timeToGrow) && (waterAmount >= neededWater)) {
				seedObj.SetActive (true);
				seedObj.GetComponent<SeedScript> ().Activate (this.gameObject, true);
				Destroy (this.gameObject,5.0f);
			}
		}

	}

	void Hydrate() {
		hydrated = true;
		waterAmount++;
	}

	void Plant(int index) {

		seedObj = inv.DropItem (index,1);
			seedObj.transform.position = this.transform.position;
			seedObj.transform.rotation = Quaternion.identity;
			seedObj.SetActive (false);
			haveSeed = true;
			SwitchStage (); 
	}

	void OnGUI() {
		if (showWindow) {
			ShowPlantDialog ();
		}
	}

	void ShowPlantDialog() {
		Rect plantWindowRect = new Rect ((Screen.width / 2) - 175, (Screen.height / 2) - 180, 450, 350);
		plantWindowRect = GUI.Window (1,plantWindowRect, DrawPlantDialog, "Plant seed");
	}

	void DrawPlantDialog(int windowID){
		GUI.Label (new Rect (15, 20, 160, 20), "Click on button to plant seed:");
		GUI.Label (new Rect (15, 40, 200, 20), "Click on 'close' button to close window");

		if (GUI.Button (new Rect (20,330,60,20), "Close"))
		{
			showWindow = false;
			menu.enabled = true;
			menu.locked = false;
			menu.unlockControl();
			menu.showCrosshair();
		} 
		int pos = 0;
		int k = 0;
		for (int i = 0; i < inv.items.Length; i++) {
			if ((inv.items [i] != null) && (inv.items [i].type == Item.enType.Seeds)) {
				pos++;
				if (pos > 10) {
					k = 1;
				}
				if( GUI.Button( new Rect(20+100*k,60+27*pos,140,20),inv.items[i].title ))
				{
					Plant (i);

					showWindow = false;
					menu.enabled = true;
					menu.locked = false;
					menu.unlockControl();
					menu.showCrosshair();

				}
			}
		}


	}

}
