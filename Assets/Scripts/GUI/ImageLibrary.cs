using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class namedImage : MonoBehaviour{
	public string name;
	public Texture2D icon;
	public namedImage(string n, Texture2D i){
		name = n;
		icon = i;
	}
}

public class ImageLibrary : MonoBehaviour {

	//public namedImage[] images = new Itm[inventorySize + 1];
	public List<namedImage> images = new List<namedImage>();
	Texture2D tempImage;
	public Texture2D getImage(string name){
		for (int i = 0; i < images.Count; i++) {
			if (images [i].name == name) {
				return images [i].icon;

			}
		}
		tempImage = (Texture2D)Resources.Load ("itemImages/"+name, typeof(Texture2D));
		if (tempImage == null) {
			images.Add (new namedImage (name, images[0].icon));
			return images [0].icon;
		} else {
			images.Add (new namedImage (name, tempImage));
			return tempImage;
		}
	}
}
