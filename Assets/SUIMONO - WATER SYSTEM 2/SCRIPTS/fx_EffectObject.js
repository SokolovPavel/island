#pragma strict

@script ExecuteInEditMode()

//enum Sui_FX_Type{
//		particle,audio
//		}
enum Sui_FX_Rules{
		none,isUnderWater,isAboveWater,isAtWaterSurface,speedIsGreater,speedIsLess,
		waterDepthGreater,waterDepthLess
		}
enum Sui_FX_RuleModifiers{
		and,or
		}
enum Sui_FX_System{
		none,bubbles,rings,ringfoam,splash,splashdrops
		}
enum Sui_FX_ActionType{
		none,once,repeat
		}

//particle
//var effectType : Sui_FX_Type[];
var fxObject : SuimonoModuleFX;			
var effectRule : Sui_FX_Rules[];
//var effectRuleModifier : Sui_FX_RuleModifiers[];
var effectData : float[];
var resetRule : Sui_FX_Rules[];
var effectSystemName : String[];
var effectSystem : Sui_FX_System[];
var actionType : Sui_FX_ActionType = Sui_FX_ActionType.none;
var effectDelay : Vector2 = Vector2(1.0,1.0);
var emitTime : Vector2 = Vector2(1.0,1.0);
var emitNum : Vector2 = Vector2(1.0,1.0);
var effectSize : Vector2 = Vector2(1.0,1.0);
var emitSpeed : float;
var speedThreshold : float;
var directionMultiplier : float;
//var linkSizeToSpeed : boolean = false;
//var linkDirections : boolean = false;
var emitAtWaterLevel : boolean = false;

//audio
var audioObj : AudioClip;
var audioVol : Vector2 = Vector2(0.9,1.0);
var audioPit : Vector2 = Vector2(0.8,1.2);
var audioSpeed : float;

//color
var tintCol : Color = Color(1,1,1,1);
var clampRot : boolean = false;


// for custom editor
var typeIndex : int = 0;
var ruleIndex : int[];
var ruleOptions = new Array("None","Object Is Underwater","Object Is Above Water","Object Is At Surface",
	"Object Speed Is Greater Than","Object Speed Is Less Than","Water Depth Is Greater Than",
	"Water Depth Is Less Than"
	);


	

var systemIndex : int = 0;
var sysNames = new Array();
//var systemOptions = new Array("splash","bubbles");

//

var useDarkUI : boolean = true;

var currentSpeed : float;
private var savePos : Vector3 = Vector3(0,0,0);
private var moduleObject : SuimonoModule;
//private var suimonoCamera : sui_demo_Controller2;
private var delayTimer : float;
private var emitTimer : float;
private var delayPass : boolean = true;
private var useEffectDelay : float;
private var useEmitTime : float;
private var numEmissions : int = 0;
private var useSpd : float;
private var useAudioSpd : float;
private var currentWaterPos : float;
private var emitPos : Vector3;
private var rulepass : boolean = false;

private var timerAudio : float = 0.0;
private var timerParticle : float = 0.0;
 
 
 
function Start () {

	// Object References
	if (GameObject.Find("SUIMONO_Module")){
		moduleObject = GameObject.Find("SUIMONO_Module").GetComponent(SuimonoModule) as SuimonoModule;
		fxObject = moduleObject.fxObject;
		//suimonoCamera = moduleObject.setCamera.gameObject.GetComponent(sui_demo_Controller2) as sui_demo_Controller2;
	}
	
	//set starting variables
	useEmitTime = Random.Range(emitTime.x,emitTime.y);
	useEffectDelay = Random.Range(effectDelay.x,effectDelay.y);
	
	//invoke fx handler
	//useSpd = emitSpeed;
	//useAudioSpd = audioSpeed;
	//InvokeRepeating("EmitFX",0.2,useSpd);
	//InvokeRepeating("EmitSoundFX",1.0,useAudioSpd);
	

}




function Update () {

	//get objects while in editor mode
	#if UNITY_EDITOR
	if (!Application.isPlaying){
		if (moduleObject == null){	
		if (GameObject.Find("SUIMONO_Module")){
			moduleObject = GameObject.Find("SUIMONO_Module").GetComponent(SuimonoModule) as SuimonoModule;
			fxObject = moduleObject.fxObject;
		}
		}
	}
	#endif
	
	//set ui
	useDarkUI = moduleObject.useDarkUI;
	
	//populate system names
	if (fxObject != null){
		sysNames.Push("None");
		for (var sN : int = 0; sN < fxObject.effectsSystems.Length; sN++){
			//sysNames.Add(fxObject.effectsLabels[sN]);
			var setName : String = "---";
			if (fxObject.effectsSystems[sN] != null) setName = fxObject.effectsSystems[sN].transform.name;
				for (var s : int = 0; s < sN; s++){
					setName += " ";
				}
			sysNames.Push(setName);
		}
	}
	
		
	if (Application.isPlaying){	
	

		//track position / speed
		if (savePos != this.transform.position){
			currentSpeed = Vector3.Distance(savePos,Vector3(this.transform.position.x,savePos.y,this.transform.position.z))/Time.deltaTime;
		}
		savePos = this.transform.position;


		// track timers and emit
		timerParticle += Time.deltaTime;
		timerAudio += Time.deltaTime;
		
		if (timerParticle > emitSpeed){
			timerParticle = 0.0;
			EmitFX();
		}
		if (timerAudio > audioSpeed){
			timerAudio = 0.0;
			EmitSoundFX();
		}
		

	}
	
}




function EmitSoundFX(){

	if (audioObj != null){
	if (rulepass){
		moduleObject.AddSoundFX(audioObj,emitPos,Vector3(0,Random.Range(audioPit.x,audioPit.y),Random.Range(audioVol.x,audioVol.y)));
	}
	}

}




function EmitFX () {
if (Application.isPlaying){	
	
	//######################################
	//##    CALCULATE TIMING and DELAYS   ##
	//######################################
	delayPass = false;

	emitTimer += Time.deltaTime;
	if (emitTimer >= emitSpeed){
		emitTimer = 0.0;
		delayTimer = 0.0;
		useEffectDelay = Random.Range(effectDelay.x,effectDelay.y);
		delayPass = true;
	}

	
	
	//####################################
	//##    CALCULATE WATER RELATION   ##
	//####################################
	currentWaterPos = moduleObject.SuimonoGetHeight(transform.position,"object depth");

	
	//##########################
	//##    CALCULATE RULES   ##
	//##########################
	rulepass = false;
	var ruleCheck : boolean[] = new boolean[effectRule.Length];
	var ruleCKNum : int = 0;
	var resetCheck : boolean[] = new boolean[resetRule.Length];
	var resetCKNum : int = 0;
	
	for (var rCK : int = 0; rCK < effectRule.Length; rCK++){
		/*
		// RULE : Is Underwater
		if (effectRule[rCK] == Sui_FX_Rules.isUnderWater){
			if (moduleObject.SuimonoGetHeight(transform.position,"object depth") > 0.0) ruleCheck[rCK] = true;
		}
		if (effectRule[rCK] == Sui_FX_Rules.isAboveWater){
			if (moduleObject.SuimonoGetHeight(transform.position,"object depth") <= 0.0) ruleCheck[rCK] = true;
		}
		if (effectRule[rCK] == Sui_FX_Rules.isAtWaterSurface){
			if (moduleObject.SuimonoGetHeight(transform.position,"isAtSurface") == 1.0) ruleCheck[rCK] = true;
		}
		// No Rule
		 else if (effectRule[rCK] == Sui_FX_Rules.none){
			ruleCheck[rCK] = true;
		}
		*/
		ruleCheck[rCK] = CheckRules(ruleIndex[rCK],rCK);
		//if (actionType == Sui_FX_ActionType.once) resetCheck[rCK] = CheckRules(resetRule[rCK],rCK);
	}
	
	//determine if all rules are passed
	for (rCK = 0; rCK < effectRule.Length; rCK++){
		if (ruleCheck[rCK]) ruleCKNum += 1;
	}
	if (ruleCKNum == effectRule.Length) rulepass = true;
	
	//determine if all resets are passed
	//if (actionType == Sui_FX_ActionType.once){
	//	for (rCK = 0; rCK < resetRule.Length; rCK++){
	//		if (resetCheck[rCK]) resetCKNum += 1;
	//	}
	//	if (resetCKNum == resetRule.Length && resetRule.Length > 0) numEmissions = 0;
	//}
	
	//no rules
	if (effectRule.Length == 0){
		rulepass = true;
	}
	
	//check action type
	//if (rulepass){
	//	numEmissions += 1;
	//	if (numEmissions >= 2 && actionType == Sui_FX_ActionType.once){
	//		rulepass = false;
	//	} else {
	//		rulepass = true;
	//	}
	//}
	
	
	//######################
	//##    INITIATE FX   ##
	//######################
	if (delayPass && rulepass){
	//for (rCK = 0; rCK < effectSystem.Length; rCK++){
	
		var emitN : int = Mathf.Floor(Random.Range(emitNum.x,emitNum.y));
		var emitS : float = Random.Range(effectSize.x,effectSize.y);
		var emitV : Vector3 = Vector3(0,0,0);
		emitPos = transform.position;
		#if UNITY_3_5
			var emitR : float = 135.0+transform.eulerAngles.y;
		#else
			var emitR : float = 90.0+transform.eulerAngles.y;
		#endif
		if (!clampRot){
			emitR = Random.Range(0.0,360.0);
		}
		var emitAR : float = Random.Range(-360.0,360.0);
		
		//get water level
		if (emitAtWaterLevel){
			emitPos.y = (transform.position.y + currentWaterPos)+0.01;
		}
		
		if (directionMultiplier > 0.0){
			emitV = transform.up * (directionMultiplier * Mathf.Clamp((currentSpeed/speedThreshold),0.0,1.0));
		}
		

		//EMIT PARTICLE SYSTEM
		if (systemIndex-1 >= 0){
			//moduleObject.AddFX(systemIndex-1, emitPos, 1*emitN, Random.Range(0.5,0.75)*emitS, Random.Range(0.0,1.0), emitV);
			moduleObject.AddFX(systemIndex-1, emitPos, emitN, Random.Range(0.5,0.75)*emitS, emitR, emitAR, emitV, tintCol);
		}
		//rulepass = false;
	}
	
}
}





function CheckRules( rule : Sui_FX_Rules, ruleNum : int){
if (Application.isPlaying){	

	var rp : boolean = false;
	var ruleData : float = speedThreshold;
	
	//get depth
	//var depth = moduleObject.SuimonoGetHeight(transform.position,"object depth");
	var depth = currentWaterPos;
	
	if (ruleNum < effectData.Length) ruleData = effectData[ruleNum];
	
	if (rule == Sui_FX_Rules.isUnderWater){
		if (depth > 0.0) rp = true;
	}
	if (rule == Sui_FX_Rules.isAboveWater){
		if (depth <= 0.0) rp = true;
	}
	if (rule == Sui_FX_Rules.isAtWaterSurface){
		if (depth < 0.25 && depth > -0.25) rp = true;
	}
	if (rule == Sui_FX_Rules.waterDepthGreater){
		if (depth > ruleData) rp = true;
	}
	if (rule == Sui_FX_Rules.waterDepthLess){
		if (depth < ruleData) rp = true;
	}
	if (rule == Sui_FX_Rules.speedIsGreater){
		if (currentSpeed > ruleData) rp = true;
	}
	if (rule == Sui_FX_Rules.speedIsLess){
		if (currentSpeed < ruleData) rp = true;
	}
	//if (rule == Sui_FX_Rules.enterFromAbove){
		//if (currentSpeed < ruleData) rp = true;
	//	rp = true;
	//}

	else if (rule == Sui_FX_Rules.none){
		rp = true;
	}
	
	return rp;

}
}



function AddRule(){

	var tempRules : Sui_FX_Rules[]  = effectRule;
	var tempIndex : int[]  = ruleIndex;
	var tempData : float[] = effectData;
	effectRule = new Sui_FX_Rules[tempRules.Length+1];
	ruleIndex = new int[tempRules.Length+1];
	effectData = new float[tempRules.Length+1];
	
	for (var aR : int = 0; aR < tempRules.Length; aR++){
		effectRule[aR] = tempRules[aR];
		ruleIndex[aR] = tempIndex[aR];
		effectData[aR] = tempData[aR];
	}
	effectRule[tempRules.Length] = Sui_FX_Rules.none;
	ruleIndex[tempRules.Length] = 0;
	effectData[tempRules.Length] = 0;
}




function DeleteRule(ruleNum : int){

 	var tempRules : Sui_FX_Rules[]  = effectRule;
 	var tempIndex : int[]  = ruleIndex;
 	var tempData : float[] = effectData;
 	
	var endLP : int = tempRules.Length-1;
	if (endLP <= 0){
		endLP = 0;
		
		effectRule = new Sui_FX_Rules[endLP];
		ruleIndex = new int[endLP];
		effectData = new float[endLP];
		
	} else {
	//if (endLP >= 1){
	
	
		effectRule = new Sui_FX_Rules[endLP];
		ruleIndex = new int[endLP];
		effectData = new float[endLP];
		
	//if (ruleNum != tempRules.Length-1){
	
		var setInt : int = -1;
		for (var aR : int = 0; aR < endLP; aR++){
			if (aR != ruleNum){
				setInt += 1;
			} else {
				setInt += 2;
			}
			
			if (setInt < tempRules.Length){
				effectRule[aR] = tempRules[setInt];
				ruleIndex[aR] = tempIndex[setInt];
				effectData[aR] = tempData[setInt];
			}
		}
	}
	//} else {
		
		//if ((endLP-1) > 0){
			//for (var xR : int = 0; xR < endLP-1; xR++){
			//	effectRule[xR] = tempRules[xR];
			//	ruleIndex[xR] = tempIndex[xR];
			//}
		//} else {
		
		//}
		
	//}
	
}
