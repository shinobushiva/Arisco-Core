using UnityEngine;
using System.Collections;

public class GenericWalkerFactory : WalkerFactory
{
	
	//public int number = 1;
	public AAgent walkerPrefab;
	public Animator[] characters;

	public override AAgent GetAWalker ()
	{
		return GetAWalker(Random.Range (0, characters.Length));
	}

	public override AAgent GetAWalker (int n)
	{

		GameObject go = Instantiate<GameObject> (walkerPrefab.gameObject);
		WalkerModelChanger wmc = go.GetComponent<WalkerModelChanger>();

		wmc.factory = this;
		wmc.num = n;
		wmc.name = "Generic - " + characters [wmc.num].name;

		AAgent agent = wmc.GetComponent<AAgent> ();
		Animator anim = agent.GetComponentInChildren<Animator> ();
		Animator anim2 = Instantiate<Animator>(characters [n]);
		anim2.transform.SetParent (anim.transform.parent, true);
		anim.transform.CopyTo (anim2.transform);
		DestroyImmediate (anim.gameObject);


		return agent;
	}

	public override bool HasAWalker(int n){
		if (n > characters.Length-1)
			return false;

		return true;
	}

}
