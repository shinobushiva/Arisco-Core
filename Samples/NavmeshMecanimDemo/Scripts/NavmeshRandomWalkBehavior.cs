using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NavmeshRandomWalkBehavior : AAnimatorBehavior
{
	private SpawningPoint[] sps;
	private Transform target;
	protected NavMeshAgent agent;
	
	public bool isOnNavMesh;
	
	public Transform initialDestination;

	void Start ()
	{
		
		agent = GetComponent<NavMeshAgent> ();
		if (initialDestination)
			agent.SetDestination (initialDestination.position);
	}

	void Update ()
	{
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
		target = transform;
	}

	protected void SetupAgentLocomotion ()
	{
		
		if (AgentDone ()) {
			AvatarLocomotion.Do (0, 0);
			SpawningPoint[] orderd = sps.OrderBy (x => Vector3.Distance (x.transform.position, transform.position)).ToArray ();
			
			target = orderd [Random.Range (1, orderd.Length)].transform;
			SetDestination (target.position);

		} else {
			
			float speed = agent.desiredVelocity.magnitude;
			
			Vector3 velocity = Quaternion.Inverse (transform.rotation) * agent.desiredVelocity;
			float angle = Mathf.Atan2 (velocity.x, velocity.z) * 180.0f / Mathf.PI;

			AvatarLocomotion.Do (speed, angle);
				
		}
	}

	void OnAnimatorMove ()
	{
//		print ("OnAnimatorMove()");
		if (!AttachedAgent.Began) {
			if(agent != null)
				agent.velocity = Vector3.zero;
			return;
		}
		
		//only perform if walking
		if (AvatarLocomotion.inWalkRun) {
			//set the navAgent's velocity to the velocity of the animation clip currently playing
			agent.velocity = Avatar.deltaPosition / Time.deltaTime;
			//set the rotation in the direction of movement
			if (agent.desiredVelocity != Vector3.zero)
				transform.rotation = Quaternion.LookRotation (agent.desiredVelocity);
		}else if (AvatarLocomotion.inTurn){
			if (agent.desiredVelocity != Vector3.zero)
				transform.rotation = Quaternion.LookRotation (agent.desiredVelocity);
		}else {
			agent.velocity = Vector3.zero;
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
	
	void Step ()
	{	
		SetupAgentLocomotion ();
	}

	void Commit ()
	{
		
	}
	
}
