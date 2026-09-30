using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameRenderer
{
    public class IndirectRenderer
    {
        internal static readonly int INSTANCE_DATA_ID = Shader.PropertyToID("_InstanceData");
    }

    public class IndirectRenderer<T> : IndirectRenderer where T : struct
    {
        protected readonly int Capacity;
        protected readonly Material Material;
        protected readonly Mesh Quad;
        protected readonly GameObject GameObject;
        public IndirectRenderer(GameObject gameObject, Mesh quad, Material material, int capacity = 10000) {
            GameObject = gameObject;
            Capacity = capacity;
            Material = material;
            Quad = quad;
        }
        
        private GraphicsBuffer _instanceBuffer;
        private GraphicsBuffer.IndirectDrawIndexedArgs[] _args;
        private MaterialPropertyBlock _properties;
        
        public int Count { get; private set; }
        
        protected RenderParams RenderParams;
        protected GraphicsBuffer ArgsBuffer;
        
        private T[] _cpuData;
        private int _ids = 0;
        private readonly BidirectionalDictionary<int, int> _id2Index = new BidirectionalDictionary<int, int>();
        
        public void Init()
        {
            _cpuData = new T[Capacity];

            _instanceBuffer = new GraphicsBuffer(
                GraphicsBuffer.Target.Structured,
                Capacity,
                Marshal.SizeOf<T>());

            _properties = new MaterialPropertyBlock();

            _properties.SetBuffer(
                INSTANCE_DATA_ID,
                _instanceBuffer);
            
            SetArgs();
            SetRenderParams();
        }

        protected virtual void SetArgs() {
            _args = new GraphicsBuffer.IndirectDrawIndexedArgs[1];
            
            ArgsBuffer = new GraphicsBuffer(
                GraphicsBuffer.Target.IndirectArguments,
                1,
                GraphicsBuffer.IndirectDrawIndexedArgs.size);
            
            _args[0].indexCountPerInstance =
                Quad.GetIndexCount(0);

            _args[0].instanceCount = 0;

            _args[0].startIndex =
                Quad.GetIndexStart(0);

            _args[0].baseVertexIndex =
                Quad.GetBaseVertex(0);

            _args[0].startInstance =
                0;
        }

        protected virtual void SetRenderParams() {
            RenderParams = new RenderParams(Material)
            {
                worldBounds = new Bounds(
                    Vector3.zero,
                    Vector3.one * 10000f),

                matProps = _properties,

                shadowCastingMode =
                    ShadowCastingMode.Off,

                receiveShadows = false,

                layer = GameObject.layer
            };
        }

        protected void CPU2GPU() {
            _instanceBuffer.SetData(
                _cpuData,
                0,
                0,
                Count);
            
            _args[0].instanceCount =
                (uint)Count;
            
            ArgsBuffer.SetData(_args);
        }

        public void Render() {
            if (Count == 0) return;
            
            CPU2GPU();

            Graphics.RenderMeshIndirect(
                RenderParams,
                Quad,
                ArgsBuffer,
                1);
        }

        public void Release() {
            _instanceBuffer?.Release();
            ArgsBuffer?.Release();

            _instanceBuffer = null;
            ArgsBuffer = null;
            
            _args = null;
            _id2Index.Clear();
        }
        
        public int Add(T value) {
            if (Count >= Capacity)
                return -1;
            _cpuData[Count] = value;
            _id2Index.Add(_ids, Count++);
            return _ids++;
        }

        public void Remove(int id) {
            var index = _id2Index[id];
            _id2Index.Remove(id);

            Count--;
            if (index != Count) {
                _cpuData[index] = _cpuData[Count];
                var swapId = _id2Index.Inverse[Count];
                _id2Index[swapId] = index;
            }
            
        }
    }
}