Shader "Suimono2/water_under_pro" {


Properties {

	_Tess ("Tessellation", Float) = 4.0
    _minDist ("TessMin", Range(-180.0, 0.0)) = 10.0
    _maxDist ("TessMax", Range(20.0, 500.0)) = 25.0
    _Displacement ("Displacement", Range(0, 8.0)) = 0.3
    _MaskAmt ("Mask Strength", Range(1, 8.0)) = 1.0

    _WaveLargeTex ("Wave Large", 2D) = "white" {}
   	_WaveHeight ("Wave Height", Range(0, 20.0)) = 0.0
   	_DetailHeight ("Detail Height", Range(0, 20.0)) = 0.0
   	_WaveShoreHeight ("Wave Shore Height", Range(0, 8.0)) = 0.0
   	_WaveScale ("Wave Scale", Range(0, 1.0)) = 0.25
	
	_CenterHeight ("Center Height", Float) = 0.0
	_MaxVariance ("Maximum Variance", Float) = 3.0

	_HighColor ("High Color", Color) = (1.0, 0.0, 0.0, 1.0)
	_LowColor ("Low Color", Color) = (0.0, 1.0, 0.0, 0.1)
		
	_Surface1 ("Surface Distortion 1", 2D) = "white" {}
	_Surface2 ("Surface Distortion 2", 2D) = "white" {}
	_WaveRamp ("Wave Ramp", 2D) = "" {}
	
	_RefrStrength ("Refraction Strength (0.0 - 25.0)", Float) = 25.0
    _RefrSpeed ("Refraction Speed (0.0 - 0.5)", Float) = 0.5
    _RefrScale ("Refraction Scale", Float) = 0.5
	
	_SpecScatterWidth ("Specular Width", Range(1.0,10.0)) = 2.0
	_SpecScatterAmt ("Specular Scatter", Range(0.0001,0.05)) = 0.02
	_SpecColorH ("Hot Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	_SpecColorL ("Reflect Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	
	_DynReflColor ("Reflection Dynamic", Color) = (1.0, 1.0, 1.0, 0.5)
	_ReflDist ("Reflection Distance", Float) = 1000
	_ReflBlend ("Reflection Blend", Range(0.002,0.1)) = 0.01
	_ReflBlur ("Reflection Blur", Range (0.0, 0.125)) = 0.01
	_ReflectionTex ("Reflection", 2D) = "white" {}

	_DepthAmt ("Depth Amount", Float) = 0.1
	
	_DepthColor ("Depth Over Tint", Color) = (0.25,0.25,0.5,1.0)
	_DepthColorR ("Depth Color 1(r)", Color) = (0.25,0.25,0.5,1.0)
	_DepthColorG ("Depth Color 2(g)", Color) = (0.25,0.25,0.5,1.0)
	_DepthColorB ("Depth Color 3(b)", Color) = (0.25,0.25,0.5,1.0)
	_DepthRamp ("Depth Color Ramp", 2D) = "white" {}
	
	_BlurSpread ("Blur Spread", Range (0.0, 0.125)) = 0.01
	_BlurRamp ("Blur Ramp", 2D) = "white" {}
	
	_FoamHeight ("Foam Height", Float) = 5.0
	_HeightFoamAmount ("Height Foam Amount", Range (0.0, 1.0)) = 1.0
	_HeightFoamSpread ("Height Foam Spread", Float) = 2.0
	
	_FoamSpread ("Foam Spread", Range (0.0, 1.0)) = 0.5
	_FoamColor ("Foam Color", Color) = (1,1,1,1)
	_FoamRamp ("Foam Ramp", 2D) = "white" {}
	_FoamOverlay ("Foam Overlay (RGB)", 2D) = "white" {}
	_FoamTex ("Foam Texture (RGB)", 2D) = "white" {}

	_EdgeBlend ("Edge Spread", Range (0.04,5.0)) = 10.0
	_EdgeSpread ("Edge Spread", Range (0.04,5.0)) = 10.0
	_EdgeColor ("Edge Color", Color) = (1,1,1,1)
	
	_BumpStrength ("Normal Strength", Float) = 0.9
	_ReflectStrength ("Reflection Strength", Float) = 1.0
		
	_CubeMobile ("Cubemap Mobile", CUBE) = "white" {}
    
	_MasterScale ("Master Scale", Float) = 1.0
	_UnderReflDist ("Under Reflection", Float) = 1.0
	_UnderColor ("Underwater Color", Color) = (0.25,0.25,0.5,1.0)
	
	_WaveTex ("_WaveTex", 2D) = "white" {}
	_FlowMap ("_FlowMap", 2D) = "white" {}
	_FlowScale ("Flowmap Scale", Range(0.1,10.0)) = 0.0

	_TideColor ("Tide Color", Color) = (0.0,0.0,0.2,1.0)
	_TideAmount ("Tide Amount", Range(0.0,1.0)) = 1.0
	_TideSpread ("Tide Amount", Range(0.02,1.0)) = 0.4

	_WaveMap ("_WaveMap", 2D) = "white" {}
	
	_Ramp2D ("_BRDF Ramp", 2D) = "white" {}
	_RimPower ("RimPower", Range(0.01,10.0)) = 1.0
		
	
}



Subshader 
{ 

























// ---------------------------------
//   SURFACE REFLECTIONS
// ---------------------------------
Tags {"RenderType"="Transparent" "Queue"= "Transparent"}
Cull Front
Blend SrcAlpha OneMinusSrcAlpha
ZWrite Off




CGPROGRAM
#pragma target 3.0
#include "SuimonoFunctions.cginc"
#pragma surface surf SuimonoUnderSurface addshadow vertex:vertexSuimonoDisplace nolightmap
#pragma glsl


float4 _DepthColor;
float4 _DepthColorR;
float4 _DepthColorG;
float4 _DepthColorB;
float _DepthAmt;
float _SpecScatterWidth;
float _SpecScatterAmt;
float _RimPower;
sampler2D _Ramp2D;
float4 _SpecColorH;
float4 _SpecColorL;
float4 _DynReflColor;
float4 _UnderColor;
float _OverallTrans;


inline fixed4 LightingSuimonoUnderSurface (SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
{
	half3 h = normalize (lightDir + viewDir);
	
	float NdotL = dot(s.Normal, lightDir);
	float NdotE = dot(s.Normal, viewDir);
	fixed NdotH = max(0, dot(s.Normal, h));
	float NdotS = dot(lightDir, viewDir);
	float NdotV = dot(h, viewDir*6.0);
	
	fixed diff = max (0, dot (s.Normal, fixed3(-lightDir.x,lightDir.y,lightDir.z)));
	
	float nh = max (0, dot (s.Normal, diff));
	float spec = saturate(pow(nh,  _SpecScatterAmt));
	float spec2 = saturate(pow(nh,  _SpecScatterWidth)*150.0);
	
	fixed4 c;
	//c.rgb = _UnderColor.rgb*0.3;
	//c.rgb = lerp(c.rgb,s.Albedo,1.0-s.Alpha)*diff;
	//if (spec * s.Alpha * 1.0 > 0.05) c.rgb = lerp(c.rgb,_UnderColor.rgb*0.3,lerp(0.25,1.0,s.Alpha*0.5));
	//c.a = ((s.Alpha) + (spec2 * 2.0)) * lerp(0.4,1.0,s.Alpha*0.5);
	
	//c.a = saturate(c.a);
	//c.a *= saturate(min(NdotV,s.Gloss));
	//c.a *= (s.Gloss);

	c.rgb = s.Albedo*diff;
	c.a = diff*2.0;
	
	c = saturate(c);
	
	c.a *= _OverallTrans;
	
	
	return c;
}


struct Input {
	float4 screenPos;
	float2 uv_Surface1;
	float2 uv_Surface2;
	float2 uv_WaveLargeTex;  
	float3 worldRefl;
    INTERNAL_DATA
};


sampler2D _CameraDepthTexture;
float _ReflBlend;
float _ReflDist;
float _ReflectStrength; 
float useReflection;
float _UnderReflDist;
float _EdgeBlend;
sampler2D _ReflectionTex;
samplerCUBE _CubeMobile;

void surf (Input IN, inout SurfaceOutput o) {
	 
	//calculate distance masks
	float mask = (((IN.screenPos.w + _UnderReflDist))*0.1);
	if (mask > 1.0) mask = 1.0;
	if (mask < 0.0) mask = 0.0;
	float mask1 = ((IN.screenPos.w - 1600.0)*0.0005);
	if (mask1 > 1.0) mask1 = 1.0;
	if (mask1 < 0.0) mask1 = 0.0;

	// calculate wave height and roughness
	half waveHeight = (_WaveHeight/10.0);
	half detailHeight = (_DetailHeight/3.0);
	
	// calculate normals
	half4 d1 = tex2D(_WaveLargeTex,IN.uv_Surface1);
	half4 n1 = tex2D(_WaveLargeTex,IN.uv_WaveLargeTex);
	half4 n2 = tex2D(_WaveLargeTex,IN.uv_WaveLargeTex*14.0);

	o.Normal = lerp(half3(0,0,1),UnpackNormal(d1),saturate(lerp(0.0,2.0,_BumpStrength)));
	o.Normal += UnpackNormal(n1) * saturate(lerp(0.0,2.0,_BumpStrength));
	o.Normal -= UnpackNormal(n2) * saturate(lerp(-1.0,1.0,_BumpStrength));
	o.Normal = normalize(o.Normal);
	
	
	// decode cube / mobile reflection
	float nUV = 1.0;
	nUV += (o.Normal.y*_ReflectStrength*1.5)-(1.0*_ReflectStrength*0.25);
	half3 cubeRef = half3(0,0,1);
	cubeRef = texCUBE(_CubeMobile, WorldReflectionVector (IN, o.Normal)*float4(1.0,nUV,1.0,1.0)).rgb; 
	cubeRef = lerp(_DynReflColor.rgb,(cubeRef.rgb*_DynReflColor.rgb*_DynReflColor.a),_DynReflColor.a);
	
	
	// decode dynamic reflection
	float4 uv1 = IN.screenPos; uv1.xy;
	uv1.y += (o.Normal.y*4.0*_BumpStrength*1.5)-(1.0*4.0*_BumpStrength*0.25);
	half4 refl = tex2Dproj( _ReflectionTex, UNITY_PROJ_COORD(uv1));
	
	// calculate dynamic reflection colors
	half3 dynRef = lerp(_DynReflColor.rgb,(refl.rgb*2.5*_DynReflColor.rgb*_DynReflColor.a),_DynReflColor.a);
	dynRef = refl.rgb;

	// switch in cube reflection if dynamic not available
	o.Albedo = lerp(cubeRef,dynRef,useReflection);

	// set alpha by distance
	o.Alpha = mask;

	
	//set edge alpha
	half depth = UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)));
	depth = LinearEyeDepth(depth); 
	float4 edgeFade = float4(1.0,_EdgeBlend,0.0,0.0);
	float4 edgeBlendFactors = float4(1.0, 0.0, 0.0, 0.0);
	edgeBlendFactors = saturate(edgeFade * (depth-IN.screenPos.w));		
	float4 edgeSpread = (half4(1,0,0,1) - (edgeFade.w) * float4(0.15, 0.03, 0.01, 0.0));
	edgeSpread.a = (edgeBlendFactors.y);
	edgeSpread.a = saturate(1.0-edgeSpread.a);
	o.Gloss = mask;//edgeSpread.a;

}
ENDCG

















//-------------------
//    FOAM
//-------------------
Tags {"Queue"= "Transparent"}
Cull Front
Blend SrcAlpha OneMinusSrcAlpha
ZWrite Off







CGPROGRAM
#pragma target 3.0
#include "SuimonoFunctions.cginc"
#pragma surface surf Lambert addshadow vertex:vertexSuimonoDisplace nolightmap noambient
#pragma glsl
#include <UnityCG.cginc>


float4 _DepthColor;
float4 _DepthColorR;
float4 _DepthColorG;
float4 _DepthColorB;
float _DepthAmt;
float _SpecScatterWidth;
float _SpecScatterAmt;
float _RimPower;
sampler2D _Ramp2D;
float4 _SpecColorH;
float4 _SpecColorL;



struct Input {
	float2 uv_FoamTex;
	float2 uv_FoamOverlay;
	float2 uv_Surface1;
	float2 uv_WaveLargeTex;
	float2 uv_WaveTex;
	float2 uv_FlowMap;
	float4 screenPos;
	float4 color : Color;
};



float _FoamHeight;
float _HeightFoamAmount;
float _HeightFoamSpread;
sampler2D _WaveRamp;
float4 _FoamColor;
float4 _EdgeColor;
sampler2D _FoamTex;
sampler2D _CameraDepthTexture;
float _EdgeSpread;
float _FoamSpread;
sampler2D _FoamRamp;
sampler2D _FoamOverlay;


void surf (Input IN, inout SurfaceOutput o) {

	float4 foamFade = float4(1.0,_FoamSpread,0.0,0.0);
	float4 edgeBlendFactors = float4(1.0, 0.0, 0.0, 0.0);
	half depth = UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)));
	depth = LinearEyeDepth(depth); 
	
	edgeBlendFactors = saturate(foamFade * (depth-IN.screenPos.w));		
	float4 foamSpread = (_FoamColor - (foamFade.w) * float4(0.15, 0.03, 0.01, 0.0));
	foamSpread.a = (edgeBlendFactors.y);
	foamSpread.a = saturate(1.0-foamSpread.a);

	float4 foamFade2 = float4(1.0,_EdgeSpread,0.0,0.0);
	float4 edgeBlendFactors2 = float4(1.0, 0.0, 0.0, 0.0);
	edgeBlendFactors2 = saturate(foamFade2 * (depth-IN.screenPos.w));		
	float4 foamSpread2 = (_FoamColor - (foamFade2.w) * float4(0.15, 0.03, 0.01, 0.0));
	foamSpread2.a = 1.0-edgeBlendFactors2.y;
	half fspread2 = saturate(foamSpread2.a);
	

	// calculate normals
	half4 d1 = tex2D(_WaveLargeTex,IN.uv_Surface1);
	half4 n1 = tex2D(_WaveLargeTex,IN.uv_WaveLargeTex);
	half4 n2 = tex2D(_WaveLargeTex,IN.uv_WaveLargeTex*14.0);
	//o.Normal = UnpackNormal(d1);
	//o.Normal += UnpackNormal(n1);
	//o.Normal += UnpackNormal(n2) *_BumpStrength;
	//o.Normal = normalize(o.Normal); //final detail
	//o.Normal.z = lerp(0.9,1.1,o.Normal.z);
	//o.Normal = lerp(half3(0,1,0),o.Normal,_BumpStrength);
		
	//init foam components
	half4 getflowmap = tex2D(_FlowMap,IN.uv_FlowMap);
	half3 texwave = tex2D(_WaveMap, IN.uv_FlowMap).rgb;
	half4 foamTexture = tex2D(_FoamTex, IN.uv_FoamTex);
	half4 foamTextureb = tex2D(_FoamTex, half2(IN.uv_FoamTex.x*0.1,IN.uv_FoamTex.y*0.1));
	half4 foamTexturec = tex2D(_FoamTex, half2(IN.uv_FoamTex.x*2.5,IN.uv_FoamTex.y*2.5));
	half4 foamOverlay = tex2D(_FoamOverlay, IN.uv_FoamOverlay*2.0);
	half alphaCalc = 0.0;

	//new height foam
	half4 w1 = tex2D(_Surface1,IN.uv_WaveLargeTex);
	half4 w2 = tex2D(_Surface1,IN.uv_Surface1);
	half hfoam = lerp(0.0,1.0,(w2.r + (w1.r * (w2.r*_HeightFoamSpread)) *1.5))*_HeightFoamAmount * (_WaveHeight/(_FoamHeight*10.0));
		
	//edge foam
	half alphaCalc2 = 0.0;
	half fspread3 = saturate(foamSpread.a+((getflowmap.g * 0.3))*texwave.g + hfoam);
	alphaCalc2 = lerp(0.0,foamTexture.b,tex2D(_FoamRamp, float2(1.0-fspread3, 0.5)).b);
	alphaCalc2 += lerp(0.0,foamTexture.r,tex2D(_FoamRamp, float2(1.0-fspread3, 0.5)).r);
	alphaCalc2 += lerp(0.0,foamTexture.g,tex2D(_FoamRamp, float2(1.0-fspread3, 0.5)).g);
	if (alphaCalc2 > 1.0) alphaCalc2 = 1.0;
	alphaCalc2 *= _FoamColor.a;
	alphaCalc2 *= tex2D(_FoamRamp, float2(1.0-fspread3, 0.5)).a * 2.0;
	if (alphaCalc2 >= 0.99) alphaCalc2 = 0.99;
	if (alphaCalc2 < 0.0) alphaCalc2 = 0.0;
	
	//shore foam
	half edgeFac = lerp(-10.0,1.0,getflowmap.r)*0.5;
	if (edgeFac < 0.0) edgeFac = 0.0;
	
	//edge line
	half edgePos = ((fspread2) * _EdgeColor.a);

	//final color and alpha
	o.Alpha = (saturate(alphaCalc2)*foamOverlay.a)+(edgePos);
	o.Albedo = lerp(_FoamColor.rgb*0.5,foamOverlay.rgb*_FoamColor.rgb*2.0,o.Alpha);
	o.Albedo = lerp(o.Albedo,_EdgeColor.rgb*1.4,edgePos);
	
	o.Specular = 0.05;
	o.Gloss = 1.0;
	_SpecColor.rgb = _FoamColor.rgb*0.8;
	
			
}
ENDCG











// -------------------------------------
//   UNDERWATER REFRACTION and BLURRING 
// -------------------------------------
GrabPass {
	Tags {"Queue" = "Transparent"}
	Name "BlurGrab"
}

//Pass{
	Tags {"Queue"= "Transparent"}
	Cull Front
	//Blend SrcAlpha OneMinusSrcAlpha
	ZWrite on
	



CGPROGRAM
#pragma target 3.0
#include "SuimonoFunctions.cginc"
#pragma surface surf Lambert addshadow vertex:vertexSuimonoDisplace nolightmap
#pragma glsl


struct Input {
	float4 screenPos;
	float2 uv_Surface1;
	float2 uv_Surface2;
	float2 uv_WaveLargeTex;  
	float3 worldRefl;
    INTERNAL_DATA
};


sampler2D _GrabTexture;

float4 _GrabTexture_TexelSize;
float _BlurSpread;
float _RefrStrength;
sampler2D _CameraDepthTexture;
sampler2D _BlurRamp;
float _DepthAmt;
float _EdgeBlend;
float _RefrShift;
float _isForward;
float _isMac;
float _OverallTrans;


void surf (Input IN, inout SurfaceOutput o) {


	//calculate edge alpha
	half depth = UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)));
	depth = LinearEyeDepth(depth); 
	float4 edgeFade = float4(1.0,_EdgeBlend,0.0,0.0);
	float4 edgeBlendFactors = float4(1.0, 0.0, 0.0, 0.0);
	edgeBlendFactors = saturate(edgeFade * (depth-IN.screenPos.w));		
	float4 edgeSpread = (half4(1,0,0,1) - (edgeFade.w) * float4(0.15, 0.03, 0.01, 0.0));
	edgeSpread.r = saturate(edgeBlendFactors.y);
	edgeSpread.a = saturate(0.1+edgeBlendFactors.y);
	
	// calculate normals
	half4 d1 = tex2D(_WaveLargeTex,IN.uv_Surface1);
	half4 n1 = tex2D(_WaveLargeTex,IN.uv_WaveLargeTex);
	half4 n2 = tex2D(_WaveLargeTex,IN.uv_WaveLargeTex*14.0);

	half3 useNormal;
	useNormal = lerp(half3(0,0,1),UnpackNormal(d1),saturate(lerp(0.0,2.0,_BumpStrength)));
	useNormal += UnpackNormal(n1) * saturate(lerp(0.0,2.0,_BumpStrength));
	useNormal += UnpackNormal(n2) * saturate(lerp(-1.0,1.0,_BumpStrength)) * 0.16;
	useNormal = normalize(useNormal);


	//calculate texture and displace
	float4 uvs = IN.screenPos;
	
	if (_isForward == 1.0 && _isMac == 0.0){
		uvs.y = uvs.w - uvs.y;
	}
	
	float4 uv1 = uvs;
	float4 uv2 = IN.screenPos;
	float4 uvx = IN.screenPos;
		uv1.y -= (useNormal.y*_RefrStrength*edgeSpread.a);
		uvx.y -= (useNormal.y*_RefrStrength*edgeSpread.a);


	//calculated distorted depth
	half4 depth1 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(uv1.x, uv1.y, uv1.z,uv1.w))).r;
	half rShift = _RefrShift * (lerp(0.0,2.0,useNormal.y) * _RefrStrength * edgeSpread.a);
	depth1.r += tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(uvx.x+rShift, uvx.y, uvx.z,uvx.w))).r;
	depth1.r += tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(uvx.x-rShift, uvx.y, uvx.z,uvx.w))).b;
	
	//calculate distorted color
	half4 oCol = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x, uv1.y, uv1.z,uv1.w))) * 1.4;
	oCol.r = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x+rShift, uv1.y, uv1.z,uv1.w))).r * 1.4;
	oCol.b = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x-rShift, uv1.y, uv1.z,uv1.w))).b * 1.4;
	oCol.a = 1.0;

	//calculated original depth
	half4 depth2 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(uv2.x, uv2.y, uv2.z,uv2.w))).r;
	depth2.r += tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(uv2.x+rShift, uv2.y, uv2.z,uv2.w))).r;
	depth2.r += tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(uv2.x-rShift, uv2.y, uv2.z,uv2.w))).b;
	
	//calculate original color
	half4 oCol2 = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(uv1)) * 1.4;

	//calculate blur texture
	//half blur = 1.0;
	//half4 xCol = oCol;//half4(0,0,0,0);
	//half res = 1.0;
	//int blursamples = 30;
	//int divsamples = blursamples-1;
	//if (_BlurSpread > 0.0){
	//	for(int i=1; i < blursamples ; i++){
	//		res = 0.004 * i * edgeSpread.r;
	//		xCol += (tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x-res*blur, uv1.y-res*blur, uv1.z,uv1.w)))*(1.0/divsamples)*0.125);
	//		xCol += (tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x+res*blur, uv1.y-res*blur, uv1.z,uv1.w)))*(1.0/divsamples)*0.125);
	//		xCol += (tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x-res*blur, uv1.y+res*blur, uv1.z,uv1.w)))*(1.0/divsamples)*0.125);
	//		xCol += (tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x+res*blur, uv1.y+res*blur, uv1.z,uv1.w)))*(1.0/divsamples)*0.125);
		
	//		xCol += (tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x, uv1.y-res*blur, uv1.z,uv1.w)))*(1.0/divsamples)*0.125);
	//		xCol += (tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x, uv1.y+res*blur, uv1.z,uv1.w)))*(1.0/divsamples)*0.125);
	//		xCol += (tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x+res*blur, uv1.y, uv1.z,uv1.w)))*(1.0/divsamples)*0.125);
	//		xCol += (tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x-res*blur, uv1.y, uv1.z,uv1.w)))*(1.0/divsamples)*0.125);
	//	}
	//	xCol = saturate(xCol*1.2);
	//}
	
	o.Gloss = 0.0;
	o.Specular = 0.0;
	
	//blend normal texture and blur texture
	half useAlpha = 1.0;
	half3 useAlbedo = oCol.rgb;//lerp(oCol.rgb,xCol.rgb,_BlurSpread);
	
	if ((depth2.r - depth1.r) > 0.05){
		useAlbedo = oCol2.rgb;
	}

	o.Albedo = useAlbedo;
	o.Alpha = useAlpha;

	o.Alpha *= _OverallTrans;
	
}

ENDCG










}
FallBack "Diffuse"
}
