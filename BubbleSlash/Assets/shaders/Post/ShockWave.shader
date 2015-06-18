﻿Shader "Custom/Post/ShockWave" {
    Properties {
        _MainTex ("", 2D) = "white" {}
    }

    SubShader {

        ZTest Always Cull Off ZWrite Off Fog { Mode Off } //Rendering settings

        Pass{
            CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc" 

                struct v2f {
                    float4 pos : POSITION;
                    half2 uv : TEXCOORD0;
                };

            v2f vert (appdata_img v){
                v2f o;
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.texcoord.xy;
                return o; 
            }

            sampler2D _MainTex;
            uniform float2 _Center;
            uniform float _Radius;
            uniform float _Sigma;
            uniform float _Amplitude;
            uniform float _Ratio;
            
            float deform(float x) {
            	float y = x/_Radius;
            	y*=y;
            	y -= 1;
            	y *= y;
            	float sigma = _Sigma/(_Sigma*2+_Radius);
            	float deform = exp(-y/sigma/sigma)/sigma/(_Sigma+_Radius);
            	return deform*_Amplitude;
            }

            fixed4 frag (v2f i) : COLOR{
            	float2 u = i.uv - _Center;
            	float2 u1 = u;
            	u1.x *= _Ratio;
            	float l = length(u1);
            	float d = deform(l);
            	float2 v = _Center + u*(l-d)/l;
            
                fixed4 orgCol = tex2D(_MainTex, v);
                fixed4 shock = fixed4(d/l*0.5, d/l, d/l, 0);
				
                return orgCol*(d*0.1+l)/l + shock*0.2;
            }
            ENDCG
        }
    } 
    FallBack "Diffuse"
}
