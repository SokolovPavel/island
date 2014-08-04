#pragma strict

var setManual : boolean = false;
var lightRange : float = 20.0;
var lightFalloff : float = 20.0;
var shiftTime : float = 5.0;

var calcDist : float;

private var surfaceHeight : float = 0.0;
private var followObject : Transform;

private var moduleObject : SuimonoModule;
private var causticObject : fx_causticModule;




function Awake () {
	
	//get master objects
	moduleObject = GameObject.Find("SUIMONO_Module").GetComponent(SuimonoModule);
	causticObject = GameObject.Find("_caustic_effects").GetComponent(fx_causticModule);
	
	Random.seed = Mathf.Floor(transform.position.x + transform.position.y + transform.position.z);
	shiftTime = 3.0 + Random.Range(0.0,12.0);
	
}




function Start(){

	InvokeRepeating("SetCaustics",0.1,(1.0/causticObject.causticFPS));
	
}




function SetCaustics () {
	
	//enable caustic light
	if (!causticObject.enableCaustics){
		light.enabled = false;
	} else {
	
	
	//get the current follow object from module
	if (moduleObject.setCamera != null){
		followObject = moduleObject.setCamera;
	} else {
		followObject = Camera.main.transform;
	}
	
	//set manual/auto controls
	if (!setManual){
		lightRange = causticObject.causticRange * 0.4;
		lightFalloff = causticObject.causticRange * 0.2;
	}
	
	//get the water surface height
	surfaceHeight = 2.0-0.1;
	light.enabled = false;
	
	var layer : int = 4;
	var layermask : int = 1 << layer;
	surfaceHeight = -500;
	var hasHit : boolean = true;
	var hits : RaycastHit[];
	var testpos : Vector3 = Vector3(transform.position.x,transform.position.y+5000.0,transform.position.z);
	hits = Physics.RaycastAll(testpos, -Vector3.up, 10000.0, layermask);
	for (var i = 0;i < hits.Length; i++) {
		var ht : RaycastHit = hits[i];
		if (ht.transform.gameObject.layer==4){
			surfaceHeight = ht.transform.position.y - 0.01;
			light.enabled = true;
			break;
		}
	}
	
	
	//get the current light texture from Module
	light.cookie = causticObject.useTex;
	light.cullingMask = causticObject.useTheseLayers;
	
	
	//ping pong the light angle and position
	light.spotAngle = 155.0 + (Mathf.Sin(Time.time / (shiftTime*2.25)) * shiftTime);
	//light.spotAngle = 158.0;// + (Mathf.Sin(Time.time / (shiftTime*0.5)) * shiftTime);
	light.transform.position.y = surfaceHeight + 0.25 - (Mathf.Sin(Time.time / (shiftTime*2.2)) * 0.2);
	light.transform.eulerAngles.y = 0.0 + (Mathf.Sin(Time.time / (shiftTime*50.0)) * 360.0);
	
	//ping pong the light color
	light.color = Color(1,1,1,1) * (0.75+(Mathf.Sin(Time.time / (shiftTime*0.4)) * 0.25));
	light.color.b = (0.85+(Mathf.Sin(Time.time / (shiftTime*0.3)) * 0.15));
	
	//dim the light based on distance
	calcDist = Vector3.Distance(this.transform.position, followObject.transform.position);
	if (calcDist <= lightRange+lightFalloff){
		light.enabled = true;
		//if (calcDist <= lightRange){
			//light.intensity = 0.65;
		//} else {
			light.intensity = Mathf.Lerp(0.65,0.0,((calcDist-lightRange)/lightFalloff));
		//}
	} else {
		light.enabled = false;
		light.intensity = 0.0;
	}
	light.intensity *= 2.0;
	
	}
	
}