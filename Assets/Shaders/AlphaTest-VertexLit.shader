Shader "GUIPlayerStat" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff ("Cutoff", Range(0,1)) = 0.5
	_Color ("Color", Color) = (1,0,0,1)
}

SubShader {
	Tags {"Queue"="Transparent" "RenderType"="TransparentCutout"}
	Blend SrcAlpha OneMinusSrcAlpha
	Pass {
		//Name "Caster"
		//Fog {Mode Off}
		//ZWrite On ZTest LEqual Cull Off

CGPROGRAM
#pragma vertex vert
#pragma fragment frag
//#pragma multi_compile_shadowcaster
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform fixed _Cutoff;
uniform fixed4 _Color;

struct vertexInput {
	float4 vertex : POSITION;
};

struct fragmentInput {
	float4 pos : SV_POSITION;
	float4 color : COLOR;
};

fragmentInput vert(vertexInput i){
	fragmentInput o;
	o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
	o.color = _Color;
	return o;
}

half4 frag(fragmentInput i) :COLOR
{
	//fixed4 texcol = tex2D( _MainTex, i.pos);
	//i.color.Alpha = clip (i.color.Alpha - _Cutoff);
	i.color -= half4(0,0,0,_Cutoff);
	return i.color;
	//return i.color;
	//half4 c;
	//c.rgb = (s.Albedo * diffuseFactor + _Specular.rgb * specularFactor) * _LightColor0.rgb;
	//c.rgb *= (atten * 2.0);
	//c.a = s.Alpha;
	//clip (c.a - 0.01);
	//return c;
	//
	//return clip( texcol.a - _Cutoff );
}
ENDCG
}
}

}