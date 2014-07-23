﻿using System.Collections;

public class Itm  

{
	public string name;
	public string title;
	public string desc;
	public float weight;
	public int maxQ;
	public int quantity;
	public int durability;
	public int maxDur;
	public Item.enType type;
	/*public enum enType
	{
		Weapon,
		Instrument,
		Food,
		Seeds,
		Resource,
		Equipment
	}
*/

	public Itm(string n, string t, string d, float w, int mQ, int q, int dur, int mDur, Item.enType typ)
	{
		name = n;
		title = t;
		desc = t;
		weight = w;
		maxQ = mQ;
		quantity = q;
		durability = dur;
		maxDur = mDur;
		type = typ;
	}

	public Itm()
	{
		name = "DebugCube";
		title = "DebugItem";
		desc = "Just a placeholder";
		weight = 0.01f;
		maxQ = 0;
		quantity = 0;
		durability = 1;
		maxDur = 1;
		type = Item.enType.Resource;
	}

}
