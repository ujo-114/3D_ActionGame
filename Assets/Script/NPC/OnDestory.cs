using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestory : MonoBehaviour 
{
    public GameObject[] onDestoryActive;
    public GameObject[] onDestoryDisactive;



    private void OnDisable()
    {
        for (int i = 0; i < onDestoryActive.Length; i++)
        {
            onDestoryActive[i].SetActive(true);
        }
        for (int i = 0; i < onDestoryDisactive.Length; i++)
        {
            onDestoryDisactive[i].SetActive(false);
        }
    }
}
