using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IUserInput : MonoBehaviour
{
    public float v;//v
    public float h;//h
    public float Dmag;//Dmag
    public Vector3 DVec;//DVec
    public float CameraH;//CameraH
    public float CameraV;// Camerav


    public bool run;
    public bool defense;
    public bool jump;
    public bool roll;
    public bool lockon;
    protected float vh;


    public bool inputEnable = true;
    protected float targetDup;
    protected float targetDright;
    protected float velocityDup;
    protected float velocityDright;

    protected Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }
    protected void UpdateDmagDvec(float _v,float _h)
    {
        Dmag = Mathf.Sqrt(_v * _v + _h * _h);
        DVec = _v * transform.right + _h * transform.forward;
    }

}
