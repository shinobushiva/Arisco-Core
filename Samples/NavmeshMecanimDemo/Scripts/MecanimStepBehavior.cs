using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AAgent))]
public class MecanimStepBehavior : AAnimatorBehavior
{
	protected UnityEngine.AI.NavMeshAgent agent;

	void Initialize ()
	{
		recording = -1;
	}

	int recording = -1;

	void Update ()
	{
		if (AttachedAgent.World && !AttachedAgent.World.timeTicking) {
			if(recording < 0)
				return;

			//print (""+recording+" : "+(avatar.recorderStopTime - avatar.recorderStartTime));
			if (recording >= 0 && recording < 1) {
				recording++;
			}else if (recording >= 1) {
				Avatar.StopRecording ();
				Avatar.StartPlayback ();
				Avatar.speed = 0;
				recording = -1;
			}
		}
	}

	void Step ()
	{
		if (recording == -1) {
			Avatar.StopPlayback ();
			Avatar.StartRecording (1);
			Avatar.speed = 1;
			recording = 0;
		}

		//avatar.SetFloat ("Speed", 1, 0.25f, Time.deltaTime);
		//avatar.speed = 1;

	}
	
	void End ()
	{
		//avatar.SetFloat ("Speed", 0);
		Avatar.StartPlayback ();
		Avatar.speed = 0;
		recording = -1;
	}

}
