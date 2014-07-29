﻿/// <summary>
/// Inventory script. Manages player inventory
/// </summary>
using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{	
	public static int inventorySize = 127;
	public Itm[] items = new Itm[inventorySize + 1];	// Элементы инвентаря
	private int lastPickedItem;

	/// <summary>
	/// The debug object. Удалить после отладки
	/// </summary>
	public GameObject debugObj;
	public GameObject debugObj2;

	public int AddItem(GameObject obj) // Добавить придмет в инвентарь. Исходный предмет не уничтожает, следует об этом позаботиться
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

		} else { 				
			bool stacked = false;
			for (int i = 0; i < inventorySize; i++) {
				if (items [i] != null) {
					if (it.name == items [i].name) {
						if ((items [i].quantity + it.quantity) > (items [i].maxQ)) {
							it.quantity = it.quantity + items [i].quantity - items [i].maxQ;
							items [i].quantity = items [i].maxQ;
							stacked = false;
							break; //Идем добавлять остатки в новый слот.
						} else {
							items [i].quantity += it.quantity;
							resIndex = i;
							stacked = true;
							break;
						}
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
		if (resIndex >= 0) {
			lastPickedItem = resIndex;
		}
		return resIndex;
	}

	public int AddItem(GameObject obj, int q) // Добавить придмет в инвентарь (не весь стак, только часть). Использовать только для стакабельных предметов!
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

		} else { 				
			bool stacked = false;
			for (int i = 0; i < inventorySize; i++) {
				if (items [i] != null) {
					if (it.name == items [i].name) {
						if ((items [i].quantity + q) > (items [i].maxQ)) {
							it.quantity = q + items [i].quantity - items [i].maxQ;
							items [i].quantity = items [i].maxQ;
							stacked = false;
							break; //Идем добавлять остатки в новый слот.
						} else {
							items [i].quantity += it.quantity;
							resIndex = i;
							stacked = true;
							break;
						}
					}
				}
			}


			if (!stacked) {
				for (int i = 0; i < inventorySize; i++) {

					if (items [i] == null) {
						// Итак, начинаем. Ибо простое присваивание запихает сюда только ссылку на объект, копируем все ручками
						items [i] = new Itm (it.name, it.title, it.desc, it.weight, it.maxQ, q, it.durability, it.maxDur, it.type);
						resIndex = i;
						break;
					}

				}

			}
		}
		if (resIndex >= 0) {
			lastPickedItem = resIndex;
		}
		return resIndex;
	}



	public int AddItem(Itm it) //Прям таки набиваю строчки, перегружая функцию и меняя в ней всего пару строк. Тут можно добавить итем с помощью экземпляра служебного класса инвентаря.
	{
		int resIndex = -1;
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

		} else { 				
			bool stacked = false;
			for (int i = 0; i < inventorySize; i++) {
				if (items [i] != null) {
					if (it.name == items [i].name) {
						if ((items [i].quantity + it.quantity) > (items [i].maxQ)) {
							it.quantity = it.quantity + items [i].quantity - items [i].maxQ;
							items [i].quantity = items [i].maxQ;
							stacked = false;
							break; //Идем добавлять остатки в новый слот.
						} else {
							items [i].quantity += it.quantity;
							resIndex = i;
							stacked = true;
							break;
						}
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
		if (resIndex >= 0) {
			lastPickedItem = resIndex;
		}
		return resIndex;
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

	public GameObject DropLastItem (){ // Выбрасывает из инвентаря последний добавленный предмет
		GameObject it = new GameObject();
		if (items [lastPickedItem] == null) {
			for (int i = inventorySize; i >= 0; i--) {
				if (items [i] != null) {
					lastPickedItem = i;
					break;
				}
			}
		}
		if (items [lastPickedItem] != null) {
			it = (GameObject)Instantiate (Resources.Load (items [lastPickedItem].name, typeof(GameObject)));

			Item im = it.GetComponent<Item> ();
			im.SetItem (items [lastPickedItem].name, items [lastPickedItem].title, items [lastPickedItem].desc, items [lastPickedItem].weight, items [lastPickedItem].maxQ, items [lastPickedItem].quantity, items [lastPickedItem].durability, items [lastPickedItem].maxDur, items [lastPickedItem].type);
			items [lastPickedItem] = null;
		}
		return it;

	}


	public int GetQuantity (int index)          //Returns item quantity. If not found, returns 0
	{
		if (index >= 0) {
			int ret = 0;
			ret = items [index].quantity;	 
			return ret;
		} else
			return 0;
	}

	public void SetQuantity (int index, int amount)          //Задать количество предмету. Ограничения на максимальное количество не учитывает (пока)
	{
		items [index].quantity = amount;
		if (items [index].quantity <= 0) {
			items [index] = null;
		}
	}

	public void AddQuantity (int index, int amount)          //Добавить предмету количество. Ограничения на максимальное количество не учитывает (пока)
	{
		items [index].quantity += amount;
		if (items [index].quantity <= 0) {
			items [index]  = null;
		}

	}	

	public int FindByName (string n)          //Returns item index. If not found, returns -1
	{
		int ret;
		for (int i=0; i<inventorySize; i++) {
			if (items [i] != null) {
				if (items [i].name == n) {
					ret = i;	
					return ret;
				}
			}
		}
		ret = -1;
		return ret;

	}

	public void DestroyItem(int index) // Уничтожает объект из инвентаря под заданным индексом.
	{
		items[index] = null;

	}

	public float GetTotalWeight() // Считает общую массу инвентаря. Будет полезно при введении ограничения на носимый груз. Выводит одно число флоатом.
	{
		float mass = 0.0f;
		for (int i = 0; i < inventorySize; i++) {
			mass += items [i].weight;
		}

		return mass;
	}

	public void SwapItems(int index1, int index2) { //Боюсь, может уничтожать итемы, надо тестить
		Itm temp =  new Itm (items[index1].name,items[index1].title, items[index1].desc, items[index1].weight, items[index1].maxQ, items[index1].quantity, items[index1].durability, items[index1].maxDur, items[index1].type);
		items [index1] = items [index2];
		items [index2] = temp;
	}
}