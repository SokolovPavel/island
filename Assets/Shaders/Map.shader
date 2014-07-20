Shader "Custom/NewShader" {
	Properties{
		_MainTex ("Main Texture", 2D) = "white" {}
		_Overlay ("Minimap Skin", 2D) = "white" {}
	}
	SubShader{
		Tags { "Queue"="Transparent"}
		Lighting On
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{
			SetTexture[_Mask] {combine texture}
			SetTexture[_Overlay] {combine texture lerp(texture) previous}		
		}
	}
}