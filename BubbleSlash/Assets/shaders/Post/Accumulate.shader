Shader "Custom/Accumulate" {
	Properties {
        _MainTex ("", 2D) = "white" {}
        _Factor ("", Float) = 0
	}

    SubShader {

        ZTest Always Cull Off ZWrite Off Fog { Mode Off } //Rendering settings

        Pass{
			CGPROGRAM
			#include "UnityCG.cginc"
            #pragma vertex vert_img
			#pragma fragment frag
            #pragma target 3.0

            sampler2D _MainTex;
            uniform sampler2D _Buffer;
            float _Factor;

            fixed4 frag (v2f_img i) : COLOR{
                fixed4 orgCol = tex2D(_MainTex, i.uv);
                fixed4 lastCol = tex2D(_Buffer, i.uv);
				
                return lerp(orgCol, lastCol, _Factor);
            }
            ENDCG
        }
    } 
    FallBack "Diffuse"
}
