using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blueprint {

	public string name;
	public int craftTime;
	public bool availability;
	public List<string> elements = new List<string>();
	public List<int> quantities = new List<int>();
	public string result;


	public Blueprint(){
		//elements.Add ("DebugItem");
		//quantities.Add (1);
		//results.Add("Apple");
		//resQ.Add(1);
	}


}
