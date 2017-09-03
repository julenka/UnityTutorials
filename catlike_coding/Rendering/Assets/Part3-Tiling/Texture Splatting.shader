// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Texture Splatting"
{
    Properties{
        _SplatMap("Splat Map", 2D) = "white" {}
        [NoScaleOffset] _Texture1("Texture 1", 2D) = "white" {}
        [NoScaleOffset] _Texture2("Texture 2", 2D) = "white" {}
    }
    SubShader{
        Pass {
            CGPROGRAM

                #pragma vertex MyVertexProgram
                #pragma fragment MyFragmentProgram

                #include "UnityCG.cginc"

                sampler2D _SplatMap;
                sampler2D _Texture1;
                sampler2D _Texture2;
                float4 _SplatMap_ST;

                struct Interpolators {
                    float4 position : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float2 uvSplat : TEXCOORD1;
                };

                struct VertexData {
                    float4 position : POSITION;
                    float2 uv : TEXCOORD0;
                };

                Interpolators MyVertexProgram(VertexData v) {
                    Interpolators i;
                    i.uv = TRANSFORM_TEX(v.uv, _SplatMap);
                    i.uvSplat = v.uv;
                    i.position = UnityObjectToClipPos(v.position);
                    return i;
                }

                float4 MyFragmentProgram(Interpolators i) : SV_TARGET {
                    float4 color1 = tex2D(_Texture1, i.uv);
                    float4 color2 = tex2D(_Texture2, i.uv);
                    float blend = tex2D(_SplatMap, i.uvSplat).r;
                    float4 color = color1 * blend + color2 * (1 - blend);
                    return color;
                }

            ENDCG
        }
    }

}
