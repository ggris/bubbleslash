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
			uniform float _GridScale;
			uniform float _InvGridScale;
			uniform float _VelScale;
			float2 _Gravity;
			float _GradScale;
			
			float2 getOrigin(float2 uv) {
			   	float delta = _VelScale * _InvGridScale;
			   	float total = 1;
			   	float2 speed = (0, 0);
			   	float2 samp = tex2D(_UxUyD, uv).xy;
			   	total += length(samp) > _VelScale/2 ? 1 : 0;
			   	speed += samp;
			   	samp = tex2D(_UxUyD, uv + float2(0, delta)).xy;
			   	total += length(samp) > _VelScale/2? 1 : 0;
			   	speed += samp;
			   	samp = tex2D(_UxUyD, uv + float2(-delta, 0)).xy;
			   	total += length(samp) > _VelScale/2 ? 1 : 0;
			   	speed += samp;
			   	samp = tex2D(_UxUyD, uv + float2(0, -delta)).xy;
			   	total += length(samp) > _VelScale/2 ? 1 : 0;
			   	speed += samp;
			   	samp = tex2D(_UxUyD, uv + float2(delta, 0)).xy;
			   	total += length(samp) > _VelScale/2 ? 1 : 0;
			   	speed += samp;
			   	
			   	return uv - speed * _Delta / total;
			}
			
			float3 contrib(float2 uv0, float2 uv1)
			{
				float3 sample = tex2D(_UxUyD, uv1);
				
			   	float2 uv2 = ((uv1 - uv0) + sample.xy * _Delta) * _GridScale;
			   	uv2 = float2(1, 1) - abs( uv2);
			   	if (uv2.x>0 && uv2.y>0)
			   		return sample * uv2.x * uv2.y;
			   	
				return float3(0, 0, 0);
			}
			
			float3 frag(v2f_img IN) : SV_Target	{
			
			    float3 result = float3(0,0,0);
			    
			    float2 origin = getOrigin(IN.uv);
			    
				int _PatchR = 3;
			   	for (int i=-_PatchR; i<=_PatchR; i++)
			   	{
			   		for (int j=-_PatchR; j<=_PatchR; j++)
			   		{
			   			result += contrib(IN.uv, origin + float2(i, j) * _InvGridScale);
			   		}
			   	}
			    
			    float k = _InvGridScale * 0.7;
			    float pN = tex2D(_UxUyD, IN.uv + float2(0, k)).z;
			    float pS = tex2D(_UxUyD, IN.uv + float2(0, -k)).z;
			    float pE = tex2D(_UxUyD, IN.uv + float2(k, 0)).z;
			    float pW = tex2D(_UxUyD, IN.uv + float2(-k, 0)).z;
			
				float d = 1.6;
				float p = 2.4;
				if (pN < d) pN=p*d/(d+pN);
				if (pS < d) pS=p*d/(d+pN);
				if (pE < d) pE=p*d/(d+pN);
				if (pW < d) pW=p*d/(d+pN);
			    
			    float obs = tex2D(_Obstacles, IN.uv).x;
			
			    float2 grad = float2(pE - pW, pN - pS) * _GradScale;
			    grad *= abs(grad);
			    	
			    result.xy -= grad * _Delta * result.z * 50;
			    
			    result.xy += _Gravity * _Delta * result.z * 50;
			    
			    if (obs !=0)
			    	result.xy *= 0.7;
			    
			    result.z *=0.998;
			    
			    return result;
			}
			
			ENDCG

    	}
	}
}
