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
	public float camMapDistance = 10f;

	public enum ha {left, center, right};
	public enum va {top, middle, bottom};
	public ha horisontalAlignment = ha.left;
	public va verticalAlignment = va.top;
	public int miniMapSize = 50;
	public int mapSize = 80;
	public float xOffset = 0f;
	public float yOffset = 0f;
	public Vector3 CameraMapModePosition;
	public RenderTexture RTexture;
	public Material RMaterial;
	public Material RMapMaterial;

	public float changeTime = 1;
	private float currentTime = 0;

	//Флаги включения анимации между миникартой и картой
	private bool isMovingDown = false;
	private bool isMovingUp = false;

	private Rect _miniMapRect;
	private Rect _mapRect;
	private Rect _currentRect;
	private Vector3 _prevPos;
	private bool isMinimap = true;

	void Start () {
		Vector3 angles = transform.eulerAngles;
		angles.x = 90;
		angles.y = target.transform.eulerAngles.y;
		transform.eulerAngles = angles;
		_miniMapRect = DrawSetup ();
		_mapRect = setupMapRect ();
		_prevPos = transform.position;
	}

	void Update () {
		if (isMinimap) {

			if (_currentRect != _miniMapRect) {
				float proportion = currentTime/changeTime;
				currentTime -= Time.deltaTime;
				if (proportion <= 0) {
					proportion = 0;
					currentTime = 0;
				}
				_currentRect.x = _mapRect.x * (proportion) + (1 - proportion) * _miniMapRect.x;
				_currentRect.y = _mapRect.y * ( proportion) + (1 - proportion) * _miniMapRect.y;
				_currentRect.width = _mapRect.width * (proportion) + (1 - proportion) * _miniMapRect.width;
				_currentRect.height = _mapRect.height * (proportion) + (1 - proportion) * _miniMapRect.height;
			} 

			//transform.position.y = 0.1f * (target.transform.position.y + camHeight - transform.position.y);
			//transform.position += 0.1f * (new Vector3 (target.transform.position.x, target.transform.position.y + camHeight, target.transform.position.z) - transform.position);
			transform.position = new Vector3 (target.transform.position.x, target.transform.position.y + camHeight + 0.1f * (transform.position.x - target.transform.position.y), target.transform.position.z);
			if (!freezeRotation) {
				Vector3 angles = transform.eulerAngles;
				float diff = angles.y - target.transform.eulerAngles.y;
				if (Mathf.Abs (diff) < 0.9) {
					angles.y = target.transform.eulerAngles.y;
				} else {
					if ((diff > 180) || (diff < -180)) {
						diff = 360 * Mathf.Sign (diff) - diff;
						angles.y += diff * Time.deltaTime * rotationSpeed;
					} else {
						angles.y -= diff * Time.deltaTime * rotationSpeed;
					}
				}
				transform.eulerAngles = angles;
			}
		} else {
			if (_currentRect != _mapRect) {
				float proportion = currentTime/changeTime;
				currentTime += Time.deltaTime;
				if (proportion >= 1) {
					proportion = 1;
					currentTime = changeTime;
				}
				_currentRect.x = _miniMapRect.x * (1 - proportion) + proportion * _mapRect.x;
				_currentRect.y = _miniMapRect.y * (1 - proportion) + proportion * _mapRect.y;
				_currentRect.width = _miniMapRect.width * (1 - proportion) + proportion * _mapRect.width;
				_currentRect.height = _miniMapRect.height * (1 - proportion) + proportion * _mapRect.height;
			} 
		}
	}

	void FixedUpdate() {
		if (isMinimap) {
			//Изменение масштаба камеры в зависимости от движения игрока
			Vector3 curPos = transform.position;
			curPos.y = 0;
			_prevPos.y = 0;
			Vector3 move = curPos - _prevPos;
			_prevPos = transform.position;
			float speed = move.magnitude;
			camera.orthographicSize = camDistance +  target.position.y * heightScale;
		}
	}

	Rect setupMapRect() {
		if (Screen.width > Screen.height) {
			return new Rect (Screen.width / 2 - Screen.height / 2 * 0.01f * mapSize, Screen.height / 2 - Screen.height / 2 * 0.01f * mapSize, Screen.height * 0.01f * mapSize, Screen.height * 0.01f * mapSize);
		} else {
			return new Rect (Screen.width / 2 - Screen.width / 2 * 0.01f * mapSize, Screen.height / 2 - Screen.width / 2 * 0.01f * mapSize, Screen.width * 0.01f * mapSize, Screen.width * 0.01f * mapSize);

		}
	}

	Rect DrawSetup () {
		int mMsize;
		if (Screen.width < Screen.height) {
			mMsize = Mathf.RoundToInt (miniMapSize * 0.01f * Screen.width);
		}else {
			mMsize = Mathf.RoundToInt (miniMapSize * 0.01f * Screen.height);
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
	
	public void Draw(){	
		if (isMinimap) {
			if ((currentTime / changeTime) >= 0) {
				RMapMaterial.SetFloat ("_Opacity", (currentTime / changeTime));
				RMaterial.SetFloat ("_Opacity", (1 - currentTime / changeTime));
				Graphics.DrawTexture (_currentRect, RTexture, RMapMaterial);
			}
			Graphics.DrawTexture (_miniMapRect, RTexture, RMaterial);
		} else {
			if ((currentTime / changeTime) <= 1) {
				RMapMaterial.SetFloat ("_Opacity", (currentTime / changeTime));
				RMaterial.SetFloat ("_Opacity", (1 - currentTime / changeTime));
				Graphics.DrawTexture (_miniMapRect, RTexture, RMaterial);
			}
			Graphics.DrawTexture (_currentRect, RTexture, RMapMaterial);
			if (GUI.Button (new Rect (_mapRect.x, _mapRect.y + _mapRect.height -30 , 30, 30), "+")) {
				camera.orthographicSize -= 10;
			}

			if (GUI.Button (new Rect (_mapRect.x, _mapRect.y + _mapRect.height - 60, 30, 30), "-")) {
				camera.orthographicSize += 10;
			}
		}
	}

	public void setMiniMap () {
		isMinimap = true;
		isMovingDown = false;
		isMovingUp = true;
	}

	public void setMap () {
		isMinimap = false;
		camera.orthographicSize = camMapDistance;
		transform.position = CameraMapModePosition;
		Vector3 angles = transform.eulerAngles;
		angles.y = 0;
		transform.eulerAngles = angles;
		isMovingDown = true;
		isMovingUp = false;
	}
}
