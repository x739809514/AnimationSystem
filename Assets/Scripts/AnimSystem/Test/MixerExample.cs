using AnimSystem.Core;
using UnityEngine;
using UnityEngine.Playables;

public class MixerExample : MonoBehaviour
{
    private Mixer mixer;
    private PlayableGraph graph;
    public AnimationClip[] clips;
    public float enterTime;
    private void Start()
    {
        graph = PlayableGraph.Create();
        mixer = new Mixer(graph);
        for (int i = 0; i < clips.Length; i++)
        {
            var unit = new AnimUnit(graph, clips[i], enterTime);
            mixer.AddInput(unit);
        }
        AnimHelper.SetOutPut(graph,mixer,GetComponent<Animator>());
        AnimHelper.Go(graph,mixer);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            mixer.TransitionTo(0);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            mixer.TransitionTo(1);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            mixer.TransitionTo(2);
        }
    }

    private void OnDisable()
    {
        mixer.Disable();
    }
}