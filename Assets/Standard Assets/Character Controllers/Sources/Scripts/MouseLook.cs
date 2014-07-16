using UnityEngine;
using System.Collections;

//10.07.2014
//Скрипт управления направления взглада игрока
//Pavel S

public class MouseLook : MonoBehaviour {

	public GameObject HeadCamera;

	public float sensitivityYaw = 15F;
	public float sensitivityPitch = 15F;

	public float minimumPitch = -90F;
	public float maximumPitch = 90F;

	float yawRotation = 0F;
	float pitchRotation = 0F;
	void Update ()
	{
		yawRotation = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityYaw;
		
		pitchRotation += Input.GetAxis("Mouse Y") * sensitivityPitch;
		pitchRotation = Mathf.Clamp (pitchRotation, minimumPitch, maximumPitch);

		transform.localEulerAngles = new Vector3(0, yawRotation, 0);
		HeadCamera.transform.localEulerAngles = new Vector3(-pitchRotation, 0, 0);
	}
	
	void Start ()
	{

	}
}