using UnityEngine;
using System.Collections;


public class ItemG : MonoBehaviour {
	private string _name;
	private string _desc;//Description
	private int _quantity;
	//private ItemTypes _type;

	public ItemG() {
		_name = "NoName";
		_desc = "No description";
		_quantity = 1;
		//_type = ItemTypes.Resource;
	}
	public ItemG(string n, int q){
		_name = n;
		_quantity = q;
		//_type = t;
	}
	public string Name {
		get  { return _name; }
		set  { _name = value; }
	}
	public string Descrtiption {
		get  { return _desc; }
		set  { _desc = value; }
	}
	public int Quantity {
		get  { return _quantity; }
		set  { 
			if (value>=0)_quantity = value; 
		}
	}
	/*public ItemTypes Type {
		get  { return _type; }
		set  { _type = value; }
	}*/

	/*public enum ItemTypes {
		Weapon,
		Instrument,
		Food,
		Seeds,
		Resource,
		Equipment
	}*/


}
