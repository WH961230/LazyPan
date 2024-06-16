using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;

public class RuntimeGraph : MonoBehaviour
{
	public BaseGraph	graph;
	public ProcessGraphProcessor	processor;

	public GameObject	assignedGameObject;
	public GameObject	assignedCompGameObject;

	private void Start()
	{
		if (graph != null)
			processor = new ProcessGraphProcessor(graph);
	}

	int i = 0;

    void Update()
    {
		if (graph != null)
		{
			graph.SetParameterValue("Input", (float)i++);
			graph.SetParameterValue("GameObject", assignedGameObject);
			graph.SetParameterValue("CompGameObject", assignedCompGameObject);
			processor.Run();
		}
    }
}
