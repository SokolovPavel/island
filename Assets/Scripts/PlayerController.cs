using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

	public float moveSpeed = 10;
	public float runSpeed = 20;
	public float crouchSpeed = 2f;
	public float JumpHeight = 1;
	public float gravity = 9.8f;
	private Vector3 moveSpeedDir = Vector3.zero;
	private bool isGrounded = false;
	private Vector3 _prevSpeed = Vector3.zero;
	private float _jumpSpeed = 0;
	CharacterController controller;
	public bool enable = true;

	//private float vScale = 1.0f; //Vertical scale of the character
	private bool crouching = false;
	private float height;
	private Transform tr;
	void Start () {
		controller = GetComponent<CharacterController> ();
		_jumpSpeed = Mathf.Sqrt (2 * gravity * JumpHeight);
		height = controller.height;
		tr = this.transform;
	}

	void FixedUpdate () {
		if (enable) {
			float h = height;

			moveSpeedDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			if (moveSpeedDir != Vector3.zero) {
				//moveSpeedDir = moveSpeedDir / moveSpeedDir.magnitude;


				if ((crouching == true) && (moveSpeedDir.z > 0)) {
					moveSpeedDir *= crouchSpeed;
				} else	if ((Input.GetButton("Run"))&&(moveSpeedDir.z > 0)) {
					moveSpeedDir *= runSpeed;
				} else {
					moveSpeedDir *= moveSpeed;
				}


				moveSpeedDir = transform.TransformDirection(moveSpeedDir);

			
			}

			if( isGrounded ){
				if(Input.GetButton("Jump")){
					moveSpeedDir.y = _jumpSpeed;
				} else {
					moveSpeedDir.y = 0;
				}

			}

			if (Input.GetButton ("Crouch")) {
				crouching = true;
				h = height*0.5f;



			} else {
				if (crouching == true) {
					Vector3 up = transform.TransformDirection(Vector3.up);
					up.y -= 0.5f;
					if (Physics.Raycast (transform.position, up, 1.5f)) {
						crouching = true;
						h = height*0.5f;
					} else {
						crouching = false;
						h = height;
					}

				}
			}

			var lastHeight = controller.height; // crouch/stand up smoothly 
			controller.height = Mathf.Lerp(controller.height, h, 8*Time.deltaTime);

			Vector3 temp = tr.position;
			temp.y = temp.y + (controller.height-lastHeight)/2; // fix vertical position
			tr.position = temp;


		}
		if (!isGrounded) {
			moveSpeedDir.y = _prevSpeed.y - gravity * Time.deltaTime;
		}
		_prevSpeed = moveSpeedDir;
		CollisionFlags flags = controller.Move(moveSpeedDir * Time.deltaTime);
		isGrounded = (flags & CollisionFlags.CollidedBelow) != 0;
	}
}
