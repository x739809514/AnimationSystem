using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace AnimSystem.Core
{
    public static class AnimHelper
    {
        public static AnimAdapter GetAdapter(Playable playable)
        {
            if (typeof(AnimAdapter).IsAssignableFrom(playable.GetPlayableType()))
            {
                return ((ScriptPlayable<AnimAdapter>)playable).GetBehaviour();
            }

            return null;
        }

        public static void Enable(Playable playable)
        {
            var adapter = GetAdapter(playable);
            if (adapter != null)
                adapter.Enable();
        }

        public static void Disable(Playable playable)
        {
            var adapter = GetAdapter(playable);
            if (adapter != null)
                adapter.Disable();
        }

        public static void SetOutPut(PlayableGraph graph,AnimBehaviour behaviour,Animator animator)
        {
            var output = AnimationPlayableOutput.Create(graph,"Animation Output",animator);
            output.SetSourcePlayable(behaviour.GetAnimAdapter());
        }

        public static void Go(PlayableGraph graph, AnimBehaviour behaviour)
        {
            ((ScriptPlayable<AnimAdapter>)behaviour.GetAnimAdapter()).GetBehaviour().Enable();
            graph.Play();
        }

        public static ComputeShader GetCompute()
        {
            var res = Resources.Load<ComputeShader>("Shaders/BlendTree2D");
            var shader = Object.Instantiate(res);
            return shader;
        }
    }
}