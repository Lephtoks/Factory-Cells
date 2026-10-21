Shader "Custom/Drops"
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
                float2 localPosition : TEXCOORD1;
            };

            struct DropItem
            {
                int2 position;
                int2 targetPosition;
                int  spriteIndex;
            };

            StructuredBuffer<DropItem> _InstanceData;

            float4x4 _ObjectToWorld;
            float4 _LocalClipRect;

            // [0, 1]
            float _GlobalTime;

            sampler2D _SpriteAtlas;

            float4 _SpriteAtlas_TexelSize;

            #define SPRITE_SIZE 16.0

            Varyings vert(appdata_base v, uint svInstanceID : SV_InstanceID)
            {
                Varyings output;

                InitIndirectDrawArgs(0);
                uint instanceID = GetIndirectInstanceID(svInstanceID);

                DropItem instance = _InstanceData[instanceID];
                float3 localPos =
                    v.vertex.xyz + 
                        float3(lerp(float2(instance.position), float2(instance.targetPosition), _GlobalTime), 0) + 
                        float3(0.5f, 0.5f, 0.0f);

                float4 worldPos = mul(
                    _ObjectToWorld,
                    float4(localPos, 1.0)
                );

                worldPos.z = -1.0;

                output.positionCS = mul(UNITY_MATRIX_VP, worldPos);
                output.uv = (float2(instance.spriteIndex * SPRITE_SIZE, 0.0) + v.texcoord.xy * SPRITE_SIZE) *
                    _SpriteAtlas_TexelSize.xy;
                output.localPosition = localPos.xy;

                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                float2 p = input.localPosition;

                clip(p.x - _LocalClipRect.x);
                clip(p.y - _LocalClipRect.y);
                clip(_LocalClipRect.z - p.x);
                clip(_LocalClipRect.w - p.y);
                
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