#pragma strict

var generateOnStart : boolean = true;
var generateMap : boolean = false;

var autoGenerateFPS : float = 0.0;

var resolutionScale : int = 50;

var shoreRange : float = 3.0;
var waveRange : float = 10.0;
var detectLayers : LayerMask;

var shoreMapTex : Texture2D;
var parentMesh : Mesh;

private var autoTimer : float = 0.0;
//private var waveObject : Suimono_waveGenerator;

private var renderObject : GameObject;

function Start () {
	
	//get parent object and mesh
	renderObject = this.transform.parent.gameObject.Find("Suimono_Object").gameObject;
	if (renderObject.gameObject.GetComponent(MeshFilter)){
		parentMesh = renderObject.gameObject.GetComponent(MeshFilter).sharedMesh;
	}
	
	//waveObject = transform.parent.gameObject.Find("Suimono_waveObject").gameObject.GetComponent(Suimono_waveGenerator);

	if (generateOnStart) Generate2();

	//InvokeRepeating("UnloadTex",30.0,60.0);
	
}

function Update () {

	shoreRange = Mathf.Clamp(shoreRange,-0.0,1000.0);
	waveRange = Mathf.Clamp(waveRange,0.0,1000.0);
	
	if (autoGenerateFPS > 0.0){
		autoTimer += Time.deltaTime;
		if (autoTimer >= autoGenerateFPS){
			autoTimer = 0.0;
			generateMap = true;
		}
	}
	
	if (generateMap){
		generateMap = false;
		Generate2();
	}

}




function ReGenerate(){
	generateMap = true;
}



function Generate2(){

	if (parentMesh){
		
		var vertices : Vector3[] = parentMesh.vertices;
		var ppos : Vector2[]  = new Vector2[vertices.Length];
		var wcolors = new Color[resolutionScale*resolutionScale];
		var scolors = new float[vertices.Length];
		var bounds : Bounds = parentMesh.bounds;

		var sideLength :int = Mathf.Floor(Mathf.Sqrt(vertices.Length));
		var meshWidth : int = sideLength;
		var meshHeight : int = sideLength;
	
		shoreMapTex = null;
		shoreMapTex = new Texture2D(resolutionScale, resolutionScale);
		
		//get pixel positions
		var ht : RaycastHit;
		var setDistance : float = 0.0;
		var setDistance2 : float = 0.0;	
		
		var i : int = 0;
		var startPos : Vector3;
		startPos.x = transform.parent.transform.position.x - (transform.parent.localScale.x*20.0);
		startPos.z = transform.parent.transform.position.z - (transform.parent.localScale.z*20.0);
		
		var scaleAmtX : float = (transform.parent.localScale.x*40.0)/resolutionScale;
		var scaleAmtY : float = (transform.parent.localScale.z*40.0)/resolutionScale;
			
		for (var xP = 0; xP < resolutionScale; xP++){
		for (var yP = 0; yP < resolutionScale; yP++){

			var testPos : Vector3;
			testPos.x = startPos.x + (xP * scaleAmtX);
			testPos.z = startPos.z + (yP * scaleAmtY);
			testPos.y = transform.parent.transform.localPosition.y;
		
			if (Physics.Raycast (testPos, -Vector3.up, ht,1000.0, detectLayers)) {
				
				setDistance = ht.distance/waveRange;
				setDistance = Mathf.Clamp(setDistance,0.0,1.0);
				setDistance = 1.0-setDistance;

				setDistance2 = ht.distance/shoreRange;
				setDistance2 = Mathf.Clamp(setDistance2,0.0,1.0);
				setDistance2 = 1.0-setDistance2;
				
				//if (ht.transform.gameObject.GetComponent(fx_waveInfluencer) != null) useWaveInfluencer = ht.transform.gameObject.GetComponent(fx_waveInfluencer);
				//if (useWaveInfluencer != null) isFXObject = true;
			}
			
			
			shoreMapTex.SetPixel(resolutionScale-xP,resolutionScale-yP, Color(setDistance,setDistance2,0,1));
			//wcolors[i] = Color(setDistance,0.0,0.0,1.0);
			i += 1;
			
			if (i >= 10000){
				i=0;
				//yield;
			}
		}
		}
		//Debug.Log("length:"+transform.parent.localScale.x*4.0+" scale:"+scaleAmtX);
		
		//shoreMapTex.SetPixels(wcolors,0);
		
		//assign colors
		//if (!isFXObject){
		
		//	wcolors[i] = setDistance;

		//} else {

			//var useInf : float = Mathf.Lerp(0.0,1.0,(setDistance*3.0));
			//if (useWaveInfluencer.FlowType == Suimono_FX_FlowType.outward) useOutward = true;
			//if (useOutward){
			//	scolors[i] = (1.0-setDistance) * useInf;
			//} else {
			//	scolors[i] = setDistance;
			//}
			
		//}
		
		//scolors[i] += setDistance2;
		
		//wcolors[i] = Mathf.Clamp(wcolors[i],0.0,1.0);
		//scolors[i] = Mathf.Clamp(scolors[i],0.0,1.0);
		
		//shoreMapTex.SetPixel((sideLength*0.5)-ppos[i].x*1.3, (sideLength*0.5)-ppos[i].y*1.3, Color(wcolors[i],scolors[i],0,1));
		//shoreMapTex.SetPixel((sideLength*0.5)-ppos[i].x*1.3, (sideLength*0.5)-ppos[i].y*1.3, Color(wcolors[i],scolors[i],0,1));
		

		//apply all SetPixel calls
		shoreMapTex.Apply(false,false);
		
		//set texture to renderer position
		if (renderObject.renderer){
			shoreMapTex.wrapMode = TextureWrapMode.Clamp;
			renderObject.renderer.sharedMaterial.SetTexture("_FlowMap",shoreMapTex);
			renderObject.renderer.sharedMaterial.SetTextureScale("_FlowMap",Vector2(1.0,1.0));
			//if (waveObject != null) waveObject.maskMapTex = shoreMapTex;
		}
	}
	
	
}

















function Generate(){

	if (parentMesh){
		
		var vertices : Vector3[] = parentMesh.vertices;
		var ppos : Vector2[]  = new Vector2[vertices.Length];
		var wcolors = new float[vertices.Length];
		var scolors = new float[vertices.Length];
		var bounds : Bounds = parentMesh.bounds;

		var sideLength :int = Mathf.Floor(Mathf.Sqrt(vertices.Length));
		var meshWidth : int = sideLength;
		var meshHeight : int = sideLength;
	
		shoreMapTex = null;
		shoreMapTex = new Texture2D(meshWidth,meshHeight);
		
		//get pixel positions
		var ht : RaycastHit;
		var tstpos : Vector3;
		var tstpos2 : Vector3;
		var setDistance : float = 0.0;
		var setDistance2 : float = 0.0;	
		
		for (var i = 0; i < vertices.Length; i++){
			ppos[i] = Vector2 (vertices[i].x*0.98,vertices[i].z*0.98);
			
			//get heights
			tstpos = transform.parent.transform.TransformPoint(Vector3(ppos[i].x,0.0,ppos[i].y));
			tstpos2 = transform.parent.transform.TransformPoint(Vector3(ppos[i].x,(0.0-(transform.parent.transform.position.y*2.0)),ppos[i].y));
			
			setDistance = 1.0;
			setDistance2 = 1.0;
			var isFXObject : boolean = false;
			var useWaveInfluencer : fx_waveInfluencer = null;
			var useOutward : boolean = false;
			
			if (Physics.Raycast (tstpos, -Vector3.up, ht,1000.0, detectLayers)) {
				
				setDistance = ht.distance/waveRange;
				setDistance = Mathf.Clamp(setDistance,0.0,1.0);
				setDistance = 1.0-setDistance;
				//if (ht.point.y <= transform.parent.transform.localPosition.y) setDistance = 0.0;
				
				setDistance2 = ht.distance/shoreRange;
				setDistance2 = Mathf.Clamp(setDistance,0.0,1.0);
				setDistance2 = 1.0-setDistance2;
				
				if (ht.transform.gameObject.GetComponent(fx_waveInfluencer) != null) useWaveInfluencer = ht.transform.gameObject.GetComponent(fx_waveInfluencer);
				if (useWaveInfluencer != null) isFXObject = true;
			}


			//assign colors
			if (!isFXObject){
			
				wcolors[i] = setDistance;

			} else {

				var useInf : float = Mathf.Lerp(0.0,1.0,(setDistance*3.0));
				if (useWaveInfluencer.FlowType == Suimono_FX_FlowType.outward) useOutward = true;
				if (useOutward){
					scolors[i] = (1.0-setDistance) * useInf;
				} else {
					scolors[i] = setDistance;
				}
				

			}
			
			//scolors[i] += setDistance2;
			
			wcolors[i] = Mathf.Clamp(wcolors[i],0.0,1.0);
			scolors[i] = Mathf.Clamp(scolors[i],0.0,1.0);
			
			shoreMapTex.SetPixel((sideLength*0.5)-ppos[i].x*1.3, (sideLength*0.5)-ppos[i].y*1.3, Color(wcolors[i],scolors[i],0,1));

		}

		
		//apply all SetPixel calls
		shoreMapTex.Apply(false,false);
		
		//resize texture
		//--no code here yet --
		//
		
		//set texture to renderer position
		if (renderObject.renderer){
			shoreMapTex.wrapMode = TextureWrapMode.Clamp;
			renderObject.renderer.sharedMaterial.SetTexture("_FlowMap",shoreMapTex);
			renderObject.renderer.sharedMaterial.SetTextureScale("_FlowMap",Vector2(1.0,1.0));
			//if (waveObject != null) waveObject.maskMapTex = shoreMapTex;
		}
	}
}




function UnloadTex(){
	
	//unloads unused textures to conserve memory
	//EditorUtility.UnloadUnusedAssetsIgnoreManagedReferences();

}