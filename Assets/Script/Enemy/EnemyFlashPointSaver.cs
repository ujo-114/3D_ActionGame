using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlashPointSaver : MonoBehaviour
{
    public Vector3 flashPoint;
    public Quaternion flashRotation;
    private void Awake()
    {
        flashPoint = this.transform.position;
        flashRotation = this.transform.rotation;
    }
}
