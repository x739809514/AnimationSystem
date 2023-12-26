using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class PlayableQueenBehavior : PlayableBehaviour
{
    private float timeToNextClip;
    private int curClipIndex;
    public Playable mixer;

    public void Initialize(AnimationClip[] clips, Playable owner, PlayableGraph graph)
    {
        owner.SetInputCount(1);
        mixer = AnimationMixerPlayable.Create(graph,clips.Length);
        graph.Connect(mixer,0,owner,0);
        owner.SetInputWeight(0,1.0f);
        for(int index=0; index<mixer.GetInputCount();++index)
        {
            graph.Connect(AnimationClipPlayable.Create(graph,clips[index]),0,mixer,index);
            mixer.SetInputWeight(index,1.0f);
        }
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        if(mixer.GetInputCount()==0) return;
        timeToNextClip-=info.deltaTime;
        if(timeToNextClip<0.0f)
        {
            curClipIndex++;
            if(curClipIndex>=mixer.GetInputCount())
            {
                curClipIndex=0;
            }
            var curClip = (AnimationClipPlayable)mixer.GetInput(curClipIndex);
            curClip.SetTime(0);
            timeToNextClip = curClip.GetAnimationClip().length;
        }

        for(int i = 0; i< mixer.GetInputCount(); i++)
        {
            if(i == curClipIndex)
            {
                mixer.SetInputWeight(i,1.0f);
            }else{
                mixer.SetInputWeight(i,0f);
            }
        }
    }
}

[RequireComponent(typeof(Animator))]
public class PlayableQueen : MonoBehaviour 
{
    private PlayableGraph graph;
    public AnimationClip[] clips;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        graph = PlayableGraph.Create();
        var playableQueenPlayable = ScriptPlayable<PlayableQueenBehavior>.Create(graph);
        var playQueue = playableQueenPlayable.GetBehaviour();
        playQueue.Initialize(clips,playableQueenPlayable,graph);
        var playableOutPut = AnimationPlayableOutput.Create(graph,"Animation Output",GetComponent<Animator>());
        playableOutPut.SetSourcePlayable(playableQueenPlayable,0);
        graph.Play();
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        graph.Destroy();
    }
}