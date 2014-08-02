using UnityEngine;
using System.Collections;

public class Builder : MonoBehaviour {

	public Blueprint blueprint;
	public bool built = false;
	private GameObject buildMarker;
	public MonoBehaviour mainBuildingScript;
	// Use this for initialization
	void Start () {
		if (built) {
			mainBuildingScript.enabled = true;
			DestroyObject (this);

		} else {
			mainBuildingScript.enabled = false;
			buildMarker = Instantiate (Resources.Load ("BuildMarker", typeof(GameObject)), this.gameObject.transform.position, this.transform.rotation) as GameObject;
			//	renderer.material.color = new Color (50, 50, 200);

		}
	}
	
	// Update is called once per frame
	void Use (GameObject user) {
		Inventory inv = user.GetComponent<Inventory> ();
		InventoryGUI invGUI = user.GetComponent<InventoryGUI> ();
		int index = invGUI.selectedIndex;

		for (int i = 0; i < blueprint.elements.Count; i++) {
			if ((inv.items [index].name == blueprint.elements [i])&&(blueprint.quantities[i]>0)) {
				if (blueprint.quantities [i] > inv.items [index].quantity) {

					blueprint.quantities [i] -= inv.items [index].quantity;
					inv.DestroyItem (index);
					break;
				} else {
					inv.AddQuantity (index, -blueprint.quantities [i]);
					blueprint.quantities [i] = 0;
				}
			}
		}
		int q = 0;
		for (int j = 0; j < blueprint.elements.Count; j++) {
			q += blueprint.quantities [j];

		}
			Debug.Log (q);
		if (q <= 0) {
			built = true;
			mainBuildingScript.enabled = true;
			DestroyObject (this);
			Destroy (buildMarker);
		}

	}

	void Update() {
		buildMarker.transform.position = transform.position;

	}
}
