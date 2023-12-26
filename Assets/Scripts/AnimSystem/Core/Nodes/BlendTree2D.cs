using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace AnimSystem.Core
{
    [Serializable]
    public struct ClipData
    {
        public AnimationClip clip;
        public Vector2 pos;
    }

    public class BlendTree2D : AnimBehaviour
    {
        private AnimationMixerPlayable mix;
        private ComputeShader computeShader;
        private ComputeBuffer computeBuffer;
        private int kernel;
        private float eps;
        private int posX;
        private int posY;
        private Vector2 pointer;
        private int clipCount;
        private float total;

        private struct DataPair
        {
            public float x;
            public float y;
            public float output;
        }

        private DataPair[] dataPairs;

        public BlendTree2D(PlayableGraph graph, float enterTime, ClipData[] clips, float eps=1e-5f) : base(graph, enterTime)
        {
            mix = AnimationMixerPlayable.Create(graph);
            animAdapter.AddInput(mix, 0, 1.0f);
            dataPairs = new DataPair[clips.Length];
            this.eps = eps;
            for (int i = 0; i < clips.Length; i++)
            {
                mix.AddInput(AnimationClipPlayable.Create(graph, clips[i].clip), 0, 0);
                dataPairs[i].x = clips[i].pos.x;
                dataPairs[i].y = clips[i].pos.y;
            }

            computeShader = AnimHelper.GetCompute();
            computeBuffer = new ComputeBuffer(clips.Length, 12);
            kernel = computeShader.FindKernel("Compute");
            computeShader.SetBuffer(kernel, "dataBuffer", computeBuffer);
            computeShader.SetFloat("eps", eps);
            posX = Shader.PropertyToID("pointerX");
            posY = Shader.PropertyToID("pointerY");
            clipCount = clips.Length;
            pointer.Set(1, 1);
            SetPoint(0,0);
        }

        public void SetPoint(float x, float y)
        {
            if (Mathf.Approximately(pointer.x, x) && Mathf.Approximately(pointer.y, y))
            {
                return;
            }

            pointer.Set(x, y);
            computeShader.SetFloat(posX, x);
            computeShader.SetFloat(posY, y);
            computeBuffer.SetData(dataPairs);
            computeShader.Dispatch(kernel,clipCount,1,1);
            computeBuffer.GetData(dataPairs);
            for (int i = 0; i < clipCount; i++)
            {
                total += dataPairs[i].output;
            }

            for (int i = 0; i < clipCount; i++)
            {
                mix.SetInputWeight(i,dataPairs[i].output/total);
            }

            total = 0f;
        }

        public override void Enable()
        {
            base.Enable();
            SetPoint(0,0);
            for (int i = 0; i < clipCount; i++)
            {
                mix.GetInput(i).Play();
                mix.GetInput(i).SetTime(0);
            }
        
            mix.SetTime(0);
            mix.Play();
            animAdapter.SetTime(0);
            animAdapter.Play();
        }

        public override void Disable()
        {
            base.Disable();
            for (int i = 0; i < clipCount; i++)
            {
                mix.GetInput(i).Pause();
            }
            mix.Pause();
            animAdapter.Pause();
        }

        protected override void Stop()
        {
            base.Stop();
            computeBuffer.Dispose();
        }
    }
}