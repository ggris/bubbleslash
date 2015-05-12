Shader "FluidSim/Divergence" 
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
			uniform float _HalfInverseCellSize;
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

			    // Find neighboring velocities:
			    float2 U = tex2D(_Velocity, coords.uv + float2(0, _InverseSize.y)).xy;
			    float2 D = tex2D(_Velocity, coords.uv + float2(0, -_InverseSize.y)).xy;
			    float2 R = tex2D(_Velocity, coords.uv + float2(_InverseSize.x, 0)).xy;
			    float2 L = tex2D(_Velocity, coords.uv + float2(-_InverseSize.x, 0)).xy;
			
			    float result = _HalfInverseCellSize * (R.x - L.x + U.y - D.y);
			    
			    return float4(result,0,0,1);
			}
			
			ENDCG

    	}
	}
}