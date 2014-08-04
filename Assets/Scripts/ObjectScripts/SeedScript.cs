using UnityEngine;
using System.Collections;

public class SeedScript : MonoBehaviour {
	public string PlantName;
	public float lifeSpaceRadius = 0.3f;
	public GameObject origin;
	public float timeToAppear;
	public bool activated;
	public bool canGrowSelf;
	private float timer;
	private bool buried;
	private Collider[] colliders;


	// Use this for initialization
	void Start () {
		timer = 0.0f;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (activated) {
			timer += Time.fixedDeltaTime;
			if (timer > timeToAppear) {
				Instantiate (Resources.Load (PlantName),this.transform.position,this.transform.rotation);
				if (buried) {
					Destroy (origin);
				}
				Destroy (this.gameObject);
			}
		}
	}

	public bool Activate(GameObject activator,bool bur) {

		if (CheckLifeSpace (activator)) {
			activated = true;
			this.gameObject.renderer.material.color = new Color (0, 255, 0);
			this.rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
			this.rigidbody.freezeRotation = true;
			this.gameObject.GetComponent<Item> ().enabled = false;

			if (bur) {
				origin = activator;
				this.gameObject.GetComponent<MeshRenderer> ().enabled = false;
				this.gameObject.collider.enabled = false;
			}
			return true;
		} else {
			return false;
		}

	}

	public void Activate() {

		activated = true;
		this.gameObject.renderer.material.color = new Color (0, 255, 0);
		this.rigidbody.constraints = RigidbodyConstraints.FreezePositionX |RigidbodyConstraints.FreezePositionY|RigidbodyConstraints.FreezePositionZ;
		this.rigidbody.freezeRotation=true;
		this.gameObject.GetComponent<Item> ().enabled = false;
	}

	void Use(GameObject user) {
		Inventory inv = user.GetComponent<Inventory> ();
		Item it = this.gameObject.GetComponent<Item> ();
		if ((!activated)&&(canGrowSelf)) {
			if (it.quantity > 1) {
				it.quantity -= 1;
				inv.AddItem (this.gameObject);
			}
			Activate ();

		}
	}

	bool CheckLifeSpace() {

		colliders = Physics.OverlapSphere (transform.position,lifeSpaceRadius);

		for (int i = 0; i <colliders.Length; i++) 
		{
			if((colliders[i].name!="Ground")||(colliders[i].name!="WaterLevel")||(colliders[i].gameObject!=this.gameObject)||(colliders[i].gameObject.name!="Player"))
			{
				//	GameObject.FindGameObjectWithTag ("GameLogic").GetComponent<MessageBox> ().AddMessage (new GameMessage ("Bad place for planting. Try another", GameMessage.messageType.ObjectMessage));
				return false;
			}

		}
		return true;
	}

	bool CheckLifeSpace(GameObject hole) {

		colliders = Physics.OverlapSphere (transform.position,lifeSpaceRadius);

		for (int i = 0; i <colliders.Length; i++) 
		{
			if((colliders[i].gameObject.name=="Ground")||(colliders[i].gameObject.name=="WaterLevel")||(colliders[i].gameObject==this.gameObject)||(colliders[i].gameObject == hole.gameObject)||(colliders[i].gameObject.name=="Player"))
			{
				//	GameObject.FindGameObjectWithTag ("GameLogic").GetComponent<MessageBox> ().AddMessage (new GameMessage ("Bad place for planting. Try another", GameMessage.messageType.ObjectMessage));
				//Debug.Log (hole.name);
				//Debug.Log (colliders [i].gameObject.name);
				return true;
			}

		}
		return false;
	}

}
