Shader "Suimono2/water_under_pro_fwd" {


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
Tags {"RenderType"="Transparent" "Queue"= "Overlay"}
Cull Front
Blend SrcAlpha OneMinusSrcAlpha
ZWrite On




CGPROGRAM
#pragma target 3.0
#include "SuimonoFunctions.cginc"
#pragma surface surf SuimonoUnderSurfaceFwd addshadow vertex:vertexSuimonoDisplace nolightmap
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


inline fixed4 LightingSuimonoUnderSurfaceFwd (SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
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
	
	//c.rgb = s.Albedo*lerp(diff,1.0,);
	c.rgb = s.Albedo;//*diff;//*lerp(diff,1.0,s.Alpha);
	c.rgb *= 0.15;
		
	c.a = diff*2.0;



	c.a *= lerp(c.a,0.0,s.Alpha);
	c.a = lerp(1.0,c.a,lerp(0.5,1.0,s.Alpha));
	

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
	float mask = (((IN.screenPos.w - _UnderReflDist))*0.02);
	if (mask > 1.0) mask = 1.0;
	if (mask < 0.0) mask = 0.0;
	float mask1 = ((IN.screenPos.w - 1600.0)*0.0005);
	if (mask1 > 1.0) mask1 = 1.0;
	if (mask1 < 0.0) mask1 = 0.0;
	float mask2 = ((IN.screenPos.w - 500.0)*0.0005);
	if (mask2 > 1.0) mask2 = 1.0;
	if (mask2 < 0.0) mask2 = 0.0;
	
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
	dynRef = refl.rgb*4.0;

	// switch in cube reflection if dynamic not available
	o.Albedo = lerp(cubeRef,dynRef,useReflection);

	// set alpha by distance
	o.Alpha = lerp(0.8,1.0,mask);


}
ENDCG









//-------------------
//    FOAM
//-------------------
Tags {"Queue"= "Transparent"}
Cull Front
Blend SrcAlpha OneMinusSrcAlpha
ZWrite On


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











}
FallBack "Diffuse"
}
