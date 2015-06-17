Shader "Custom/Accumulate" {
	Properties {
        _MainTex ("", 2D) = "white" {}
        _Factor ("", Float) = 0
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
            uniform sampler2D _Buffer;
            float _Factor;

            fixed4 frag (v2f i) : COLOR{
                fixed4 orgCol = tex2D(_MainTex, i.uv);
                fixed4 lastCol = tex2D(_Buffer, i.uv);
				
                return lerp(orgCol, lastCol, _Factor);
            }
            ENDCG
        }
    } 
    FallBack "Diffuse"
}
