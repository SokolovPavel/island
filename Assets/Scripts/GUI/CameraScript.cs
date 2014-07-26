using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraScript : MonoBehaviour {

	public Texture texture;
	public Material material;
	public int size=25;
	void Start() {
		material.SetFloat ("_Width", Screen.width*0.015f*size);
	}
	void OnGUI() {
		if (Event.current.type.Equals (EventType.Repaint))
			Graphics.DrawTexture (new Rect (0, 0, Screen.width*0.015f*size, Screen.height*0.01f*size), texture, material);
	}
}