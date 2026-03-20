using System;
using UnityEngine;

public class GrowableSize : BaseGrowable
{
    public override void Grow()
    {
        if(hasGrown) 
            return;
        
        hasGrown = true;
        transform.localScale *= 1.5f;
        //Debug.Log(name + " grew!");
    }

    public override void UnGrow()
    {
        if(!hasGrown)
            return;
        
        hasGrown = false;
        transform.localScale *= 0.75f;
    }
}
