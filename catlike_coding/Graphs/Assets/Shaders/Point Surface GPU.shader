Shader "Graph/Point Surface GPU" {
    Properties {
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
    }
    SubShader {
        CGPROGRAM
        #pragma surface ConfigureSurface Standard fullforwardshadows addshadow
        #pragma instancing_options procedural:ConfigureProcedural
        #pragma editor_sync_compilation

        #pragma target 4.5
        float _Smoothness;
        // Store scale in first component and inverse scale in second component
        float2 _Scale;

        struct Input {
            float3 worldPos;
        };

		#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
			StructuredBuffer<float3> _Positions;
		#endif

		void ConfigureProcedural () {
			#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
				float3 position = _Positions[unity_InstanceID];
                unity_ObjectToWorld = 0.0;
                unity_ObjectToWorld._m03_m13_m23_m33 = float4(position, 1.0);
				unity_ObjectToWorld._m00_m11_m22 = _Scale.x;

                unity_WorldToObject = 0.0;
				unity_WorldToObject._m03_m13_m23_m33 = float4(-position, 1.0);
				unity_WorldToObject._m00_m11_m22 = 1.0 / _Scale.y;
			#endif
		}

        void ConfigureSurface (Input input, inout SurfaceOutputStandard surface) {
            surface.Albedo = saturate(input.worldPos * 0.5 + 0.5);
            surface.Smoothness = _Smoothness;
        }

        ENDCG
    }
    
    FallBack "Diffuse"
}