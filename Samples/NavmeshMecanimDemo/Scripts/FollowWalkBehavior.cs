using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FollowWalkBehavior : AAnimatorBehavior
{
	public Transform target;
	protected NavMeshAgent agent;

	public Vector3 offset;
	
	public bool isOnNavMesh;

	void Start ()
	{
		
		agent = GetComponent<NavMeshAgent> ();
		agent.SetDestination (target.TransformPoint(offset));
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
		

	}

	void Begin ()
	{
	}

	protected void SetupAgentLocomotion ()
	{
		
		if (AgentDone ()) {
			AvatarLocomotion.Do (0, 0);

		} else {
			
			float speed = agent.desiredVelocity.magnitude;
			
			Vector3 velocity = Quaternion.Inverse (transform.rotation) * agent.desiredVelocity;
			float angle = Mathf.Atan2 (velocity.x, velocity.z) * 180.0f / Mathf.PI;

			AvatarLocomotion.Do (speed, angle);
				
		}


		SetDestination (target.TransformPoint(offset));
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
