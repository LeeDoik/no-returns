Shader "NoReturns/ReceiptCRT" {
 Properties { _BaseMap("Body texture",2D)="white"{} _BaseColor("Tint",Color)=(1,1,1,1) _ScreenMap("CRT UI",2D)="black"{} _Progress("Scan progress",Float)=-1 }
 SubShader {
 Tags { "RenderType"="Opaque" "Queue"="Geometry" "RenderPipeline"="UniversalPipeline" }
 Pass {
 Name "ForwardLit"
 Tags { "LightMode"="UniversalForward" }
 ZWrite On ZTest LEqual Cull Back
 HLSLPROGRAM
 #pragma vertex Vert
 #pragma fragment Frag
 #pragma multi_compile_fog
 #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
 #pragma multi_compile_fragment _ _SHADOWS_SOFT
 #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
 #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
 TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
 TEXTURE2D(_ScreenMap); SAMPLER(sampler_ScreenMap);
 CBUFFER_START(UnityPerMaterial)
 float4 _BaseMap_ST; half4 _BaseColor;float _Progress;
 CBUFFER_END
 struct A { float4 positionOS:POSITION;float3 normalOS:NORMAL;float2 uv:TEXCOORD0; };
 struct V { float4 positionCS:SV_POSITION;float3 positionWS:TEXCOORD0;half3 normalWS:TEXCOORD1;float2 uv:TEXCOORD2;half fog:TEXCOORD3; };
 V Vert(A a){V o;VertexPositionInputs p=GetVertexPositionInputs(a.positionOS.xyz);o.positionCS=p.positionCS;o.positionWS=p.positionWS;o.normalWS=TransformObjectToWorldNormal(a.normalOS);o.uv=TRANSFORM_TEX(a.uv,_BaseMap);o.fog=ComputeFogFactor(p.positionCS.z);return o;}
 half4 Frag(V i):SV_Target {
  half3 n=normalize(i.normalWS);
  // Project only onto the measured original CRT glass, never onto the case or rear.
  float2 screenUV=(i.positionWS.xy-float2(-8.335,1.035))/float2(.49,.425);
  bool glass=all(screenUV>=0)&&all(screenUV<=1)&&abs(i.positionWS.z-8.7743)<.035&&n.z<-.65;
  half3 body=SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap,i.uv).rgb*_BaseColor.rgb;
  SurfaceData surface=(SurfaceData)0;surface.albedo=body;surface.alpha=1;surface.occlusion=1;surface.smoothness=0;
  InputData input=(InputData)0;input.positionWS=i.positionWS;input.normalWS=n;input.viewDirectionWS=GetWorldSpaceNormalizeViewDir(i.positionWS);input.shadowCoord=TransformWorldToShadowCoord(i.positionWS);input.bakedGI=SampleSH(n);input.normalizedScreenSpaceUV=GetNormalizedScreenSpaceUV(i.positionCS);input.shadowMask=half4(1,1,1,1);
  half3 color=UniversalFragmentPBR(input,surface).rgb;
  if(glass){
   color=SAMPLE_TEXTURE2D(_ScreenMap,sampler_ScreenMap,screenUV).rgb;
   // Live progress below the fixed-language label, only on the measured CRT glass.
   float2 pixel=floor(screenUV*256)/256;
   if(_Progress>=0&&pixel.x>=.12&&pixel.x<=.88&&pixel.y>=.22&&pixel.y<=.26)
    color=(pixel.x-.12)/.76<saturate(_Progress)?half3(.35,.8,.9):half3(.04,.12,.10);
  }
  return half4(MixFog(color,i.fog),1);
 }
 ENDHLSL
 }
 UsePass "Universal Render Pipeline/Lit/ShadowCaster"
 UsePass "Universal Render Pipeline/Lit/DepthOnly"
 }
}
