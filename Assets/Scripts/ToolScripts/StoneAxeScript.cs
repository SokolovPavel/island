using UnityEngine;
using System.Collections;

public class StoneAxeScript : toolBaseScript {

	// Use this for initialization
	void Start () {
		toolName = "StoneAxe";
		scriptName = "StoneAxeScript";
		toolObj = Resources.Load (toolName, typeof(GameObject)) as GameObject;
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (1)) {
			float range = 10f;
			Vector3 direction = cam.transform.TransformDirection (Vector3.forward);
			RaycastHit hit;
			if (Physics.Raycast (cam.transform.position, direction, out hit, range)) {	

				if ((hit.rigidbody)&&(hit.rigidbody.gameObject.name!="Player"))
				{hit.transform.gameObject.SendMessage("Chop",this.transform.gameObject,SendMessageOptions.DontRequireReceiver);}
				else if (hit.collider.tag=="Usable")
				{hit.transform.gameObject.SendMessage("Chop",this.transform.gameObject,SendMessageOptions.DontRequireReceiver);}
			}
		}

		if (Input.GetMouseButtonDown (2)) {
			float range = 10f;
			Vector3 direction = cam.transform.TransformDirection (Vector3.forward);
			RaycastHit hit;
			if (Physics.Raycast (cam.transform.position, direction, out hit, range)) {	

				if ((hit.rigidbody)&&(hit.rigidbody.gameObject.name!="Player"))
				{hit.transform.gameObject.SendMessage("Hydrate",this.transform.gameObject,SendMessageOptions.DontRequireReceiver);}
				else if (hit.collider.tag=="Usable")
				{hit.transform.gameObject.SendMessage("Hydrate",this.transform.gameObject,SendMessageOptions.DontRequireReceiver);}
			}
		}
		
	}



	void FixedUpdate (){
		Validate();
	}
}
