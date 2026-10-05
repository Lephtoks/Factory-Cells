Shader "Custom/DestroyedBlocks"
{
    Properties
    {
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
            Name "DestroyedBlocks"

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest Always
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
                float  alpha      : TEXCOORD1;
            };

            // xy = позиция
            // z  = время создания [0, 4)
            StructuredBuffer<float3> _InstanceData;

            float4x4 _ObjectToWorld;

            // Глобальное текущее время [0, 4)
            float _GlobalTime;

            // Глобальный спрайт
            sampler2D _DestroyedBlockSprite;

            Varyings vert(appdata_base v, uint svInstanceID : SV_InstanceID)
            {
                Varyings output;

                InitIndirectDrawArgs(0);
                uint instanceID = GetIndirectInstanceID(svInstanceID);

                float3 blockData = _InstanceData[instanceID];

                float creationTime = blockData.z;

                // Время жизни от момента создания.
                // Учитываем зацикливание [0, 4).
                float age = _GlobalTime - creationTime;

                if (age < 0.0)
                    age += 4.0;
                
                v.vertex.xy *= 1.5f;
                
                // Последние 2 секунды жизни — fade out.
                //
                // age = 0..2 -> alpha = 1
                // age = 2..4 -> alpha = 1..0
                float alpha = 1.0 - smoothstep(2.0, 4.0, age);
                
                float3 localPos =
                    v.vertex.xyz +
                    float3(blockData.xy, 0.0) + 
                        float3(0.5f, 0.5f, 0.25f);

                float4 worldPos = mul(
                    _ObjectToWorld,
                    float4(localPos, 1.0)
                );

                worldPos.z = -1.0;

                output.positionCS = mul(UNITY_MATRIX_VP, worldPos);
                output.uv = v.texcoord.xy;
                output.alpha = alpha;

                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                float4 color = tex2D(
                    _DestroyedBlockSprite,
                    input.uv
                );
                color.rgb /= 2.5;
                
                color.a *= input.alpha;

                clip(color.a - 0.001);

                return color;
            }

            ENDHLSL
        }
    }
}