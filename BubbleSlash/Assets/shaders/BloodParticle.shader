Shader "Custom/BloodParticle" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		ZTest Always Cull Off ZWrite On Fog { Mode Off } //Rendering settings
		Tags { "RenderType"="Transparent" }
		Tags { "Queue" = "Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed2 r = 2*IN.uv_MainTex - fixed2(1, 1);
		
			// Albedo comes from a texture tinted by color
			fixed4 c = _Color;
			c.a *= 2*(1 - length(r));
			r /=2;
			o.Normal = normalize(fixed3(r.x, r.y, sqrt(1-dot(r, r))));
			o.Albedo = c*2;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
