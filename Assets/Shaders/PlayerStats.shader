Shader "_OwnShaders/PlayerStats" {
	Properties {
		_MainTex ("Front image", 2D) = "white" {}
		_SecondTex ("Background", 2D) = "white" {}
		_HealthTex ("Health bar", 2D) = "white" {}
		_EnergyTex ("Energy bar", 2D) = "white" {}
		_HungerTex ("Hunger bar", 2D) = "white" {}
		_ThirstTex ("Thirst bar", 2D) = "white" {}
		
		_Health ("Health", Range(0.0, 1.0)) = 0.5
		_Energy ("Energy", Range(0.0, 1.0)) = 0.5
		_Hunger ("Hunger", Range(0.0, 1.0)) = 0.5
		_Thirst ("Thirst", Range(0.0, 1.0)) = 0.5
		
		_Color ("Background color", Color ) = (1,1,1,1)
		_Width ("Width", int) = 300
	}
	SubShader {
		Tags { "Queue"="Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
	Pass{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		//#pragma fragmentoption ARB_precision_hint_fastest
		#include "UnityCG.cginc"
		
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		
		uniform sampler2D _HealthTex;
		uniform sampler2D _EnergyTex;
		uniform sampler2D _HungerTex;
		uniform sampler2D _ThirstTex;
		
		uniform fixed _TexMix;
		uniform fixed _Health;
		uniform fixed _Energy;
		uniform fixed _Hunger; 
		uniform fixed _Thirst;
		
		uniform fixed4 _Color;
		uniform half _Width;
		struct vertexInput {
			float4 vertex : POSITION0;
			half4 texcoord : TEXCOORD0;
		};

		struct fragmentInput {
			float4 pos : SV_POSITION;
			half2 uv : TEXCOORD0;
		};

		fragmentInput vert(vertexInput i : TEXCOORD) {
			fragmentInput o;
			o.pos = mul( UNITY_MATRIX_P, i.vertex);
			o.uv = TRANSFORM_TEX(i.texcoord, _MainTex);
			return o;
		}
		
		half4 frag(fragmentInput i : TEXCOORD) : COLOR {
			float4 frameTexColor = tex2D( _MainTex, i.uv );
			float4 statColor = tex2D( _HealthTex, i.uv);
			if ((i.uv.x> _Health )&&(tex2D( _HealthTex, i.uv).w > 0 )){
				statColor = _Color;
			}
			if ((i.uv.x > _Energy )&&(tex2D( _EnergyTex, i.uv).w > 0 )){
				statColor = _Color;
			}else{
				statColor +=tex2D( _EnergyTex, i.uv);
			}
			if ((i.uv.x > _Hunger )&&(tex2D( _HungerTex, i.uv).w > 0 )){
				statColor = _Color;
			}else{
				statColor +=tex2D( _HungerTex, i.uv);
			}
			if ((i.uv.x > _Thirst )&&(tex2D( _ThirstTex, i.uv).w > 0 )){
				statColor = _Color;
			}else{
				statColor +=tex2D( _ThirstTex, i.uv);
			}
			 fixed4 resultColor = lerp(statColor, frameTexColor, frameTexColor.w);
			 if(statColor.w>0)resultColor.w = 1;
			 return resultColor;
		}
		ENDCG
	}    
	}
	FallBack "Diffuse"
}
