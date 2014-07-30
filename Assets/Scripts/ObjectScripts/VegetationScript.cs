using UnityEngine;
using System.Collections;

public class VegetationScript : MonoBehaviour {
	public string plantName;
	public float timeToGrow;
	public float timeToFruits;
	public float treeMaxAge;
	public float treeAge;
	public bool grown;
	public bool haveFruits;
	public GameObject fruit;
	public bool fruits; // Если true, то на дереве выросли фрукты
	public GameObject[] fruitPlace;
	public GameObject[] fr;
	public AudioClip[] hitSound;
	public TreeLoot lootScript;
	private bool sprouted;
	private float lastResizeTime;
	private GameObject fruitRes;
	private float initScale;

	void Start () {
		if (!grown) {
			initScale = this.transform.localScale.y;
			this.gameObject.GetComponent<MeshRenderer> ().enabled = false;
			this.collider.enabled = false;
			treeAge = 0.0f;
			sprouted = false;
			if (lootScript != null) {
				lootScript.enabled = false;
			}
		}
	}

	void FixedUpdate () {
		treeAge += Time.fixedDeltaTime;


		if ((treeAge > (timeToGrow * 0.03f)) && (!grown)&&(!sprouted)) {
			sprouted = true;
			this.gameObject.GetComponent<MeshRenderer> ().enabled = true;
			this.collider.enabled = true;
			this.transform.localScale *= 0.01f;
			lastResizeTime = treeAge;

		}

		if ((!grown) && (sprouted)) {
			if ((treeAge - lastResizeTime) > 0.2f) {
				float coef =  initScale * ((float)treeAge/ timeToGrow);
				this.transform.localScale = new Vector3(coef,coef,coef);
			}
		}

		if ((!grown) && (treeAge > timeToGrow)) {
			grown = true;
			if (lootScript != null) {
				lootScript.enabled = true;
			}
		}

		if ((!fruits) && (treeAge > timeToFruits)&&(haveFruits)) {

			//	fruitRes = Resources.Load (fruit, typeof(GameObject));

			for (int i = 0; i<fruitPlace.Length; i++) {
				fr[i] = Instantiate (fruit, fruitPlace[i].transform.position,  fruitPlace[i].transform.rotation) as GameObject;
				fr[i].rigidbody.constraints = RigidbodyConstraints.FreezePositionX |RigidbodyConstraints.FreezePositionY|RigidbodyConstraints.FreezePositionZ;
				fr[i].rigidbody.freezeRotation=true;
			}
			fruits = true;
		}

		if ((fruits)&&(treeAge>treeMaxAge)&&(haveFruits)) {
			for (int i = 0; i < fr.Length; i++) {
				if (fr [i] != null) {
					treeAge = timeToFruits + 1.0f;
					return;
				}
			}
			Destroy (this.gameObject);
			return;
		}

	}
}
