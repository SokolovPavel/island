﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Library : MonoBehaviour {

	public static List<Food> FoodLibrary = new List<Food>();

	void Awake () {
		//Apple 0
		Food food = new Food (0, 5, 10, 5);
		food.Name = "Apple";
		food.Descrtiption = "Green ripe apple";
		food.Quantity = 1;
		FoodLibrary.Add (food);
		//Banana 1
		food = new Food (0, 10, 10, 0);
		food.Name = "Banana";
		food.Descrtiption = "Yummy banana";
		food.Quantity = 1;
		FoodLibrary.Add (food);

	}

}
