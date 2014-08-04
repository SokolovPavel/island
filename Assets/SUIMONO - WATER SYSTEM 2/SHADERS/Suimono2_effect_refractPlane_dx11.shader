Shader "Suimono2/effect_refractPlane_dx11" {


Properties {
	_MasterScale ("Master Scale", Float) = 1.0
	
	_BlurSpread ("Blur Spread", Range (0.0, 0.125)) = 0.001
	_BlurRamp ("Blur Ramp", 2D) = "" {}

	_underFogStart ("Underwater Fog Start", Range(0.0,1.0)) = 0.0
	_underFogStretch ("Underwater Fog Stretch", Range(0.0,0.02)) = 0.0
	
	//_underTex ("Underwater Fog Texture 2", 2D) = "" {}
	
	_TestHeight1 ("Test Height 1", 2D) = "" {}

	//refraction
	_RefrStrength ("Refraction Strength", Range(0.0,1.0)) = 0.0
    _RefrSpeed ("Refraction Speed", Float) = 0.5
	_AnimSpeed ("Animation Speed", Float) = 1.0

	_DepthAmt ("Depth Amount", Float) = 0.1
	_DiffuseColor ("Diffuse Color", Color) = (0.5, 0.5, 1.0, 1.0)
	
	_DepthColor ("Depth Over Tint", Color) = (0.25,0.25,0.5,1.0)
	_DepthColorR ("Depth Color 1(r)", Color) = (0.25,0.25,0.5,1.0)
	_DepthColorG ("Depth Color 2(g)", Color) = (0.25,0.25,0.5,1.0)
	_DepthColorB ("Depth Color 3(b)", Color) = (0.25,0.25,0.5,1.0)

	_Ramp2D ("BRDF Ramp", 2D) = "gray" {}
	_FalloffTex ("Falloff Texture", 2D) = "gray" {}

}



Subshader 
{ 















//------------------------------
//      UNDERWATER FOG
//------------------------------
Tags {"Queue"= "Transparent"}
Cull Back
Blend SrcAlpha OneMinusSrcAlpha
ZWrite Off
ZTest Always

CGPROGRAM
#pragma target 3.0
#pragma surface surf BlinnPhongBRDF noambient

sampler2D _Ramp2D;
sampler2D _CameraDepthTexture;
sampler2D _FalloffTex;
float4 _DepthColorB;


struct Input {
	float3 viewDir;
	float4 pos;
	float4 screenPos;
	float2 uv_FalloffTex;
};

fixed4 LightingBlinnPhongBRDF (SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
{
	half3 h = normalize (lightDir + viewDir);
	
	//fixed diff = max (0, dot (s.Normal, lightDir));
	
	float nh = max (0, dot (s.Normal, h));
	float spec = pow (nh, s.Specular*128.0) * s.Gloss;
	
	//lighting dot products
	float NdotL = dot(s.Normal, lightDir);
	float NdotE = dot(s.Normal, viewDir);
	fixed NdotH = max(0, dot(s.Normal, h));
	
	//do diffuse wrap
	float diff = (NdotL * 0.5) + 0.5;
	float2 brdfUV = float2(NdotE * 1.0, diff);
	float3 BRDF = tex2D(_Ramp2D, brdfUV.xy).rgb;
	float3 BRDFColor = _LightColor0.rgb;
	BRDFColor = lerp(BRDFColor,_LightColor0.rgb,BRDF.r);
	
	//fixed3 c;
	fixed3 r;
	fixed4 col;
	fixed4 lCol = lerp(_LightColor0,_DepthColorB,0.25);

	r = (lCol.rgb*(h.g));
	col.rgb = (s.Albedo * _LightColor0.rgb * atten * 2) + max(0.0,r) * BRDFColor;
	col.a = s.Alpha + _LightColor0.a * _SpecColor.a * spec * atten;
	
	col.rgb *= s.Gloss;
	//col.rgb = s.Albedo;
	
	//
	//col.rgb = s.Albedo;
	col.a = s.Alpha;

	col = saturate(col);
	col.rgb *= (atten);


	return col;
}



float4 _DepthColor;
sampler2D _BlurRamp;
float _DepthAmt;
float _underFogStretch;
float _underFogStart;


void surf (Input IN, inout SurfaceOutput o) {

	//CALCULATE DEPTH FOG
	//float4 DepthFade = float4(1.0,0.0,0.0,0.0);
	//float4 edgeBlendFactors = float4(0.0, 0.0, 0.0, 0.0);
	float4 edgeBlendFactors = float4(0.0, 0.0, 0.0, 0.0);
	//half dpth = UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)));
	half dpth = UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, IN.screenPos));
	half depth = LinearEyeDepth(dpth);
	float4 DepthFade = float4(_underFogStretch,2.0,0.0,-0.25+_underFogStart);
	edgeBlendFactors = saturate(DepthFade * (depth-IN.screenPos.w));
	
	
	float depthAmt = tex2D(_BlurRamp, float2(0.0, 0.5)) - (DepthFade.w * float4(0.15, 0.08, 0.01, 0.0)).a;
	float4 falloff = tex2D(_FalloffTex, IN.uv_FalloffTex);
	//if (depthAmt < 0.5) depthAmt = 0.0;
	depthAmt *= (edgeBlendFactors.y);
	depthAmt = edgeBlendFactors.x * depthAmt;
	float depthPos = depthAmt-DepthFade.w;
	half depthViz = 1.0-tex2D(_BlurRamp, float2(depthPos, 0.5)).r;


	o.Albedo = lerp(_DepthColorB.rgb*0.25, half3(1,0,0), 0.0);
	o.Albedo = lerp(half3(0,0,0), _DepthColorB.rgb*0.25, depthViz);
	
	o.Albedo = _DepthColorB.rgb;
	
	//o.Emission = o.Albedo;
	o.Alpha = saturate(depthViz);
	o.Gloss = falloff.r;
	
	//
	//o.Albedo=half3(dpth.r,0.0,0.0);
	//o.Alpha = max(depthViz,edgeBlendFactors);
	o.Alpha = depthViz;

  }
ENDCG          


 









// -------------------------------------
//   UNDERWATER REFRACTION and BLURRING 
// -------------------------------------
GrabPass {
	Tags {"Queue" = "Transparent"}
	Name "BlurGrab"
}


Tags {"Queue"= "Transparent"}
Cull Back
//Blend SrcAlpha OneMinusSrcAlpha
ZWrite off
ZTest Always



CGPROGRAM
#pragma target 3.0
#pragma surface surf SuimonoNoLight noambient
#pragma glsl


struct Input {
	float4 screenPos;
	//float2 uv_Surface1;
	//float2 uv_Surface2;
	float2 uv_TestHeight1; 
	float2 uv_FalloffTex;
	//float3 worldRefl;
    //INTERNAL_DATA
};


sampler2D _GrabTexture;
float4 _GrabTexture_TexelSize;
float4 _DepthColorB;
float _BlurSpread;
float _RefrStrength;
sampler2D _TestHeight1;
sampler2D _CameraDepthTexture;
sampler2D _BlurRamp;
float _DepthAmt;
float _EdgeBlend;
float _RefrShift;
float _BumpStrength;
float _MasterScale;
float _RefrSpeed;
sampler2D _FalloffTex;
float _blurSamples;
float _isForward;
float _UVReversal;

fixed4 LightingSuimonoNoLight (SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
{
	fixed4 col;
	
	col.rgb = s.Albedo * lerp(fixed3(1,1,1),_DepthColorB.rgb,0.2);// * _LightColor0.a;
	col.rgb *= s.Gloss;
	col.a = s.Alpha;

	col = saturate(col);
	col.rgb *= (atten);

	return col;
}



void surf (Input IN, inout SurfaceOutput o) {

	// calculate normals
	half4 d1 = tex2D(_TestHeight1,IN.uv_TestHeight1 * _MasterScale + (_Time * (_RefrSpeed*0.1)));
	half3 useNormal = UnpackNormal(d1);
	useNormal = normalize(useNormal);

	//get falloff texture
	float4 falloff = tex2D(_FalloffTex, IN.uv_FalloffTex);
		
	//calculate texture and displace
	float4 uvs = IN.screenPos;
	
	if (_isForward == 1.0){
		uvs.y = uvs.w - uvs.y;
	}

	if (_UVReversal == 1.0){
		if (_isForward == 1.0){
			uvs.y = IN.screenPos.y;
		} else {
			uvs.y = uvs.w - IN.screenPos.y;
		}
	}
	
	float4 uv1 = uvs;
		uv1.y -= (useNormal.y*(_RefrStrength*0.25));

	//calculate distorted color
	half4 oCol = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x, uv1.y, uv1.z,uv1.w))) * 1.4;
	oCol.a = 1.0;

	//calculated original depth
	half4 depth2 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)).r;

	//calculate blur texture
	half blur = 1.0;
	half4 xCol = half4(0,0,0,0);
	half res = 1.0;
	int divsamples = _blurSamples-1;
		for(int i=1; i < _blurSamples ; i++){
			res = 0.001 * i;
			xCol += (tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x-res*blur, uv1.y-res*blur, uv1.z,uv1.w)))*(1.0/divsamples)*0.125);
			xCol += (tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x+res*blur, uv1.y-res*blur, uv1.z,uv1.w)))*(1.0/divsamples)*0.125);
			xCol += (tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x-res*blur, uv1.y+res*blur, uv1.z,uv1.w)))*(1.0/divsamples)*0.125);
			xCol += (tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x+res*blur, uv1.y+res*blur, uv1.z,uv1.w)))*(1.0/divsamples)*0.125);
		
			xCol += (tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x, uv1.y-res*blur, uv1.z,uv1.w)))*(1.0/divsamples)*0.125);
			xCol += (tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x, uv1.y+res*blur, uv1.z,uv1.w)))*(1.0/divsamples)*0.125);
			xCol += (tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x+res*blur, uv1.y, uv1.z,uv1.w)))*(1.0/divsamples)*0.125);
			xCol += (tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x-res*blur, uv1.y, uv1.z,uv1.w)))*(1.0/divsamples)*0.125);
		}
		xCol = saturate(xCol*1.4);

	
	o.Gloss = 0.0;
	o.Specular = 0.0;
	
	half useAlpha = 1.0;
	half3 useAlbedo = lerp(oCol.rgb,xCol.rgb,_BlurSpread*saturate(lerp(-3.0,1.0,depth2.r)));
	
	o.Gloss = lerp(1.0,falloff.g,_DepthColorB.a);
	o.Albedo = saturate(useAlbedo);
	o.Alpha = 1.0;
	
	
}

ENDCG





            
















}
FallBack "Diffuse"
}




