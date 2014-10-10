using UnityEngine;
using System.Collections;

public class Book : MonoBehaviour {
	public string Text;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Use (GameObject user) {
		GameObject.FindGameObjectWithTag ("GameLogic").GetComponent<MessageBox> ().AddMessage (new GameMessage (Text, GameMessage.messageType.Other));

	}
}
