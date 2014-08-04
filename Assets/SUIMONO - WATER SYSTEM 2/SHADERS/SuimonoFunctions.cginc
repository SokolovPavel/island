#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED


		
		
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
float shoreOffX;
float shoreOffY;
float shoreWaveOffX;
float shoreWaveOffY;
float detailScale;
float waveScale;
float normalShore;
float shoreWaveScale;



inline void vertexSuimonoDisplace (inout appdata_full v){


//calculate waves
	half2 tex = v.texcoord;
	half2 tex1 = v.texcoord * waveScale;
	half2 tex2 = v.texcoord * detailScale;
	half2 tex3 = v.texcoord * shoreWaveScale;
	
	half2 _offset = half2(_TimeX,_TimeY)*1.0;
	half2 _Doffset = half2(_DTimeX,_DTimeY)*1.0;
	half2 _Doffset2 = half2(_DTimeX2,_DTimeY2)*14.0;
	
	//calculate flowmap wvalues
	half2 _offsetFlow = half2(flowOffX,flowOffY);
	half2 _offsetShore = half2(shoreOffX,shoreOffY);
	
	float4 getflowmap = tex2Dlod(_FlowMap, float4(tex.x, tex.y,0.0,0.0));
 	float2 flowmap = float2(saturate(getflowmap.r + getflowmap.g),getflowmap.b) * 2.0f - 1.0f;
	flowmap.x = lerp(0.0,flowmap.x,_FlowShoreScale);
	flowmap.y = lerp(0.0,flowmap.y,_FlowShoreScale);

	
	//flowmap values
	float z = tex2Dlod(_FlowMap, float4(tex.xy + _offsetShore,0.0,0.0)).r;
	float s = tex2Dlod(_FlowMap, float4(tex.xy + _offsetShore,0.0,0.0)).g;
	
	//offsetShoreWave
	half4 waveTex = tex2Dlod(_WaveTex, float4((tex3.xy+_offsetFlow+flowmap),0.0,0.0));


	// calculate maps
	float texwaveRedChannel = tex2Dlod(_WaveMap, float4(tex.x, tex.y,0,0)).r;
	float texwaveGreenChannel = tex2Dlod(_WaveMap, float4(tex.x, tex.y,0,0)).g;
	
	// calculate waves
	float texwaveDeep = tex2Dlod(_Surface1, float4(tex1.x+_offset.x, tex1.y+_offset.y,0,0)).r;
	float texwaveFacDetail1 = tex2Dlod(_Surface1, float4((tex2.x)+_Doffset.x, (tex2.y)+_Doffset.y,0,0)).r;
	float texwaveFacDetail2 = tex2Dlod(_Surface1, float4((tex2.x)+_Doffset2.x, (tex2.y)+_Doffset2.y,0,0)).r;
	
	
	//SET VERTICES
	//vertical wave
	fixed origY = v.vertex.y;
	v.vertex.y += lerp(0.0,_WaveHeight,(texwaveDeep));

	//detail texture
	v.vertex.y += lerp(0.0,_DetailHeight, (texwaveFacDetail1 * (1.0-(z*0.25))));// + lerp(0.0,0.1,(texwaveFacDetail3+texwaveFacDetail4*_BumpStrength)*_DetailHeight);
	v.vertex.y += lerp(0.0,_DetailHeight*0.25, (texwaveFacDetail2 * (1.0-(z*0.25))));
	
	//normalize shoreline
	v.vertex.y = lerp(v.vertex.y,origY, (z) * normalShore);
	
	//raise and warp wavetex at shoreline
	v.vertex.y += (waveTex * _WaveShoreHeight) * (z);// * (1.0-s);
	
	
	//raise shoreline
	//v.vertex.y += lerp(0.0,0.5,z) * (texwaveGreenChannel);
	
	//edit waves with flowmap values
	//v.vertex.y += ((flowFactor * _WaveShoreHeight) * z);// * (1.0-s) * 0.5);

	//update normal
	v.normal.y = lerp(0.01,1.0,(v.vertex.y/_WaveHeight));
	v.normal = normalize(v.normal);

	
	
}



#endif