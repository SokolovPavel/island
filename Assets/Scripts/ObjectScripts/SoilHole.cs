using UnityEngine;
using System.Collections;

public class SoilHole : MonoBehaviour {
	public GameObject secondStage;
	public bool hydrated;
	public bool haveSeed;
	private int stage;
	public float timeToGrow;
	private int waterAmount;
	private int neededWater;
	private float timer;
	public GameObject seedObj;

	private int seedIndex;
	// Use this for initialization
	void Start () {
		hydrated = false;
		stage = 0;
	}
	
	void Use(GameObject user) //Если растение полито, то открываем окно посадки
	{
		if ((hydrated)&&(stage==0)) {
			//Open Plant Dialog
			Inventory inv = user.GetComponent<Inventory> ();
			seedObj = inv.DropItem (seedIndex);
			seedObj.transform.position = this.transform.position;
			seedObj.transform.rotation = Quaternion.identity;
			seedObj.SetActive (false);
			haveSeed = true;
			SwitchStage ();

		}

		if ((hydrated) && (stage == 1)) { //Если юзаем уже посаженную семку, то она уничтожается и лунка снова становится пустой
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

			}
		}

	}
}
