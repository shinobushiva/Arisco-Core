using UnityEngine;
using System.Collections;

public class GenericWalkerFactory : WalkerFactory
{
	
	//public int number = 1;
	public AAgent walkerPrefab;
	public Animator[] characters;

	public override AAgent GetAWalker ()
	{
		WalkerModelChanger wmc = walkerPrefab.gameObject.GetComponent<WalkerModelChanger> ();
		wmc.num = Random.Range (0, characters.Length);
		wmc.characters = characters;
		wmc.name = "Generic - " + characters [wmc.num].name;

		return walkerPrefab;
	}
}
