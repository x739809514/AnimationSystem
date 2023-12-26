using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class MyfirstPlayable : MonoBehaviour
{
    public AnimationClip clip;
    public PlayableGraph graph;
    private AnimationClipPlayable clipPlayable;

    // Start is called before the first frame update
    void Start()
    {
        graph = PlayableGraph.Create();
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        var output = AnimationPlayableOutput.Create(graph,"Animation Output",GetComponent<Animator>());
        clipPlayable = AnimationClipPlayable.Create(graph,clip);
        output.SetSourcePlayable(clipPlayable);
        graph.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(clipPlayable.GetPlayState()==PlayState.Playing)
            {
                clipPlayable.Pause();
            }
            else
            {
                clipPlayable.Play();
            }
        }
    }

    private void OnDisable() {
        graph.Destroy();
    }
}
