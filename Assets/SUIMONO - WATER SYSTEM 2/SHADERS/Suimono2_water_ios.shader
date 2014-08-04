Shader "Suimono2/water_ios" {

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
Tags {"RenderType"="Transparent" "Queue"= "Transparent-101"}
Cull Back
Blend SrcAlpha OneMinusSrcAlpha
ZWrite On


CGPROGRAM
#pragma target 3.0
#pragma surface surf SuimonoSurface
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





struct Input {
	float2 uv_Surface1;
	float2 uv_FlowMap;
	float2 uv_WaveLargeTex;  
	float3 worldRefl;
	float3 viewDir;
    INTERNAL_DATA
};


float _ReflBlend;
float _ReflDist;
float _ReflectStrength; 
float useReflection;
float _EdgeBlend;
samplerCUBE _CubeMobile;
float4 _DepthColorG;
float4 _DepthColorB;
float _DepthAmt;
float _OverallTrans;
float _OverallBright;




inline fixed4 LightingSuimonoSurface (SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
{
	//calculate dots
	half3 lDir = half3(lightDir.x,lightDir.y,lightDir.z);
	half3 vDir = half3(viewDir.x,viewDir.y,viewDir.z);
	half3 h = normalize (lDir + vDir);
	float NdotL = dot(s.Normal, lDir);
	float NdotE = dot(s.Normal, vDir);
	fixed diff = max (0, dot(s.Normal, lightDir));
	
	
	//calculate specular
	float nh = max (0, dot (s.Normal, h));
	float spec = saturate(pow(nh,  _SpecScatterAmt)*1.0);
	float spec2 = pow (nh, _SpecScatterWidth*50)*(100.0);

	fixed4 c;
	//base color
	c.rgb = lerp(_DynReflColor.rgb,s.Albedo * 2.0,_DynReflColor.a);
	c.rgb *= (_LightColor0.rgb * _DynReflColor.rgb + _LightColor0.rgb);// * saturate(diff*3.0);// * diff;
	c.a = lerp(diff*_DynReflColor.a,s.Alpha,_DynReflColor.a);

	//clamp
	if (c.a > 1.0) c.a = 1.5;
	c.a = lerp(0.0,c.a,s.Alpha);
	c.a = lerp(c.a,c.a*0.3,s.Gloss);

	//edge transparency
	c.a *= (1.0-s.Gloss);

	//clamp surface colors
	c.rgb *= (atten);
	
	//add specular
	c.rgb += lerp(half3(0,0,0),(spec)*_SpecColorL.rgb,_SpecColorL.a);

	c = saturate(c);
	
	//reveal texture
	c.a *= NdotL;

	c.rgb *= _LightColor0.a;
	//c.rgb *= _OverallBright;
	c.a *= _OverallTrans;
	
	//add hot specular
	//c.rgb += lerp(half3(0,0,0),((spec2)*_SpecColorH.rgb),_SpecColorH.a);
	spec2 *= _LightColor0.a;
	_SpecColorH.rgb *= _LightColor0.a;
	//c.rgb += lerp(half3(0,0,0),(saturate(spec2)*(_SpecColorH.rgb*_LightColor0.rgb)),_SpecColorH.a*_LightColor0.a*atten);
	//c.a *= (1.0+saturate(spec2*_SpecColorH.a));
	c.rgb += lerp(half3(0,0,0),((spec2)*(_SpecColorH.rgb*_LightColor0.rgb)),_SpecColorH.a*_LightColor0.a*atten);
	c.a += ((spec2*_SpecColorH.a));

	c.rgb = lerp(s.Albedo*_LightColor0.a*NdotL,c.rgb,c.a);
	c.a = 1.0;

	c = saturate(c);
	return c;
	
}



void surf (Input IN, inout SurfaceOutput o) {

	// calculate normals
	float _Parallax = lerp(0.0,0.06,tex2D(_Surface1, IN.uv_WaveLargeTex).r);
	float2 offset = ParallaxOffset(tex2D(_Surface1, IN.uv_Surface1).z, _Parallax, IN.viewDir);
	half4 d1 = tex2D(_WaveLargeTex,IN.uv_Surface1+offset);
	offset = ParallaxOffset(tex2D (_Surface1, IN.uv_WaveLargeTex).z, _Parallax, IN.viewDir);
	half4 n1 = tex2D(_WaveLargeTex,IN.uv_WaveLargeTex+offset);//uv_WaveLargeTex
	offset = ParallaxOffset(tex2D (_Surface1, IN.uv_WaveLargeTex*14.0).z, _Parallax, IN.viewDir);
	half4 n2 = tex2D(_WaveLargeTex,(IN.uv_WaveLargeTex*14.0));//uv_WaveLargeTex
	//half4 d1 = tex2D(_WaveLargeTex,IN.uv_Surface1);
	//half4 n1 = tex2D(_WaveLargeTex,IN.uv_WaveLargeTex);
	//half4 n2 = tex2D(_WaveLargeTex,IN.uv_WaveLargeTex*14.0);
	

	o.Normal = lerp(half3(0,0,1),UnpackNormal(d1),saturate(lerp(0.0,2.0,_BumpStrength)));
	o.Normal += UnpackNormal(n1) * saturate(lerp(0.0,2.0,_BumpStrength));
	o.Normal -= UnpackNormal(n2) * saturate(lerp(-1.0,1.0,_BumpStrength));
	o.Normal = normalize(o.Normal);
	
	// decode cube / mobile reflection
	fixed nUV = 1.0;
	nUV += (o.Normal.y*_ReflectStrength*1.5)-(1.0*_ReflectStrength*0.25);
	fixed3 cubeRef = half3(0,0,1);
	cubeRef = texCUBE(_CubeMobile, WorldReflectionVector (IN, o.Normal)*float4(1.0,nUV,1.0,1.0)).rgb; 
	cubeRef = lerp(_DynReflColor.rgb,(cubeRef.rgb*_DynReflColor.rgb*_DynReflColor.a),_DynReflColor.a);

	//get flowmap texture
	fixed4 getflowmap = tex2D(_FlowMap,IN.uv_FlowMap);
	
	//basic under color
	//o.Albedo = lerp(_DepthColorB.rgb,_DepthColorG.rgb,getflowmap.r) * 0.5;
	o.Albedo = _DepthColorB.rgb;
	fixed msk = lerp(1.0,saturate(n1.r + d1.r),_BumpStrength) ;
	// switch in cube reflection if dynamic not available
	o.Albedo = lerp(o.Albedo,cubeRef,saturate(lerp(-20.0,1.0,o.Normal.z)));//lerp(cubeRef,dynRef,useReflection);
	//o.Albedo = _DepthColorB.rgb;
	
	// set alpha by distance
	//half4 getflowmap = tex2D(_FlowMap,IN.uv_FlowMap);
	o.Alpha = _DepthColorB.a;
	o.Alpha += msk;//lerp(0.0,0.75,saturate(getflowmap.r*2.0))*(1.0-_EdgeBlend);;


	o.Alpha *= 1.0-(_DepthAmt*0.04);
	
	//set edge alpha
	o.Gloss = 0.0;//edgeSpread.a;


}
ENDCG












}
FallBack "Diffuse"
}
