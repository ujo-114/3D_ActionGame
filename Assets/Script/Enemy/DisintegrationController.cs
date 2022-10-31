using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisintegrationController : MonoBehaviour
{
    public Material bodyMaterial;
    public Material weaponMaterial;
    public Material swordMaterial;
    public Shader disShader;
    private bool isDisShader=false;

    public ActorManager am;
    float currentWeight = 0;

    private void Awake()
    {
        am = GetComponent<ActorManager>();
        bodyMaterial.shader = Shader.Find("Standard");
        weaponMaterial.SetFloat("_Weight", 0);
        swordMaterial.SetFloat("_Weight", 0);
    }

    public void Update()
    {
        if (am.sm.isDie)
        {
            if (isDisShader == false)
            {
                bodyMaterial.shader = disShader;
                isDisShader = true;
            }
            currentWeight = Mathf.Lerp(currentWeight, 1.0f,0.6f* Time.deltaTime);
            bodyMaterial.SetFloat("_Weight", currentWeight);
            weaponMaterial.SetFloat("_Weight", currentWeight);
            swordMaterial.SetFloat("_Weight", currentWeight);
        }
    }
}
