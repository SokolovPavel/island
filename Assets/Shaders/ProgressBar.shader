Shader "_OwnShaders/Progressbar" {
	Properties {
		_MainTex ("Front image", 2D) = "white" {}
		_ProgressTex ("Progressbar image", 2D) = "white" {}
		_Progress ("Progress", Range(0.0, 1.0)) = 0.5
		_Color ("Background color", Color ) = (1,1,1,1)
	}
	SubShader {
		Tags { "Queue"="Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
	Pass{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform sampler2D _ProgressTex;
		uniform fixed _Progress;
		uniform fixed4 _Color;
		
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
			float4 statColor = tex2D( _ProgressTex, i.uv);
			if ((i.uv.x > _Progress )&&(tex2D( _ProgressTex, i.uv).w > 0 )){
				statColor = _Color;
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
