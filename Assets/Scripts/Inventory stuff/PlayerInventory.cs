using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour {

	public static List<ItemG> Inventory = new List<ItemG>();
	// Use this for initialization
	void Start () {
		List<ItemG> Inventory = new List<ItemG>();
		ItemG item = new ItemG ("Apple", 1);
		Inventory.Add (item);
	}

	//Find
	public int FindByName (string name){
		for (int i=0; i<Inventory.Count; i++) {
			if (Inventory[i].Name==name) {	
					return i;
			}
		}
		return -1;	
	}

	//Quantity
	public int GetQuantity (string name){
		int index = FindByName (name);
		if ( index != -1 ) {
				return Inventory [index].Quantity;
		}else{
			return -1;
		}
	}
	public int GetQuantity (int index){
		return Inventory [index].Quantity;
	}

	//Work with inventory
	public int AddItem(string name,int quantity){
		int index = FindByName (name);
		if (index != -1) {
			Inventory[index].Quantity+=quantity;
			return index;
		} else {
			ItemG item=new ItemG(name,quantity);
			Inventory.Add( item );
			return Inventory.Count-1;
		}	
	}

	// Update is called once per frame
	void Update () {
		//var fwd = transform.TransformDirection (Vector3.forward);

		//if (Physics.Raycast (transform.position, fwd, 10)) {
		//	print ("There is something in front of the object!");
		//}
	}
}
