using AnimSystem.Core;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class RandomSelectorExample : MonoBehaviour
{
    public AnimationClip[] clips;
    private RandomSelector selector;
    private void Start()
    {
        var graph = PlayableGraph.Create();
        selector = new RandomSelector(graph);
        for (int i = 0; i < clips.Length; i++)
        {
            var playable = AnimationClipPlayable.Create(graph, clips[i]);
            if (playable.IsNull())
            {
                Debug.Log("playable is null");
            }
            else
            {
                selector.AddInput(clips[i],0.1f);
            }
            
        }
        AnimHelper.SetOutPut(graph,selector,GetComponent<Animator>());
        selector.Select();
        AnimHelper.Go(graph,selector);
    }

    public void OnDisable()
    {
        selector.Disable();
    }
}