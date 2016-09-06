Shader "Custom/My First Lighting Shader"
{
    Properties{
        _Tint("Tint", Color) = (1,1,1,1)
        _MainTex("Albedo", 2D) = "white" {}
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
        [Gamma] _Metallic ("Metallic", Range(0, 1)) = 0
    }
        SubShader{
            Pass {
        
                Tags {
                    "LightMode" = "ForwardBase"
                }

                CGPROGRAM
                    #pragma target 3.0
                    #pragma vertex MyVertexProgram
                    #pragma fragment MyFragmentProgram

                    #include "UnityPBSLighting.cginc"

                    float4 _Tint;
                    sampler2D _MainTex;
                    float4 _MainTex_ST;
                    float _Smoothness;
                    float4  _Metallic;

                    struct Interpolators {
                        float4 position : SV_POSITION;
                        float2 uv : TEXCOORD0;
                        float3 normal : TEXCOORD1;
                        float3 worldPos : TEXCOORD2;
                    };

                    struct VertexData {
                        float4 position : POSITION;
                        float2 uv : TEXCOORD0;
                        float3 normal : NORMAL;
                    };

                    Interpolators MyVertexProgram(VertexData v) {
                        Interpolators i;
                        i.uv = TRANSFORM_TEX(v.uv, _MainTex);
                        i.position = mul(UNITY_MATRIX_MVP, v.position);
                        i.normal = UnityObjectToWorldNormal(v.normal);
                        i.worldPos = mul(unity_ObjectToWorld, v.position);
                        return i;
                    }

                    float4 MyFragmentProgram(Interpolators i) : SV_TARGET {
                        float3 albedo = tex2D(_MainTex, i.uv).rgb * _Tint.rgb;
                        float oneMinusReflectivity;
                        float3 specularTint;
                        albedo = DiffuseAndSpecularFromMetallic(albedo, _Metallic, specularTint, oneMinusReflectivity);

                        i.normal = normalize(i.normal);
                        float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                        float3 lightDir = _WorldSpaceLightPos0.xyz;
                        float3 lightColor = _LightColor0.rgb;

                        UnityLight light;
                        light.color = lightColor;
                        light.dir = lightDir;
                        light.ndotl = DotClamped(i.normal, lightDir);

                        UnityIndirect indirectLight;
                        indirectLight.diffuse = 0;
                        indirectLight.specular = 0;

                        return UNITY_BRDF_PBS(albedo, specularTint, oneMinusReflectivity, _Smoothness, i.normal, viewDir, light, indirectLight);
                        /*
                        
                        float3 reflectionDir = reflect(-lightDir, i.normal);
                        float3 halfVector = normalize(lightDir + viewDir);
                        
                        
                        float3 diffuse = albedo * DotClamped(lightDir, i.normal) * lightColor;

                        float3 specular = specularTint * lightColor * pow(
                            DotClamped(halfVector, i.normal),
                            _Smoothness * 100
                        );
                      
                        return float4(diffuse + specular, 1);*/
                    }

                ENDCG
            }
    }

}
