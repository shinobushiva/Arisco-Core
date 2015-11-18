using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SchelingWorldBehavior : WorldBehavior
{
	
	public AAgent pennyPrefab;
	public AAgent dimePrefab;
	public AAgent emptyPrefab;

	//
    private SchelingAgentBehavior[] emptyAgents;
	private float rate;
    private List<SchelingAgentBehavior> all;
	//
	private bool allAgentAtOnce = false;
	private List<float> satisfiedRates;

	void Initialize ()
	{	

		int num = 8;
		float rateToRemove = 0.25f;
		int numToRemove = (int)(num * num * rateToRemove);

		Camera.main.transform.position = new Vector3 (0, num, 0);

        float offset = num / 2 + (num%2 == 0 ? -.5f : 0);

		int[,] map = new int[num, num];
		
		for (int i=0; i<num; i++) {
			for (int j=0; j<num; j++) {
				map [i, j] = 0;

				if (i == 0 && j == 0)
					continue;
				if (i == num - 1 && j == 0)
					continue;
				if (i == 0 && j == num - 1)
					continue;
				if (i == num - 1 && j == num - 1)
					continue;

				map [i, j] = ((i + j) % 2) + 1;
			}
		}

		int counter = 0;
		while (counter <= numToRemove) {
			int i = Random.Range (0, num);
			int j = Random.Range (0, num);

			if (map [i, j] == 1 || map [i, j] == 2) {
				map [i, j] = -1;
				counter ++;
			}
		}

		
		for (int i=0; i<num; i++) {
			for (int j=0; j<num; j++) {
				int type = map [i, j];

				if (type == -1) {
					AAgent a = CreateAgent (AttachedWorld, emptyPrefab);
					a.transform.position = new Vector3 (i - offset, 0, j - offset);
				} else if (type == 1) {
					AAgent a = CreateAgent (AttachedWorld, pennyPrefab);
					a.transform.position = new Vector3 (i - offset, 0, j - offset);
				} else if (type == 2) {
					AAgent a = CreateAgent (AttachedWorld, dimePrefab);
					a.transform.position = new Vector3 (i - offset, 0, j - offset);
				}
			}
		}

		AriscoChart.Instance.AddChart ("satisfied", "Satisfied Rate", AriscoChart.ChartType.Line, 100, 50);
		AriscoChart.Instance.AddChart ("satisfied_pie", "Satisfied Rate", AriscoChart.ChartType.Pie, 100, 50);
	}

	void Begin ()
	{

		rate = 0.66f;
		if (true) {
            all = GetAllAgents<SchelingAgentBehavior>().OrderBy (x => Random.value).ToList ();
		} else {
            all = GetAllAgents<SchelingAgentBehavior>();
		}
        emptyAgents = all.Where (x => x.type == SchelingAgentBehavior.Type.Empty).ToArray ();
		
		satisfiedRates = new List<float> ();
	}

	private int counter = 0;
	private bool peopleMoved = false;

	void Step ()
	{
        List<SchelingAgentBehavior> agents = all;

		bool flag = false;
		allAgentAtOnce = false;
	
		while (!flag) {
            SchelingAgentBehavior agent = agents [counter++];
			if (counter >= agents.Count) {
				if (!peopleMoved) {
					AttachedWorld.EndRequest = true;
					return;
				}
				flag = true;
				counter = 0;
				peopleMoved = false;
			}

			if (agent.type == SchelingAgentBehavior.Type.Empty)
				continue;

			if (agent.IsSatisfied (agent.transform.position, rate)) {
				continue;
			}

            SchelingAgentBehavior[] sorted = emptyAgents.OrderBy (x => Vector3.Distance (x.transform.position, agent.transform.position)).ToArray ();
            foreach (SchelingAgentBehavior aa in sorted) {
				if (agent.IsSatisfied (aa.transform.position, rate)) {
					Vector3 pos = aa.transform.position;
					aa.transform.position = agent.transform.position;
					agent.transform.position = pos;

					if (!allAgentAtOnce)
						flag = true;
					peopleMoved = true;
					break;
				}
			}
		}
	}

	void Commit ()
	{
		//UpdateChart ();
	}

	void UpdateChart ()
	{
		int satisfied = 0;
        foreach (SchelingAgentBehavior sa in all) {
			if (sa.type == SchelingAgentBehavior.Type.Empty)
				continue;
			
			if (sa.IsSatisfied (sa.transform.position, rate)) {
				satisfied++;
			}
		}
		float sfr = satisfied / (float)(all.Count - emptyAgents.Length);
		satisfiedRates.Add (sfr);

		List<object> titles = new List<object> (){
			"Step", "Rate"
		};
		
		List<List<object>> values = new List<List<object>> ();
		for (int i=0; i< satisfiedRates.Count; i++) {
			float f = satisfiedRates [i];
			values.Add (new List<object> (){i, f});
		}
		AriscoChart.Instance.SetDataString ("satisfied",
			AriscoChart.Instance.ToDataString (titles, values)
		);
		
		titles = new List<object> (){
			"Satfied?", "Number"
		};
		values = new List<List<object>> ();
		values.Add (new List<object> (){"Satisfied", satisfied});
		values.Add (new List<object> (){"Unsatisfied", all.Count-emptyAgents.Length -satisfied});
		
		AriscoChart.Instance.SetDataString ("satisfied_pie",
			AriscoChart.Instance.ToDataString (titles, values)
		);
	}

	void End ()
	{
		UpdateChart ();
		print ("Simulation End");
	}


}
