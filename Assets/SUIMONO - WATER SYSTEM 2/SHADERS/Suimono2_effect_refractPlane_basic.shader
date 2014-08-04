Shader "Suimono2/effect_refractPlane_basic" {


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







Tags {"Queue"= "Overlay+5"}
Cull Back
Blend SrcAlpha OneMinusSrcAlpha
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


fixed4 LightingSuimonoNoLight (SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
{
	fixed4 col;
	
	col.rgb = s.Albedo * lerp(fixed3(1,1,1),_DepthColorB.rgb,0.2);// * _LightColor0.a;
	col.rgb *= s.Gloss;
	col.a = s.Alpha;
	
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
	float4 uv1 = IN.screenPos;
		uv1.y -= (useNormal.y*(_RefrStrength*0.25));

	//calculate distorted color
	half4 oCol = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x, uv1.y, uv1.z,uv1.w))) * 1.4;
	oCol.a = 1.0;

	//calculated original depth
	half4 depth2 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(float4(uv1.x, uv1.y, uv1.z,uv1.w))).r;

	//calculate blur texture
	half blur = 1.0;
	half4 xCol = half4(0,0,0,0);
	half res = 1.0;
	int blursamples = 30;
	int divsamples = blursamples-1;
		for(int i=1; i < blursamples ; i++){
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
		xCol = saturate(xCol*1.2);

	
	o.Gloss = 0.0;
	o.Specular = 0.0;
	
	half useAlpha = 1.0;
	half3 useAlbedo = lerp(oCol.rgb,xCol.rgb,_BlurSpread*saturate(lerp(-3.0,1.0,depth2.r)));
	
	o.Gloss = lerp(1.0,falloff.g,_DepthColorB.a);
	o.Albedo = useAlbedo;
	
	o.Albedo = _DepthColorB.rgb*(falloff.g);
	o.Alpha = saturate(_DepthColorB.a+(1.0-falloff.g));
	
}

ENDCG





            
















}
FallBack "Diffuse"
}




