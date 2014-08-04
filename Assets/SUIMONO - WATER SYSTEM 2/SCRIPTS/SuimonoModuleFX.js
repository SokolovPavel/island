#pragma strict

import System.Collections.Generic;
@script ExecuteInEditMode()

enum Sui_FX_ClampType{
		none,atSurface,belowSurface,aboveSurface
		}
	
		
//PUBLIC VARIABLES
var effectsLabels : String[];
var effectsSystems : Transform[];
var systemClampType : Sui_FX_ClampType[];
var fxObjects : Transform[];

var useDarkUI : boolean = true;

//editor
var clampIndex : int[];
var clampOptions = new Array("No Clamp","Clamp to Surface","Keep Below Surface","Keep Above Surface"
	);
	
//PRIVATE VARIABLES
private var fxParentObject : Transform;
private var moduleObject : SuimonoModule;

private var updateTimer : float = 0.0;
private var resetSystems : boolean = false;

var particleReserve : List.<ParticleSystem.Particle> = new List.<ParticleSystem.Particle>();


function Start () {
	
	//set objects
	fxParentObject = this.transform.Find("_particle_effects");
	moduleObject = this.transform.GetComponent(SuimonoModule) as SuimonoModule;
	
	
	//instantiate systems
	if (Application.isPlaying){
	if (effectsSystems.Length > 0 && fxParentObject != null){
		var instPos : Vector3 = transform.position;
		instPos.y = -10000.0;
		fxObjects = new Transform[effectsSystems.Length];

		for (var fx=0; fx < (effectsSystems.Length); fx++){
			var fxObjectPrefab = Instantiate(effectsSystems[fx], instPos, transform.rotation);
			fxObjectPrefab.transform.parent = fxParentObject.transform;
			fxObjects[fx] = (fxObjectPrefab);
		}
	}
	}
	
	//do clamp checks at 30fps
	var clampSpeed = 1.0/30.0;
	InvokeRepeating("ClampSystems",1.0,clampSpeed);
	
}





function Update () {

	//get objects while in editor mode
	#if UNITY_EDITOR
	if (!Application.isPlaying){
		if (moduleObject == null){
		if (GameObject.Find("SUIMONO_Module")){
			moduleObject = GameObject.Find("SUIMONO_Module").GetComponent(SuimonoModule) as SuimonoModule;
		}
		}
	}
	#endif
	
	//set ui
	useDarkUI = moduleObject.useDarkUI;



	//update objects at set time interval
	//updateTimer += Time.deltaTime;
	//if (updateTimer > 0.0001){
	//if (particleReserve.Count > 0){
		//updateTimer = 0.0;
		//updateFX();
		//resetSystems = true;
		//particleReserve = new List.<ParticleSystem.Particle>();
	//}
	//}
	

}









function ClampSystems(){


	for (var fx : int = 0; fx < fxObjects.Length; fx++){

		if (fxObjects[fx] != null){
		//if (systemClampType[fx] != null){
		//if (systemClampType[fx] != Sui_FX_ClampType.none){
		if (clampIndex[fx] != 0){


			var currPXWaterPos : float = 0.0;
			
			//get particles
			var setParticles = new ParticleSystem.Particle[fxObjects[fx].particleSystem.particleCount];
			fxObjects[fx].particleSystem.GetParticles(setParticles);
			//set particles
			if (fxObjects[fx].particleSystem.particleCount > 0.0){
			for (var px : int = 0; px < fxObjects[fx].particleSystem.particleCount; px++){
				//Clamp to Surface
				//if (systemClampType[fx] == Sui_FX_ClampType.atSurface){
				if (clampIndex[fx] == 1){
					currPXWaterPos = moduleObject.SuimonoGetHeight(setParticles[px].position,"surfaceLevel");
					setParticles[px].position.y = currPXWaterPos;
				}
				//Clamp Under Water
				//if (systemClampType[fx] == Sui_FX_ClampType.belowSurface){
				if (clampIndex[fx] == 2){
					currPXWaterPos = moduleObject.SuimonoGetHeight(setParticles[px].position,"surfaceLevel");
					if (setParticles[px].position.y > currPXWaterPos-0.2) setParticles[px].position.y = currPXWaterPos-0.2;
				}
				//Clamp Above Water
				//if (systemClampType[fx] == Sui_FX_ClampType.aboveSurface){
				if (clampIndex[fx] == 3){
					currPXWaterPos = moduleObject.SuimonoGetHeight(setParticles[px].position,"surfaceLevel");
					if (setParticles[px].position.y < currPXWaterPos+0.2) setParticles[px].position.y = currPXWaterPos+0.2;
				}
			}
			fxObjects[fx].particleSystem.SetParticles(setParticles,setParticles.length);
			fxObjects[fx].particleSystem.Play();

			
			}
			
		}
		//}
		}

	}
	
	
}






function AddSystem(){

	var tempSystems : Transform[]  = effectsSystems;
	var tempClamp : int[]  = clampIndex;
	//var tempData : float[] = effectData;
	effectsSystems = new Transform[tempSystems.Length+1];
	clampIndex = new int[tempClamp.Length+1];
	//effectData = new float[tempRules.Length+1];
	
	for (var aR : int = 0; aR < tempSystems.Length; aR++){
		effectsSystems[aR] = tempSystems[aR];
		clampIndex[aR] = tempClamp[aR];
		//effectData[aR] = tempData[aR];
	}
	effectsSystems[tempSystems.Length] = null;
	clampIndex[tempClamp.Length] = 0;
	//effectData[tempRules.Length] = 0;
}





function AddParticle( particleData : ParticleSystem.Particle){

	particleReserve.Add(particleData);
	//fxObjects[Mathf.Floor(particleData.angularVelocity)].particleSystem.Emit(1);
	
}


function updateFX(){
	
	
	
	//EMIT New Particles
	for (var efx : int = 0; efx < effectsSystems.length; efx++){
		for (var epx : int = 0; epx < particleReserve.Count; epx++){
			if (Mathf.Floor(particleReserve[epx].angularVelocity) == efx){
				fxObjects[efx].particleSystem.Emit(1);
			}
		}				
	}
	
	
	//YIELD Systems till end of frame
	//This is a Unity particels system behavior fudge... maybe someday they fix this?(hope)
	//yield new WaitForEndOfFrame();
	
	
	//SET NEW Particle position and behaviors
	for (var fx : int = 0; fx < effectsSystems.length; fx++){
		for (var px : int = 0; px < particleReserve.Count; px++){
			if (Mathf.Floor(particleReserve[px].angularVelocity) == fx){
			
				//fxObjects[fx].particleSystem.Emit(1);
				
				//get particles
				var setParticles = new ParticleSystem.Particle[fxObjects[fx].particleSystem.particleCount];
				fxObjects[fx].particleSystem.GetParticles(setParticles);
				//set particles
				//if (fxObjects[fx].particleSystem.particleCount > 0.0){
				for (var sx : int = (fxObjects[fx].particleSystem.particleCount-1); sx < fxObjects[fx].particleSystem.particleCount; sx++){
					//set position
					setParticles[px].position = particleReserve[px].position;
					//set variables
					setParticles[px].size = particleReserve[px].size;
					setParticles[px].rotation = particleReserve[px].rotation;
					setParticles[px].velocity = particleReserve[px].velocity;
					//setParticles[px].color *= particleReserve[px].color;	
				}

				fxObjects[fx].particleSystem.SetParticles(setParticles,setParticles.length);
				//fxObjects[fx].particleSystem.Play();
			}
		}						
	}
	
	yield;
	particleReserve = new List.<ParticleSystem.Particle>();
	
	//}
	
}








function DeleteSystem(sysNum : int){

	var tempSystems : Transform[]  = effectsSystems;
 	var tempClamp : int[]  = clampIndex;
 	//var tempData : float[] = effectData;
 	
	var endLP : int = tempSystems.Length-1;
	if (endLP <= 0){
		endLP = 0;
		
		effectsSystems = new Transform[tempSystems.Length+1];
		clampIndex = new int[tempSystems.Length+1];
		//effectData = new float[endLP];
		
	} else {

		effectsSystems = new Transform[endLP];
		clampIndex = new int[endLP];

		

		var setInt : int = -1;
		for (var aR : int = 0; aR < endLP; aR++){
			if (aR != sysNum){
				setInt += 1;
			} else {
				setInt += 2;
			}
			
			if (setInt < tempSystems.Length){
				effectsSystems[aR] = tempSystems[setInt];
				clampIndex[aR] = tempClamp[setInt];

			}
		}
	}

	
}



//function SetLabel( newLabel : String, labelNum : int){

	//if (labelNum < effectsLabels.Length && labelNum >= 0){
//		effectsLabels[labelNum] = newLabel;
	//}

//}



function OnApplicationQuit(){

	for (var fx=0; fx < (effectsSystems.Length); fx++){
		Destroy(fxObjects[fx]);
	}

}




