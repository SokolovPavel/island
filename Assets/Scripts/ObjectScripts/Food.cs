using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

	public float hungerAmount; //hunger restore amount. may be negative to make player feel more hungry
	public float healthAmount; //health restore amount. may be negative to harm player (ex. poison mushrooms)
	public bool haveSeeds; //if food should drop seeds, switch this flag on;
	private Itm seeds; //if food have haveSeeds on, here should be link to seed object;
	public byte seedsDropChance; //seed drop chance, larger number -> less drop chance;
	public string seedName;
	public string seedTitle;
	public string seedDescription;
	public float seedWeight;
	public int seedMaxQ;
	public int seedQuantity;

	void Use(GameObject eater) {
		eater.SendMessage("addHunger",hungerAmount, SendMessageOptions.DontRequireReceiver);
		if(healthAmount!=0) {eater.SendMessage("addHealth",healthAmount, SendMessageOptions.DontRequireReceiver);}
		if(haveSeeds)
		{
			if (haveSeeds) {
				seeds = new Itm (seedName, seedTitle, seedDescription, seedWeight, seedMaxQ, seedQuantity, 1, 1, Item.enType.Seeds);
			}
			int dice;
			dice=Mathf.CeilToInt(Random.value*seedsDropChance);
			if (dice==1)
			{
				eater.GetComponent<Inventory> ().AddItem (seeds);

			}			
		}
		this.gameObject.GetComponent<Item>().quantity -= 1;	
		if(this.gameObject.GetComponent<Item>().quantity<1){Destroy(gameObject);}
	}
}
