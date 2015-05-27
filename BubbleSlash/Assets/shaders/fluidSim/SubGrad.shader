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
			    float2 delta = _InverseSize*0.7;
			    float pN = tex2D(_Pressure, coords.uv + float2(0, delta.y)).x;
			    float pS = tex2D(_Pressure, coords.uv + float2(0, -delta.y)).x;
			    float pE = tex2D(_Pressure, coords.uv + float2(delta.x, 0)).x;
			    float pW = tex2D(_Pressure, coords.uv + float2(-delta.x, 0)).x;
			
				float d = 2;
				float p = 2.2;
				if (pN < d) pN=p;
				if (pS < d) pS=p;
				if (pE < d) pE=p;
				if (pW < d) pW=p;
			
			    // Enforce the free-slip boundary condition:
			    float2 oldV = tex2D(_Velocity, coords.uv).xy;
			    float2 grad = float2(pE - pW, pN - pS) * _GradScale;
			    grad *= abs(grad);
			    float2 newV = oldV - grad;
			    newV *= 1.02;
			    newV.y -= 0.02;
			    
			    return float4(newV,0,1);  
			}
			
			ENDCG

    	}
	}
}