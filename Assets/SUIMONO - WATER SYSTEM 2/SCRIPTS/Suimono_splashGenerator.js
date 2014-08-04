#pragma strict

private var mainObject : SuimonoObject;

function Awake () {

	mainObject = this.transform.parent.gameObject.GetComponent(SuimonoObject);

}




// ########## SPLASH / COLLISION FUNCTIONS ##########

// Note this only works by objects intersecting the actual collision
// mesh which is not deformed along with the waves! 
function OnTriggerEnter(other : Collider) {
	mainObject.CurrentCollider.Add(other);
	mainObject.CurrCollPos.Add(other.transform.position);
	mainObject.objectRingsTimes.Add(0.0);
	
	mainObject.objectRingsTime = mainObject.objectRingsTimes.ToBuiltin(float) as float[];
	mainObject.CurrCollPoss = mainObject.CurrCollPos.ToBuiltin(Vector3) as Vector3[];
	mainObject.CurrentColliders = mainObject.CurrentCollider.ToBuiltin(Collider) as Collider[];										
}

function OnTriggerStay  (other : Collider) {
	//mainObject.isinwater = true;
	//mainObject.atDepth = this.gameObject.transform.position.y - other.gameObject.transform.position.y ;
	var setindex : int = 0;
	for (var ix : int = 0; ix < mainObject.CurrentCollider.length; ix++){
		if (mainObject.CurrentCollider[ix] == other) setindex = ix;
	}
	mainObject.CurrCollPos[setindex] = other.transform.position;
	mainObject.CurrCollPoss = mainObject.CurrCollPos.ToBuiltin(Vector3) as Vector3[];
	//mainObject.CurrentCollider.Remove(other);
	//mainObject.CurrCollPos.RemoveAt(setindex);
	
	//mainObject.CurrentColliders = mainObject.CurrentCollider.ToBuiltin(Collider) as Collider[];
	//mainObject.CurrCollPoss = mainObject.CurrCollPos.ToBuiltin(Vector3) as Vector3[];
	
	
}

function OnTriggerExit  (other : Collider) {
	var setindex : int = 0;
	for (var ix : int = 0; ix < mainObject.CurrentCollider.length; ix++){
		if (mainObject.CurrentCollider[ix] == other) setindex = ix;
	}
	mainObject.CurrentCollider.Remove(other);
	mainObject.CurrCollPos.RemoveAt(setindex);
	
	mainObject.CurrentColliders = mainObject.CurrentCollider.ToBuiltin(Collider) as Collider[];
	mainObject.CurrCollPoss = mainObject.CurrCollPos.ToBuiltin(Vector3) as Vector3[];
}



//function checkColliders(){
	//for (coll as Collider in AllColliders){ 
		//check bounds against wavespace point
		//add or remove colliders from intersection system
	//}
//}

