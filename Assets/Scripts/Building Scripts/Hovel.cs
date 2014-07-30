using UnityEngine;
using System.Collections;

public class Hovel : MonoBehaviour {

	public float RestingSpeed;
	public float bedQuality;
	private bool sleeping = false;
	public Transform sleepPlace;
	private GameObject ply;
	private PlayerController controller;
	private PlayerStats stats;
	private Vector3 lastPos;
	//private var gui:Transform;
	private float time = 0.0f;

	void Use(GameObject user)
	{
		controller = user.GetComponent<PlayerController> ();
		stats = user.GetComponent<PlayerStats> ();

		if(!sleeping)
		{
			ply = user;
			controller.enabled = false;
			lastPos = user.transform.position;
			user.transform.position = sleepPlace.transform.position;
			sleeping=true;

			return;
		}



	}

	void Update () {
		if (sleeping) {
			time += Time.deltaTime;
			if (time > 1.0f) {
				ply.SendMessage("addEnergy",RestingSpeed);
				ply.SendMessage("addHunger",-bedQuality);
				ply.SendMessage("addThirst",-bedQuality*1.5f);
				time = 0.0f;
				if (stats.energy > (stats.maxEnergy - 1)) {
					sleeping = false;
					Wake ();
				}

			}
			if (Input.anyKeyDown) {
				Wake ();
			}

		}
	}

	void Wake() {
		sleeping = false;
		controller.enabled = true;
		ply.transform.position = lastPos;

	}
}
