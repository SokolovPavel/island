using UnityEngine;
using System.Collections;

public class SeedScript : MonoBehaviour {
	public string PlantName;
	public GameObject origin;
	public float timeToAppear;
	public bool activated;
	public bool canGrowSelf;
	private float timer;
	private bool buried;
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

	public void Activate(GameObject activator,bool bur) {

		activated = true;
		this.gameObject.renderer.material.color = new Color (0, 255, 0);
		this.rigidbody.constraints = RigidbodyConstraints.FreezePositionX |RigidbodyConstraints.FreezePositionY|RigidbodyConstraints.FreezePositionZ;
		this.rigidbody.freezeRotation=true;
		this.gameObject.GetComponent<Item> ().enabled = false;

		if (bur) {
			origin = activator;
			this.gameObject.GetComponent<MeshRenderer> ().enabled = false;
			this.gameObject.collider.enabled = false;
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

}
