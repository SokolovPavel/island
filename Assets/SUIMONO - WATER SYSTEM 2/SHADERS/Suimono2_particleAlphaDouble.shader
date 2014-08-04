Shader "Suimono2/particle_AlphaDouble" {

Properties {
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("Particle Texture", 2D) = "white" {}
}



SubShader {


Tags {"Queue"= "Transparent-101" "IgnoreProjector"="True"}
Blend SrcAlpha OneMinusSrcAlpha
Cull Off


CGPROGRAM
#pragma surface surf SuimonoNoLight

struct Input {
	float2 uv_MainTex;
};

sampler2D _MainTex;
float4 _TintColor;

fixed4 LightingSuimonoNoLight (SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
{
	fixed4 col;
	col.rgb = s.Albedo;
	col.a = s.Alpha;
	return col;
}

void surf (Input IN, inout SurfaceOutput o) {

	//o.Gloss = 1.0;
	//o.Specular = 1.0;
	half4 col = tex2D(_MainTex, IN.uv_MainTex);
	o.Albedo = col.rgb;
	o.Alpha = col.a;
	
}
ENDCG









}
FallBack "Diffuse"
}