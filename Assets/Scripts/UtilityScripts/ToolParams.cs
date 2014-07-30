using UnityEngine;
using System.Collections;

public class ToolParams {
	public GameObject owner;
	public float mainCoef;

	public ToolParams(GameObject own,float coef) {
		owner = own;
		mainCoef = coef;
	}
}
