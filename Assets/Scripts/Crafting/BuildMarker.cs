using UnityEngine;
using System.Collections;

public class BuildMarker : MonoBehaviour {

	public float rotSpd = 0.5f;
	public AudioClip placingSound;
	public GameObject smokePuff;


	// Update is called once per frame
	void FixedUpdate () {

		transform.rotation *= Quaternion.Euler(0,rotSpd,0);
	}

	void Start() {
		smokePuff.SetActive (false);
	}

	public void Activate() { 
		smokePuff.SetActive (true);
		audio.PlayOneShot (placingSound);
		Destroy (smokePuff, 2);

	}
}
