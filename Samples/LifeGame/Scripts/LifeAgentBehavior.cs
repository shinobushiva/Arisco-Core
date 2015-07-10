using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LifeAgentBehavior : ABehavior
{
	
	private bool alive = false;
	private bool nextAlive;

	public bool Alive {
		get {
			return alive;
		}
		set {
			if (value) {
				GetComponent<Renderer>().material.color = Color.red;
			} else {
				GetComponent<Renderer>().material.color  = Color.gray;
			}
			alive = value;
		}
	}
    
    private List<LifeAgentBehavior> agents;


	public override void Initialize ()
	{
		float rate = AriscoGUI.Instance.Get<float> ("rate", 0.5f);
		Alive = (Random.Range (0f, 1f) > rate);
	}

	public override void Begin ()
	{
		agents = GetAgentsAroundPosition<LifeAgentBehavior> (AttachedAgent.World, transform.position, AComponent.MOORE, false, true); 
	}
	
	public override void Step ()	
	{
		int aliveNum = agents.Where (x => x.Alive).Count ();
		
		if (alive) {
			if (aliveNum <= 1 || aliveNum >= 4) {
				nextAlive = false;
			}
		} else {
			if (aliveNum == 3) {
				nextAlive = true;
			}
		}
	}

    public override void Commit ()
    {
        Alive = nextAlive;
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }


    void OnGUI(){
        if (agents == null)
            return;

        int aliveNum = agents.Where (x => x.Alive).Count ();

        Vector3 v = Camera.main.WorldToScreenPoint(transform.position);
        
        GUI.Label(new Rect(v.x-4, Screen.height - v.y-13, 30, 30), ""+aliveNum);
    }


    public void Start(){
        
    }

}
