﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class simpleGuiScript : MonoBehaviour {

	public Texture texture;
	public Material material;
	public int size=50;

	void OnGUI() {
		material.SetFloat("_Progress", 0.7f);
		if (Event.current.type.Equals (EventType.Repaint))
			Graphics.DrawTexture (new Rect (Screen.width/2 - texture.width/2*0.01f*size, Screen.height/2 - texture.height*0.01f*size, texture.width * size * 0.01f, texture.height * 0.01f * size), texture, material);
	}
}