Shader "Custom/BloodPBR" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	 	_Blood ("Blood density", 2D) = "white" {}
	}
	SubShader {
		ZTest Always Cull Back ZWrite On Fog { Mode Off } //Rendering settings
		Tags { "RenderType"="Transparent" }
		Tags { "Queue" = "Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		// Physically based Standard lighting model
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
		sampler2D _Blood;


	  float filterDens(float dens, float smooth)
	  {
	  	float result = dens;
	    result*= smooth;
	    result -= 0.01;
	    result = saturate(result);
	    result *= result;
	    result *= 1.6;
	  	return result;
	  }

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
   			float4 fluid = tex2D(_Blood, IN.uv_MainTex);
   			
   			
		    float2 smooth_border = cos((IN.uv_MainTex-0.5)*2.9);
		    float smooth = smooth_border.x * smooth_border.y;
		   
		    float d = filterDens(fluid.z, smooth);
		   
		    float du = 0.01;
		    float3 U = float3(1, 0, tex2D(_Blood, IN.uv_MainTex + float2(du, 0)).z - fluid.z);
		    float3 L = float3(0, 1, tex2D(_Blood, IN.uv_MainTex + float2(0, du)).z - fluid.z);
   			o.Normal = normalize(cross(U, L));
   			
			fixed4 c = _Color;
			c.a *= d;
			o.Albedo = c;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
