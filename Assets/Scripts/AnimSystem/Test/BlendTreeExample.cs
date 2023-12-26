using AnimSystem.Core;
using UnityEngine;
using UnityEngine.Playables;

public class BlendTreeExample : MonoBehaviour
{
    public Vector2 pointer;
    public ClipData[] clips;
    public ComputeShader shader;

    private BlendTree2D m_blend;
    private PlayableGraph m_graph;

    private void Start()
    {
        m_graph = PlayableGraph.Create();

        m_blend = new BlendTree2D(m_graph, 0.1f,clips);

        AnimHelper.SetOutPut(m_graph,m_blend,GetComponent<Animator>());
        AnimHelper.Go(m_graph,m_blend);
    }

    private void Update()
    {
        m_blend.SetPoint(pointer.x,pointer.y);
    }

    private void OnDisable()
    {
        m_graph.Destroy();
    }
}