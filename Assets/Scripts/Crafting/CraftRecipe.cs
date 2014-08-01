using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class CraftRecipe {
	public string name;
	public int craftTime;
	public bool availability;
	public List<string> elements = new List<string>();
	public List<int> quantities = new List<int>();
	public List<string> results = new List<string>();
	public List<int> resQ = new List<int>();


	public CraftRecipe(){
		//elements.Add ("DebugItem");
		//quantities.Add (1);
		//results.Add("Apple");
		//resQ.Add(1);
	}


}
