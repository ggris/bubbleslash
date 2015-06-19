Shader "Custom/Post/ShockWave" {
    Properties {
        _MainTex ("", 2D) = "white" {}
        _Factor ("", Float) = 0.5
    }

    SubShader {

        ZTest Always Cull Off ZWrite Off Fog { Mode Off } //Rendering settings

        Pass{
            CGPROGRAM
			#include "UnityCG.cginc"
            #pragma vertex vert
			#pragma fragment frag
            #pragma target 3.0
            
            uniform float2 _Center;

            v2f_img vert( appdata_img v ) { 
            	v2f_img o;
            	o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
            	o.uv = MultiplyUV( UNITY_MATRIX_TEXTURE0, v.texcoord );
            	return o;
            } 

            sampler2D _MainTex;
            uniform float _Radius;
            uniform float _Sigma;
            uniform float _Amplitude;
            uniform float _Ratio;
            float _Factor;
       
            float deform(float x) {
            	float y = x/_Radius;
            	y*=y;
            	y -= 1;
            	y *= y;
            	float sigma = _Sigma/(_Sigma*2+_Radius);
            	float deform = exp(-y/sigma/sigma)/sigma/(_Sigma+_Radius);
            	return deform*_Amplitude;
            }
            
            uniform sampler2D _Buffer;

            fixed4 frag (v2f_img i) : COLOR{
				#if UNITY_UV_STARTS_AT_TOP
				_Center.y = 1-_Center.y;
				#endif
            	float2 u = i.uv - _Center;
            	float2 u1 = u;
            	u1.x *= _Ratio;
            	float l = length(u1);
            	float d = deform(l);
            	float2 v = _Center + u*(l-d)/l;
            
                fixed4 orgCol = tex2D(_MainTex, v);
                fixed4 shock = fixed4(d/l*0.5, d/l, d/l, 0);
				
                fixed4 lastCol = tex2D(_Buffer, i.uv);
				
				orgCol *= (d*0.1+l)/l;
				orgCol += shock*0.2;
                return lerp(orgCol, lastCol, _Factor);
            }
            ENDCG
        }
    } 
    FallBack "Diffuse"
}
