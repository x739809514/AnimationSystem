using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class PlayableMixTree : MonoBehaviour
{
    public AnimationClip clip1;
    public AnimationClip clip2;
    [Range(0,1)]
    public float weight;
    private PlayableGraph graph;
    private AnimationMixerPlayable mixerPlayable;

    // Start is called before the first frame update
    void Start()
    {
        graph = PlayableGraph.Create();
        var playableOutPut = AnimationPlayableOutput.Create(graph,"Animation Output", GetComponent<Animator>());
        mixerPlayable = AnimationMixerPlayable.Create(graph,2);
        playableOutPut.SetSourcePlayable(mixerPlayable);
        //create clip and contact them
        var animClip1 = AnimationClipPlayable.Create(graph,clip1);
        var animClip2 = AnimationClipPlayable.Create(graph,clip2);
        graph.Connect(animClip1,0,mixerPlayable,0);
        graph.Connect(animClip2,0,mixerPlayable,1);
        graph.Play();
    }

    // Update is called once per frame
    void Update()
    {
        weight = Mathf.Clamp01(weight);
        mixerPlayable.SetInputWeight(0,1.0f-weight);
        mixerPlayable.SetInputWeight(1,weight);
    }

    private void OnDisable() 
    {
        graph.Destroy();
    }
}
