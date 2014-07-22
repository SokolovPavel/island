using UnityEngine;
using System.Collections;
using System.IO;

//10.07.2014
//Скрипт управления направления взглада игрока
//Pavel S

public class MouseLook : MonoBehaviour {
	public bool enabled = true;

	public GameObject HeadCamera;

	public float sensitivityYaw = 15F;
	public float sensitivityPitch = 15F;

	public float minimumPitch = -90F;
	public float maximumPitch = 90F;

	float yawRotation = 0F;
	float pitchRotation = 0F;
	void Update ()
	{
		if (enabled) {
			yawRotation = transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * sensitivityYaw;

			pitchRotation += Input.GetAxis ("Mouse Y") * sensitivityPitch;
			pitchRotation = Mathf.Clamp (pitchRotation, minimumPitch, maximumPitch);

			transform.localEulerAngles = new Vector3 (0, yawRotation, 0);
			HeadCamera.transform.localEulerAngles = new Vector3 (-pitchRotation, 0, 0);
		}
		if (Input.GetButtonDown ("Screenshot")) {
			StartCoroutine(ScreenCapture());
		}
	}

	IEnumerator ScreenCapture() {
		#if !UNITY_WEBPLAYER
		yield return new WaitForEndOfFrame ();
		int width = Screen.width;
		int height = Screen.height;

		Texture2D texture = new Texture2D (width, height, TextureFormat.RGB24, false);
		texture.ReadPixels ( new Rect (0, 0, width, height), 0, 0);
		texture.Apply ();

		byte[] data = texture.EncodeToPNG();
		Destroy (texture);
		string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "\\Island\\";
		if (!System.IO.Directory.Exists(path))
		{
			System.IO.Directory.CreateDirectory(path);
		}

		string filename =  path + "Screenshot" + System.DateTime.Now.ToString("MMddyyyy") + "_" + System.DateTime.Now.ToString("hhmmss") + ".png";//
		int i=0;
		while(System.IO.File.Exists(filename))
		{
			filename =  path + "Screenshot" + System.DateTime.Now.ToString("MMddyyyy") + "_" + System.DateTime.Now.ToString("hhmmss") + "_" + i + ".png";
			i++;
		}
		if(i==0){
			filename =  path + "Screenshot" + System.DateTime.Now.ToString("MMddyyyy") + "_" + System.DateTime.Now.ToString("hhmmss")+ ".png";
		}
			System.IO.File.WriteAllBytes (filename, data);

		#endif
	}
}