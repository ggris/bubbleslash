Shader "Custom/Post/ShockWave" {
    Properties {
        _MainTex ("", 2D) = "white" {}
        //_Center ("Center", Vector) = (0.5,0.5,0,0)
        //_Radius ("Radius", Float) = 0.2
        _Sigma ("Variance", Float) = 0.2
        _Amplitude ("Amplitude", Float) = 0.01
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
                o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
                return o; 
            }

            sampler2D _MainTex;
            uniform float2 _Center;
            uniform float _Radius;
            float _Sigma;
            float _Amplitude;
            
            float deform(float x) {
            	float result = x;
            	float y = x/_Radius;
            	y -= 1;
            	y *= y;
            	float deform = exp(-y/_Sigma/_Sigma)/_Sigma;
            	return result + deform*_Amplitude;
            }

            fixed4 frag (v2f i) : COLOR{
            	float2 u = i.uv - _Center;
            	float l = length(u);
            	float d = deform(l);
            	float2 v = _Center + u*d/l;
            
                fixed4 orgCol = tex2D(_MainTex, v);

                float avg = (orgCol.r + orgCol.g + orgCol.b)/3.0;
                fixed4 col = fixed4(avg+0.2, avg+0.1, avg, 1);

                return orgCol;
            }
            ENDCG
        }
    } 
    FallBack "Diffuse"
}
