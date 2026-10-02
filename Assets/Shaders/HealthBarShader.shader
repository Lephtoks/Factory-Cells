Shader "Custom/HealthBar"
{
    Properties
    {
        _FillColor ("Fill Color", Color) = (0.1, 1.0, 0.1, 1.0)
        _BackgroundColor ("Background Color", Color) = (0.15, 0.15, 0.15, 1.0)
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        Pass
        {
            Name "HealthBar"

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest LEqual
            Cull Off

            HLSLPROGRAM

            #pragma target 4.5
            #pragma vertex vert
            #pragma fragment frag

            #define UNITY_INDIRECT_DRAW_ARGS IndirectDrawIndexedArgs
            #include "UnityCG.cginc"
            #include "UnityIndirect.cginc"

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
                float  health     : TEXCOORD1;
            };

            StructuredBuffer<float3> _InstanceData;

            float4x4 _ObjectToWorld;

            CBUFFER_START(UnityPerMaterial)
                float4 _FillColor;
                float4 _BackgroundColor;
            CBUFFER_END

            Varyings vert(appdata_base v, uint svInstanceID : SV_InstanceID)
            {
                Varyings output;

                InitIndirectDrawArgs(0);
                uint instanceID = GetIndirectInstanceID(svInstanceID);

                float3 barData = _InstanceData[instanceID];
                // barData.xy — offset
                // barData.z  — fill amount (0..1)
                v.vertex.y /= 8;
                float3 localPos = v.vertex.xyz + float3(barData.xy, 0.0) + float3(0.5f, -0.2, 0);

                float4 worldPos = mul(_ObjectToWorld, float4(localPos, 1.0));

                output.positionCS = mul(UNITY_MATRIX_VP, worldPos);
                output.uv         = v.texcoord.xy;
                output.health     = barData.z;

                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                float health = saturate(input.health);

                if (input.uv.x > health)
                    return _BackgroundColor;

                return _FillColor;
            }

            ENDHLSL
        }
    }
}