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
	public string[] craftNames;

	private int selectedItem = 0;
	private bool toogleBool = false;
	Rect craftWindowRect;
	string descriptionText;
	int prevSelectedItem = 0;
	//public InventoryGUI invGUIholder;
	ImageLibrary library = new ImageLibrary();
	Texture2D itemImage;
	void Start () {
		craftWindowRect = new Rect (Screen.width*0.3f, (Screen.height / 2) - 180, Screen.width*0.4f, 350);
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

		craftNames = new string[recList.Count];
	}
	

	void OnGUI () {
		if (show) {
			ShowCraftDialog ();
		}
	}

	public void ShowCraftDialog() {

		craftWindowRect = GUI.Window (1,craftWindowRect, DrawCraftDialog, "Craft Item");
	}

	void DrawCraftDialog(int windowID){
		int pos = 0;
		int k = 0;
		for (int i = 0; i < recList.Count; i++) {
			if (recList[i].availability) {
				pos++;
				if (selectedItem == i) {
					toogleBool = true;
				} else {
					toogleBool = false;
				}
				if (GUI.Toggle (new Rect (20, 20 + 27 * pos, 140, 20), toogleBool, recList [i].name)) {
					selectedItem = i;
				}

			}
		}
		if (recList [selectedItem].availability) {
			if (selectedItem != prevSelectedItem) {
				item = Resources.Load (recList [selectedItem].results [0], typeof(GameObject)) as GameObject;
				descriptionText = item.GetComponent<Item> ().desc;
				prevSelectedItem = selectedItem;
				itemImage = library.getImage (recList [selectedItem].results[0]);
			}
			GUI.Label (new Rect (200, 20, 100, 100), itemImage);
			GUI.Label (new Rect (200, 150, 300, 100), descriptionText);
		}
		if( recList[selectedItem].availability && GUI.Button( new Rect(craftWindowRect.width-150,craftWindowRect.height-40,100,20),"Craft item" ))
		{
			for (int j = 0; j < recList [selectedItem].elements.Count; j++) {
				int q = recList [selectedItem].quantities [j];
				while (q > 0) {
					int index = inv.FindMaxQuantity (recList [selectedItem].elements [j]);
					if (q > inv.items [index].quantity) {
						q -= inv.items [index].quantity;
						inv.DestroyItem (index);
					} else {
						inv.AddQuantity (index, -q);
						q = 0;
					}
				}
			}

			for (int j = 0; j < recList [selectedItem].results.Count; j++) {
				item = Resources.Load (recList [selectedItem].results [j], typeof(GameObject)) as GameObject;
				int dex = inv.AddItem (item);
				GameObject.FindGameObjectWithTag ("GameLogic").GetComponent<MessageBox> ().AddMessage (new GameMessage ("You've crafted "+item.GetComponent<Item>().title, GameMessage.messageType.ObjectMessage));

				if (recList [selectedItem].resQ [j] > 1) {
					inv.AddQuantity (dex,( recList [selectedItem].resQ [j]-1));
				}
				CheckAvailability ();
				item = null;
			}
			if (!recList [selectedItem].availability)
				selectedItem = -1;
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
					//craftNames [i] = recList [i].name;
				} else {
					recList [i].availability = false;
					break;
				}
			}
		}
	}
}
