Shader "Custom/Liquid" {
Properties {
 _GradScale ("Grad scale", Float) = 1.0
 _Gravity ("Gravity", Vector) = (0, -9.81, 0, 0)
}

SubShader 
	{
    	Pass 
    	{
			ZTest Off

			CGPROGRAM
			#include "UnityCG.cginc"
            #pragma vertex vert_img
			#pragma fragment frag
            #pragma target 3.0
			
			uniform sampler2D _UxUyD;
			uniform sampler2D _Obstacles;
			uniform float _Delta;
			float2 _Gravity;
			float _GradScale;
			
			float2 getOrigin(float2 uv) {
			   	float2 origin = tex2D(_UxUyD, uv).xy;
			   	origin += tex2D(_UxUyD, uv + float2(0, _Delta)).xy;
			   	origin += tex2D(_UxUyD, uv + float2(-_Delta, 0)).xy;
			   	origin += tex2D(_UxUyD, uv + float2(0, -_Delta)).xy;
			   	origin += tex2D(_UxUyD, uv + float2(_Delta, 0)).xy;
			   	
			   	return origin / 5;
			}
			
			float3 contrib(float2 uv, float2 dij)
			{
			   	float2 Dij = dij + tex2D(_UxUyD, uv + dij * _Delta).xy;
			   	Dij = float2(1, 1) - abs( Dij);
			   	if (Dij.x>0 && Dij.y>0)
			   		return tex2D(_UxUyD, uv + dij * _Delta) * Dij.x * Dij.y;
			   	
				return float3(0, 0, 0);
			}
			
			float3 frag(v2f_img IN) : SV_Target	{
			
			    float3 result = float3(0,0,0);
			    
			    float2 origin = getOrigin(IN.uv);
			    
				int _PatchR = 2;
			   	for (int i=-_PatchR; i<=_PatchR; i++)
			   	{
			   		for (int j=-_PatchR; j<=_PatchR; j++)
			   		{
			   			result += contrib(IN.uv, -origin + float2(i, j));
			   		}
			   	}
			    
			    float pN = tex2D(_UxUyD, IN.uv + float2(0, _Delta)).z;
			    float pS = tex2D(_UxUyD, IN.uv + float2(0, -_Delta)).z;
			    float pE = tex2D(_UxUyD, IN.uv + float2(_Delta, 0)).z;
			    float pW = tex2D(_UxUyD, IN.uv + float2(-_Delta, 0)).z;
			
				float d = 1.8;
				float p = 2.4;
				if (pN < d) pN=p;
				if (pS < d) pS=p;
				if (pE < d) pE=p;
				if (pW < d) pW=p;
			    
			    float obs = tex2D(_Obstacles, IN.uv).x;
			
			    float2 grad = float2(pE - pW, pN - pS) * _GradScale;
			    grad *= abs(grad);
			    if (obs !=0)
			    	grad*=10;
			    result.xy -= grad * _Delta;
			    
			    if (obs !=0)
			    	result.xy *= 0.5;
			    else
			    	result.xy += _Gravity * _Delta;
			    
			    result.z *=0.96;
			    
			    return result;
			}
			
			ENDCG

    	}
	}
}
