using UnityEngine;
using System.Collections;

public class TreeLoot : MonoBehaviour {

	public string ChopLoot;
	private GameObject chLtObj;
	private Itm chItem;
	public int chopQuantity;


	public string useLoot;
	private GameObject useLtObj;
	private Itm useItem;
	public int useQuantity = 1;

	void Start() {
		chLtObj = Resources.Load (ChopLoot, typeof(GameObject)) as GameObject;
		Item it = chLtObj.GetComponent<Item> ();
		chItem = new Itm (it.name, it.title, it.desc, it.weight, it.maxQ, it.quantity, it.durability, it.maxDur, it.type);

		if (useLoot != null) {
			useLtObj = Resources.Load (useLoot, typeof(GameObject)) as GameObject;
			it = useLtObj.GetComponent<Item> ();
			useItem = new Itm (it.name, it.title, it.desc, it.weight, it.maxQ, it.quantity, it.durability, it.maxDur, it.type);
		}
	}

	public void Use(GameObject user) {
		if (useItem != null) {
			Inventory inv = user.GetComponent<Inventory> ();
			useItem.quantity = useQuantity;
			inv.AddItem (useItem);
		}
	}

	public void Chop(ToolParams par) {
		Debug.Log ("Chopping");
		if (chItem != null) {
			Debug.Log ("AddingItem");
			Inventory inv = par.owner.GetComponent<Inventory> ();
			chItem.quantity = Mathf.CeilToInt (chopQuantity * par.mainCoef);
			inv.AddItem (chItem);
		}
	}



}
