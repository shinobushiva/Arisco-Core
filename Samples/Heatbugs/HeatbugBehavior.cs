using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeatbugBehavior : ABehavior {

	public float idealTemperture = 100;
	public float heatAmount = 100;

    private SpeedDirectionBehavior sp;

	public override void Initialize ()
	{
		idealTemperture = Random.Range(idealTemperture/10, idealTemperture);
		heatAmount = Random.Range(heatAmount/10, heatAmount);

        sp = GetComponent<SpeedDirectionBehavior>();
	}

	public override void Step ()
	{
		List<AAgent> list = GetAgentsAroundPosition(AttachedAgent.World, transform.position, .5f, false);
		HeatFieldBehavior hf = list[0].GetComponent<HeatFieldBehavior>();
        if (hf == null)
        {
            return;
        }
		float heatHere = hf.Amount;

		float unhappiness = Mathf.Abs(idealTemperture - heatHere);
		if(unhappiness == 0){
			sp.Speed = 0;
		}else{
			sp.Speed = 1;
		}

		hf.Amount = hf.Amount + heatAmount;
	}

	// Use this for initialization
	void Start () {
	
	}
	

}
