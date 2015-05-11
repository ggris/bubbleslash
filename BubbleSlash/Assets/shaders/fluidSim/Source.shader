﻿Shader "FluidSim/Source" 
{
Properties {
	_Radius ("Radius", Float) = 0.05
}
	SubShader 
	{
    	Pass 
    	{
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha 

			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			
			uniform float2 _Point;
			uniform float3 _FillColor;
			float _Radius;
			
			struct v2f 
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
			};
			
			v2f vert(appdata_base v)
			{
				v2f coords;
				coords.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				coords.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
				return coords;
			}
			
			float4 frag(v2f coords) : COLOR
			{
			    float d = distance(_Point, coords.uv);
			    
			    float4 result = float4(0,0,0,0);
			    
			    if(d < _Radius) 
			    {
			        float a = (_Radius - d) * 0.5;
        			a = min(a, 1.0);
			        result = float4(_FillColor, a);
			    } 
			  
			  	return result;
			}
			
			ENDCG

    	}
	}
}