using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSenser : MonoBehaviour
    
{
    public CapsuleCollider capcol;

    private Vector3 point1;//碰撞检测点1
    private Vector3 point2;//检测点2
    private float radius;
    // Start is called before the first frame update
    void Awake()
    {
        capcol = GetComponent<CapsuleCollider>();
        radius = capcol.radius - 0.05f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        point1 = transform.position + transform.up * (radius-0.1f);
        point2 = transform.position + transform.up * (capcol.height-0.1f) - transform.up * radius;
        Collider[] outputCols = Physics.OverlapCapsule(point1, point2, radius, LayerMask.GetMask("Default"));
        
        if (outputCols.Length!=0) 
        {
            SendMessageUpwards("isGround");
        }
        else
        {
            SendMessageUpwards("isNotGround");
        }
    }
}
