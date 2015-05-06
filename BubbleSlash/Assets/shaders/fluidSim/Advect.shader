﻿Shader "FluidSim/Advect" {
Properties {
 	_Velocity ("Velocity", 2D) = "white" {}
 	_Source ("Qty to advect", 2D) = "white" {}
 	_Obstacles ("Obstacles", 2D) = "white" {}
 	
	_InverseGridScale ("1 / grid scale", Float) = 1.0
	_TimeStep ("Time step", Float) = 1.0
	_Dissipation ("Dissipation", Range (0.0, 1.0)) = 0.0
}

SubShader 
	{
    	Pass 
    	{
			ZTest Always

			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
		
			struct v2f 
			{
    			float4  pos : SV_POSITION;
    			float2  uv : TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
    			v2f coords;
    			coords.pos = mul(UNITY_MATRIX_MVP, v.vertex);
    			coords.uv = v.texcoord.xy;
    			return coords;
			}
			
			uniform sampler2D _Velocity;
			uniform sampler2D _Source;
			uniform sampler2D _Obstacles;

			uniform float _InverseGridScale;
			uniform float _TimeStep;
			uniform float _Dissipation;
			
			float4 frag(v2f coords) : COLOR	{
			
			    float4 result = float4(0,0,0,0);
			    
			    float solid = tex2D(_Obstacles, coords.uv).x;
			    if(solid == 0.0) {
			   	
			    	float2 u = tex2D(_Velocity, coords.uv).xy;
			    	float2 pos = coords.uv - (u * _InverseGridScale * _TimeStep);
			   		result = (1.0 - _Dissipation) * tex2D(_Source, pos);
			   	}
			    
			    return result;
			}
			
			ENDCG

    	}
	}
}
