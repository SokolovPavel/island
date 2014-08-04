#pragma strict

@script ExecuteInEditMode()
import System.IO;


//preset variables
var useDarkUI : boolean = true;
var renderTex : RenderTexture;
var presetIndex : int;
private var presetUseIndex : int;
var presetOptions : String[];

var refractShift : float = 1.0;
var refractScale : float = 1.0;
var blurSpread : float = 0.1;
var hasStarted : boolean = false;
var setShoreWaveScale : float = 1.0;
var setFlowShoreScale : float = 1.0;
var setflowOffX : float = 0.0;
var setflowOffY : float = 0.0;
var usewaveShoreHt : float = 0.0;
var waveBreakAmt : float = 1.0;
var shallowFoamAmt : float = 1.0;

var shoreOffX : float = 0.0;
var shoreOffY : float = 0.0;

var presetTransIndexFrm : int = 0;
var presetTransIndexTo : int = 0;
var presetStartTransition : boolean = false;
var presetTransitionTime : float = 3.0;
var presetTransitionCurrent : float = 1.0;
var presetSaveName : String = "my custom preset";
var presetToggleSave : boolean = false;
var presetDataArray : String[];
var presetDataString : String;
var baseDir : String = "SUIMONO - WATER SYSTEM 2";
var suimonoModuleObject : GameObject;
var suimonoModuleLibrary : SuimonoModuleLib;
var mirrorTexture : RenderTexture;

var hFoamHeight : float = 5.5;
var hFoamAmt : float = 1.0;
var hFoamSpread : float = 2.0;

var reflectionObject : GameObject;
var shorelineObject : GameObject;
var shorelineComponent : Suimono_flowGenerator;

var overallBright : float = 1.0;
var overallTransparency : float = 1.0;
var enableUnderDebris : boolean = true;
var enableUnderDebrisWrite : float = 1.0;

var waterForce : Vector2 = Vector2(0.0,0.0);
var flowForce : Vector2 = Vector2(0.0,0.0);
var convertAngle:Vector3;

//general variables
var presetFile : String = "/RESOURCES/_PRESETS.txt";
var typeIndex : int;
var typeUseIndex : int;
var typeOptions = new Array("Infinite 3D Ocean","3D Waves","Flat Plane");
var enableCustomTextures : boolean = false;

//editor variables
var showGeneral : boolean = false;
var showWaves : boolean = false;
var showSurface : boolean = false;
var showFoam : boolean = false;
var showUnderwater : boolean = false;
var showEffects : boolean = false;
var showPresets : boolean = false;
var showTess : boolean = false;
var autoTess : boolean = true;

//underwater settings
var enableUnderwaterFX : boolean = true;
var enableUnderRefraction : boolean = true;
var enableDynamicReflections : boolean = true;

var underRefractionAmount : float = 1.0;
var underRefractionScale : float = 1.0;
var underRefractionSpeed : float = 1.0;

var enableUnderBlur : boolean = true;
var underBlurAmount : float = 0.01;
var blurSamples : int = 20;
var enableUnderAutoFog : boolean = true;
var underwaterColor : Color = Color(0.2,0.2,1.0,1.0);
var enableUnderEthereal : boolean = true;
var etherealShift : float = 0.1;
var underwaterFogDist : float = 1.0;
var underwaterFogSpread : float = 1.0;

//wave settings
var waveHeight : float = 0.0;
var detailHeight : float = 0.0;
var waveShoreHeight : float = 0.0;
var waveScale : float = 0.0;
var detailScale : float = 0.0;
var waveShoreScale : float = 0.0;
var shoreInfluence : float = 0.0;
var normalShore : float = 0.0;
var overallScale : float = 5.0;
var lightAbsorb : float = 0.125;
var lightRefract : float = 1.0;
var foamScale : float = 0.5;
var foamAmt : float = 0.3;
var foamColor : Color = Color(0.9,0.9,0.9,0.9);
var useFoamColor : Color = Color(0.9,0.9,0.9,0.9);
var edgeSpread : float = 0.3;
var edgeBlend : float = 0.3;
var edgeColor : Color = Color(0.9,0.9,0.9,0.9);
var useEdgeColor : Color = Color(0.9,0.9,0.9,0.9);
var reflectDist : float = 0.2;
var reflectDistUnderAmt : float = 0.2;
var reflectSpread : float = 0.05;
var colorDynReflect : Color = Color(1.0,1.0,1.0,0.4);
var reflectionOffset : float = 0.35;
var reflectPlaneObject : GameObject;

var colorSurfHigh : Color = Color(0.25,0.5,1.0,0.75);
var colorSurfLow : Color = Color(0,0,0,0);
var colorHeight : float = 6.0;
var colorHeightSpread : float = 2.5;
var surfaceSmooth : float = 1.0;

//color variables
var depthColor : Color;
var depthColorR : Color;
var depthColorG : Color;
var depthColorB : Color;

var specColorH : Color;
var specColorL : Color;
var specScatterWidth : float;
var specScatterAmt : float;

//tessellation settings
var waveTessAmt : float = 8;
var waveTessMin : float = 0.0;
var waveTessSpread : float = 0.2;
var waveFac : float = 1.0;

//flowmap
var inheritColor : boolean = true;
var wave_speed : Vector2 = Vector2(0.0015,0.0015);
var shore_speed : Vector2 = Vector2(0.0015,0.0015);
var foam_speed : Vector2 = Vector2(-0.02,-0.02);

//tide
var tideColor : Color;
var tideAmount : float;
var tideSpread : float;

private var useDepthColor : Color; 

//splash & collision variables
var splashIsOn : boolean = true;
var UpdateSpeed : float = 0.5;
var rippleSensitivity : float = 0.0;
var splashSensitivity :float = 0.2;
var isinwater : boolean = false;
var isUnderwater : boolean = false;
var atDepth : float = 0.0;
private var setvolume = 0.65;
private var ringsTime = 0.0;

var objectRingsTime : float[];
var objectRingsTimes = new Array();

var CurrentColliders : Collider[];
var CurrCollPoss : Vector3[];
var CurrCollPos = new Array();
var CurrentCollider = new Array();
private var moduleSplashObject : SuimonoModule;
private var thisSuimonoObject : GameObject;
private var suimonoScaleObject : GameObject;

private var shoreAmt : float = 0.85;
private var shoreAmtDk : float = 1.0;
private var shoreTideTimer : float = 0.0;

//wave texture animation variables
var flowSpeed : float = 0.1;
var setflowSpeed : float = 0.1;
var setshoreflowSpeed : float = 0.1;
var waveSpeed : float = 0.1;
var foamSpeed : float = 0.1;
var shoreSpeed : float = 0.1;
var flow_dir : Vector2 = Vector2(0.0015,0.0015);
var flow_dir_degrees : float = 0.0;

var shore_dir : Vector2 = Vector2(0.0015,0.0015);
var shore_dir_degrees : float = 0.0;

var wave_dir : Vector2 = Vector2(0.0015,0.0015);
var foam_dir : Vector2 = Vector2(-0.02,-0.02);
var water_dir : Vector2 = Vector2(0.0,0.0);
private var animationSpeed : float = 1.0;
private var timex : float = 0.0;
private var timey : float = 0.0;

private var shaderSurface : Shader;
private var shaderSurfaceScale : Shader;
private var shaderUnderwater : Shader;
private var shaderUnderwaterFX : Shader;

//flowmap
private var m_animationSpeed : float = 1.0;
private var systemSpeed : float = 1.0;
private var m_fFlowMapOffset0 : float = 0.0f;
private var m_fFlowMapOffset1 : float = 0.0f;
private var m_fFlowSpeed : float = 0.05f;
private var m_fCycle : float = 1.0f;
private var m_fWaveMapScale : float = 2.0f;

//infinite ocean
private var saveScale : float = 10.0;
private var oceanTimer : float = 1.0;
private var oceanHasStarted : boolean = false;

private var currentWaterLevel : float = 0.0;
private var currentSurfaceLevel : float = 0.0;

private var tempMaterial : Material;
private var tempMaterialScale : Material;

private var setPos : Vector3 = Vector3(0,0,0);
private var setSpace : Vector2 = Vector2(0.0,0.0);
private var setSpace2 : Vector2 = Vector2(0.0,0.0);
private var setSpace3 : Vector2 = Vector2(0.0,0.0);
private var setSpace4 : Vector2 = Vector2(0.0,0.0);
private var uvMult : float = 1.0;
private var uvMult2 : float = 1.0;
private var uvMult3 : float = 1.0;
private var uvMult4 : float = 1.0;		

private var currVersionIndex : int = 50;
private var shaderIsSet : boolean = false;
	

function Start () {

	//DISCONNECT FROM PREFAB
	#if UNITY_EDITOR
		PrefabUtility.DisconnectPrefabInstance(this.gameObject);
	#endif

	//SET DIRECTORIES
	#if UNITY_EDITOR
		baseDir = "SUIMONO - WATER SYSTEM 2";
		presetFile  = "/RESOURCES/_PRESETS.txt";
	#else
		baseDir = "";
		presetFile  = "/Resources/_PRESETS.txt";
	#endif
	
	//REFERENCE OBJECTS
	if (GameObject.Find("SUIMONO_Module") != null) suimonoModuleObject = GameObject.Find("SUIMONO_Module").gameObject;
	
	thisSuimonoObject = this.transform.Find("Suimono_Object").gameObject;
	suimonoScaleObject = this.transform.Find("Suimono_ObjectScale").gameObject;
	reflectionObject = this.transform.Find("Suimono_reflectionObject").gameObject;
	shorelineObject = this.transform.Find("Suimono_shorelineObject").gameObject;
	shorelineComponent = shorelineObject.GetComponent(Suimono_flowGenerator) as Suimono_flowGenerator;
	
	//INIT RENDER TEXTURE FOR REFLECTION
	renderTex = new RenderTexture(512,512,16,RenderTextureFormat.ARGB32);

	//SPLASH & COLLISION SETUP
	objectRingsTime = objectRingsTimes.ToBuiltin(float) as float[];
	if (suimonoModuleObject != null){
		moduleSplashObject = suimonoModuleObject.GetComponent(SuimonoModule);
		suimonoModuleLibrary = suimonoModuleObject.GetComponent(SuimonoModuleLib);
	} else {
		Debug.Log("SUIMONO: Warning... SUIMONO_Module game object cannot be found!");
	}
	
	CurrentColliders = CurrentCollider.ToBuiltin(Collider) as Collider[];
	CurrCollPoss = CurrCollPos.ToBuiltin(Vector3) as Vector3[];
	//Save Original Scale
	saveScale = this.transform.localScale.x;

	//setup custom material
	tempMaterial = new Material(thisSuimonoObject.renderer.sharedMaterial);
	thisSuimonoObject.renderer.sharedMaterial = tempMaterial;
	tempMaterialScale = new Material(suimonoScaleObject.renderer.sharedMaterial);
	suimonoScaleObject.renderer.sharedMaterial = tempMaterialScale;
	

	PresetLoad();
	InvokeRepeating("StoreSurfaceHeight",0.1,0.1);
	Invoke("MarkAsStarted",0.5);
	presetStartTransition = false;
	
	
	//shaderSurfaceScale = Shader.Find("Suimono2/waterscale_pro");
	//suimonoScaleObject.renderer.sharedMaterial.shader = shaderSurfaceScale;
	
}









function Update(){

	//get objects while in editor mode
	#if UNITY_EDITOR
	if (!Application.isPlaying){	
		if (moduleSplashObject == null){	
		if (GameObject.Find("SUIMONO_Module")){
			moduleSplashObject = GameObject.Find("SUIMONO_Module").GetComponent(SuimonoModule) as SuimonoModule;
		}
		}
	}
	#endif
	
	//set UI color
	if (moduleSplashObject != null){
		useDarkUI = moduleSplashObject.useDarkUI;
	}


	if (moduleSplashObject.unityVersionIndex != currVersionIndex){
		shaderIsSet = false;
	}
	
	
	
	//SET SHADER DEFAULTS
	//if (!shaderIsSet){
	
		//avoids incompatible shader assignments in various unity/target versions
		#if UNITY_EDITOR
			//UNITY BASIC VERSION SPECIFIC
			if (moduleSplashObject.unityVersionIndex == 0){//unity basic version
				shaderSurface = Shader.Find("Suimono2/water_basic");
				shaderSurfaceScale = Shader.Find("Suimono2/waterscale_basic");
				shaderUnderwater = Shader.Find("Suimono2/water_under_basic");
				shaderUnderwaterFX = Shader.Find("Suimono2/effect_refractPlane_basic");
			}
			
			//UNITY BASIC DX11 VERSION SPECIFIC
			if (moduleSplashObject.unityVersionIndex == 1){//unity basic version
				shaderSurface = Shader.Find("Suimono2/water_basic_dx11");
				shaderSurfaceScale = Shader.Find("Suimono2/water_basic_dx11");
				shaderUnderwater = Shader.Find("Suimono2/water_under_basic_dx11");
				shaderUnderwaterFX = Shader.Find("Suimono2/effect_refractPlane_basic");
			}	
					
			//UNITY iOS VERSION SPECIFIC
			else if (moduleSplashObject.unityVersionIndex == 4){//iOS
				shaderSurface = Shader.Find("Suimono2/water_ios");
				shaderSurfaceScale = Shader.Find("Suimono2/waterscale_ios");
				shaderUnderwater = Shader.Find("Suimono2/water_under_ios");
				shaderUnderwaterFX = Shader.Find("Suimono2/effect_refractPlane_basic");
			}
			
			//UNITY ANDROID VERSION SPECIFIC
			else if (moduleSplashObject.unityVersionIndex == 5){//android
				shaderSurface = Shader.Find("Suimono2/water_android");
				shaderSurfaceScale = Shader.Find("Suimono2/waterscale_android");
				shaderUnderwater = Shader.Find("Suimono2/water_under_android");
				shaderUnderwaterFX = Shader.Find("Suimono2/effect_refractPlane_basic");
			}
				
			//UNITY PRO DX11 VERSION SPECIFIC
			else if (moduleSplashObject.unityVersionIndex == 3){//dx11
				shaderSurface = Shader.Find("Suimono2/water_pro_dx11");
				shaderSurfaceScale = Shader.Find("Suimono2/water_pro_dx11");
				shaderUnderwater = Shader.Find("Suimono2/water_under_pro_dx11");
				shaderUnderwaterFX = Shader.Find("Suimono2/effect_refractPlane_dx11");
			}
	
			//UNITY PRO VERSION SPECIFIC
			else if (moduleSplashObject.unityVersionIndex == 2){//pro
				shaderSurface = Shader.Find("Suimono2/water_pro");
				if(!moduleSplashObject.enableRefraction) shaderSurface = Shader.Find("Suimono2/water_pro_norefract");
				shaderSurfaceScale = Shader.Find("Suimono2/waterscale_pro");
				if(!moduleSplashObject.enableRefraction) shaderSurfaceScale = Shader.Find("Suimono2/waterscale_pro_norefract");
				shaderUnderwater = Shader.Find("Suimono2/water_under_pro");
				shaderUnderwaterFX = Shader.Find("Suimono2/effect_refractPlane");
				if(!moduleSplashObject.enableRefraction) shaderUnderwaterFX = Shader.Find("Suimono2/effect_refractPlane_norefract");
				if (moduleSplashObject.setCamera.camera.actualRenderingPath == RenderingPath.Forward){
					shaderUnderwater = Shader.Find("Suimono2/water_under_pro_fwd");
				}
			}
		
			suimonoModuleLibrary.shader1 = shaderSurface;
			suimonoModuleLibrary.shader2 = shaderUnderwater;
			suimonoModuleLibrary.shader3 = shaderUnderwaterFX;
			suimonoModuleLibrary.shader4 = shaderSurfaceScale;
		#else
			shaderSurface = suimonoModuleLibrary.shader1;
			shaderUnderwater = suimonoModuleLibrary.shader2;
			shaderUnderwaterFX = suimonoModuleLibrary.shader3;
			shaderSurfaceScale = suimonoModuleLibrary.shader4;
		#endif


	//}

		//#######  SET SHADERS  #######
		if (Application.isPlaying){
		if (moduleSplashObject != null){
			if (currentWaterLevel >= 0.0){
				thisSuimonoObject.renderer.sharedMaterial.shader = shaderUnderwater;
			} else {
				thisSuimonoObject.renderer.sharedMaterial.shader = shaderSurface;
			}
		}
		} else {
			thisSuimonoObject.renderer.sharedMaterial.shader = shaderSurface;
		}
	
		//if (Application.isPlaying){
			//currVersionIndex = moduleSplashObject.unityVersionIndex;
			//shaderIsSet = true;
		//}
		
	if (!shaderIsSet){
		suimonoScaleObject.renderer.sharedMaterial.shader = shaderSurfaceScale;
		currVersionIndex = moduleSplashObject.unityVersionIndex;
		shaderIsSet = true;
	}








	//UPDATE PRESETS
	//get the current preset data.
	#if UNITY_EDITOR
		PresetGetData();
		if (presetIndex != presetUseIndex) PresetLoad();
		if (presetToggleSave) PresetSave("");
		if (presetStartTransition) PresetDoTransition();
		if (!presetStartTransition) presetTransitionCurrent = 0.0;
	#else
		if (moduleSplashObject.includePresetsInBuild){
			PresetGetData();
			if (presetIndex != presetUseIndex) PresetLoad();
			if (presetToggleSave) PresetSave("");
			if (presetStartTransition) PresetDoTransition();
			if (!presetStartTransition) presetTransitionCurrent = 0.0;
		}
	#endif
	
	//CONVERT DEGREES to FLOW DIRECTION
	//convert 0-360 degree setting to useable Vector2 data.
	//used for the normal and foam flow/uv scrolling.
	if (moduleSplashObject != null){
		flow_dir = moduleSplashObject.SuimonoConvertAngleToDegrees(flow_dir_degrees);
		shore_dir = moduleSplashObject.SuimonoConvertAngleToDegrees(270.0);
	}

	
	//####### RESET WAVE SETTINGS ON FLAT SURFACE ##########
	if (typeIndex == 2){
		waveFac = 1.0;
	} else {
		waveFac = 1.0;
	}
	

	//#######  MANAGE LOCAL REFLECTION TEXTURE  #######
	var renderTex : boolean = true;



	//######## HANDLE FORWARD RENDERING SWITCH #######
	if (moduleSplashObject.setCamera.camera.actualRenderingPath == RenderingPath.Forward){
		Shader.SetGlobalFloat("_isForward",1.0);
	} else {
		Shader.SetGlobalFloat("_isForward",0.0);
	}
	
	//######## HANDLE MAC RENDERING SWITCH #######
	var isMac : float = 0.0;
	#if UNITY_STANDALONE_OSX
		isMac = 1.0;
	#endif
	Shader.SetGlobalFloat("_isMac",isMac);
	
	//set blursamples
	if (moduleSplashObject != null){
		blurSamples = moduleSplashObject.blurSamples;
		thisSuimonoObject.renderer.sharedMaterial.SetFloat("_blurSamples", floatRound(blurSamples));
	}
	
	//FLOW MAP HANDLING
	m_animationSpeed = 1.0;
	m_animationSpeed = Mathf.Clamp(m_animationSpeed,0.0,1.0);
	
	//set speed limits
	setflowSpeed = flowSpeed;
	wave_speed.x = -flow_dir.x*(setflowSpeed);
	wave_speed.y = -flow_dir.y*(setflowSpeed);
	
	setshoreflowSpeed = Mathf.Lerp(0.0,2.0,shoreSpeed);
	shore_speed.x = -shore_dir.x*(setshoreflowSpeed);
	shore_speed.y = -shore_dir.y*(setshoreflowSpeed);

	//assign speed to shader
	thisSuimonoObject.renderer.sharedMaterial.SetTextureOffset("_WaveTex",Vector2((wave_speed.x*Time.time*m_animationSpeed)%1,(wave_speed.y*Time.time*m_animationSpeed)%1));
	thisSuimonoObject.renderer.sharedMaterial.SetTextureOffset("_WaveTex",Vector2((wave_speed.x*Time.time*m_animationSpeed),(wave_speed.y*Time.time*m_animationSpeed)));
	
	setflowOffX = thisSuimonoObject.renderer.sharedMaterial.GetTextureOffset("_Surface2").x;
	setflowOffY = thisSuimonoObject.renderer.sharedMaterial.GetTextureOffset("_Surface2").y;
	
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("flowOffX",floatRound(setflowOffX));
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("flowOffY",floatRound(setflowOffY));
	
	shoreOffX = floatRound(thisSuimonoObject.renderer.sharedMaterial.GetTextureOffset("_FlowMap").x);
	shoreOffY = floatRound(thisSuimonoObject.renderer.sharedMaterial.GetTextureOffset("_FlowMap").y);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("shoreOffX",shoreOffX);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("shoreOffY",shoreOffY);

	thisSuimonoObject.renderer.sharedMaterial.SetFloat("shoreWaveOffX",floatRound(thisSuimonoObject.renderer.sharedMaterial.GetTextureOffset("_WaveTex").x));
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("shoreWaveOffY",floatRound(thisSuimonoObject.renderer.sharedMaterial.GetTextureOffset("_WaveTex").y));



	//FILL MAIN TEXTURES BY DEFAULT
	//fills default texture slots from the Module object
	if (enableCustomTextures == false){
		if (suimonoModuleLibrary){
			if (suimonoModuleLibrary.texDisplace) thisSuimonoObject.renderer.sharedMaterial.SetTexture("_WaveLargeTex",suimonoModuleLibrary.texDisplace);
			if (suimonoModuleLibrary.texHeight1) thisSuimonoObject.renderer.sharedMaterial.SetTexture("_Surface1",suimonoModuleLibrary.texHeight1);
			if (suimonoModuleLibrary.texHeight2) thisSuimonoObject.renderer.sharedMaterial.SetTexture("_Surface2",suimonoModuleLibrary.texHeight2);
			if (suimonoModuleLibrary.texFoam1) thisSuimonoObject.renderer.sharedMaterial.SetTexture("_FoamOverlay",suimonoModuleLibrary.texFoam1);
			if (suimonoModuleLibrary.texFoam2) thisSuimonoObject.renderer.sharedMaterial.SetTexture("_FoamTex",suimonoModuleLibrary.texFoam2);
			if (suimonoModuleLibrary.texRampWave) thisSuimonoObject.renderer.sharedMaterial.SetTexture("_WaveRamp",suimonoModuleLibrary.texRampWave);
			if (suimonoModuleLibrary.texRampDepth) thisSuimonoObject.renderer.sharedMaterial.SetTexture("_DepthRamp",suimonoModuleLibrary.texRampDepth);
			if (suimonoModuleLibrary.texRampBlur) thisSuimonoObject.renderer.sharedMaterial.SetTexture("_BlurRamp",suimonoModuleLibrary.texRampBlur);
			if (suimonoModuleLibrary.texRampFoam) thisSuimonoObject.renderer.sharedMaterial.SetTexture("_FoamRamp",suimonoModuleLibrary.texRampFoam);
			if (suimonoModuleLibrary.texCube1) thisSuimonoObject.renderer.sharedMaterial.SetTexture("_CubeMobile",suimonoModuleLibrary.texCube1);
			if (suimonoModuleLibrary.texWave) thisSuimonoObject.renderer.sharedMaterial.SetTexture("_WaveTex",suimonoModuleLibrary.texWave);
		}
	}
	

	// SET WAVE SCALE
	if (hasStarted){
		var setWavScale : float = (this.transform.localScale.x/(10.0-waveScale));
		var setDetScale : float = (this.transform.localScale.x/(20.0-detailScale));
		setWavScale = waveScale * this.transform.localScale.x;
		setDetScale = detailScale * this.transform.localScale.x * 10.0;
		
		setShoreWaveScale = thisSuimonoObject.renderer.sharedMaterial.GetTextureScale("_WaveTex").x;
		
		//setWavScale = setDetScale;
		thisSuimonoObject.renderer.sharedMaterial.SetTextureScale("_Surface1",Vector2(setWavScale,setWavScale));
		thisSuimonoObject.renderer.sharedMaterial.SetFloat("waveScale",floatRound(setWavScale));
		thisSuimonoObject.renderer.sharedMaterial.SetTextureScale("_WaveLargeTex",Vector2(setDetScale,setDetScale));
		thisSuimonoObject.renderer.sharedMaterial.SetTextureScale("_Surface2",Vector2(setDetScale,setDetScale));
		thisSuimonoObject.renderer.sharedMaterial.SetFloat("detailScale",floatRound(setDetScale));
		thisSuimonoObject.renderer.sharedMaterial.SetFloat("normalShore",floatRound(normalShore));
		
		thisSuimonoObject.renderer.sharedMaterial.SetFloat("shoreWaveScale",floatRound(setShoreWaveScale));
		
		//set shore wave breaks
		thisSuimonoObject.renderer.sharedMaterial.SetTextureScale("_WaveTex",Vector2(floatRound(waveBreakAmt),0.0));
	}

	//SET SHADER TIME and SCALE
    thisSuimonoObject.renderer.sharedMaterial.SetFloat("_Phase", Time.time );
    thisSuimonoObject.renderer.sharedMaterial.SetFloat("_dScaleX", floatRound(thisSuimonoObject.renderer.sharedMaterial.GetTextureScale("_Surface1").x));
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_dScaleY", floatRound(thisSuimonoObject.renderer.sharedMaterial.GetTextureScale("_Surface1").y));
	

	//TESSELLATION SETTINGS
	var setTessScale : float = waveTessAmt;
	if (autoTess) setTessScale *= this.transform.localScale.x;
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_Tess", floatRound(setTessScale));
	var setTessStart : float = Mathf.Lerp(-180.0,0.0,waveTessMin);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_minDist", floatRound(setTessStart));
	var setTessSpread : float = Mathf.Lerp(20.0,500.0,waveTessSpread);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_maxDist", floatRound(setTessSpread));
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_Displacement", 1.0);
	

	//EDITOR MODE TWEAKS
	//certain calculations rely on depth buffer generation which the scene camera
	//won't calculate while in editor mode. The below temporarily addresses these
	//issues so the water surface doesn't look whack in editor mode.  this shouldn't
	//effect the in-game modes at all.
	useFoamColor = foamColor;
	useDepthColor = depthColor;
	useEdgeColor = edgeColor;

	if (!Application.isPlaying){
		useFoamColor.a = 0.0;
		useDepthColor.a = 0.35;
		useEdgeColor.a = 0.0;
	}
	

	//SPLASH AND COLLISION EFFECTS
	//advance fx timer
	ringsTime += Time.deltaTime;
	if (CurrentColliders.length > 0){
		for (var cx = 0; cx < CurrentColliders.length; cx++){
			objectRingsTime[cx] += Time.deltaTime;
		}
	}

	
	
	//Read the collision function
	if (splashIsOn){
		//CallCollisionFunction();
	}
	

	if (moduleSplashObject.unityVersionIndex == 0 || moduleSplashObject.unityVersionIndex == 1) renderTex = false;
	
	if (!moduleSplashObject.enableDynamicReflections || !enableDynamicReflections){
		renderTex = false;
	}
	if (!renderTex){
	if (Application.isPlaying){
		#if !UNITY_3_5
			if (reflectionObject != null) reflectionObject.SetActive(false);
	   	#else
			if (reflectionObject != null) reflectionObject.active = false;
	  	#endif
	}

	} else {
	
		if (reflectionObject != null && renderTex){
			//enable reflection based on distance
			var reflDist : float;
			if (moduleSplashObject.setCamera) reflDist = Vector3.Distance(transform.localPosition,moduleSplashObject.setCamera.localPosition);
			if (reflectionObject != null){
			if (Application.isPlaying){
			#if !UNITY_3_5
				//unity 4+ compilation
				if (reflDist <= (60.0*transform.localScale.x)) reflectionObject.SetActive(true);
				if (reflDist > (60.0*transform.localScale.x)) reflectionObject.SetActive(false);
	    	#else
	    		//unity 3.5 compilation
				if (reflDist <= (60.0*transform.localScale.x)) reflectionObject.active = true;
				if (reflDist > (60.0*transform.localScale.x)) reflectionObject.active = false;
	   		#endif
			}
			}
							
			//check for underwater
			isUnderwater = false;
			if (currentWaterLevel >= 0.0){
				//swap reflection coordinates underwater
				if (reflectionObject != null) reflectionObject.transform.eulerAngles = Vector3(0.0,0.0,180.0);
			} else {
				if (reflectionObject != null) reflectionObject.transform.eulerAngles = Vector3(0.0,0.0,0.0);
			}
		
		if (Application.isPlaying){	
			var getTex = reflectionObject.renderer.sharedMaterial.GetTexture("_ReflectionTex");
			thisSuimonoObject.renderer.sharedMaterial.SetTexture("_ReflectionTex",getTex);
		}
		}
	}
	
	
	

	
	
	
	
	
	
	if (shorelineObject != null){
	if (Application.isPlaying){
			#if !UNITY_3_5
				//unity 4+ compilation
				if (reflDist <= (60.0*transform.localScale.x)) shorelineObject.SetActive(true);
				if (reflDist > (60.0*transform.localScale.x)) shorelineObject.SetActive(false);
	    	#else
	    		//unity 3.5 compilation
				if (reflDist <= (60.0*transform.localScale.x)) shorelineObject.active = true;
				if (reflDist > (60.0*transform.localScale.x)) shorelineObject.active = false;
	   		#endif
	}
	}
	


	// ########## ASSIGN GENERAL ATTRIBUTES ############
	var useRefract : float = 1.0;
	if (!moduleSplashObject.enableRefraction) useRefract = 0.0;
	var refractScl : float = Mathf.Lerp(0.0,(2.25),refractScale);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_RefrScale",floatRound(refractScl));
	var useScale : float = (this.transform.localScale.x * refractScl);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_MasterScale",floatRound(useScale));
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_WaveAmt",floatRound(useScale));
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_NormalAmt",floatRound(useScale*10.0));
	
	//Calculate Shore & Wave FX
	var shoreWaveStretch : float = 4.5;
	var shoreWaveStretch2 : float = 0.0;
	shoreAmt = ((1.0-shoreWaveStretch)+Mathf.Sin(Time.time*0.75)*shoreWaveStretch);
	tideAmount = ((0.3)+Mathf.Sin(Time.time*0.45)*0.2);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_ShoreAmt",floatRound(shoreAmt));
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_TideAmount",floatRound(tideAmount));
	
	//Calculate Waves
	if (Application.isPlaying){
		var usewaveHt : float = Mathf.Lerp(0.0001,10.0,(waveHeight/10.0)*waveFac);
		thisSuimonoObject.renderer.sharedMaterial.SetFloat("_WaveHeight",floatRound(usewaveHt));
		var useDetHt = Mathf.Lerp(0.0001,3.0,(detailHeight/3.0)*waveFac);
		thisSuimonoObject.renderer.sharedMaterial.SetFloat("_DetailHeight",floatRound(useDetHt));
		
		usewaveShoreHt = Mathf.Lerp(0.0001,1.5,(waveShoreHeight*waveFac)/20.0);
		thisSuimonoObject.renderer.sharedMaterial.SetFloat("_WaveShoreHeight",floatRound(usewaveShoreHt));
	} else {
		thisSuimonoObject.renderer.sharedMaterial.SetFloat("_WaveHeight",0.01);
		thisSuimonoObject.renderer.sharedMaterial.SetFloat("_DetailHeight",0.01);
		thisSuimonoObject.renderer.sharedMaterial.SetFloat("_WaveShoreHeight",0.01);	
	}

	setFlowShoreScale = Mathf.Lerp(0.1,4,waveShoreScale);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_FlowShoreScale",floatRound(setFlowShoreScale));

	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_TimeX",thisSuimonoObject.renderer.sharedMaterial.GetTextureOffset("_Surface1").x);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_TimeY",thisSuimonoObject.renderer.sharedMaterial.GetTextureOffset("_Surface1").y);

	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_DTimeX",thisSuimonoObject.renderer.sharedMaterial.GetTextureOffset("_WaveLargeTex").x);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_DTimeY",thisSuimonoObject.renderer.sharedMaterial.GetTextureOffset("_WaveLargeTex").y);
	
	timex += Time.deltaTime * waveSpeed;
	timey += Time.deltaTime * waveSpeed;
	
	//Calculate Overall Brightness
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_OverallBright",floatRound(overallBright));
	
	//Calculate Overall Transparency
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_OverallTrans",floatRound(overallTransparency));
	
	//Calculate Light Absorption
	var absorbAmt : float = Mathf.Lerp(0.0,25.0,lightAbsorb);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_DepthAmt",floatRound(absorbAmt));
	
	//Calculate Refraction
	var setSCL :float = transform.localScale.x;
	var refractAmt : float = Mathf.Lerp(0.0,(500.0/setSCL),Mathf.Lerp(0.0,0.1,lightRefract));
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_RefrStrength",floatRound(refractAmt)*useRefract);
	var refractShft : float = Mathf.Lerp(0.0,0.2,refractShift);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_RefrShift",floatRound(refractShft)*useRefract);

	
	//Calculate Reflections
	if (thisSuimonoObject.renderer.sharedMaterial.GetTexture("_ReflectionTex") == null){
		thisSuimonoObject.renderer.sharedMaterial.SetFloat("useReflection",0.0);
	} else {
		thisSuimonoObject.renderer.sharedMaterial.SetFloat("useReflection",1.0);
	}
	
	var reflectDistAmt : float = Mathf.Lerp(-200,200, reflectDist);
	var reflectSpreadAmt : float = Mathf.Lerp(0.015,0.001,reflectSpread);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_ReflDist",floatRound(reflectDistAmt));
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_ReflBlend",floatRound(reflectSpreadAmt));
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_DynReflColor",colorDynReflect);
	var reflectAmt : float = Mathf.Lerp(0.0,40.0,reflectionOffset);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_ReflectStrength",floatRound(reflectAmt));
	
	//Calculate Underwater Reflections
	var reflectUnderDist : float = Mathf.Lerp(-30,0,reflectDistUnderAmt);
	reflectUnderDist = Mathf.Lerp(-10.0,0.0,reflectDistUnderAmt);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_UnderReflDist",floatRound(reflectUnderDist));

	//Calculate Blur
	var blurSprd : float = Mathf.Lerp(0.0,1.0,blurSpread);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_BlurSpread",floatRound(blurSprd)*useRefract);
	
	//surface smoothness
	var surfaceSmoothAmt : float = Mathf.Lerp(0.0,0.45,surfaceSmooth);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_BumpStrength",floatRound(surfaceSmoothAmt));
	
	//colors
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_HighColor",colorSurfHigh);
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_LowColor",colorSurfLow);
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_DepthColor",useDepthColor);
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_DepthColorR",depthColorR);
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_DepthColorG",depthColorG);
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_DepthColorB",depthColorB);
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_UnderColor",underwaterColor);
	
	var useUnderFogDist : float = Mathf.Lerp(-0.1,0.2,underwaterFogDist);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_UnderFogDist",floatRound(useUnderFogDist));
	
	//specular
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_SpecColorH",specColorH);
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_SpecColorL",specColorL);
	var _SpecHotAmt : float = Mathf.Lerp(0.1,5.0,specScatterWidth);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_SpecScatterWidth",floatRound(_SpecHotAmt));
	var _SpecAmt : float = Mathf.Lerp(0.5,10.0,specScatterAmt);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_SpecScatterAmt",floatRound(_SpecAmt));
	
	//tide
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_TideColor",tideColor);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_TideAmount",floatRound(tideAmount));
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_TideSpread",floatRound(tideSpread));

	
	// SET FOAM SCALING
	var setFoamScale : float = Mathf.Lerp(0.0001,10.0,foamScale);
	var useFoamScale : float = this.transform.localScale.x*setFoamScale;
	thisSuimonoObject.renderer.sharedMaterial.SetTextureScale("_FoamTex",Vector2(useFoamScale,useFoamScale));
	thisSuimonoObject.renderer.sharedMaterial.SetTextureScale("_FoamOverlay",Vector2(useFoamScale,useFoamScale));
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_ShallowFoamAmt",shallowFoamAmt);
	
	var foamSpread : float = (foamAmt) + (Mathf.Sin(Time.time*1.0)*(0.05 * (1.0+foamAmt)));
	var useFoamHt : float = hFoamHeight;
	if (QualitySettings.activeColorSpace == ColorSpace.Linear) useFoamHt = hFoamHeight*0.5;
		    		
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_FoamHeight", floatRound(useFoamHt));
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_HeightFoamAmount", floatRound(hFoamAmt));
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_HeightFoamSpread", floatRound(hFoamSpread));
	
	var setFoamSpread : float = Mathf.Lerp(0.02,1.0,foamSpread);
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_FoamSpread",floatRound(setFoamSpread));
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_FoamColor",useFoamColor);
	
	var setEdgeSpread : float = Mathf.Lerp(0.02,1.0*transform.localScale.x,edgeSpread);
	var setEdgeBlend : float = Mathf.Lerp(0.02,1.0*transform.localScale.x,edgeBlend);
	//if (edgeBlend == 0.01) setEdgeBlend = 2.0;
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_EdgeBlend",floatRound(setEdgeBlend));
	thisSuimonoObject.renderer.sharedMaterial.SetFloat("_EdgeSpread",floatRound(setEdgeSpread));
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_EdgeColor",useEdgeColor);

	//set uv reversal
	var useUVR = 0.0;
	if (moduleSplashObject.useUVReversal) useUVR = 1.0;
	//thisSuimonoObject.renderer.sharedMaterial.SetFloat("_UVReversal",useUVR);
	Shader.SetGlobalFloat("_UVReversal",useUVR);
	
	//WAVE TEXTURE ANIMATION
	animationSpeed = 1.0;
	
	//set speed limits
	flow_dir.x = Mathf.Clamp(flow_dir.x,-1.0,1.0);
	flow_dir.y = Mathf.Clamp(flow_dir.y,-1.0,1.0);
	
	shore_dir.x = Mathf.Clamp(shore_dir.x,-1.0,1.0);
	shore_dir.y = Mathf.Clamp(shore_dir.y,-1.0,1.0);
	
	wave_dir.x = flow_dir.x;
	wave_dir.y = flow_dir.y;

	foam_dir.x = flow_dir.x;
	foam_dir.y = flow_dir.y;
	
	uvMult = floatRound(setDetScale) * 0.1;
	uvMult2 = floatRound(setWavScale) * 0.1;
	uvMult3 = useFoamScale * 0.1;
	uvMult4 = 0.1;	

}











function FixedUpdate(){

	//####### MANAGE INFINITE SIZING #######
	if (Application.isPlaying && hasStarted){
		if (typeIndex == 0){
		
			var upStep : float = 20.0;
			var spacer : float = ((this.transform.localScale.x * 4.0)/upStep);
			var spacerUV :float = (1.0/upStep);

			//test
			upStep = 2.0;
			spacer = ((this.transform.localScale.x * 4.0)/upStep);
			spacerUV = (1.0/upStep);
			
			//set scale and intial position
			if (!oceanHasStarted){
			oceanHasStarted = true;
			var setScale : float = 1.0;
			
			if (moduleSplashObject.unityVersionIndex == 3 || moduleSplashObject.unityVersionIndex == 1){
				//scale for dx11
				setScale = moduleSplashObject.setCamera.camera.farClipPlane/ 10.0;
				this.transform.localScale = Vector3(setScale,1.0,setScale);
			//} else if (moduleSplashObject.unityVersionIndex == 2){
			} else {
				//scale for dx9
				setScale = moduleSplashObject.setCamera.camera.farClipPlane/ 1.0;
				this.transform.localScale = Vector3(overallScale,1.0,overallScale);
				suimonoScaleObject.transform.localScale = Vector3(overallScale,1.0,overallScale);
				//suimonoScaleObject.renderer.material.CopyPropertiesFromMaterial(thisSuimonoObject.renderer.sharedMaterial);
				//suimonoScaleObject.renderer.sharedMaterial.SetFloat("_infScale",5.0);
			}
			
			this.transform.position = Vector3(moduleSplashObject.setTrack.position.x,this.transform.position.y,moduleSplashObject.setTrack.position.z);
			}
			
			
			//set properties from master material in dx9
			if (moduleSplashObject.unityVersionIndex != 3 && moduleSplashObject.unityVersionIndex != 1){
				suimonoScaleObject.renderer.enabled = true;
				suimonoScaleObject.transform.localScale = Vector3(overallScale,1.0,overallScale);
				suimonoScaleObject.renderer.material.CopyPropertiesFromMaterial(thisSuimonoObject.renderer.sharedMaterial);
				var useSc : float = overallScale;
				var setSc : Vector2 = suimonoScaleObject.renderer.sharedMaterial.GetTextureScale("_WaveLargeTex");
				suimonoScaleObject.renderer.sharedMaterial.SetTextureScale("_WaveLargeTex", setSc*useSc);
				setSc = suimonoScaleObject.renderer.sharedMaterial.GetTextureScale("_Surface1");
				suimonoScaleObject.renderer.sharedMaterial.SetTextureScale("_Surface1", setSc*useSc);
				setSc = suimonoScaleObject.renderer.sharedMaterial.GetTextureScale("_Surface2");
				suimonoScaleObject.renderer.sharedMaterial.SetTextureScale("_Surface2", setSc*useSc);
				var setDpth : float = thisSuimonoObject.renderer.sharedMaterial.GetFloat("_DepthAmt");
				suimonoScaleObject.renderer.sharedMaterial.SetFloat("_DepthAmt", setDpth*useSc);
			}
			
			
			//set position
			var newPos : Vector3 = Vector3(moduleSplashObject.setTrack.position.x,this.transform.position.y,moduleSplashObject.setTrack.position.z);
			if (Mathf.Abs(this.transform.position.x - newPos.x) > spacer){
				var fudgex : float = (this.transform.position.x - newPos.x)/spacer;
				setSpace.x += (spacerUV*fudgex)*uvMult;
				setSpace2.x += (spacerUV*fudgex)*uvMult2;
				setSpace3.x += (spacerUV*fudgex)*uvMult3;
				setSpace4.x += (spacerUV*fudgex)*uvMult4;
				this.transform.position.x = newPos.x;
			}
			if (Mathf.Abs(this.transform.position.z - newPos.z) > spacer){
				var fudgey : float = (this.transform.position.z - newPos.z)/spacer;
				setSpace.y += (spacerUV*fudgey)*uvMult;
				setSpace2.y += (spacerUV*fudgey)*uvMult2;
				setSpace3.y += (spacerUV*fudgey)*uvMult3;
				setSpace4.y += (spacerUV*fudgey)*uvMult4;
				this.transform.position.z = newPos.z;
			}	
		} else {
			suimonoScaleObject.renderer.enabled = false;
		}
	}

	//assign speed to shader
	thisSuimonoObject.renderer.sharedMaterial.SetTextureOffset("_FoamTex",Vector2((foam_dir.x*Time.time*animationSpeed * foamSpeed)+setSpace3.x,(foam_dir.y*Time.time*animationSpeed * foamSpeed)+setSpace3.y));
	thisSuimonoObject.renderer.sharedMaterial.SetTextureOffset("_FoamOverlay",Vector2((-foam_dir.x*Time.time*animationSpeed * foamSpeed *0.5)+setSpace3.x,(-foam_dir.y*Time.time*animationSpeed * foamSpeed *0.5)+setSpace3.y));
	thisSuimonoObject.renderer.sharedMaterial.SetTextureOffset("_Surface1",Vector2((flow_dir.x*Time.time*animationSpeed*setflowSpeed*0.2)+setSpace2.x,(flow_dir.y*Time.time*animationSpeed * setflowSpeed *0.2)+setSpace2.y));
	thisSuimonoObject.renderer.sharedMaterial.SetTextureOffset("_Surface2",Vector2((shore_dir.x*Time.time*-animationSpeed*setshoreflowSpeed*0.5*(setflowSpeed*5.0))+setSpace.x,(shore_dir.y*Time.time*animationSpeed*-setshoreflowSpeed*(setflowSpeed*5.0))+setSpace.y));
	thisSuimonoObject.renderer.sharedMaterial.SetTextureOffset("_WaveLargeTex",Vector2((flow_dir.x*Time.time*animationSpeed*setflowSpeed)+setSpace.x,(flow_dir.y*Time.time*animationSpeed * setflowSpeed) +setSpace.y));
	thisSuimonoObject.renderer.sharedMaterial.SetTextureOffset("_FlowMap",Vector2(setSpace4.x,setSpace4.y));	
}











// ###################################################################
// ##### START CUSTOM FUNCTIONS ######################################
// ###################################################################
function MarkAsStarted(){
	hasStarted = true;
}


function StoreSurfaceHeight(){
	currentWaterLevel = moduleSplashObject.currentObjectDepth;
	currentSurfaceLevel = moduleSplashObject.currentSurfaceLevel;
}


// ########## SPLASH / COLLISION FUNCTIONS ##########
function CallCollisionFunction(){

	for (var cx : int = 0; cx < CurrentColliders.length; cx++){
		var ckSpeed = UpdateSpeed;
		if (CurrentColliders[cx] == null){
			CurrentCollider.RemoveAt(cx);
			CurrCollPos.RemoveAt(cx);
			CurrentColliders = CurrentCollider.ToBuiltin(Collider) as Collider[];
			CurrCollPoss = CurrCollPos.ToBuiltin(Vector3) as Vector3[];
			
		} else {
		
			//get optional parameters
			var alwaysEmit : boolean = false;
			var addSize : float = 0.2;
			var addRot : float = Random.Range(0.0,359.0);
			var addVel : Vector3 = Vector3(0.0,0.0,0.0);
			
			var hitVeloc = Vector3(1.0,1.0,1.0);
			
			if (CurrentColliders[cx].gameObject.rigidbody){
				hitVeloc = CurrentColliders[cx].gameObject.rigidbody.velocity;
			}
			
			//calculate rotation
			if (Mathf.Abs(hitVeloc.x) >= rippleSensitivity || Mathf.Abs(hitVeloc.z) >= rippleSensitivity){
				var tempPointer : GameObject;
				var tempDetector : GameObject;
				tempPointer = new GameObject ("tempPointer");
				tempDetector = new GameObject ("tempDetector");
				
				tempPointer.transform.position = CurrentColliders[cx].transform.position + (hitVeloc*10.0);
				tempDetector.transform.position = CurrentColliders[cx].transform.position;
				tempDetector.transform.LookAt(tempPointer.transform.position);
				addRot = tempDetector.transform.eulerAngles.y+40.0;	
				gameObject.Destroy(tempPointer);
				gameObject.Destroy(tempDetector);
			}
				
			
			var CollSplashFX_component : fx_splashEffects = CurrentColliders[cx].gameObject.GetComponent(fx_splashEffects);
			if (CollSplashFX_component != null){
				ckSpeed = CollSplashFX_component.setSplashRingsTimer;
				addSize = CollSplashFX_component.splashRingsSize;
				if (CollSplashFX_component.splashRingsRotation != 0.0){
					addRot = CollSplashFX_component.splashRingsRotation;
				}
				if (CollSplashFX_component.alwaysEmitRipples || moduleSplashObject.alwaysEmitRipples){
					alwaysEmit = true;
				}
			}

			
			if (objectRingsTime[cx] > ckSpeed){
				objectRingsTime[cx] = 0.0;
				var checkColl : Collider = CurrentColliders[cx];
				var collsetpos : Vector3 = checkColl.transform.position;
				
				//calculate Y-Height
				collsetpos.y = moduleSplashObject.SuimonoGetHeight(collsetpos,"surfaceLevel");
				var sizeScale : float = 0.12;
				
				//check for movement and init splash effects
				if (sizeScale > 0.0 && CurrCollPoss[cx].ToString("F4") != collsetpos.ToString("F4")){
					sizeScale = Mathf.Clamp(sizeScale,0.1,0.3) * 2.0;
					//moduleSplashObject.AddEffect("rings",collsetpos,1,addSize*sizeScale,addRot,addVel);
					//moduleSplashObject.AddEffect("ringfoam",collsetpos,1,addSize*sizeScale,addRot,addVel);
					if ((addSize*sizeScale*10.0) >= 0.45){
						//moduleSplashObject.AddEffect("splash",collsetpos,4,addSize*sizeScale*10.0,addRot,addVel);
					}
					if ((addSize*sizeScale*10.0) >= 0.5){
						//moduleSplashObject.AddEffect("splashDrop",collsetpos,40,addSize*0.25,0.0,addVel);
					}
					//add sound
					var soundVol : float = 0.35;
					if ((addSize*sizeScale*10.0) >= 0.45){
						soundVol = 0.7;
					}
					if ((addSize*sizeScale*10.0) >= 0.5){
						soundVol = 1.0;
					}
					moduleSplashObject.AddSound("splash",collsetpos,Vector3(0,0,soundVol));
				}
			}
		}
	}
}









function OnApplicationQuit(){

	#if UNITY_EDITOR
	thisSuimonoObject.renderer.sharedMaterial.shader = shaderSurface;
	#endif
	
	if (reflectionObject != null){
		#if !UNITY_3_5
			//unity 4+ compilation
			reflectionObject.SetActive(true);
		#else
			//unity 3.5 compilation
			reflectionObject.active = true;
		#endif
	}
}








// ########## PUBLIC FUNCTIONS ##########
function SetPreset( useIndex : int){
	if (useIndex < presetDataArray.Length){
		presetIndex = useIndex;
	}
}

function SetPresetTransition( frmIndex : int, toIndex : int, setDuration : float){
	if (frmIndex < presetDataArray.Length && toIndex < presetDataArray.Length){
		presetTransIndexFrm = frmIndex;
		presetTransIndexTo = toIndex;
		presetTransitionTime = setDuration;
		presetStartTransition = true;
	}
}

function SetLerpTransition( frmIndex : int, toIndex : int, setLerp : float){
	if (frmIndex < presetDataArray.Length && toIndex < presetDataArray.Length){
		presetTransIndexFrm = frmIndex;
		presetTransIndexTo = toIndex;
		presetTransitionTime = setLerp;
		PresetDoTransition();
	}
}








// ########## PRESET FUNCTIONS ##########

function PresetLoad(){
	presetUseIndex = presetIndex;

	var workData : String;
	for (var px = 0; px < (presetDataArray.length); px++){
		workData = presetDataArray[px];
		if (px == presetUseIndex) break;
	}
	
	//set data
	var pName : String = workData.Substring(0,20);

	//set colors
	var sK : int = 21;
	depthColor = Color(float.Parse(workData.Substring((sK*1)+1,4)),float.Parse(workData.Substring((sK*1)+6,4)),float.Parse(workData.Substring((sK*1)+11,4)),float.Parse(workData.Substring((sK*1)+16,4)));
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_DepthColor",depthColor);
	colorSurfHigh = Color(float.Parse(workData.Substring((sK*2)+1,4)),float.Parse(workData.Substring((sK*2)+6,4)),float.Parse(workData.Substring((sK*2)+11,4)),float.Parse(workData.Substring((sK*2)+16,4)));
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_HighColor",colorSurfHigh);
	colorSurfLow = Color(float.Parse(workData.Substring((sK*3)+1,4)),float.Parse(workData.Substring((sK*3)+6,4)),float.Parse(workData.Substring((sK*3)+11,4)),float.Parse(workData.Substring((sK*3)+16,4)));
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_LowColor",colorSurfLow);
	depthColorR = Color(float.Parse(workData.Substring((sK*4)+1,4)),float.Parse(workData.Substring((sK*4)+6,4)),float.Parse(workData.Substring((sK*4)+11,4)),float.Parse(workData.Substring((sK*4)+16,4)));
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_DepthColorR",depthColorR);
	depthColorG = Color(float.Parse(workData.Substring((sK*5)+1,4)),float.Parse(workData.Substring((sK*5)+6,4)),float.Parse(workData.Substring((sK*5)+11,4)),float.Parse(workData.Substring((sK*5)+16,4)));
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_DepthColorG",depthColorG);
	depthColorB = Color(float.Parse(workData.Substring((sK*6)+1,4)),float.Parse(workData.Substring((sK*6)+6,4)),float.Parse(workData.Substring((sK*6)+11,4)),float.Parse(workData.Substring((sK*6)+16,4)));
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_DepthColorB",depthColorB);		
	specColorH = Color(float.Parse(workData.Substring((sK*7)+1,4)),float.Parse(workData.Substring((sK*7)+6,4)),float.Parse(workData.Substring((sK*7)+11,4)),float.Parse(workData.Substring((sK*7)+16,4)));
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_SpecColorH",specColorH);
	specColorL = Color(float.Parse(workData.Substring((sK*8)+1,4)),float.Parse(workData.Substring((sK*8)+6,4)),float.Parse(workData.Substring((sK*8)+11,4)),float.Parse(workData.Substring((sK*8)+16,4)));
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_SpecColorL",specColorL);
	colorDynReflect = Color(float.Parse(workData.Substring((sK*9)+1,4)),float.Parse(workData.Substring((sK*9)+6,4)),float.Parse(workData.Substring((sK*9)+11,4)),float.Parse(workData.Substring((sK*9)+16,4)));
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_DynReflColor",colorDynReflect);
	foamColor = Color(float.Parse(workData.Substring((sK*10)+1,4)),float.Parse(workData.Substring((sK*10)+6,4)),float.Parse(workData.Substring((sK*10)+11,4)),float.Parse(workData.Substring((sK*10)+16,4)));
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_FoamColor",useFoamColor);
	edgeColor = Color(float.Parse(workData.Substring((sK*11)+1,4)),float.Parse(workData.Substring((sK*11)+6,4)),float.Parse(workData.Substring((sK*11)+11,4)),float.Parse(workData.Substring((sK*11)+16,4)));
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_EdgeColor",useEdgeColor);			
	underwaterColor = Color(float.Parse(workData.Substring((sK*12)+1,4)),float.Parse(workData.Substring((sK*12)+6,4)),float.Parse(workData.Substring((sK*12)+11,4)),float.Parse(workData.Substring((sK*12)+16,4)));
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_UnderColor",underwaterColor);	
	tideColor = Color(float.Parse(workData.Substring((sK*13)+1,4)),float.Parse(workData.Substring((sK*13)+6,4)),float.Parse(workData.Substring((sK*13)+11,4)),float.Parse(workData.Substring((sK*13)+16,4)));
	thisSuimonoObject.renderer.sharedMaterial.SetColor("_TideColor",tideColor);	

	//set attributes
	lightAbsorb = float.Parse(workData.Substring((sK*14)+(8*1)+1,6));
	lightRefract = float.Parse(workData.Substring((sK*14)+(8*2)+1,6));
	refractShift = float.Parse(workData.Substring((sK*14)+(8*3)+1,6));

	blurSpread = float.Parse(workData.Substring((sK*14)+(8*5)+1,6));
	surfaceSmooth = float.Parse(workData.Substring((sK*14)+(8*6)+1,6));
	reflectDist = float.Parse(workData.Substring((sK*14)+(8*7)+1,6));
	reflectSpread = float.Parse(workData.Substring((sK*14)+(8*8)+1,6));
	reflectionOffset = float.Parse(workData.Substring((sK*14)+(8*9)+1,6));
	edgeBlend = float.Parse(workData.Substring((sK*14)+(8*10)+1,6));
	normalShore = float.Parse(workData.Substring((sK*14)+(8*11)+1,6));
	specScatterAmt = float.Parse(workData.Substring((sK*14)+(8*12)+1,6));
	specScatterWidth = float.Parse(workData.Substring((sK*14)+(8*13)+1,6));
	hFoamHeight = float.Parse(workData.Substring((sK*14)+(8*14)+1,6));
	hFoamAmt = float.Parse(workData.Substring((sK*14)+(8*15)+1,6));
	hFoamSpread = float.Parse(workData.Substring((sK*14)+(8*16)+1,6));
	foamAmt = float.Parse(workData.Substring((sK*14)+(8*17)+1,6));
	foamScale = float.Parse(workData.Substring((sK*14)+(8*18)+1,6));
	edgeSpread = float.Parse(workData.Substring((sK*14)+(8*19)+1,6));
	detailHeight = float.Parse(workData.Substring((sK*14)+(8*20)+1,6));
	detailScale = float.Parse(workData.Substring((sK*14)+(8*21)+1,6));
	UpdateSpeed = float.Parse(workData.Substring((sK*14)+(8*22)+1,6));
	rippleSensitivity = float.Parse(workData.Substring((sK*14)+(8*23)+1,6));
	splashSensitivity = float.Parse(workData.Substring((sK*14)+(8*24)+1,6));
	reflectDistUnderAmt = float.Parse(workData.Substring((sK*14)+(8*25)+1,6));
	underRefractionAmount = float.Parse(workData.Substring((sK*14)+(8*26)+1,6));
	underBlurAmount = float.Parse(workData.Substring((sK*14)+(8*27)+1,6));
	etherealShift = float.Parse(workData.Substring((sK*14)+(8*28)+1,6));

	underwaterFogDist = float.Parse(workData.Substring((sK*14)+(8*29)+1,6));	
	underwaterFogSpread = float.Parse(workData.Substring((sK*14)+(8*30)+1,6));

	waveHeight = float.Parse(workData.Substring((sK*14)+(8*31)+1,6));
	waveShoreHeight = float.Parse(workData.Substring((sK*14)+(8*32)+1,6));
	waveScale = float.Parse(workData.Substring((sK*14)+(8*33)+1,6));		
											
	waveShoreScale = float.Parse(workData.Substring((sK*14)+(8*34)+1,6));	
	shoreSpeed = float.Parse(workData.Substring((sK*14)+(8*35)+1,6));	

    enableUnderDebrisWrite = float.Parse(workData.Substring((sK*14)+(8*36)+1,6));	
    enableUnderDebris = false;
    if (enableUnderDebrisWrite == 1.0) enableUnderDebris = true;
    
	tideAmount = float.Parse(workData.Substring((sK*14)+(8*37)+1,6));	
    tideSpread = float.Parse(workData.Substring((sK*14)+(8*38)+1,6));	

	underRefractionScale = float.Parse(workData.Substring((sK*14)+(8*39)+1,6));
	underRefractionSpeed = float.Parse(workData.Substring((sK*14)+(8*40)+1,6));

	waveBreakAmt = float.Parse(workData.Substring((sK*14)+(8*42)+1,6));
	shallowFoamAmt = float.Parse(workData.Substring((sK*14)+(8*43)+1,6));

	overallBright = float.Parse(workData.Substring((sK*14)+(8*44)+1,6));
	overallTransparency = float.Parse(workData.Substring((sK*14)+(8*45)+1,6));
	
}





function PresetGetColor( presetCheck : int, presetKey : String) : Color {
	var workData : String;
	for (var px = 0; px < (presetDataArray.length); px++){
		workData = presetDataArray[px];
		if (px == presetCheck) break;
	}
	
	var retCol : Color = Color(0,0,0,1);
	
	//set data
	var pName : String = workData.Substring(0,20);
	
	//set colors
	var sK : int = 21;
	
	if (presetKey == "_DepthColor") retCol = Color(float.Parse(workData.Substring(sK+1,4)),float.Parse(workData.Substring(sK+6,4)),float.Parse(workData.Substring(sK+11,4)),float.Parse(workData.Substring(sK+16,4)));
	if (presetKey == "_HighColor") retCol = Color(float.Parse(workData.Substring((sK*2)+1,4)),float.Parse(workData.Substring((sK*2)+6,4)),float.Parse(workData.Substring((sK*2)+11,4)),float.Parse(workData.Substring((sK*2)+16,4)));
	if (presetKey == "_LowColor") retCol = Color(float.Parse(workData.Substring((sK*3)+1,4)),float.Parse(workData.Substring((sK*3)+6,4)),float.Parse(workData.Substring((sK*3)+11,4)),float.Parse(workData.Substring((sK*3)+16,4)));
	if (presetKey == "_DepthColorR") retCol = Color(float.Parse(workData.Substring((sK*4)+1,4)),float.Parse(workData.Substring((sK*4)+6,4)),float.Parse(workData.Substring((sK*4)+11,4)),float.Parse(workData.Substring((sK*4)+16,4)));
	if (presetKey == "_DepthColorG") retCol = Color(float.Parse(workData.Substring((sK*5)+1,4)),float.Parse(workData.Substring((sK*5)+6,4)),float.Parse(workData.Substring((sK*5)+11,4)),float.Parse(workData.Substring((sK*5)+16,4)));
	if (presetKey == "_DepthColorB") retCol = Color(float.Parse(workData.Substring((sK*6)+1,4)),float.Parse(workData.Substring((sK*6)+6,4)),float.Parse(workData.Substring((sK*6)+11,4)),float.Parse(workData.Substring((sK*6)+16,4)));
	if (presetKey == "_SpecColorH") retCol = Color(float.Parse(workData.Substring((sK*7)+1,4)),float.Parse(workData.Substring((sK*7)+6,4)),float.Parse(workData.Substring((sK*7)+11,4)),float.Parse(workData.Substring((sK*7)+16,4)));
	if (presetKey == "_SpecColorL") retCol = Color(float.Parse(workData.Substring((sK*8)+1,4)),float.Parse(workData.Substring((sK*8)+6,4)),float.Parse(workData.Substring((sK*8)+11,4)),float.Parse(workData.Substring((sK*8)+16,4)));
	if (presetKey == "_DynReflColor") retCol = Color(float.Parse(workData.Substring((sK*9)+1,4)),float.Parse(workData.Substring((sK*9)+6,4)),float.Parse(workData.Substring((sK*9)+11,4)),float.Parse(workData.Substring((sK*9)+16,4)));
	if (presetKey == "_FoamColor") retCol = Color(float.Parse(workData.Substring((sK*10)+1,4)),float.Parse(workData.Substring((sK*10)+6,4)),float.Parse(workData.Substring((sK*10)+11,4)),float.Parse(workData.Substring((sK*10)+16,4)));
	if (presetKey == "_EdgeColor") retCol = Color(float.Parse(workData.Substring((sK*11)+1,4)),float.Parse(workData.Substring((sK*11)+6,4)),float.Parse(workData.Substring((sK*11)+11,4)),float.Parse(workData.Substring((sK*11)+16,4)));
	if (presetKey == "_UnderwaterColor") retCol = Color(float.Parse(workData.Substring((sK*12)+1,4)),float.Parse(workData.Substring((sK*12)+6,4)),float.Parse(workData.Substring((sK*12)+11,4)),float.Parse(workData.Substring((sK*12)+16,4)));
	if (presetKey == "_TideColor") retCol = Color(float.Parse(workData.Substring((sK*13)+1,4)),float.Parse(workData.Substring((sK*13)+6,4)),float.Parse(workData.Substring((sK*13)+11,4)),float.Parse(workData.Substring((sK*13)+16,4)));

	return retCol;
}




function PresetGetFloat( presetCheck : int, presetKey : String) : float {
	var workData : String;
	for (var px = 0; px < (presetDataArray.length); px++){
		workData = presetDataArray[px];
		if (px == presetCheck) break;
	}
	
	var retVal : float = 0.0;
	
	//set data
	var pName : String = workData.Substring(0,20);

	//set attributes
	var sK : int = 21;
	if (presetKey == "_MasterScale") retVal = float.Parse(workData.Substring((sK*14)+1,6));
	if (presetKey == "_LightAbsorb") retVal = float.Parse(workData.Substring((sK*14)+(8*1)+1,6));
	if (presetKey == "_LightRefract") retVal = float.Parse(workData.Substring((sK*14)+(8*2)+1,6));
	if (presetKey == "_RefractShift") retVal = float.Parse(workData.Substring((sK*14)+(8*3)+1,6));
	if (presetKey == "_BlurSpread") retVal = float.Parse(workData.Substring((sK*14)+(8*5)+1,6));
	if (presetKey == "_SurfaceSmooth") retVal = float.Parse(workData.Substring((sK*14)+(8*6)+1,6));
	if (presetKey == "_ReflectDist") retVal = float.Parse(workData.Substring((sK*14)+(8*7)+1,6));
	if (presetKey == "_ReflectSpread") retVal = float.Parse(workData.Substring((sK*14)+(8*8)+1,6));
	if (presetKey == "_ReflectionOffset") retVal = float.Parse(workData.Substring((sK*14)+(8*9)+1,6));
	if (presetKey == "_EdgeBlend") retVal = float.Parse(workData.Substring((sK*14)+(8*10)+1,6));
	if (presetKey == "_NormalShore") retVal = float.Parse(workData.Substring((sK*14)+(8*11)+1,6));
	if (presetKey == "_SpecScatterAmt") retVal = float.Parse(workData.Substring((sK*14)+(8*12)+1,6));
	if (presetKey == "_SpecScatterWidth") retVal = float.Parse(workData.Substring((sK*14)+(8*13)+1,6));
	if (presetKey == "_HFoamHeight") retVal = float.Parse(workData.Substring((sK*14)+(8*14)+1,6));
	if (presetKey == "_HFoamAmt") retVal = float.Parse(workData.Substring((sK*14)+(8*15)+1,6));
	if (presetKey == "_HFoamSpread") retVal = float.Parse(workData.Substring((sK*14)+(8*16)+1,6));
	if (presetKey == "_FoamAmt") retVal = float.Parse(workData.Substring((sK*14)+(8*17)+1,6));
	if (presetKey == "_FoamScale") retVal = float.Parse(workData.Substring((sK*14)+(8*18)+1,6));
	if (presetKey == "_EdgeSpread") retVal = float.Parse(workData.Substring((sK*14)+(8*19)+1,6));
	if (presetKey == "_DetailHeight") retVal = float.Parse(workData.Substring((sK*14)+(8*20)+1,6));
	if (presetKey == "_DetailScale") retVal = float.Parse(workData.Substring((sK*14)+(8*21)+1,6));
	if (presetKey == "_UpdateSpeed") retVal = float.Parse(workData.Substring((sK*14)+(8*22)+1,6));
	if (presetKey == "_RippleSensitivity") retVal = float.Parse(workData.Substring((sK*14)+(8*23)+1,6));
	if (presetKey == "_SplashSensitivity") retVal = float.Parse(workData.Substring((sK*14)+(8*24)+1,6));
	if (presetKey == "_ReflectDistUnderAmt") retVal = float.Parse(workData.Substring((sK*14)+(8*25)+1,6));
	if (presetKey == "_UnderRefractionAmount") retVal = float.Parse(workData.Substring((sK*14)+(8*26)+1,6));
	if (presetKey == "_UnderBlurAmount") retVal = float.Parse(workData.Substring((sK*14)+(8*27)+1,6));
	if (presetKey == "_EtherealShift") retVal = float.Parse(workData.Substring((sK*14)+(8*28)+1,6));
	
	if (presetKey == "_UnderwaterFogDist") retVal = float.Parse(workData.Substring((sK*14)+(8*29)+1,6));
	if (presetKey == "_UnderwaterFogSpread") retVal = float.Parse(workData.Substring((sK*14)+(8*30)+1,6));

	if (presetKey == "_WaveHeight") retVal = float.Parse(workData.Substring((sK*14)+(8*31)+1,6));
	if (presetKey == "_WaveShoreHeight") retVal = float.Parse(workData.Substring((sK*14)+(8*32)+1,6));
	if (presetKey == "_WaveScale") retVal = float.Parse(workData.Substring((sK*14)+(8*33)+1,6));

	if (presetKey == "_WaveShoreScale") retVal = float.Parse(workData.Substring((sK*14)+(8*34)+1,6));
	if (presetKey == "_ShoreSpeed") retVal = float.Parse(workData.Substring((sK*14)+(8*35)+1,6));

    if (presetKey == "_EnableUnderDebris") retVal = float.Parse(workData.Substring((sK*14)+(8*36)+1,6));
    
	if (presetKey == "_TideAmount") retVal = float.Parse(workData.Substring((sK*14)+(8*37)+1,6));
	if (presetKey == "_TideSpread") retVal = float.Parse(workData.Substring((sK*14)+(8*38)+1,6));

	if (presetKey == "_UnderRefractionScale") retVal = float.Parse(workData.Substring((sK*14)+(8*39)+1,6));
	if (presetKey == "_UnderRefractionSpeed") retVal = float.Parse(workData.Substring((sK*14)+(8*40)+1,6));

	if (presetKey == "_TypeIndex") retVal = float.Parse(workData.Substring((sK*14)+(8*41)+1,6));

	if (presetKey == "_WaveBreakAmt") retVal = float.Parse(workData.Substring((sK*14)+(8*42)+1,6));
	if (presetKey == "_ShallowFoamAmt") retVal = float.Parse(workData.Substring((sK*14)+(8*43)+1,6));

	if (presetKey == "_OverallBright") retVal = float.Parse(workData.Substring((sK*14)+(8*44)+1,6));
	if (presetKey == "_OverallTransparency") retVal = float.Parse(workData.Substring((sK*14)+(8*45)+1,6));

	return retVal;
}


	
	

function PresetSave( useName : String ){
	var pName : String;
	pName = useName;
	presetToggleSave = false;
	var workCol : Color;
	var pL : int = pName.Length;
	
	PresetGetData();
	
	//check name
	if (pName == "") pName = "my custom preset"+(presetDataArray.length+1);
	if (pL < 20) pName = pName.PadRight(20);
	if (pL > 20) pName = pName.Substring(0,20);

	//SET COLORS
	workCol = thisSuimonoObject.renderer.sharedMaterial.GetColor("_DepthColor");
	var useDepthCol : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = thisSuimonoObject.renderer.sharedMaterial.GetColor("_HighColor");
	var useHighCol : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = thisSuimonoObject.renderer.sharedMaterial.GetColor("_LowColor");
	var useLowCol : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = thisSuimonoObject.renderer.sharedMaterial.GetColor("_DepthColorR");
	var useDepthColR : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = thisSuimonoObject.renderer.sharedMaterial.GetColor("_DepthColorG");
	var useDepthColG : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = thisSuimonoObject.renderer.sharedMaterial.GetColor("_DepthColorB");
	var useDepthColB : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";	
	workCol = thisSuimonoObject.renderer.sharedMaterial.GetColor("_SpecColorH");
	var useSpecColorH : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = thisSuimonoObject.renderer.sharedMaterial.GetColor("_SpecColorL");
	var useSpecColorL : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = thisSuimonoObject.renderer.sharedMaterial.GetColor("_DynReflColor");
	var useDynRefCol : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = thisSuimonoObject.renderer.sharedMaterial.GetColor("_FoamColor");
	var useFoamCol : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = thisSuimonoObject.renderer.sharedMaterial.GetColor("_EdgeColor");
	var useEdgeCol : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = underwaterColor;
	var useUnderCol : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = tideColor;
	var useTideCol : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	
	
	//SET ATTRIBUTES
	var useMScale : String = "("+overallScale.ToString("00.000")+")";
	var useAbsorb : String = "("+lightAbsorb.ToString("00.000")+")";
	var useRefractAmt : String = "("+lightRefract.ToString("00.000")+")";
	var userefractShift : String = "("+refractShift.ToString("00.000")+")";
	var useblurSpread : String = "("+blurSpread.ToString("00.000")+")";
	var usesurfaceSmooth : String = "("+surfaceSmooth.ToString("00.000")+")";
	var usereflectDist : String = "("+reflectDist.ToString("00.000")+")";
	var usereflectSpread : String = "("+reflectSpread.ToString("00.000")+")";
	var usereflectionOffset : String = "("+reflectionOffset.ToString("00.000")+")";
	var useedgeBlend : String = "("+edgeBlend.ToString("00.000")+")";
	var usenormalShore : String = "("+normalShore.ToString("00.000")+")";
	var usespecScatterAmt : String = "("+specScatterAmt.ToString("00.000")+")";
	var usespecScatterWidth : String = "("+specScatterWidth.ToString("00.000")+")";
	var usehFoamHeight : String = "("+hFoamHeight.ToString("00.000")+")";
	var usehFoamAmt : String = "("+hFoamAmt.ToString("00.000")+")";
	var usehFoamSpread : String = "("+hFoamSpread.ToString("00.000")+")";
	var usefoamAmt : String = "("+foamAmt.ToString("00.000")+")";
	var usefoamScale : String = "("+foamScale.ToString("00.000")+")";
	var useedge : String = "("+edgeSpread.ToString("00.000")+")";
	var usedetailHeight : String = "("+detailHeight.ToString("00.000")+")";
	var usedetailScale : String = "("+detailScale.ToString("00.000")+")";
	var useUpdateSpeed : String = "("+UpdateSpeed.ToString("00.000")+")";
	var userippleSensitivity : String = "("+rippleSensitivity.ToString("00.000")+")";
	var usesplashSensitivity : String = "("+splashSensitivity.ToString("00.000")+")";
	var usereflectDistUnderAmt : String = "("+reflectDistUnderAmt.ToString("00.000")+")";
	var useunderRefractionAmount : String = "("+underRefractionAmount.ToString("00.000")+")";
	var useunderBlurAmount : String = "("+underBlurAmount.ToString("00.000")+")";
	var useetherealShift : String = "("+etherealShift.ToString("00.000")+")";
	var useunderwaterFogDist : String = "("+underwaterFogDist.ToString("00.000")+")";
	var useunderwaterFogSpread : String = "("+underwaterFogSpread.ToString("00.000")+")";																																						

	var usewaveHeight : String = "("+waveHeight.ToString("00.000")+")";	
	var usewaveShoreHeight : String = "("+waveShoreHeight.ToString("00.000")+")";	
	var usewaveScale : String = "("+waveScale.ToString("00.000")+")";	

	var usewaveShoreScale : String = "("+waveShoreScale.ToString("00.000")+")";
	var useshoreSpeed : String = "("+shoreSpeed.ToString("00.000")+")";

	enableUnderDebrisWrite = 0.0;
    if (enableUnderDebris) enableUnderDebrisWrite = 1.0;
    var useenableUnderDebris : String = "("+enableUnderDebrisWrite.ToString("00.000")+")";
    
	var useTideAmount : String = "("+tideAmount.ToString("00.000")+")";
	var useTideSpread : String = "("+tideSpread.ToString("00.000")+")";

	var useUnderRefractionScale : String = "("+underRefractionScale.ToString("00.000")+")";
	var useUnderRefractionSpeed : String = "("+underRefractionSpeed.ToString("00.000")+")";

	var usetypeIndex : String = "("+typeIndex.ToString("00.000")+")";
	var useWaveBreakAmt : String = "("+waveBreakAmt.ToString("00.000")+")";
	var useshallowFoamAmt : String = "("+shallowFoamAmt.ToString("00.000")+")";

	var useoverallBright : String = "("+overallBright.ToString("00.000")+")";
	var useoverallTransparency : String = "("+overallTransparency.ToString("00.000")+")";


	//SAVE DATA																																																																																																																									
	var saveData : String = pName+" "+useDepthCol+useHighCol+useLowCol+useDepthColR+useDepthColG+useDepthColB+useSpecColorH+useSpecColorL+useDynRefCol+useFoamCol+useEdgeCol+useUnderCol+useTideCol;
	saveData += useMScale+useAbsorb+useRefractAmt+userefractShift+"(00.000)"+useblurSpread+usesurfaceSmooth+usereflectDist+usereflectSpread+usereflectionOffset;
	saveData += useedgeBlend+usenormalShore+usespecScatterAmt+usespecScatterWidth+usehFoamHeight+usehFoamAmt+usehFoamSpread+usefoamAmt+usefoamScale+useedge;
	saveData += usedetailHeight+usedetailScale+useUpdateSpeed+userippleSensitivity+usesplashSensitivity+usereflectDistUnderAmt+useunderRefractionAmount+useunderBlurAmount;
	saveData += useetherealShift+useunderwaterFogDist+useunderwaterFogSpread+usewaveHeight+usewaveShoreHeight+usewaveScale;
	saveData += usewaveShoreScale+useshoreSpeed+useenableUnderDebris+useTideAmount+useTideSpread+useUnderRefractionScale+useUnderRefractionSpeed;
	
	//add padding for future variables
	saveData += usetypeIndex + useWaveBreakAmt + useshallowFoamAmt + useoverallBright + useoverallTransparency + "(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)";
	
	//check for already existing preset match and insert data
	var ckNme : boolean = false;
	var workData : String;
	var rName : String;
	var rL : int;
	for (var cx = 0; cx < (presetDataArray.length); cx++){
		workData = presetDataArray[cx];
		rName = workData.Substring(0,20);
		rL = rName.Length;
		if (rL < 20) rName = rName.PadRight(20);
		if (rL > 20) rName = rName.Substring(0,20);
		if (rName == pName){
			ckNme = true;
			presetDataArray[cx] = saveData;
			break;
		}
	}
	
	//save to file
	var fileName = baseDir+presetFile;
	var sw = new StreamWriter(Application.dataPath + "/" + fileName);
	sw.AutoFlush = true;
	for (var px = 0; px < (presetDataArray.length); px++){
		sw.Write(presetDataArray[px]);
		if (px != presetDataArray.length-1) sw.Write("\n");
	}
	if (ckNme == false){
		sw.Write("\n"+saveData);
	}
    sw.Close();
	Debug.Log("Preset '"+presetSaveName+"' has been saved!"); 
}









function PresetRename( oldName : String, newName : String ){
	var oName : String;
	oName = oldName;
	var nName : String;
	nName = newName;

	PresetGetData();
	
	//check name
	if (oName.Length < 20) oName = oName.PadRight(20);
	if (oName.Length > 20) oName = oName.Substring(0,20);
	if (nName.Length < 20) nName = nName.PadRight(20);
	if (nName.Length > 20) nName = nName.Substring(0,20);
	
	//check for already existing preset match and insert data
	var workData : String;
	var rName : String;
	for (var cx = 0; cx < (presetDataArray.length); cx++){
		workData = presetDataArray[cx];
		rName = workData.Substring(0,20);
		if (rName == oName){
			var repString : String = presetDataArray[cx];
			repString = nName + workData.Substring(20,(workData.length-20));
			presetDataArray[cx] = repString;
		}
	}
	
	//save to file
	var fileName = baseDir+presetFile;
	var sw = new StreamWriter(Application.dataPath + "/" + fileName);
	sw.AutoFlush = true;
	for (var px = 0; px < (presetDataArray.length); px++){
		sw.Write(presetDataArray[px]);
		if (px != presetDataArray.length-1) sw.Write("\n");
	}
    sw.Close();
	Debug.Log("Preset '"+oldName+"' has been renamed to "+newName+"!"); 
}











function PresetDelete( preName : String ){
	var oName : String;
	oName = preName;

	PresetGetData();
	
	//check name
	if (oName.Length < 20) oName = oName.PadRight(20);
	if (oName.Length > 20) oName = oName.Substring(0,20);
	var workData : String;
	var rName : String;

	//save to file
	var fileName = baseDir+presetFile;
	var sw = new StreamWriter(Application.dataPath + "/" + fileName);
	sw.AutoFlush = true;
	
	//remove line
	for (var px = 0; px < (presetDataArray.length); px++){
		workData = presetDataArray[px];
		rName = workData.Substring(0,20);
		if (rName != oName){
			sw.Write(presetDataArray[px]);
			if (px != presetDataArray.length-1) sw.Write("\n");
		}
	}
    sw.Close();
	Debug.Log("Preset '"+preName+"' has been deleted!"); 
}










function PresetDoTransition(){
	
	//waterStartState = currentState;
	presetTransitionCurrent += (Time.deltaTime/presetTransitionTime);
	
	//transition
	depthColor = Color.Lerp(PresetGetColor(presetTransIndexFrm,"_DepthColor"),PresetGetColor(presetTransIndexTo,"_DepthColor"),presetTransitionCurrent);
	colorSurfHigh = Color.Lerp(PresetGetColor(presetTransIndexFrm,"_HighColor"),PresetGetColor(presetTransIndexTo,"_HighColor"),presetTransitionCurrent);
	colorSurfLow = Color.Lerp(PresetGetColor(presetTransIndexFrm,"_LowColor"),PresetGetColor(presetTransIndexTo,"_LowColor"),presetTransitionCurrent);
	depthColorR = Color.Lerp(PresetGetColor(presetTransIndexFrm,"_DepthColorR"),PresetGetColor(presetTransIndexTo,"_DepthColorR"),presetTransitionCurrent);
	depthColorG = Color.Lerp(PresetGetColor(presetTransIndexFrm,"_DepthColorG"),PresetGetColor(presetTransIndexTo,"_DepthColorG"),presetTransitionCurrent);
	depthColorB = Color.Lerp(PresetGetColor(presetTransIndexFrm,"_DepthColorB"),PresetGetColor(presetTransIndexTo,"_DepthColorB"),presetTransitionCurrent);
	specColorH = Color.Lerp(PresetGetColor(presetTransIndexFrm,"_SpecColorH"),PresetGetColor(presetTransIndexTo,"_SpecColorH"),presetTransitionCurrent);
	specColorL = Color.Lerp(PresetGetColor(presetTransIndexFrm,"_SpecColorL"),PresetGetColor(presetTransIndexTo,"_SpecColorL"),presetTransitionCurrent);
	colorDynReflect = Color.Lerp(PresetGetColor(presetTransIndexFrm,"_DynReflColor"),PresetGetColor(presetTransIndexTo,"_DynReflColor"),presetTransitionCurrent);
	foamColor = Color.Lerp(PresetGetColor(presetTransIndexFrm,"_FoamColor"),PresetGetColor(presetTransIndexTo,"_FoamColor"),presetTransitionCurrent);
	edgeColor = Color.Lerp(PresetGetColor(presetTransIndexFrm,"_EdgeColor"),PresetGetColor(presetTransIndexTo,"_EdgeColor"),presetTransitionCurrent);
	underwaterColor = Color.Lerp(PresetGetColor(presetTransIndexFrm,"_UnderwaterColor"),PresetGetColor(presetTransIndexTo,"_UnderwaterColor"),presetTransitionCurrent);
	tideColor = Color.Lerp(PresetGetColor(presetTransIndexFrm,"_TideColor"),PresetGetColor(presetTransIndexTo,"_TideColor"),presetTransitionCurrent);
	
	lightAbsorb = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_LightAbsorb"),PresetGetFloat(presetTransIndexTo,"_LightAbsorb"),presetTransitionCurrent);
	lightRefract = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_LightRefract"),PresetGetFloat(presetTransIndexTo,"_LightRefract"),presetTransitionCurrent);
	refractShift = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_RefractShift"),PresetGetFloat(presetTransIndexTo,"_RefractShift"),presetTransitionCurrent);
	blurSpread = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_BlurSpread"),PresetGetFloat(presetTransIndexTo,"_BlurSpread"),presetTransitionCurrent);
	surfaceSmooth = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_SurfaceSmooth"),PresetGetFloat(presetTransIndexTo,"_SurfaceSmooth"),presetTransitionCurrent);
	reflectDist = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_ReflectDist"),PresetGetFloat(presetTransIndexTo,"_ReflectDist"),presetTransitionCurrent);
	reflectSpread = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_ReflectSpread"),PresetGetFloat(presetTransIndexTo,"_ReflectSpread"),presetTransitionCurrent);
	reflectionOffset = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_ReflectionOffset"),PresetGetFloat(presetTransIndexTo,"_ReflectionOffset"),presetTransitionCurrent);
	edgeBlend = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_EdgeBlend"),PresetGetFloat(presetTransIndexTo,"_EdgeBlend"),presetTransitionCurrent);
	normalShore = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_NormalShore"),PresetGetFloat(presetTransIndexTo,"_NormalShore"),presetTransitionCurrent);
	specScatterAmt = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_SpecScatterAmt"),PresetGetFloat(presetTransIndexTo,"_SpecScatterAmt"),presetTransitionCurrent);
	specScatterWidth = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_SpecScatterWidth"),PresetGetFloat(presetTransIndexTo,"_SpecScatterWidth"),presetTransitionCurrent);
	hFoamHeight = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_HFoamHeight"),PresetGetFloat(presetTransIndexTo,"_HFoamHeight"),presetTransitionCurrent);
	hFoamAmt = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_HFoamAmt"),PresetGetFloat(presetTransIndexTo,"_HFoamAmt"),presetTransitionCurrent);
	hFoamSpread = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_HFoamSpread"),PresetGetFloat(presetTransIndexTo,"_HFoamSpread"),presetTransitionCurrent);
	foamAmt = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_FoamAmt"),PresetGetFloat(presetTransIndexTo,"_FoamAmt"),presetTransitionCurrent);
	foamScale = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_FoamScale"),PresetGetFloat(presetTransIndexTo,"_FoamScale"),presetTransitionCurrent);
	edgeSpread = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_EdgeSpread"),PresetGetFloat(presetTransIndexTo,"_EdgeSpread"),presetTransitionCurrent);
	detailHeight = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_DetailHeight"),PresetGetFloat(presetTransIndexTo,"_DetailHeight"),presetTransitionCurrent);
	UpdateSpeed = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_UpdateSpeed"),PresetGetFloat(presetTransIndexTo,"_UpdateSpeed"),presetTransitionCurrent);
	rippleSensitivity = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_RippleSensitivity"),PresetGetFloat(presetTransIndexTo,"_RippleSensitivity"),presetTransitionCurrent);
	splashSensitivity = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_SplashSensitivity"),PresetGetFloat(presetTransIndexTo,"_SplashSensitivity"),presetTransitionCurrent);
	reflectDistUnderAmt = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_ReflectDistUnderAmt"),PresetGetFloat(presetTransIndexTo,"_ReflectDistUnderAmt"),presetTransitionCurrent);
	underRefractionAmount = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_UnderRefractionAmount"),PresetGetFloat(presetTransIndexTo,"_UnderRefractionAmount"),presetTransitionCurrent);
	underBlurAmount = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_UnderBlurAmount"),PresetGetFloat(presetTransIndexTo,"_UnderBlurAmount"),presetTransitionCurrent);
	etherealShift = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_EtherealShift"),PresetGetFloat(presetTransIndexTo,"_EtherealShift"),presetTransitionCurrent);
	underwaterFogDist = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_UnderwaterFogDist"),PresetGetFloat(presetTransIndexTo,"_UnderwaterFogDist"),presetTransitionCurrent);
	underwaterFogSpread = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_UnderwaterFogSpread"),PresetGetFloat(presetTransIndexTo,"_UnderwaterFogSpread"),presetTransitionCurrent);

	waveHeight = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_WaveHeight"),PresetGetFloat(presetTransIndexTo,"_WaveHeight"),presetTransitionCurrent);
	waveShoreHeight = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_WaveShoreHeight"),PresetGetFloat(presetTransIndexTo,"_WaveShoreHeight"),presetTransitionCurrent);
	waveShoreScale = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_WaveShoreScale"),PresetGetFloat(presetTransIndexTo,"_WaveShoreScale"),presetTransitionCurrent);
	shoreSpeed = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_ShoreSpeed"),PresetGetFloat(presetTransIndexTo,"_ShoreSpeed"),presetTransitionCurrent);

	tideAmount = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_TideAmount"),PresetGetFloat(presetTransIndexTo,"_TideAmount"),presetTransitionCurrent);
	tideSpread = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_TideSpread"),PresetGetFloat(presetTransIndexTo,"_TideSpread"),presetTransitionCurrent);
	underRefractionSpeed = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_UnderRefractionSpeed"),PresetGetFloat(presetTransIndexTo,"_UnderRefractionSpeed"),presetTransitionCurrent);
	
	waveBreakAmt = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_WaveBreakAmt"),PresetGetFloat(presetTransIndexTo,"_WaveBreakAmt"),presetTransitionCurrent);
	shallowFoamAmt = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_ShallowFoamAmt"),PresetGetFloat(presetTransIndexTo,"ShallowFoamAmt"),presetTransitionCurrent);
		

	overallBright = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_OverallBright"),PresetGetFloat(presetTransIndexTo,"_OverallBright"),presetTransitionCurrent);
	overallTransparency = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_OverallTransparency"),PresetGetFloat(presetTransIndexTo,"_OverallTransparency"),presetTransitionCurrent);
			
	
			
	//set final
	if (presetTransitionCurrent >= 1.0){
		//reset
		presetIndex = presetTransIndexTo;
		presetStartTransition = false;
		presetTransitionCurrent = 0.0;
	}	

}




function PresetGetData(){

	var fileName = baseDir+presetFile;
	var sr = new StreamReader(Application.dataPath + "/" + fileName);
    presetDataString = sr.ReadToEnd();
    sr.Close();

    presetDataArray = presetDataString.Split("\n"[0]);
	var workOptions = presetDataString.Split("\n"[0]);
	presetOptions = workOptions;
	
	for (var ax = 0; ax < (presetOptions.length); ax++){
		presetOptions[ax] = workOptions[ax].Substring(0,20);
		presetOptions[ax] = presetOptions[ax].Trim();
	}

}




function floatRound(inFloat : float){

	var retFloat : float = 0.0;
	retFloat = Mathf.Round(inFloat*1000.0)/1000.0;
	return retFloat;

}

