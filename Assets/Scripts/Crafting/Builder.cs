using UnityEngine;
using System.Collections;

public class Builder : MonoBehaviour {

	public Blueprint blueprint;
	public bool built = false;
	private GameObject buildMarker;
	private bool mAct = false;
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
		string outStr = "You need to add ";
		for (int i = 0; i < blueprint.elements.Count; i++) {
			if ((inv.items [index].name == blueprint.elements [i])&&(blueprint.quantities[i]>0)) {

				if (blueprint.quantities [i] > inv.items [index].quantity) {

					blueprint.quantities [i] -= inv.items [index].quantity;
					inv.DestroyItem (index);

					//break;
				} else {
					inv.AddQuantity (index, -blueprint.quantities [i]);
					blueprint.quantities [i] = 0;
				}


			}

			outStr =outStr + blueprint.quantities [i];
			outStr =outStr +"  " + blueprint.elements [i] + " ";
		}
		int q = 0;
		for (int j = 0; j < blueprint.elements.Count; j++) {
			q += blueprint.quantities [j];

		}
			Debug.Log (q);
		if (q <= 0) {
			outStr = "You successfully built the " + blueprint.name; 
			built = true;
			mainBuildingScript.enabled = true;
			DestroyObject (this);
			Destroy (buildMarker);
		}
		GameObject.FindGameObjectWithTag ("GameLogic").GetComponent<MessageBox> ().AddMessage (new GameMessage (outStr, GameMessage.messageType.ObjectMessage));

	}

	void Update() {
		buildMarker.transform.position = transform.position;
		if ((this.collider.enabled)&&(!mAct)) {
			mAct = true;
			buildMarker.GetComponent<BuildMarker> ().Activate ();
		}
	}
}
