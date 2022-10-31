using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPointController : MonoBehaviour
{
    public Collider attackPointR;
    public Collider attackPointL;
    
    public void attackPointROn()
    {
        attackPointR.enabled = true;
    }
    public void attackPointROff()
    {
        attackPointR.enabled = false;
    }
    public void attackPointLOn()
    {
        attackPointL.enabled = true;
    }
    public void attackPointLOff()
    {
        attackPointL.enabled = false;
    }
}
