using UnityEngine;
using System.Collections;

public class DrinkWater : MonoBehaviour {
	public float thirstAmount;
	public bool canBeBottled;
	public float healthPenalty;
	public AudioClip drinkSound;

	void Drink(GameObject user)
	{
		user.SendMessage ("addThirst", thirstAmount);
		AudioSource.PlayClipAtPoint (drinkSound, user.transform.position);
		if (healthPenalty != 0.0f) {
			user.SendMessage ("addHealth", -1.0f * healthPenalty);
		}

	}

}
