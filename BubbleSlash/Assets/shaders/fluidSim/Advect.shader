Shader "FluidSim/Advect" {
Properties {
 	_Velocity ("Velocity", 2D) = "white" {}
 	_Source ("Qty to advect", 2D) = "white" {}
 	_Obstacles ("Obstacles", 2D) = "white" {}
 	
	_InverseGridScale ("1 / grid scale", Float) = 0.01
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
			
			sampler2D _Velocity;
			sampler2D _Source;
			sampler2D _Obstacles;

			float _InverseGridScale;
			float _TimeStep;
			float _Dissipation;

			v2f vert(appdata_base v)
			{
    			v2f coords;
    			coords.pos = mul(UNITY_MATRIX_MVP, v.vertex);
    			coords.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
    			return coords;
			}
			
			float4 contrib(v2f coords, float2 dPos, float R)
			{
				float4 result = float4(0,0,0,0);
				
			   	float2 u = tex2D(_Velocity, coords.uv + dPos).xy * _InverseGridScale;
			   	float2 v = float2(R, R) - abs( dPos + u );
			   	float r = v.x*v.y;
			   	if (v.x>0 && v.y>0)
			   		result = tex2D(_Source, coords.uv + dPos)*r/(R*R);
			   	
				return result;
			}
			
			float4 frag(v2f coords) : COLOR	{
			
			    float4 result = float4(0,0,0,0);
			    
			    float du = _InverseGridScale;
			   	
			   	int l=3;
			   	for (int i=-l; i<=l; i++)
			   	{
			   		for (int j=-l; j<=l; j++)
			   		{
			   		result += contrib(coords, float2(i*du, j*du), du);
			   		}
			   	}
			   	
			   	//result = saturate(result);
			   	
			   	result *= (1.0 - _Dissipation);
			    
			    return result;
			}
			
			ENDCG

    	}
	}
}
