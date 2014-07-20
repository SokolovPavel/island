using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

	public float moveSpeed = 10;
	public float runSpeed = 20;
	public float JumpHeight = 1;
	public float gravity = 9.8f;
	private Vector3 moveSpeedDir = Vector3.zero;
	private bool isGrounded = false;
	private Vector3 _prevSpeed = Vector3.zero;
	private float _jumpSpeed = 0;
	CharacterController controller;
	public bool enable = true;

	void Start () {
		controller = GetComponent<CharacterController> ();
		_jumpSpeed = Mathf.Sqrt (2 * gravity * JumpHeight);
	}

	void FixedUpdate () {
		if (enable) {
			moveSpeedDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			if (moveSpeedDir != Vector3.zero) {
				//moveSpeedDir = moveSpeedDir / moveSpeedDir.magnitude;
				if ((Input.GetButton("Run"))&&(moveSpeedDir.z > 0)) {
					moveSpeedDir *= runSpeed;
				} else {
					moveSpeedDir *= moveSpeed;
				}
				moveSpeedDir = transform.TransformDirection(moveSpeedDir);

				if( isGrounded ){
					if(Input.GetButton("Jump")){
						moveSpeedDir.y = _jumpSpeed;
					} else {
						moveSpeedDir.y = 0;
					}
				}
			}
		}
		if (!isGrounded) {
			moveSpeedDir.y = _prevSpeed.y - gravity * Time.deltaTime;
		}
		_prevSpeed = moveSpeedDir;
		CollisionFlags flags = controller.Move(moveSpeedDir * Time.deltaTime);
		isGrounded = (flags & CollisionFlags.CollidedBelow) != 0;
	}
}
