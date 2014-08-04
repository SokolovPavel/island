Shader "Suimono2/effect_refraction_dx11" {
Properties {
	_MaskMap ("MaskMap", 2D) = "" {}
	_HeightMap ("HeightMap", 2D) = "" {}
	_DepthColor ("Depth Color", Color) = (0.5, 0.5, 0.5, 1)
    _Strength ("Refraction Strength", range (0,150)) = 25.0
    _Overlay ("Overlay Strength", range (0.0,1.0)) = 1.0
    _Brightness ("Brightness Strength", range (1.0,1.8)) = 1.0
    _Height ("Displace Height", range (0.0,1.0)) = 0.0
}



SubShader {



		
        
        
    

GrabPass {
	"_waterTex1"
	Tags {"Queue" = "Transparent+1" "IgnoreProjector"="True"}
	Name "ScreenGrab"
}






// -------------------------------------
//   UNDERWATER REFRACTION and BLURRING 
// -------------------------------------
Tags {"Queue"= "Transparent+1" "IgnoreProjector"="True"} //Geometry
Cull Back
//Name "DropDistortion"
Blend SrcAlpha OneMinusSrcAlpha
ColorMask RGB
ZWrite Off
ZTest LEqual
Lighting Off

CGPROGRAM
#pragma target 3.0
#pragma surface surf SuimonoNoLight
#pragma glsl


fixed4 LightingSuimonoNoLight (SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
{
	fixed4 col;
	col.rgb = s.Albedo * atten;
	col.a = s.Alpha * atten;
	return col;
}


struct Input {
	float4 screenPos;
	float2 uv_HeightMap;
	float2 uv_MaskMap;
	//INTERNAL_DATA  
};


sampler2D _GrabTexture;
sampler2D _MaskMap;
sampler2D _CameraDepthTexture;
sampler2D _HeightMap;
float _Strength;
float4 _GrabTexture_TexelSize;


void surf (Input IN, inout SurfaceOutput o) {

	//calculate mask
	half4 maskTex = tex2D(_MaskMap,IN.uv_MaskMap);

	// calculate normals
	half3 useNormal;
	useNormal = UnpackNormal(tex2D(_HeightMap,IN.uv_HeightMap));

	//calculate texture and displace
	float4 uvs = IN.screenPos;
	
	//if (_isForward == 1.0 && _isMac == 0.0){
		uvs.y = uvs.w - uvs.y;
	//}
	
	float4 uv1 = uvs;
	uv1.y -= (useNormal.y);

	//calculate distorted color
	half rShift = 0.0;
	half4 oCol = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(uv1));
	//oCol.r = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x+rShift, uv1.y, uv1.z,uv1.w))).r;
	//oCol.b = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv1.x-rShift, uv1.y, uv1.z,uv1.w))).b;
	oCol.a = 1.0;

	o.Gloss = 0.0;
	o.Specular = 0.0;
	
	o.Albedo = oCol.rgb;
	o.Alpha = maskTex.r;

	
	
}
ENDCG




      
                

}

//FallBack ""
}
