Shader "FluidSim/SubGrad" 
{
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
			
			uniform sampler2D _Velocity;
			uniform sampler2D _Pressure;
			uniform float _GradScale;
			uniform float2 _InverseSize;
			
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
			
			    // Find neighboring pressure:
			    float pN = tex2D(_Pressure, coords.uv + float2(0, _InverseSize.y)).x;
			    float pS = tex2D(_Pressure, coords.uv + float2(0, -_InverseSize.y)).x;
			    float pE = tex2D(_Pressure, coords.uv + float2(_InverseSize.x, 0)).x;
			    float pW = tex2D(_Pressure, coords.uv + float2(-_InverseSize.x, 0)).x;
			
			    // Enforce the free-slip boundary condition:
			    float2 oldV = tex2D(_Velocity, coords.uv).xy;
			    float2 grad = float2(pE - pW, pN - pS) * _GradScale;
			    float2 newV = oldV - grad;
			    
			    return float4(newV,0,1);  
			}
			
			ENDCG

    	}
	}
}