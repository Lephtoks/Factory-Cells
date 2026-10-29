Shader "Custom/Blocks"
{
    Properties
    {
        _SpriteAtlas ("Sprite Atlas", 2D) = "white" {}
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
            Name "Blocks"

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

            struct BlockRepresenter
            {
                int2 position;
                int definitionIndex;
            };

            struct BlockReprDefinition
            {
                int spriteOffset;
                int spriteCount;
                float2 scale;
                float speed;
            };

            struct BlockSprite
            {
                float2 from;
                float2 to;
            };

            StructuredBuffer<BlockRepresenter> _InstanceData;
            StructuredBuffer<BlockReprDefinition>_Definitions;
            StructuredBuffer<BlockSprite>_Sprites;

            float4x4 _ObjectToWorld;

            sampler2D _SpriteAtlas;

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
                float valid      : TEXCOORD1;
            };

            Varyings vert(appdata_base v, uint svInstanceID : SV_InstanceID)
            {
                Varyings output;

                InitIndirectDrawArgs(0);
                uint instanceID = GetIndirectInstanceID(svInstanceID);

                BlockRepresenter instance = _InstanceData[instanceID];
                BlockReprDefinition definition = _Definitions[instance.definitionIndex];

                int frame = 0;
                if (definition.spriteCount > 1 && definition.speed != 0.0) {
                    frame = (int) fmod(
                        floor(_Time.y * definition.speed),
                        (float) definition.spriteCount);
                }

                BlockSprite sprite = _Sprites[definition.spriteOffset + frame];

                output.uv = lerp(sprite.from, sprite.to, v.texcoord.xy);

                float3 localPos = float3(
                    v.vertex.xy * definition.scale + float2(instance.position) + 0.5,
                    0.0
                );

                float4 worldPos = mul(
                    _ObjectToWorld,
                    float4(localPos, 1.0)
                );

                worldPos.z = -1;

                output.positionCS = mul(UNITY_MATRIX_VP, worldPos);
                output.valid = definition.spriteCount > 0 ? 1.0 : 0.0;

                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                clip(input.valid - 0.5);

                float4 color = tex2D(
                    _SpriteAtlas,
                    input.uv
                );

                clip(color.a - 0.001);

                return color;
            }

            ENDHLSL
        }
    }
}
