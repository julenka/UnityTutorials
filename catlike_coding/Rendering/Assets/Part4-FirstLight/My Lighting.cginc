#if !defined(MY_LIGHTING_INCLUDED)
#define MY_LIGHTING_INCLUDED

#include "AutoLight.cginc"
#include "UnityPBSLighting.cginc"
#pragma target 3.0
#pragma vertex MyVertexProgram
#pragma fragment MyFragmentProgram


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

    #if defined(VERTEXLIGHT_ON)
        float3 vertexLightColor : TEXCOORD3;
    #endif
};

struct VertexData {
    float4 position : POSITION;
    float2 uv : TEXCOORD0;
    float3 normal : NORMAL;
};

UnityIndirect CreateIndirectLight(Interpolators i) {
    UnityIndirect indirectLight;
    indirectLight.diffuse = 0;
    indirectLight.specular = 0;

#if defined(VERTEXLIGHT_ON)
    indirectLight.diffuse = i.vertexLightColor;
#endif
    return indirectLight;
}

void ComputeVertexLightColor(inout Interpolators i) {
#if defined(VERTEXLIGHT_ON)
    i.vertexLightColor = Shade4PointLights(
        unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
        unity_LightColor[0].rgb, unity_LightColor[1].rgb,
        unity_LightColor[2].rgb, unity_LightColor[3].rgb,
        unity_4LightAtten0, i.worldPos, i.normal
    );
#endif
}

Interpolators MyVertexProgram(VertexData v) {
    Interpolators i;
    i.uv = TRANSFORM_TEX(v.uv, _MainTex);
    i.position = mul(UNITY_MATRIX_MVP, v.position);
    i.normal = UnityObjectToWorldNormal(v.normal);
    i.worldPos = mul(unity_ObjectToWorld, v.position);
    ComputeVertexLightColor(i);
    return i;
}

UnityLight CreateLight(Interpolators i) {
    UnityLight light;
#if defined(POINT) || defined(SPOT) || defined(POINT_COOKIE)
    light.dir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
#else
    light.dir = _WorldSpaceLightPos0.xyz;
#endif
    UNITY_LIGHT_ATTENUATION(attenuation, 0, i.worldPos);
    light.color = _LightColor0.rgb * attenuation;
    light.ndotl = DotClamped(i.normal, light.dir);
    return light;
}

float4 MyFragmentProgram(Interpolators i) : SV_TARGET{
    float3 albedo = tex2D(_MainTex, i.uv).rgb * _Tint.rgb;
    float oneMinusReflectivity;
    float3 specularTint;
    albedo = DiffuseAndSpecularFromMetallic(albedo, _Metallic, specularTint, oneMinusReflectivity);

    i.normal = normalize(i.normal);
    float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

    return UNITY_BRDF_PBS(albedo, specularTint, oneMinusReflectivity, _Smoothness, i.normal, viewDir, CreateLight(i), CreateIndirectLight(i));
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
#endif
