using UnityEngine.Playables;

namespace AnimSystem.Core
{
    public class Root : AnimBehaviour
    {
        private Playable playable;
        public Root(PlayableGraph graph) : base(graph)
        {
        
        }

        protected override void AddInput(Playable playable)
        {
            this.playable = playable;
            animAdapter.AddInput(playable, 0, 1.0f);
        }

        public override void Enable()
        {
            for (int i = 0; i < animAdapter.GetInputCount(); i++)
            {
                var adapter = AnimHelper.GetAdapter(animAdapter.GetInput(i));
                adapter.Enable();
            }
        }

        public override void Disable()
        {
            for (int i = 0; i < animAdapter.GetInputCount(); i++)
            {
                var adapter = AnimHelper.GetAdapter(animAdapter.GetInput(i));
                adapter.Disable();
            }
        }
    }
}