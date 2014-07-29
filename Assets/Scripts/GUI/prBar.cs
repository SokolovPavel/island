using UnityEngine;
using System.Collections;

public class prBar : MonoBehaviour {
	public Texture texture;
	public Material material;
	public int size=50;

	public void DrawBar(float percent) {
		material.SetFloat("_Progress",percent);
		if (Event.current.type.Equals (EventType.Repaint))
			Graphics.DrawTexture (new Rect (Screen.width/2 - texture.width/2*0.01f*size, Screen.height/2 - texture.height*0.01f*size, texture.width * size * 0.01f, texture.height * 0.01f * size), texture, material);

	}
}
