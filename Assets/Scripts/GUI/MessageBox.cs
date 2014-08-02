using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageBox : MonoBehaviour {

	private List<GameMessage> messages = new List<GameMessage>();
	public float messageLifetime = 10.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnGUI () {
		if (messages.Count > 0) {
			for (int i = 0; i < messages.Count; i++) {
				GUI.Label (new Rect ((Screen.width / 2) - 200, 20+20*i, 400, 20), messages [i].text);
				if ((Time.time - messages [i].time) > messageLifetime) {
					messages.Remove (messages [i]);
				}
			}
		}
	}

	public void AddMessage(GameMessage message){
		message.time = Time.time;
		messages.Add (message);
	}
}
