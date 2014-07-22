/// <summary>
/// Inventory script. Manages player inventory
/// </summary>
using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{	
	public static int inventorySize = 127;
	//public int[] itemQuantity = new int[inventorySize + 1];
	public Item[] items = new Item[inventorySize + 1];	// Элементы инвентаря

	public GameObject debugObj;


	public int AddItem(GameObject obj)
	{
		int resIndex = -1;
		Item it = obj.GetComponent<Item> ();
		if (it.maxQ == 1) {
			for (int i = 0; i < inventorySize; i++) {
				if (!items [i]) {
					// Итак, начинаем. Ибо простое присваивание запихает сюда только ссылку на объект, копируем все ручками
					//items [i] = Item (it.name, it.title, it.desc, it.weight, it.maxQ, it.quantity, it.durability, it.maxDur, it.type);
					items [i] = it;
					items [i].name = it.name;

					resIndex = i;
					break;
				}

			}

		} else { 				//TODO: добавить проверку на наполнение стака. Пока подождет
			bool stacked = false;
			for (int i = 0; i < inventorySize; i++) {
				if (items [i]) {
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

					if (!items [i]) {
						// Итак, начинаем. Ибо простое присваивание запихает сюда только ссылку на объект, копируем все ручками
						//	items [i] = new Item (it.name, it.title, it.desc, it.weight, it.maxQ, it.quantity, it.durability, it.maxDur, it.type);
						resIndex = i;
						break;
					}

				}

			}
		}

		return resIndex;
	}

			void Update()
			{
		if (Input.GetButtonDown("Use"))
				{
			AddItem (debugObj);
				}
			}

}