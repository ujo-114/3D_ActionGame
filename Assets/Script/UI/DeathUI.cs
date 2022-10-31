using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUI : MonoBehaviour
{
    public static DeathUI instance;

    private void Awake()
    {
        CheckSingle();
        CheckGameObject();
    }




    private void CheckSingle()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        else
            Destroy(this);
    }
    private void CheckGameObject()
    {
        if (name == "DeathUI")
        {
            return;

        }
        else
            Destroy(this);
    }
}
