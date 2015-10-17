using UnityEngine;
using System.Collections;

public class StackDetector : ABehavior {

	public float timeToDetect = 5f;
	public float minDistance = 3f;

	private bool detectionStarted = false;
	private float detectionStartTime;

	private float ccRadius;
	private float navmeshRadius;

	private Bounds bounds;

	
	protected Animator animator;
	protected NavmeshRandomWalkBehavior navmeshWalker;


	// Use this for initialization
	void Begin () {
		ccRadius = GetComponent<CharacterController> ().radius;
		navmeshRadius = GetComponent<NavMeshAgent> ().radius;
		animator = GetComponent<Animator> ();
		navmeshWalker = GetComponent<NavmeshRandomWalkBehavior> ();
	}
	
	// Update is called once per frame
	void Step () {

		if (!detectionStarted) {
			GetComponent<CharacterController> ().radius = ccRadius;
			GetComponent<NavMeshAgent> ().radius = navmeshRadius;

			detectionStarted = true;
			bounds = new Bounds (transform.position, Vector3.one);
			detectionStartTime = Time.time;
		} else {
			bounds.Encapsulate (transform.position);
		}

		if (bounds.extents.magnitude > minDistance) {
			detectionStarted = false;
			return;
		}

		if (Time.time > detectionStartTime + timeToDetect) {
			GetComponent<CharacterController> ().radius = .1f;
			GetComponent<NavMeshAgent> ().radius = .1f;

			RaycastHit hit;
			if(Physics.SphereCast(transform.position+Vector3.up*1.5f, 0.5f, transform.forward*0.5f,out hit, 1f)){
				animator.SetFloat("Speed", 0);
			}
			detectionStarted = false;
			navmeshWalker.SetDestination(transform.position);
			return;
		}

		{
			RaycastHit hit;
			if (Physics.SphereCast (transform.position + Vector3.up * 1.5f, 0.5f, transform.forward * 0.5f, out hit, 1f)) {
				animator.SetFloat ("Speed", 0);
			}
		}

	}

	void OnDrawGizmos(){
		Gizmos.DrawWireSphere (transform.position + transform.forward*0.5f + Vector3.up*1.5f, 0.5f);
	}
}
