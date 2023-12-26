using UnityEngine;
using UnityEngine.Playables;

namespace AnimSystem.Core
{
    public abstract class AnimBehaviour 
    {
        public bool enabled;
        protected Playable animAdapter;
        private PlayableGraph graph;
        protected float remainTime;
    
        protected float EnterTime
        {
            get;
            private set;
        }

        protected float AnimLength
        {
            get;
            private set;
        }
        public PlayableGraph playableGraph;

        protected AnimBehaviour(){}

        protected AnimBehaviour(PlayableGraph graph, float enter=0)
        {
            this.graph = graph;
            EnterTime = enter;
            AnimLength = float.NaN;
            animAdapter = ScriptPlayable<AnimAdapter>.Create(graph);
            ((ScriptPlayable<AnimAdapter>)animAdapter).GetBehaviour().Init(this);
            playableGraph= graph;
        }
    
        public Playable GetAnimAdapter()
        {
            return animAdapter;
        }


#region override
        protected virtual void AddInput(Playable playable)
        {
        }
    
        public virtual void AddInput(AnimBehaviour behavior)
        {
            AddInput(behavior.GetAnimAdapter());
        }

        public virtual void AddInput(AnimationClip clip,float enterTime=0)
        {
            AddInput(new AnimUnit(this.graph,clip,enterTime));
        }
    
        protected virtual void Stop()
        {
        }

        public virtual float GetEnterTime()
        {
            return EnterTime;
        }

        public virtual float GetAnimLength()
        {
            return AnimLength;
        }
    
        public virtual void Enable()
        {
            enabled=true;
            remainTime = GetAnimLength();
        }

        public virtual void Disable()
        {
            enabled=false;
        }

        public virtual void Execute(Playable playable,FrameData info)
        {
            if(enabled==false)return;
            remainTime = remainTime > 0 ? remainTime - info.deltaTime : 0f;
        }

#endregion
    
    }
}
