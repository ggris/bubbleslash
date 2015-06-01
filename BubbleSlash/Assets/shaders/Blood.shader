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
    
  //Our Fragment Shader
  float4 frag (v2f i) : COLOR{
   float3 fluid = tex2D(_Blood, i.uv);
   float boodDens = tex2D(_Blood, i.uv).b;
   float4 texval = tex2D(_MainTex, i.uv);
   
   float2 smooth_border = cos((i.uv-0.5)*2.9);
   float smooth = smooth_border.x * smooth_border.y;
   
   float du = 0.01;
   float U = filterDens(tex2D(_Blood, i.uv + float2(0,du)), smooth);
   float L = filterDens(tex2D(_Blood, i.uv + float2(0,du)), smooth);
   float2 light = float2(1, -2);
   
   boodDens = filterDens(boodDens, smooth);
   
   float4 outColor = _BloodColor *1.2/ (1	+ boodDens);
   float2 grad = float2(boodDens-L , U - boodDens);
   float spec = dot(light, grad)*0.01;
   outColor += clamp(spec , -0.05, 0.6);
   saturate(outColor);
   outColor[3] = boodDens * 2;
   
   //return orgCol * (1 - bloodCol) + bloodCol/(1+2*bloodCol)*_BloodColor;
   return outColor;
  }
  ENDCG
 }
} 
 FallBack "Diffuse"
}