Shader "Custom/Post/Flip" {

	Properties {
        _MainTex ("", 2D) = "white" {}
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off } //Rendering settings

        Pass{
            CGPROGRAM
			#include "UnityCG.cginc"
            #pragma vertex vert
			#pragma fragment frag
            #pragma target 3.0

            v2f_img vert( appdata_img v ) { 
            	v2f_img o;
            	o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
            	o.uv = MultiplyUV( UNITY_MATRIX_TEXTURE0, v.texcoord );
	            #if UNITY_UV_STARTS_AT_TOP
	        		o.uv.y = 1-o.uv.y;
				#endif
            	return o;
            } 

            sampler2D _MainTex;

            fixed4 frag (v2f_img i) : COLOR {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    } 
	FallBack "Diffuse"
}
