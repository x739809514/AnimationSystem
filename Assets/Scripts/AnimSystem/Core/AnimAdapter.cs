using UnityEngine.Playables;

namespace AnimSystem.Core
{
    public class AnimAdapter : PlayableBehaviour
    {
        public AnimBehaviour animBehaviour;
        public void Init(AnimBehaviour animBehaviour)
        {
            this.animBehaviour = animBehaviour;
        }

        public void Enable()
        {
            animBehaviour?.Enable();
        }

        public void Disable()
        {
            animBehaviour?.Disable();
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            animBehaviour?.Execute(playable,info);
        }

        public float GetAnimEnterTime()
        {
            return animBehaviour.GetEnterTime();
        }
    }
}
