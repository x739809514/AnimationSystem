using AnimSystem.Core;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Animator))]
public class AnimUnitTest : MonoBehaviour
{
    public AnimationClip clip;
    private PlayableGraph graph;
    private AnimUnit animUnit;

    // Start is called before the first frame update
    void Start()
    {
        graph = PlayableGraph.Create();
        animUnit = new AnimUnit(graph,clip);
        var root = new Root(graph);
        root.AddInput(animUnit);
        AnimHelper.SetOutPut(graph,root,GetComponent<Animator>());
        root.Enable();
        graph.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(animUnit.enabled)
            {
                animUnit.Disable();
            }else
            {
                animUnit.Enable();
            }
        }
    }

    private void OnDisable() 
    {
        graph.Destroy();
    }
}
