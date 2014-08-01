using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class BuildingCrafting : MonoBehaviour {

	public TextAsset recFile;
	public List<Blueprint> recList = new List<Blueprint>();

	private bool show;
	private GameObject user;
	private Inventory inv;
	private GameObject item;
	private Menu menu;

	void Start () {
		user = this.gameObject;
		inv = user.GetComponent<Inventory> ();
		menu = this.gameObject.GetComponent<Menu> ();
		if(recFile != null)
		{
			XmlTextReader reader = new XmlTextReader(new StringReader(recFile.text));
			while(reader.Read())
			{
				if(reader.Name == "blueprint")
				{	
					Blueprint recipe = new Blueprint ();
					recipe.name = reader.GetAttribute ("Name");
					recipe.craftTime = int.Parse(reader.GetAttribute ("CraftTime"));
					int elQ = int.Parse( reader.GetAttribute ("ElementsQuantity"));

					for (int i = 1; i <= elQ; i++) {
						recipe.elements.Add (reader.GetAttribute ("Element" + i.ToString ()));
						recipe.quantities.Add(int.Parse(reader.GetAttribute ("ElQ"+i.ToString())));

					}

					recipe.result = reader.GetAttribute ("Result");

					recList.Add (recipe);

					Debug.Log (recipe.name);

				}
			}

			//CheckAvailability ();
			for (int i = 0; i < recList.Count; i++) {
				//		Debug.Log (recList [i].elements[0]);
				//	Debug.Log (recList [i].elements[1]);
			}
		}
	}

	void CheckAvailability(){
		for (int i = 0; i < recList.Count; i++) {
			if (inv.FindByName (recList [i].elements [0]) >= 0) {
				recList [i].availability = true;
			} else {
				recList [i].availability = false;
			}
		}
	}

	public void ShowWindow(GameObject player){
		user = player;
		inv = player.GetComponent<Inventory> ();
		CheckAvailability ();
		show = true;
	}

	public void HideWindow(GameObject player) {
		show = false;
		user = player;

	}

	void OnGUI () {
		if (show) {
			ShowBuildDialog ();
		}
	}



	void ShowBuildDialog() {
		Rect buildWindowRect = new Rect ((Screen.width / 2) - 175, (Screen.height / 2) - 180, 450, 350);
		buildWindowRect = GUI.Window (1,buildWindowRect, DrawBuildDialog, "Create building");
	}


	void DrawBuildDialog(int windowID){
		int pos = 0;
		int k = 0;
		for (int i = 0; i < recList.Count; i++) {
			if (recList[i].availability) {
				pos++;
				if (pos > 10) {
					k = 1;
					pos = 0;
				}
				if( GUI.Button( new Rect(20+100*k,30+27*pos,140,20),recList[i].name ))
				{
					int index = inv.FindByName (recList [i].elements [0]);
					inv.AddQuantity (index, -1);
					CheckAvailability ();
					BuildingPlacement plc = this.gameObject.AddComponent<BuildingPlacement> ();
					menu.unlock ();
					plc.Activate (recList [i]);
					show = false;
				}
			}
		}
	}

}
