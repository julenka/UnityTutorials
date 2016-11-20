#if !defined(MY_LIGHTING_INCLUDED)
#define MY_LIGHTING_INCLUDED

#include "AutoLight.cginc"
#include "UnityPBSLighting.cginc"
#include "AutoLight.cginc"
#pragma target 3.0
#pragma vertex MyVertexProgram
#pragma fragment MyFragmentProgram


float4 _Tint;
sampler2D _MainTex, _DetailTex;
float4 _MainTex_ST, _DetailTex_ST;
float _Smoothness;
float4  _Metallic;
// From bump mapping section (6.2)
//sampler2D _HeightMap;
sampler2D _NormalMap, _DetailNormalMap;
float4 _HeightMap_TexelSize;
float _BumpScale, _DetailBumpScale;

struct Interpolators {
    float4 pos : SV_POSITION;
    float4 uv : TEXCOORD0;
    float3 normal : TEXCOORD1;
#if defined(BINORMAL_PER_FRAGMENT)
    float4 tangent : TEXCOORD2;
#else
    float3 tangent : TEXCOORD2;
    float3 binormal : TEXCOORD3;
#endif

    float3 worldPos : TEXCOORD4;

    SHADOW_COORDS(5)

    #if defined(VERTEXLIGHT_ON)
        float3 vertexLightColor : TEXCOORD5;
    #endif
};

struct VertexData {
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
    float3 normal : NORMAL;
    float4 tangent : TANGENT;
};

UnityIndirect CreateIndirectLight(Interpolators i) {
    UnityIndirect indirectLight;
    indirectLight.diffuse = 0;
    indirectLight.specular = 0;

#if defined(VERTEXLIGHT_ON)
    indirectLight.diffuse = i.vertexLightColor;
#endif

#if defined(FORWARD_BASE_PASS)
    indirectLight.diffuse += max(0, ShadeSH9(float4(i.normal, 1)));
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

float3 CreateBinormal(float3 normal, float3 tangent, float binormalSign) {
    return cross(normal, tangent.xyz) *
        (binormalSign * unity_WorldTransformParams.w);
}

Interpolators MyVertexProgram(VertexData v) {
    Interpolators i;
    i.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
    i.uv.zw = TRANSFORM_TEX(v.uv, _DetailTex);
    i.pos = mul(UNITY_MATRIX_MVP, v.vertex);
    i.normal = UnityObjectToWorldNormal(v.normal);
#if defined(BINORMAL_PER_FRAGMENT)
    i.tangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
#else
    i.tangent = UnityObjectToWorldDir(v.tangent.xyz);
    i.binormal = CreateBinormal(i.normal, i.tangent, v.tangent.w);
#endif

    i.tangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
    i.worldPos = mul(unity_ObjectToWorld, v.vertex);

    TRANSFER_SHADOW(i);

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

    UNITY_LIGHT_ATTENUATION(attenuation, i, i.worldPos);

    light.color = _LightColor0.rgb * attenuation;
    light.ndotl = DotClamped(i.normal, light.dir);
    return light;
}

void InitializeFragmentNormal(inout Interpolators i) {
    // Using Unity's convenience function, but the code to decode normals is this:
    //i.normal.xy = tex2D(_NormalMap, i.uv).wy * 2 - 1;
    //i.normal.xy *= _BumpScale;
    //i.normal.z = sqrt(1 - saturate(dot(i.normal.xy, i.normal.xy)));

    float3 mainNormal =
        UnpackScaleNormal(tex2D(_NormalMap, i.uv.xy), _BumpScale);
    float3 detailNormal =
        UnpackScaleNormal(tex2D(_DetailNormalMap, i.uv.zw), _DetailBumpScale);
    float3 tangentSpaceNormal = BlendNormals(mainNormal, detailNormal);
#if defined(BINORMAL_PER_FRAGMENT)
    float3 binormal = CreateBinormal(i.normal, i.tangent.xyz, i.tangent.w);
#else
    float3 binormal = i.binormal;
#endif
    
    i.normal = normalize(
        tangentSpaceNormal.x * i.tangent +
        tangentSpaceNormal.y * binormal +
        tangentSpaceNormal.z * i.normal
    ); 

    i.normal = i.normal.xyz;
    i.normal = normalize(i.normal);
    
    
    // From bump mapping: 6.1
    //// This performs bump mapping
    //float2 du = float2(_HeightMap_TexelSize.x * 0.5, 0);
    //float u1 = tex2D(_HeightMap, i.uv - du);
    //float u2 = tex2D(_HeightMap, i.uv + du);

    //float2 dv = float2(0, _HeightMap_TexelSize.y * 0.5);
    //float v1 = tex2D(_HeightMap, i.uv - dv);
    //float v2 = tex2D(_HeightMap, i.uv + dv);

    //// We could compute the normal as follows, by taking cross product
    //// of the du, dv components...
    ////float3 tu = float3(1, u2 - u1, 0);
    ////float3 tv = float3(0, v2 - v1, 1);
    ////i.normal = cross(tv, tu);

    //// ...or we can calculate the cross product and realize that
    //// we don't need to actually do a cross product
    //i.normal = float3(u1 - u2, 1, v1 - v2);
    //i.normal = normalize(i.normal);
}

float4 MyFragmentProgram(Interpolators i) : SV_TARGET{
    InitializeFragmentNormal(i);
    float3 albedo = tex2D(_MainTex, i.uv.xy).rgb * _Tint.rgb;
    albedo *= tex2D(_DetailTex, i.uv.zw) * unity_ColorSpaceDouble;

    float oneMinusReflectivity;
    float3 specularTint;
    albedo = DiffuseAndSpecularFromMetallic(albedo, _Metallic, specularTint, oneMinusReflectivity);

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

    return float4(diffuse + specular, 1);
    */
}
#endif
