using UnityEngine;
using System.Collections;

public class MiniMap : MonoBehaviour {

	public Transform target;
	public Texture2D marker;
	public float camHeight = 1.0f;//Расстояние от головы персонажа до камеры
	public bool freezeRotation = false;
	public int rotationSpeed = 5;
	public float speedScale = 5;
	public float heightScale = 0.1f;
	public float camDistance = 2.0f;//orthographicSize
	public enum ha {left, center, right};
	public enum va {top, middle, bottom};
	public ha horisontalAlignment = ha.left;
	public va verticalAlignment = va.top;
	public int size = 50;
	public float xOffset = 0f;
	public float yOffset = 0f;

	public RenderTexture RTexture;
	public Material RMaterial;

	private Rect _miniMapRect;
	private Vector3 _prevPos;

	void Start () {
		Vector3 angles = transform.eulerAngles;
		angles.x = 90;
		angles.y = target.transform.eulerAngles.y;
		transform.eulerAngles = angles;
		_miniMapRect = Draw ();
		_prevPos = transform.position;
	}

	void Update () {
		transform.position = new Vector3 (target.transform.position.x, target.transform.position.y + camHeight, target.transform.position.z);
		if (!freezeRotation) {
			Vector3 angles = transform.eulerAngles;
			float diff = angles.y - target.transform.eulerAngles.y;
			if (Mathf.Abs(diff) < 0.9){
				angles.y = target.transform.eulerAngles.y;
			}else{
				if ((diff > 180)||(diff < -180)){
					diff = 360 * Mathf.Sign(diff) - diff;
					angles.y += diff * Time.deltaTime * rotationSpeed;
				}else{
					angles.y -= diff * Time.deltaTime * rotationSpeed;
				}
			}
			transform.eulerAngles = angles;
		}
	}

	void FixedUpdate() {
		//Изменение масштаба камеры в зависимости от движения игрока
		Vector3 curPos = transform.position;
		curPos.y = 0;
		_prevPos.y = 0;
		Vector3 move = curPos - _prevPos ;
		_prevPos = transform.position;
		float speed = move.magnitude;
		
		camera.orthographicSize = camDistance + speed*speedScale + target.position.y*heightScale;
	}
	Rect Draw () {
		int mMsize;
		if (Screen.width < Screen.height) {
			mMsize = Mathf.RoundToInt (size * 0.01f * Screen.width);
		}else {
			mMsize = Mathf.RoundToInt (size * 0.01f * Screen.height);
		}
		int hloc = Mathf.RoundToInt (xOffset * 0.01f * Screen.width);
		int vloc = Mathf.RoundToInt (Screen.height - mMsize - yOffset * 0.01f * Screen.height);

		switch(horisontalAlignment){
		case ha.left:
			hloc = Mathf.RoundToInt (xOffset * 0.01f * Screen.width);
			break;
		case ha.center:
			hloc = Mathf.RoundToInt ((Screen.width - mMsize)*0.5f - xOffset * 0.01f * Screen.width);
			break;
		case ha.right:
			hloc = Mathf.RoundToInt (Screen.width - mMsize - xOffset * 0.01f * Screen.width);
			break;
		}
		switch(verticalAlignment){
		case va.bottom:
			vloc = Mathf.RoundToInt(Screen.height - mMsize - yOffset * 0.01f * Screen.height);
			break;
		case va.top:
			vloc = Mathf.RoundToInt(yOffset * 0.01f * Screen.height);
			break;
		case va.middle:
			vloc = Mathf.RoundToInt((Screen.height-mMsize)*0.5f - yOffset * 0.01f * Screen.height);
			break;
		}
		return new Rect (hloc, vloc, mMsize, mMsize);
	}
	
	void OnGUI(){	
		if(Event.current.type == EventType.Repaint)	
			Graphics.DrawTexture(_miniMapRect, RTexture, RMaterial);		
	}
	
}
