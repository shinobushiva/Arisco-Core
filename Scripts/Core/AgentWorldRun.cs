using UnityEngine;
using System.Collections;
using Arisco.Core;

public class AgentWorldRun : SingletonMonoBehaviour<AgentWorldRun>
{
	
	public World world;

	[HideInInspector]
	public CellAutomatonWorldRunner runner;
	private GameObject worldCopy;
	
	public IEnumerator Start ()
	{
		GameObject go = new GameObject ("CellAutomatonWorldRunner");
		runner = go.AddComponent<CellAutomatonWorldRunner> ();
		yield return StartCoroutine (Init ());
	}
	
	IEnumerator Init ()
	{
		print ("AgentWorldRun#Init");
		while (runner.Running && !runner.Paused) {
			yield return new WaitForEndOfFrame ();
		}

		if (world == null) {
			world = FindObjectOfType<World> ();
		}

		if (world == null) {
			yield break;
		}

		
		if (!worldCopy) {
			worldCopy = (GameObject)GameObject.Instantiate (world.gameObject);
			worldCopy.name = world.gameObject.name;
			worldCopy.name = worldCopy.name + "_Copy";
			worldCopy.transform.SetParent(world.transform.parent);
		}
		
		yield return new WaitForEndOfFrame ();
		DestroyImmediate (world.gameObject);
		
		world = ((GameObject)GameObject.Instantiate (worldCopy)).GetComponent<World> ();
		world.gameObject.name = worldCopy.name.Replace ("_Copy", "");
		world.transform.SetParent(worldCopy.transform.parent);
		
		yield return new WaitForEndOfFrame ();
		world.gameObject.SetActive (true);
		
		yield return new WaitForEndOfFrame ();
		worldCopy.SetActive (false);
		
		runner.World = world;

		if (AriscoChart.Instance)
			AriscoChart.Instance.Init ();
		
		AAgent[] agents = world.gameObject.GetComponentsInChildren<AAgent> ();
		
		foreach (AAgent a in agents) {
			world.RegisterAgent (a);
		}
		runner.Initialize ();
	}

	public void Play ()
	{
		if (runner == null || runner.World == null)
			return;
		
		if (!gameObject.activeSelf)
			return;

		if (!runner.World.Ended) {
			runner.Run ();
		} else {
			StartCoroutine (_Play ());
		}
	}

	private IEnumerator _Play ()
	{
		yield return StartCoroutine (Init ());
		runner.Run ();
	}
	
	public void Pause ()
	{
		if (!gameObject.activeSelf)
			return;

		runner.Pause (!runner.Paused);
	}

	public void Step ()
	{
		if (!gameObject.activeSelf)
			return;

		if (!runner.Running) {
			Play ();
			runner.Pause (true);
		} else {
			runner.Pause (true);
			runner.OneStep ();
		}
	}
	
	public void Stop ()
	{
		if (!gameObject.activeSelf)
			return;

		if (runner.Running) {
			runner.Stop ();
		} else {
			StartCoroutine (Init ());
		}
	}
}
