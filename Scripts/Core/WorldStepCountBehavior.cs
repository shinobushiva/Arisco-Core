using UnityEngine;
using System.Collections;

public class WorldStepCountBehavior : WorldBehavior
{
	
	private int stepCount = 0; 
	public int StepCount{
		get{
			return stepCount;
		}
	}

	public bool showStepCount = false;

	
	public override void Begin ()
	{
		stepCount = 0;
	}
	
	public override void Step ()
	{
		stepCount++;
	}
	
	void OnGUI ()
	{
		if(showStepCount)
			GUI.Label (new Rect (Screen.width - 200, 0, 200, 40), "" + stepCount);
	}
	
}
