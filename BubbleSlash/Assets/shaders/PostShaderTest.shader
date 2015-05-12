Shader "Custom/PostShaderTest" {
Properties {
 _MainTex ("", 2D) = "white" {}
 _Blood ("Blood", 2D) = "black" {} 
 _BloodColor ("Blood color", Color) = (1,0,0,0)
}
 
SubShader {
 
ZTest Always Cull Off ZWrite Off Fog { Mode Off } //Rendering settings
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
  sampler2D _Blood;
  float4 _BloodColor;
    
  //Our Fragment Shader
  float4 frag (v2f i) : COLOR{
   float4 orgCol = tex2D(_MainTex, i.uv);
   orgCol[3] = 0;
   float bloodCol = tex2D(_Blood, i.uv);
   bloodCol = max(2 * bloodCol -1, 0);
  
   bloodCol = saturate(bloodCol);
   
   float4 outColor = _BloodColor / (1+2*bloodCol);
   outColor[3] = bloodCol;
   
   //return orgCol * (1 - bloodCol) + bloodCol/(1+2*bloodCol)*_BloodColor;
   return outColor;
  }
  ENDCG
 }
} 
 FallBack "Diffuse"
}