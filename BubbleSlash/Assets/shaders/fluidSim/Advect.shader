Shader "FluidSim/Advect" {

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
			
			uniform sampler2D _Velocity;
			uniform sampler2D _Source;
			uniform sampler2D _Obstacles;

			uniform float _InverseGridScale;
			uniform float _TimeStep;
			uniform float _Dissipation;
			uniform float _ObstacleDissipation;

			v2f vert(appdata_base v)
			{
    			v2f coords;
    			coords.pos = mul(UNITY_MATRIX_MVP, v.vertex);
    			coords.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
    			return coords;
			}
			
			float4 contrib(v2f coords, float2 dPos, float R, float delta)
			{
				float4 result = float4(0,0,0,0);
				
			   	float2 u = tex2D(_Velocity, coords.uv + dPos).xy * _InverseGridScale / delta;
			   	float2 v = float2(R, R) - abs( dPos + u );
			   	float r = v.x*v.y;
			   	if (v.x>0 && v.y>0)
			   		result = tex2D(_Source, coords.uv + dPos)*r/(R*R);
			   	
				return result;
			}
			
			float4 frag(v2f coords) : COLOR	{
			
			    float4 result = float4(0,0,0,0);
			    
			    float obs = tex2D(_Obstacles, coords.uv).x;
			    
			    float delta = 1;
			    
			    if (obs !=0)
			    	delta = 2;
			    
			    float du = _InverseGridScale;
			   	float2 u = tex2D(_Velocity, coords.uv).xy;
			   	u += tex2D(_Velocity, coords.uv + float2(du, du)).xy;
			   	u += tex2D(_Velocity, coords.uv + float2(-du, du)).xy;
			   	u += tex2D(_Velocity, coords.uv + float2(-du, -du)).xy;
			   	u += tex2D(_Velocity, coords.uv + float2(du, -du)).xy;
			   	
			   	u /= 5;
			   	u *= _InverseGridScale / delta;
			   	
			   	int l=2;
			   	for (int i=-l; i<=l; i++)
			   	{
			   		for (int j=-l; j<=l; j++)
			   		{
			   		result += contrib(coords, float2(i*du, j*du) - u, du, delta);
			   		}
			   	}
			   	
			    if (obs !=0)
			    	result *= (1.0 - _ObstacleDissipation);
			    else
			    	result *= (1.0 - _Dissipation);
			    
			    	
			    return result;
			}
			
			ENDCG

    	}
	}
}
