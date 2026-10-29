using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Cells.Object;
using Core;
using UnityEngine;

namespace GameRenderer
{
    public class BlockRenderer : ObjectIndirectRenderer<Block, BlockRepresenter>
    {
        public BlockRenderer(GameObject gameObject, Mesh quad, Material material, int capacity = 10000) : base(
            gameObject, quad, material, capacity) {
        }
        
        public static List<BlockReprDefinition> Definitions;
        public static List<BlockSprite> Sprites;
        private static GraphicsBuffer _buffer;
        private static GraphicsBuffer _spritesBuffer;
        
        public override void Render() {
            UpdateCPU();
            base.Render();
        }
        private void UpdateCPU()
        {
        }


        public override void Init() {
            base.Init();
        }

        public static void InitStatic() {
            Definitions = new List<BlockReprDefinition>();
            Sprites = new List<BlockSprite>();

            var animations = AssetProvider.Instance.registry.render.BlockAnimations;
            if (animations != null) {
                foreach (var animation in animations) {
                    if (animation != null) {
                        AddDefinition(animation);
                    }
                }
            }

            _buffer?.Release();
            _buffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, Mathf.Max(1, Definitions.Count), Marshal.SizeOf<BlockReprDefinition>());
            if (Definitions.Count > 0) {
                _buffer.SetData(Definitions);
            }

            _spritesBuffer?.Release();
            _spritesBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, Mathf.Max(1, Sprites.Count), Marshal.SizeOf<BlockSprite>());
            if (Sprites.Count > 0) {
                _spritesBuffer.SetData(Sprites);
            }

            AssetProvider.Instance.registry.render.BlockMaterial.SetTexture("_SpriteAtlas", AssetProvider.Instance.registry.render.BlockAtlas);
            AssetProvider.Instance.registry.render.BlockMaterial.SetBuffer("_Definitions", _buffer);
            AssetProvider.Instance.registry.render.BlockMaterial.SetBuffer("_Sprites", _spritesBuffer);
        }

        private static int AddDefinition(BlockSprite[] animation, Vector2 scale, float speed = 1f) {
            var definition = new BlockReprDefinition {
                SpriteOffset = animation is { Length: > 0 } ? Sprites.Count : 0,
                SpriteCount = animation?.Length ?? 0,
                Speed = speed,
                Scale = scale
            };

            if (animation is { Length: > 0 }) {
                Sprites.AddRange(animation);
            }

            Definitions.Add(definition);
            return Definitions.Count - 1;
        }

        private static int AddDefinition(BlockAnimation animation) {
            if (animation == null) {
                throw new ArgumentNullException(nameof(animation));
            }

            return AddDefinition(ToBlockSprites(animation.Sprites), animation.Scale, animation.Speed);
        }

        private static BlockSprite[] ToBlockSprites(Sprite[] sprites) {
            if (sprites == null) {
                return Array.Empty<BlockSprite>();
            }

            var result = new BlockSprite[sprites.Length];
            for (var i = 0; i < sprites.Length; i++) {
                if (sprites[i] != null) {
                    result[i] = ToBlockSprite(sprites[i]);
                }
            }

            return result;
        }

        private static BlockSprite ToBlockSprite(Sprite sprite) {
            var texture = sprite.texture;
            var rect = sprite.textureRect;

            return new BlockSprite {
                From = new Vector2(rect.xMin / texture.width, rect.yMin / texture.height),
                To = new Vector2(rect.xMax / texture.width, rect.yMax / texture.height)
            };
        }
    }
}