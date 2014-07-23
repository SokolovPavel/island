/// <summary>
/// Inventory script. Manages player inventory
/// </summary>
using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{	
	public static int inventorySize = 127;
	//public int[] itemQuantity = new int[inventorySize + 1];
	public Itm[] items = new Itm[inventorySize + 1];	// Элементы инвентаря

	public GameObject debugObj;
	public GameObject debugObj2;

	public int AddItem(GameObject obj)
	{
		int resIndex = -1;
		Item it = obj.GetComponent<Item> ();
		if (it.maxQ == 1) {
			for (int i = 0; i < inventorySize; i++) {
				if (items [i] == null) {
					// Итак, начинаем. Ибо простое присваивание запихает сюда только ссылку на объект, копируем все ручками
					items [i] = new Itm (it.name, it.title, it.desc, it.weight, it.maxQ, it.quantity, it.durability, it.maxDur, it.type);
					resIndex = i;
					Debug.Log (resIndex);
					break;
				}

			}

		} else { 				//TODO: добавить проверку на наполнение стака. Пока подождет
			bool stacked = false;
			for (int i = 0; i < inventorySize; i++) {
				if (items [i].maxQ > 0) {
					if (it.name == items [i].name) {
						items [i].quantity += it.quantity;
						resIndex = i;
						stacked = true;
						break;
					}
				}
			}

			if (!stacked) {
				for (int i = 0; i < inventorySize; i++) {

					if (items [i] == null) {
						// Итак, начинаем. Ибо простое присваивание запихает сюда только ссылку на объект, копируем все ручками
							items [i] = new Itm (it.name, it.title, it.desc, it.weight, it.maxQ, it.quantity, it.durability, it.maxDur, it.type);
						resIndex = i;
						break;
					}

				}

			}
		}

		return resIndex;
	}



	/// <summary>
	/// Debug function
	/// </summary>
			void Update()
			{
	
			}


	public GameObject DropItem( int itemIndex) //Spawn a gameobject from inventory. You should position it manually. If no object at this position, returns empty GameObject
	{
		GameObject it = new GameObject();
		if (items [itemIndex] != null) {
			it = (GameObject)Instantiate (Resources.Load (items [itemIndex].name, typeof(GameObject)));

			Item im = it.GetComponent<Item> ();
			im.SetItem (items [itemIndex].name, items [itemIndex].title, items [itemIndex].desc, items [itemIndex].weight, items [itemIndex].maxQ, items [itemIndex].quantity, items [itemIndex].durability, items [itemIndex].maxDur, items [itemIndex].type);
			items [itemIndex] = null;
		}
		return it;

	}

	public GameObject DropItem( int itemIndex, int quantity) //Spawn a gameobject from inventory. You should position it manually. If no object at this position, returns empty GameObject
	{
		GameObject it = new GameObject();
		if (items [itemIndex] != null) {

			it = (GameObject)Instantiate (Resources.Load (items [itemIndex].name, typeof(GameObject)));
			if (items [itemIndex].quantity < quantity) {
				quantity = items [itemIndex].quantity;
			}

			Item im = it.GetComponent<Item> ();
			im.SetItem (items [itemIndex].name, items [itemIndex].title, items [itemIndex].desc, items [itemIndex].weight, items [itemIndex].maxQ, quantity, items [itemIndex].durability, items [itemIndex].maxDur, items [itemIndex].type);
			items [itemIndex].quantity -= quantity;
			if (items [itemIndex].quantity == 0) {
				items [itemIndex] = null;
			}
		}
		return it;

	}

}