Shader "Custom/NewShader" {
	Properties{
		_MainTex ("Main Texture", 2D) = "white" {}
		_Mask ("Mask Texture", 2D) = "white" {}
		_Overlay ("Minimap Skin", 2D) = "white" {}
		_Opacity ("Opacity", Range(0,1)) = 1
	}
	SubShader{
		Tags { "Queue"="Transparent"}
		Lighting On
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{
			//Color(0,0,0, [_Opacity])
			SetTexture[_Mask] {combine texture}
			SetTexture[_MainTex] {combine texture, previous}
			SetTexture[_Overlay] {combine texture lerp(texture) previous}
			SetTexture[_Overlay] {
				ConstantColor (1,1,1,[_Opacity])
                Combine previous * constant
            }		
		}
	}
}