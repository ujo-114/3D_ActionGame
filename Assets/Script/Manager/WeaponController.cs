using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponManager wm;
    public WeaponData wdata;
    void Awake()
    {
        wdata = GetComponentInChildren<WeaponData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
