using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class RecipeList : MonoBehaviour {
	public TextAsset recFile;
	public List<CraftRecipe> recList = new List<CraftRecipe>();

	private bool show;
	private GameObject user;
	private Inventory inv;
	private GameObject item;

	void Start () {
		user = this.gameObject;
		inv = user.GetComponent<Inventory> ();
		if(recFile != null)
		{
			XmlTextReader reader = new XmlTextReader(new StringReader(recFile.text));
			while(reader.Read())
			{
				if(reader.Name == "recipe")
				{	
					CraftRecipe recipe = new CraftRecipe ();
					recipe.name = reader.GetAttribute ("Name");
					recipe.craftTime = int.Parse(reader.GetAttribute ("CraftTime"));
					int elQ = int.Parse( reader.GetAttribute ("ElementsQuantity"));
					int resQ = int.Parse(reader.GetAttribute ("ResultsQuantity"));
					for (int i = 1; i <= elQ; i++) {
						recipe.elements.Add (reader.GetAttribute ("Element" + i.ToString ()));
						recipe.quantities.Add(int.Parse(reader.GetAttribute ("ElQ"+i.ToString())));

					}

					for (int i = 1; i <= resQ; i++) {
						Debug.Log ("i:" + i);
						recipe.results.Add (reader.GetAttribute ("Result" + i.ToString ()));
						recipe.resQ.Add(int.Parse(reader.GetAttribute ("ResQ"+i.ToString())));
						Debug.Log (reader.GetAttribute ("ResQ"+i.ToString()));
					}

					recList.Add (recipe);

					Debug.Log (recipe.name);

					}
				}

			CheckAvailability ();
			for (int i = 0; i < recList.Count; i++) {
				//		Debug.Log (recList [i].elements[0]);
				//	Debug.Log (recList [i].elements[1]);
			}
		}
	}
	

	void OnGUI () {
		if (show) {
			ShowCraftDialog ();
		}
	}

	void ShowCraftDialog() {
		Rect craftWindowRect = new Rect ((Screen.width / 2) - 175, (Screen.height / 2) - 180, 450, 350);
		craftWindowRect = GUI.Window (1,craftWindowRect, DrawCraftDialog, "Craft Item");
	}

	void DrawCraftDialog(int windowID){
		int pos = 0;
		int k = 0;
		for (int i = 0; i < recList.Count; i++) {
			if (recList[i].availability) {
				pos++;
				if (pos > 10) {
					k = 1;
					pos = 0;
				}
				if( GUI.Button( new Rect(20+100*k,60+27*pos,140,20),recList[i].name ))
				{

					for (int j = 0; j < recList [i].elements.Count; j++) {
						int q = recList [i].quantities [j];
						while (q > 0) {
							int index = inv.FindMaxQuantity (recList [i].elements [j]);
							if (q > inv.items [index].quantity) {
								q -= inv.items [index].quantity;
								inv.DestroyItem (index);
							} else {
								inv.AddQuantity (index, -q);
								q = 0;
							}
						}
					}

					for (int j = 0; j < recList [i].results.Count; j++) {
						item = Resources.Load (recList [i].results [j], typeof(GameObject)) as GameObject;
						int dex = inv.AddItem (item);
						GameObject.FindGameObjectWithTag ("GameLogic").GetComponent<MessageBox> ().AddMessage (new GameMessage ("You've crafted "+item.GetComponent<Item>().title, GameMessage.messageType.ObjectMessage));

						if (recList [i].resQ [j] > 1) {
							inv.AddQuantity (dex,( recList [i].resQ [j]-1));
						}
						CheckAvailability ();
						item = null;
					}

				}
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

	void CheckAvailability(){
		for (int i = 0; i < recList.Count; i++) {
			for (int j = 0; j < recList [i].elements.Count; j++) {
				if (inv.FindAndCountQuantity (recList [i].elements [j]) >= recList [i].quantities [j]) {
					recList [i].availability = true;
				} else {
					recList [i].availability = false;
					break;
				}
			}
		}
	}
}
