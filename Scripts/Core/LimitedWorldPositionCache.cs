using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arisco.Core;
using System;


[RequireComponent(typeof(LimitedWorld))]
public class LimitedWorldPositionCache : WorldBehavior
{
    
    public LimitedWorld lw;

    public SpeedDirectionBehavior[,,] cache; 

    public void Cache(SpeedDirectionBehavior a, float x, float y, float z){
        cache[(int)x,(int)y,(int)z] = a;
    }

    public override void Initialize()
    {
        cache = new SpeedDirectionBehavior[(int)lw.size.x, (int)lw.size.y, (int)lw.size.z];
    }

    public override void Begin(){

    }

    public override void Commit()
    {

    }

    public override void Dispose()
    {
    }

}
