using UnityEngine;
using System.Collections;

public class Fireplace : MonoBehaviour {
	public GameObject fire;
	public GameObject BurntModel;
	public GameObject light;
	private bool burnt= false;
	public bool isBurning;
	public float maxBurnTime;
	public float curBurnTime;
	//public  itemToRefuel;;
	//public var refuelTime : int;
	public AudioSource fireSound;
	// Use this for initialization
	void Start () {
		curBurnTime = 0;
	}

	void FixedUpdate () 
	{
		if(isBurning)
		{
			curBurnTime=curBurnTime+Time.fixedDeltaTime;
			if(curBurnTime>maxBurnTime)
			{
				Extinguish (true);
			}
		}
	}

	void Burn()
	{
		fire.SetActive(true);
		isBurning=true;
		audio.Play();
		light.SetActive (true);
	}

	void Extinguish(bool destroy){
		fire.SetActive(false);
		isBurning=false;
		audio.Stop();
		burnt = true;
		BurntModel.SetActive (true);
		this.gameObject.renderer.enabled = false;
		Destroy (light, 1);
		if (destroy) {
			Destroy (this.gameObject, 60);
		}
	}

	void Use( GameObject user)
	{

		if (!burnt) {

			if (!isBurning) {
				if (curBurnTime < maxBurnTime) {
					Burn ();
				}
			} else if (isBurning) {
				Inventory inv = user.GetComponent<Inventory> ();
				InventoryGUI invGUI = user.GetComponent<InventoryGUI> ();
				string name = inv.items [invGUI.selectedIndex].name;
				if (name == "TreeBranch") {
					maxBurnTime += 100.0f;
					inv.AddQuantity (invGUI.selectedIndex, -1);

				} else if (name == "TreeLog") {
					maxBurnTime += 1000.0f;
					inv.AddQuantity (invGUI.selectedIndex, -1);

				} else if (name == "RawPlank") {
					maxBurnTime += 500.0f;
					inv.AddQuantity (invGUI.selectedIndex, -1);

				} else {
					Debug.Log ("This item is not suitable to refuel.");
				}
	
			}
		}
	}


}
