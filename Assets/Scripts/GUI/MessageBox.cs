using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageBox : MonoBehaviour {

	private List<GameMessage> messages = new List<GameMessage>();
	public float messageLifetime = 10.0f;
	public float messageFadeDelay = 3;
	private Color messageColor;
	//GUIStyle messageStyle = new GUIStyle(GUI.skin.label);
	void OnGUI () {
		if (messages.Count > 0) {
			for (int i = 0; i < messages.Count; i++) {
				if (messages [i].type == GameMessage.messageType.ObjectMessage) {
					messageColor = Color.blue;
				} else if (messages [i].type == GameMessage.messageType.Warning) {
					messageColor = Color.red;
				} else if (messages [i].type == GameMessage.messageType.Tip) {
					messageColor = Color.gray;
				} else if (messages [i].type == GameMessage.messageType.Other) {
					messageColor = Color.yellow;
				} else if (messages [i].type == GameMessage.messageType.statMessage) {
					messageColor = Color.green;
				} else {
					messageColor = Color.white;
				}
				if ((Time.time - messages [i].time) > messageFadeDelay) {
					messageColor.a = 1 - (Time.time - messages [i].time - messageFadeDelay) / messageLifetime;
				} else {
					messageColor.a = 1;
				}
				//GUI.color = Color.white;

				//GUI.Label (new Rect ((Screen.width / 2) - 199, 19+20*i, 400, 20), messages [i].text);
				GUI.color = messageColor;
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
