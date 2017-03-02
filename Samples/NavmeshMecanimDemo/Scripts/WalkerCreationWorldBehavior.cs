using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;


public class WalkerCreationWorldBehavior : WorldBehavior
{
	public int number = 1000;
	private SpawningPoint[] sps;
	private int num = 0;
	private int counter = 0;

	public int waitSteps = 50;

	public WalkerFactory factory;

	public override void Step ()
	{
		if ((counter++) % waitSteps != 0) {
			return;
		}

		if (num < number) {
			AAgent pref = factory.GetAWalker ();
			AAgent a = CreateAgent (AttachedWorld, pref);

			SpawningPoint[] sps2 = sps.Where(x => x.spawnHere).ToArray();
		
			int i = Random.Range (0, sps2.Length);
			UnityEngine.AI.NavMeshAgent nma = a.GetComponent<UnityEngine.AI.NavMeshAgent> ();
			if (nma)
				nma.enabled = false;

			a.transform.position = sps2 [i].transform.position;
			a.transform.Translate(new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f)));

			if (nma)
				nma.enabled = true;

			num++;
		}
		
		//print ("Pedestrian Counts : " + num);

	}

	public override void Initialize ()
	{
		print ("PedestrianCreationWorldBehavior#Initialize");


		sps = FindObjectsOfType<SpawningPoint> ();


	}
}
