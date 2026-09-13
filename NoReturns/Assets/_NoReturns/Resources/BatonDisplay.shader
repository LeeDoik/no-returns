Shader "NoReturns/BatonDisplay" {
 Properties { _MainTex("Frame",2D)="white"{} _Color("Tint",Color)=(1,1,1,1) _Gauge("Gauge mode",Float)=0 _Charge("Charge",Range(0,1))=1 }
 SubShader { Tags {"RenderType"="Opaque" "RenderPipeline"="UniversalPipeline"} Pass {
 ZWrite On ZTest LEqual Cull Off
 HLSLPROGRAM
 #pragma vertex vert
 #pragma fragment frag
 #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
 struct A {float4 positionOS:POSITION;float2 uv:TEXCOORD0;};
 struct V {float4 positionCS:SV_POSITION;float2 uv:TEXCOORD0;};
 TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);float4 _Color;float _Gauge,_Charge;
 V vert(A v){V o;o.positionCS=TransformObjectToHClip(v.positionOS.xyz);o.uv=v.uv;return o;}
 half4 frag(V i):SV_Target{
 if(_Gauge<.5)return SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv)*_Color;
 float2 p=floor(saturate(i.uv)*float2(48,96));
 bool ready=_Charge>=1;
 half3 light=ready?half3(.25,1,.48):half3(1,.55,.08);
 half3 color=half3(.015,.025,.02);
 float bar=floor((p.y-7)/6);
 bool inside=p.x>=8&&p.x<40&&p.y>=7&&bar<12&&fmod(p.y-7,6)<4;
 if(inside)color=(p.y-7)<saturate(_Charge)*72?light:half3(.10,.14,.12);
 float t=p.x-16;float checkY=t<5?85-t:80+(t-5);
 bool check=t>=0&&t<14&&p.y>=checkY&&p.y<checkY+3;
 bool dash=p.x>=17&&p.x<31&&p.y>=84&&p.y<87;
 if(ready?check:dash)color=light;
 return half4(color,1);
}
 ENDHLSL
 } }
}
