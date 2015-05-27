Shader "Custom/Blood" {
Properties {
 _MainTex ("", 2D) = "white" {}
 _Blood ("", 2D) = "white" {}
 _BloodColor ("Blood color", Color) = (1,0,0,0)
}
 
SubShader {
ZTest Always Cull Back ZWrite On Fog { Mode Off } //Rendering settings
Tags { "Queue" = "Transparent" }
Blend SrcAlpha OneMinusSrcAlpha
 
 Pass{
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc" 
  //we include "UnityCG.cginc" to use the appdata_img struct
    
  struct v2f {
   float4 pos : POSITION;
   half2 uv : TEXCOORD0;
  };
   
  //Our Vertex Shader 
  v2f vert (appdata_img v){
   v2f o;
   o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
   o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   return o; 
  }
    
  sampler2D _MainTex;
  uniform sampler2D _Blood;
  float4 _BloodColor;
    
  //Our Fragment Shader
  float4 frag (v2f i) : COLOR{
   float boodDens = tex2D(_Blood, i.uv);
   float4 texval = tex2D(_MainTex, i.uv);
   
   float du = 0.01;
   float U = tex2D(_Blood, i.uv + float2(0,du));
   float L = tex2D(_Blood, i.uv + float2(0,du));
   float2 grad = float2(boodDens-L , U - boodDens);
   float2 light = float2(1, -2);
   
   float2 smooth_border = cos((i.uv-0.5)*2.9);
   boodDens*= smooth_border.x*smooth_border.y;	
   boodDens = max( boodDens - 0.1, 0);
  
  
   boodDens = saturate(boodDens);
   
   float4 outColor = _BloodColor / (1	+boodDens);
   //outColor += clamp(dot(light, grad) * 0.01, -0.2, 0.2);
   outColor[3] = boodDens * 2;
   
   //return orgCol * (1 - bloodCol) + bloodCol/(1+2*bloodCol)*_BloodColor;
   return outColor;
  }
  ENDCG
 }
} 
 FallBack "Diffuse"
}