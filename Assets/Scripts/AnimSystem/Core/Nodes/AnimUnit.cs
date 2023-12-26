using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace AnimSystem.Core
{
    public class AnimUnit : AnimBehaviour
    {
        private AnimationClipPlayable clipPlayable;
        private AnimationClip clip;
        private double animStopTime;

        public AnimUnit(PlayableGraph graph,AnimationClip clip,float enterTime=0f):base(graph,enterTime)
        {
            this.clip = clip;
            clipPlayable = AnimationClipPlayable.Create(graph,clip);
            animAdapter.AddInput(clipPlayable,0,1.0f);
            Disable();
        }


        public override void Enable()
        {
            base.Enable();
            animAdapter.SetTime(animStopTime);
            clipPlayable.SetTime(animStopTime);
            animAdapter.Play();
            clipPlayable.Play();
        }

        public override void Disable()
        {
            base.Disable();
            animAdapter.Pause();
            clipPlayable.Pause();
            animStopTime = animAdapter.GetTime();
        }

        public override float GetAnimLength()
        {
            return clip.length;
        }
    }
}
