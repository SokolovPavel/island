Shader "Suimono2/water_under_android" {



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
	_WaveRamp ("Wave Ramp", 2D) = "white" {}
	
	_RefrStrength ("Refraction Strength (0.0 - 25.0)", Float) = 25.0
    _RefrSpeed ("Refraction Speed (0.0 - 0.5)", Float) = 0.5
    _RefrScale ("Refraction Scale", Float) = 0.5
	
	_SpecScatterWidth ("Specular Width", Range(1.0,10.0)) = 2.0
	_SpecScatterAmt ("Specular Scatter", Range(0.0001,0.05)) = 0.02
	_SpecColorH ("Hot Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	_SpecColorL ("Reflect Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	
	_DynReflColor ("Reflection Dynamic", Color) = (1.0, 1.0, 1.0, 0.5)
	_ReflDist ("Reflection Distance", Float) = 1000.0
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
		
	_CubeTex ("Cubemap", CUBE) = "white" {}
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
ZWrite On


CGPROGRAM
#pragma target 3.0
#pragma surface surf SuimonoSurfaceMobile 
#pragma glsl

		
float _Tess;
float _minDist;
float _maxDist;

sampler2D _WaveMaskTex;
sampler2D _Surface1;
sampler2D _Surface2;
sampler2D _WaveLargeTex;
float _Displacement;
float _BumpStrength;
float _dScaleX;
float _dScaleY;
float _Phase;
float _WaveHeight;
float _WaveShoreHeight;
float _WaveScale;
float _WaveShoreScale;
float _MaskAmt;
float _ShoreAmt;
float _TimeX;
float _TimeY;
float _DTimeX;
float _DTimeY;
float _DTimeX2;
float _DTimeY2;
float _DetailHeight;

sampler2D _WaveMap;
sampler2D _WaveTex;
sampler2D _FlowMap;
float _MasterScale;
float _FlowScale;
float _FlowShoreScale;
float halfCycle;
float flowMapOffset0;
float flowMapOffset1;
float flowOffX;
float flowOffY;
float detailScale;
float waveScale;
float normalShore;


float _SpecScatterAmt;
float _SpecScatterWidth;
float4 _DynReflColor;
float4 _SpecColorL;
float4 _SpecColorH;


inline fixed4 LightingSuimonoSurfaceMobile (SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
{
	half3 h = normalize (lightDir + viewDir);

	fixed diff = max (0, dot (s.Normal, fixed3(lightDir.x,lightDir.y,lightDir.z)));

	float nh = max (0, dot (s.Normal, h));
	float spec = saturate(pow(nh,  _SpecScatterAmt)*100.0);
	float spec2 = pow (nh, _SpecScatterWidth*128.0)*(10.0);

	
	fixed4 c;
	//base color
	c.rgb = (s.Albedo * 2.0 * _LightColor0.rgb * _DynReflColor.rgb + _LightColor0.rgb * _SpecColor.rgb);
	c.a = diff * _DynReflColor.a;

	//clamp
	if (c.a > 1.0) c.a = 1.5;
	c.a = lerp(0.0,c.a,s.Alpha);
	c.a = lerp(c.a,c.a*0.3,s.Gloss); 

	// add specular
	c.rgb = lerp(c.rgb, _SpecColorL.rgb, (0.5+spec) * lerp(0.2,1.0,(1.0-s.Alpha)) * _LightColor0.a); //spec
	c.a += (((0.5+spec) *_SpecColorL.a) * diff * lerp(1.0,0.2,s.Gloss));
	c.a = saturate(c.a);
	
	// add hot specular highlights
	half4 useSpecCol = _SpecColorH;
	half hSpecAmt = lerp(0.0,1.5,spec2 * 100.0 * _LightColor0.a);
	c.rgb = lerp(c.rgb,useSpecCol.rgb, hSpecAmt* useSpecCol.a);
	c.a += lerp(0.0,saturate(hSpecAmt),useSpecCol.a);

	c = saturate(c);
	c.rgb *= (atten);

	return c;
}




struct Input {
	float2 uv_Surface1;
	float2 uv_WaveLargeTex;  
	float3 worldRefl;
    INTERNAL_DATA
};


sampler2D _CameraDepthTexture;
float _ReflBlend;
float _ReflDist;
float _ReflectStrength; 
float useReflection;
float _EdgeBlend;
sampler2D _ReflectionTex;
samplerCUBE _CubeMobile;
float4 _DepthColorG;


void surf (Input IN, inout SurfaceOutput o) {

	 // calculate wave height and roughness
	half waveHeight = 0.001+(_WaveHeight/10.0);
	half detailHeight = 0.001+(_DetailHeight/3.0);
	
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

	// switch in cube reflection if dynamic not available
	o.Albedo = cubeRef;//lerp(cubeRef,dynRef,useReflection);
	
	// set alpha by distance
	//half4 getflowmap = tex2D(_FlowMap,IN.uv_FlowMap);
	o.Alpha = saturate(n1.r + d1.r);//lerp(0.0,0.75,saturate(getflowmap.r*2.0))*(1.0-_EdgeBlend);;

	//set edge alpha
	o.Gloss = 0.0;//edgeSpread.a;


}
ENDCG


















}
FallBack "Diffuse"
}
