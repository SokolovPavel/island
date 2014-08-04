#pragma strict
#pragma implicit
#pragma downcast

//PUBLIC VARIABLES
var enableCaustics : boolean = true;
//var causticsOnMobile : boolean = false;
var causticRange : float = 50.0;
var causticObject : Transform;

var useTheseLayers : LayerMask = 0;
var causticFPS : int = 32;
var animationSpeed : float = 1.0;

var causticFrames : Texture2D[];


//PUBLIC VARIABLES
public var useTex : Texture2D;


//PRIVATE VARIABLES
private var useCaustics : boolean = true;
private var maxCausticEffects : int = 30;
private var step = 40.0;
private var followObject : Transform;
private var useObject : Transform;
private var moduleObject : SuimonoModule;
private var frameIndex : int = 0;
private var currentSpeed : float = 1.0;
private var causticObjects : Transform[];
private var causticObjectsFX : fx_causticObject[];

var savedPosition : Vector3;


function Awake() {
	
	//get master objects
	moduleObject = GameObject.Find("SUIMONO_Module").GetComponent(SuimonoModule);
	
}


	
	
function Start(){

	if (moduleObject.causticObjectNum > 0){
		useCaustics = true;
	} else {
		useCaustics = false;
	}
	
	//mobile check
	if (!moduleObject.causticsOnMobile){
		if (moduleObject.unityVersionIndex == 3 || moduleObject.unityVersionIndex == 4){
			useCaustics = false;
		}
	} else {
		useCaustics = true;
		if (enableCaustics) useCaustics = true;
	}
	
	if (useCaustics){
		//instantiate caustic object pool
		if (causticObject != null){
			maxCausticEffects = moduleObject.causticObjectNum;
			causticObjects = new Transform[maxCausticEffects];
			causticObjectsFX = new fx_causticObject[maxCausticEffects];
			for (var cx=0; cx < maxCausticEffects; cx++){
				var setPos : Vector3 = transform.position;
				setPos.y = -500.0;
				var causticObjectPrefab = Instantiate(causticObject, setPos, transform.rotation);
				causticObjectPrefab.transform.parent = this.transform;
				causticObjects[cx] = (causticObjectPrefab);
				causticObjectsFX[cx] = causticObjects[cx].gameObject.GetComponent(fx_causticObject);
			}
		}
		
		//set animation scheduler
		InvokeRepeating("CausticEffectUpdate", (1.0 / causticFPS), (1.0 / causticFPS)); 

	}
}



function Update () {

	if (useCaustics){

		followObject = moduleObject.setTrack;
		animationSpeed = Mathf.Clamp(animationSpeed,0.001,3.0);

		//reset invoke
		if (currentSpeed != animationSpeed){
			CancelInvoke();
			InvokeRepeating("CausticEffectUpdate", 0.0, (1.0 / (causticFPS*animationSpeed)));
			currentSpeed=animationSpeed;
		}
		
		//get the current follow object from module
		if (followObject != null){
			useObject = followObject;
		} else {
			useObject = Camera.main.transform;
		}
		
		
		SetGridSpace();
	}
}








function SetGridSpace(){	

	step = Mathf.Sqrt(causticRange) * (causticRange/maxCausticEffects);
	
	//reposition caustic objects from ppol
	if (useObject != null){
		var checkPos : Vector3 = Vector3(useObject.transform.position.x,0.0,useObject.transform.position.z);
		if (Vector3.Distance(savedPosition,checkPos) >= 5.0){
		
			savedPosition = checkPos;
			
			//move caustic lights into new positions
			for (var xP : int = (savedPosition.x - causticRange); xP <= (savedPosition.x + causticRange); xP += step){
			for (var yP : int = (savedPosition.z - causticRange); yP <= (savedPosition.z + causticRange); yP += step){
			

				for (var lx : int = 0; lx < causticObjects.length; lx++){
				
				var lightPos : Vector3 = Vector3(causticObjects[lx].transform.position.x,0.0,causticObjects[lx].transform.position.z);
				var lightDist : float = Vector3.Distance(lightPos,checkPos);
				
					if (lightDist > (causticRange*0.5)){
						causticObjects[lx].transform.localEulerAngles = Vector3(90.0,0.0,0.0);
						causticObjectsFX[lx].shiftTime = 3.0 + Random.Range(0.0,12.0);
						
						//check positions
						var posPass : boolean = true;
						var setPX = (Mathf.Round(xP/step))*step;
						var setPY = (Mathf.Round(yP/step))*step;
						for (var ly : int = 0; ly < causticObjects.length; ly++){
							if (causticObjects[ly].transform.position.x == setPX){
							if (causticObjects[ly].transform.position.z == setPY){
								posPass = false;
							}
							}
						}
						
						//set new position
						if (posPass){
							causticObjects[lx].transform.position.x = setPX;
							causticObjects[lx].transform.position.z = setPY;
							causticObjects[lx].light.intensity = 0.0;
						}

					}

				}
			
			}
			}
	
		}
	}
	
}












function CausticEffectUpdate() {
	
	if (this.enabled){
	if (animationSpeed > 0.0){
		
  		useTex = causticFrames[frameIndex];

		frameIndex += 1;
    	if (frameIndex == causticFrames.length) frameIndex = 0;

    }
    }
    
}



