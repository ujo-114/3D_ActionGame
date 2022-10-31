using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeapenType { sword, katana, arrow ,greatSword};

public class WeaponData : MonoBehaviour
{
    public float ATK;
    public float defensive;
    public WeaponController wc;
    public WeapenType weapenType;
    public bool isEnemyWeapon;
    
    // Start is called before the first frame update
    void Start()
    {
        if(weapenType!=WeapenType.arrow)
        wc = GetComponentInParent<WeaponController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
