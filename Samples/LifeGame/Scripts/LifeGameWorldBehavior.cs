using UnityEngine;
using System.Collections;

public class LifeGameWorldBehavior : WorldBehavior
{
	
	public AAgent lifePrefab;

	public override void Initialize ()
	{	

		//int num = AriscoGUI.Instance.Get<int> ("num", 20);
		//bool torus = AriscoGUI.Instance.Get<bool>("torus", true);
		//Camera.main.transform.position = new Vector3 (0,  0, -num);
        int num = 20;
        bool torus = true;

		if(GetComponent<LimitedWorld>()){
			GetComponent<LimitedWorld>().size = new Vector3(num, 1, num);
		}
		
		float offset = num / 2;
		
		for (int i=0; i<num; i++) {
			for (int j=0; j<num; j++) {
                AAgent a = CreateAgent (AttachedWorld, lifePrefab, new Vector2 (i - offset+0.5f, j - offset+0.5f));
			}
		}

		
		LimitedWorld tw = GetComponent<LimitedWorld>();
        if (tw)
        {
            tw.size = Vector3.one * num;
            tw.AdjustMainCamera();
            tw.offset = Vector3.one * (num%2 == 0 ? -.5f : 0);
        }
	}
	 
	void Start(){
	}
}
