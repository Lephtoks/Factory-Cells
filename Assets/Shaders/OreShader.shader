Shader "Custom/Ores"
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
            Name "Drops"

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
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
            };

            struct OreItem
            {
                int2 position;
                int  spriteIndex;
            };

            StructuredBuffer<OreItem> _InstanceData;

            float4x4 _ObjectToWorld;

            sampler2D _SpriteAtlas;

            float4 _SpriteAtlas_TexelSize;

            #define SPRITE_SIZE 16.0

            Varyings vert(appdata_base v, uint svInstanceID : SV_InstanceID)
            {
                Varyings output;

                InitIndirectDrawArgs(0);
                uint instanceID = GetIndirectInstanceID(svInstanceID);

                OreItem instance = _InstanceData[instanceID];
                float3 localPos =
                    v.vertex.xyz + 
                        float3(instance.position, 0) + 
                        float3(0.5f, 0.5f, 0.0f);

                float4 worldPos = mul(
                    _ObjectToWorld,
                    float4(localPos, 1.0)
                );

                worldPos.z = -1.0;

                output.positionCS = mul(UNITY_MATRIX_VP, worldPos);
                output.uv = (float2(instance.spriteIndex * SPRITE_SIZE, 0.0) + v.texcoord.xy * SPRITE_SIZE) *
                    _SpriteAtlas_TexelSize.xy;

                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
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