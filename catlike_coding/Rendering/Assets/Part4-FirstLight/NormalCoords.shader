Shader "Unlit/NormalCoords"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram
        
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            v2f MyVertexProgram(appdata v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.normal = v.normal;
                return o;
            }

            fixed4 MyFragmentProgram(v2f i) : SV_Target
            {
                // sample the texture
                float t = i.normal.y * i.normal.z;
                return t > 0 ? t : float4(1, 0, 0, 1) * -t;
            }
            ENDCG
        }
    }
}
