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

		
			int i = Random.Range (0, sps.Length);
			NavMeshAgent nma = a.GetComponent<NavMeshAgent> ();
			if (nma)
				nma.enabled = false;

			a.transform.position = sps [i].transform.position;

			if (nma)
				nma.enabled = true;

			a.transform.Translate(new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f)));

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
