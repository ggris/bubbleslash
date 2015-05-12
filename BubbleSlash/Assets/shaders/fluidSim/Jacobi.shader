Shader "FluidSim/Jacobi" 
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
			
			uniform sampler2D _Pressure;
			uniform sampler2D _Divergence;
			
			uniform float _Alpha;
			uniform float _RBeta;
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
			    float pU = tex2D(_Pressure, coords.uv + float2(0, _InverseSize.y)).x;
			    float pD = tex2D(_Pressure, coords.uv + float2(0, -_InverseSize.y)).x;
			    float pR = tex2D(_Pressure, coords.uv + float2(_InverseSize.x, 0)).x;
			    float pL = tex2D(_Pressure, coords.uv + float2(-_InverseSize.x, 0)).x;
			
			    float dC = tex2D(_Divergence, coords.uv).x;
			    
			    return (pU + pD + pR + pL + _Alpha * dC) * _RBeta;
			}
			
			ENDCG

    	}
	}
}