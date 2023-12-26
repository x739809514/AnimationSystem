using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace AnimSystem.Core
{
    public class RandomSelector : AnimBehaviour
    {
        private AnimationMixerPlayable mixer;
        private PlayableGraph graph;
        private List<float> enterTimeOfClips;
        private List<float> animLengthOfClips;
        private int CurIndex { get; set; }
        private int ClipCount { get; set; }

        public RandomSelector(PlayableGraph graph) : base(graph)
        {
            this.graph = graph;
            enterTimeOfClips = new List<float>();
            animLengthOfClips = new List<float>();
            mixer = AnimationMixerPlayable.Create(this.graph);
            animAdapter.AddInput(mixer, 0, 1.0f);
            CurIndex = -1;
        
        }

        protected override void AddInput(Playable playable)
        {
            mixer.AddInput(playable, 0);
            ClipCount++;
        }

        public override void AddInput(AnimationClip clip,float enterTime=0f)
        {
            base.AddInput(clip);
            animLengthOfClips.Add(clip.length);
            enterTimeOfClips.Add(enterTime);
        }

        public override void Enable()
        {
            if (CurIndex < 0 || CurIndex > ClipCount) return;
            mixer.SetInputWeight(CurIndex, 1.0f);
            AnimHelper.Enable(mixer.GetInput(CurIndex));
            animAdapter.SetTime(0);
            animAdapter.Play();
            mixer.SetTime(0);
            mixer.Play();
        }

        public override void Disable()
        {
            if (CurIndex < 0 || CurIndex > ClipCount) return;
            mixer.SetInputWeight(CurIndex,0.0f);
            AnimHelper.Disable(mixer.GetInput(CurIndex));
            animAdapter.Pause();
            mixer.Pause();
        }


        public void Select()
        {
            CurIndex = Random.Range(0, ClipCount);
        }

        public override float GetEnterTime()
        {
            if (enterTimeOfClips.Count==0)
            {
                return 0;
            }
            return enterTimeOfClips[CurIndex];
        }

        public override float GetAnimLength()
        {
            if (animLengthOfClips.Count==0)
            {
                return 0;
            }
            return animLengthOfClips[CurIndex];
        }
    }
}