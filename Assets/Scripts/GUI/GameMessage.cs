using UnityEngine;
using System.Collections;

public class GameMessage  {
	public string text;
	public float time;
	public enum messageType
	{
		Warning,
		Tip,
		ObjectMessage,
		Other,
		statMessage
	}
	public messageType type;

	public GameMessage(){

	}

	public GameMessage(string txt, messageType typ){
		text = txt;
		time = Time.time;
		type = typ;
	}
}
