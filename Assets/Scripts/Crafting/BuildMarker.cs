using UnityEngine;
using System.Collections;

public class BuildMarker : MonoBehaviour {

	public float rotSpd = 0.5f;
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.rotation *= Quaternion.Euler(0,rotSpd,0);
	}
}
