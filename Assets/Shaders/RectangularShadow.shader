Shader "Custom/RectangularShadow"
{
    Properties
    {
        _ShadowColor ("Shadow Color", Color) = (0.0, 0.0, 0.0, 0.55)
        _LightDirection ("Light Direction (XY)", Vector) = (0.4, -0.7, 0, 0)
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
            Name "RectangularShadow"

            Stencil
            {
                Ref 1
                Comp NotEqual
                Pass Replace
                Fail Keep
                ZFail Keep
            }

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
            };

            struct InstanceData
            {
                float2 position;
                float2 size;
                float  shadowHeight;
            };

            StructuredBuffer<InstanceData> _InstanceData;

            float4x4 _ObjectToWorld;

            CBUFFER_START(UnityPerMaterial)
                float4 _ShadowColor;
                float4 _LightDirection;
            CBUFFER_END
            
            Varyings vert(appdata_base v, uint svInstanceID : SV_InstanceID)
            {
                Varyings output;

                InitIndirectDrawArgs(0);
                uint instanceID = GetIndirectInstanceID(svInstanceID);

                InstanceData data = _InstanceData[instanceID];

                float2 pos    = data.position;
                float2 size   = data.size;
                float  height = data.shadowHeight;

                float2 local = v.vertex.xy;

                float2 lightDir = normalize(_LightDirection.xy);
                float2 O        = lightDir * height;

                float2 farCorner = float2(
                    lightDir.x >= 0.0 ? 1.0 : 0.0,
                    lightDir.y >= 0.0 ? 1.0 : 0.0
                );

                const float eps = 0.001;
                bool isFarCorner = all(abs(local - farCorner) < eps);

                bool isAdjMid;
                if (farCorner.x > 0.5 && farCorner.y > 0.5)
                {
                    isAdjMid = all(abs(local - float2(0.5, 1.0)) < eps) ||
                               all(abs(local - float2(1.0, 0.5)) < eps);
                }
                else if (farCorner.x > 0.5 && farCorner.y < 0.5)
                {
                    isAdjMid = all(abs(local - float2(0.5, 0.0)) < eps) ||
                               all(abs(local - float2(1.0, 0.5)) < eps);
                }
                else if (farCorner.x < 0.5 && farCorner.y > 0.5)
                {
                    isAdjMid = all(abs(local - float2(0.5, 1.0)) < eps) ||
                               all(abs(local - float2(0.0, 0.5)) < eps);
                }
                else
                {
                    isAdjMid = all(abs(local - float2(0.5, 0.0)) < eps) ||
                               all(abs(local - float2(0.0, 0.5)) < eps);
                }

                float3 localPos;

                if (isFarCorner)
                {
                    localPos = float3(pos + local * size + O, 0.0);
                }
                else if (isAdjMid)
                {
                    float2 fixedLocal = 2.0 * local - farCorner;
                    localPos = float3(pos + fixedLocal * size + O, 0.0);
                }
                else
                {
                    localPos = float3(pos + local * size, 0.0);
                }

                float4 worldPos = mul(_ObjectToWorld, float4(localPos, 1.0));
                worldPos.z = -0.9f;

                output.positionCS = mul(UNITY_MATRIX_VP, worldPos);
                output.uv         = v.texcoord.xy;

                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                return _ShadowColor;
            }

            ENDHLSL
        }
    }
}