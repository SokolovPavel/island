using UnityEngine;
using System.Collections;

public class ItemHolder : MonoBehaviour {
	public ItemG item;
	void Awake(){
		//if (){
			item.Name=this.transform.name;
			item.Quantity=1;
		//}
	}
}
