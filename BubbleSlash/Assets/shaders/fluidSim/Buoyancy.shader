Shader "FluidSim/Buoyancy" {
Properties {
 	_Velocity ("Velocity", 2D) = "white" {}
 	_Temperature ("Qty to advect", 2D) = "white" {}
 	_Density ("Obstacles", 2D) = "white" {}
 	
	_T0 ("Reference temperature", Float) = 1.0
	_TimeStep ("Time step", Float) = 1.0
	_Sigma ("Buoyancy factor", Float) = 0.0
	_Kappa ("Gravity factor", Float) = 0.0
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
		
			sampler2D _Velocity;
			sampler2D _Temperature;
			sampler2D _Density;
			float _T0;
			float _TimeStep;
			float _Sigma;
			float _Kappa;
		
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
			
			float4 frag(v2f coords) : COLOR
			{
			    float T = tex2D(_Temperature, coords.uv).x;
			    float2 V = tex2D(_Velocity, coords.uv).xy;
			    float D = tex2D(_Density, coords.uv).x;
			
			    float2 result = V;
			
			    if(T > _T0) 
			    {
			        result += _TimeStep * ( (T - _T0) * _Sigma - D * _Kappa ) * float2(0, 1);
			    }
			    
			    return float4(result, 0, 1);
			    
			}
			
			ENDCG

    	}
	}
}