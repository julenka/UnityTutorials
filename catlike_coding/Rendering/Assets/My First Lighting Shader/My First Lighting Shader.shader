Shader "Custom/My First Lighting Shader"
{
    Properties{
        _Tint("Tint", Color) = (1,1,1,1)
        _MainTex("Albedo", 2D) = "white" {}
        [NoScaleOffset] _NormalMap("Normals", 2D) = "bump" {}
        _BumpScale ("Bump Scale", Float) = 1
        _DetailTex("Detail Texture", 2D) = "gray" {}
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
        [Gamma] _Metallic ("Metallic", Range(0, 1)) = 0
        [NoScaleOffset] _DetailNormalMap("Detail Normals", 2D) = "bump" {}
        _DetailBumpScale("Detail Bump Scale", Float) = 1
    }


    CGINCLUDE

    #define BINORMAL_PER_FRAGMENT

    ENDCG

    SubShader{
        Pass {
            Tags {
                "LightMode" = "ForwardBase"
            }

            CGPROGRAM
            #pragma target 3.0

            #pragma multi_compile _ VERTEXLIGHT_ON
            #pragma multi_compile _ SHADOWS_SCREEN
            #define FORWARD_BASE_PASS

            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram
            #include "My Lighting.cginc"
            ENDCG
        }
        Pass{
            Tags{
            "LightMode" = "ForwardAdd"
            }

            Blend One One
            ZWrite Off
            CGPROGRAM

            #pragma target 3.0

            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram

            #pragma multi_compile_fwdadd_fullshadows
            #include "My Lighting.cginc"

            ENDCG
        }

        Pass{
            Tags{
            "LightMode" = "ShadowCaster"
            }

            CGPROGRAM

            #pragma target 3.0
            
            #pragma multi_compile_shadowcaster

            #pragma vertex MyShadowVertexProgram
            #pragma fragment MyShadowFragmentProgram

            #include "My Shadows.cginc"

            ENDCG
        }
    }

}
