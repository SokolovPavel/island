using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageBox : MonoBehaviour {

	private List<GameMessage> messages = new List<GameMessage>();
	public float messageLifetime = 5.0f;
	public float messageFadeDelay = 3;
	private Color messageColor;
	private Color backgroundColor = Color.gray;
	//GUIStyle messageStyle = new GUIStyle(GUI.skin.label);
	private Vector2 startPoint;
	private float startTime;
	private int prevMessages = 0;
	private bool _moveDown = false;
	private int _moveTime = 1;
	private Vector2 drawPoint;
	private Vector2 neededPoint = new Vector2((Screen.width / 2) - 200, 20);
	//public GUISkin skin;
	//GUIStyle messageStyle = new GUIStyle(GUI.skin.label);
	public Texture2D infoIcon;
	void OnGUI () {
	
		//GUI.skin = skin;
		GUI.skin.label.fontSize = 15;
		float frac=0;
		if (messages.Count > 0) {
			if (messages.Count > prevMessages) {
				startTime = Time.time;
				startPoint = new Vector2 ((Screen.width / 2) - 200, 0);
				_moveDown = true;
			}

			if (_moveDown) {
				frac = (Time.time - startTime) / _moveTime;
				drawPoint = Vector3.Lerp (startPoint, neededPoint, frac);
				if (((Time.time - startTime) / _moveTime) > 1) {
					_moveDown = false;
					drawPoint = neededPoint;
				}
			} 

			prevMessages = messages.Count;
			for (int i = 0; i < messages.Count; i++) {
				if (messages [i].type == GameMessage.messageType.ObjectMessage) {
					GUI.color= Color.white;
					GUI.Label(new Rect(drawPoint.x-20, drawPoint.y+20*(messages.Count - i)+1, 400, 20), infoIcon);
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
					if ((_moveDown)&&(i==(messages.Count-1))) {
						messageColor.a = frac;
					} else {
						messageColor.a = 1;
					}
				}
				//GUI.color = Color.white;

				//GUI.Label (new Rect ((Screen.width / 2) - 199, 19+20*i, 400, 20), messages [i].text);
				backgroundColor.a = messageColor.a;
				GUI.color = backgroundColor;
				GUI.Label (new Rect (drawPoint.x+1, drawPoint.y+20*(messages.Count - i)+1, 400, 20), messages [i].text);
				GUI.Label (new Rect (drawPoint.x, drawPoint.y+20*(messages.Count - i)+1, 400, 20), messages [i].text);
				GUI.color = messageColor;
				//messageStyle.normal.textColor = messageColor;
				GUI.Label (new Rect (drawPoint.x, drawPoint.y+20*(messages.Count - i), 400, 40), messages [i].text);

				if ((Time.time - messages [i].time - messageFadeDelay) > messageLifetime) {
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
