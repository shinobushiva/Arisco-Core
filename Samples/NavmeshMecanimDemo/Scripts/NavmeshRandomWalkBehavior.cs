using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NavmeshRandomWalkBehavior : ABehavior
{
	private SpawningPoint[] sps;
	private Transform target;
	protected NavMeshAgent		agent;
	protected Animator			animator;
	protected Locomotion 		locomotion;

	public bool isOnNavMesh;

	public Transform initialDestination;


	void Start(){
		
		agent = GetComponent<NavMeshAgent> ();
		if (initialDestination)
			agent.SetDestination (initialDestination.position);
	}

	void Update(){
		isOnNavMesh = agent.isOnNavMesh;

	}

	public void SetDestination (Vector3 pos)
	{
		if (agent.isOnNavMesh) {
			agent.destination = pos;
		}
	}

	void Initialize ()
	{
		agent = GetComponent<NavMeshAgent> ();
		agent.updateRotation = false;
		agent.updatePosition = true;


		sps = FindObjectsOfType<SpawningPoint> ();
	}
	
	void Begin ()
	{
		//SpawningPoint sp = sps.OrderBy (x => (Vector3.Distance (x.transform.position, transform.position))).ToArray () [0];
		
		animator = GetComponent<Animator> ();
		locomotion = new Locomotion (animator);
		target = transform;
//		SetDestination (target.position);
	}

	bool newTarget = false;

	protected void SetupAgentLocomotion ()
	{

//		if (newTarget && !AgentDone ()) {
//			newTarget = false;
//			return;
//		}

		if (!newTarget && AgentDone ()) {
			locomotion.Do (0, 0);
			SpawningPoint[] orderd = sps.OrderBy(x=>Vector3.Distance(x.transform.position, transform.position)).ToArray();

			target = orderd [Random.Range (1, orderd.Length)].transform;
			SetDestination (target.position);

//			newTarget = true;
		} else {

			float speed = agent.desiredVelocity.magnitude;
			
			Vector3 velocity = Quaternion.Inverse (transform.rotation) * agent.desiredVelocity;
			float angle = Mathf.Atan2 (velocity.x, velocity.z) * 180.0f / Mathf.PI;
			
			locomotion.Do (speed, angle);

		}
	}
	
	protected bool AgentDone ()
	{
		if (!agent.isOnNavMesh)
			return true;

		return !agent.pathPending && AgentStopping ();
	}
	
	protected bool AgentStopping ()
	{
		return agent.remainingDistance <= agent.stoppingDistance;
	}

//	void Update(){
//		if (AttachedAgent.World && !AttachedAgent.World.timeTicking) {
//			agent.updatePosition = false;
//			agent.Stop();
//		} else {
//			agent.updatePosition = true;
//			agent.Resume();
//
//		}
//	}

	void Step ()
	{	
		SetupAgentLocomotion ();
	}

	void Commit ()
	{

	}

}
