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
	private GameObject sky;
	private TOD_Time Ttime;
	private float oldDayLength;
	private float oldEnergy;

	void Use(GameObject user)
	{
		if (enabled) {

			controller = user.GetComponent<PlayerController> ();
			stats = user.GetComponent<PlayerStats> ();

			if (!sleeping) {
				oldEnergy = stats.energy;
				sleeping = true;
				time = 0.0f;
				sky = GameObject.FindGameObjectWithTag ("Sky");
				Ttime = sky.GetComponent<TOD_Time> ();
				oldDayLength = Ttime.DayLengthInMinutes;
				Ttime.DayLengthInMinutes =3f;

				ply = user;
				controller.enabled = false;
				lastPos = user.transform.position;
				user.transform.position = sleepPlace.transform.position;

				//Debug.Log ("Y U DONT WORK?!!!");
				//return;
			}
		} else {
			Debug.Log ("nah, im disabled.");
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

					Wake ();
					return;
				}

			}
			if ((Input.anyKeyDown)&&(time>0.2f)) {
				Wake ();
			}

		} 
	}

	void Wake() {
		Ttime.DayLengthInMinutes = oldDayLength;
		sleeping = false;
		controller.enabled = true;
		ply.transform.position = lastPos;
		//Debug.Log ("Wake up mazafaka");
		GameObject.FindGameObjectWithTag ("GameLogic").GetComponent<MessageBox> ().AddMessage (new GameMessage ("You've restored "+(Mathf.CeilToInt(stats.energy - oldEnergy)) + " energy points", GameMessage.messageType.ObjectMessage));

	}
}
