using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    public Transform barPoint;
    public  Image healthSlider;
    public Transform UIbar;
    Transform cam;
    public  StateManager currentState;
    void Awake()
    {
        currentState = GetComponent<StateManager>();
        barPoint = transform.Find("HealthBarPoint").transform;
    }
    private void OnEnable()//人物创建时生成血条
    {
        cam = Camera.main.transform;

        UIbar = barPoint.GetChild(0);
        UIbar.localPosition = Vector3.zero;
        healthSlider = UIbar.GetChild(0).GetChild(0).GetComponent<Image>();
        UIbar.gameObject.SetActive(true);
    }

    private void Update()
    {
        
        float sliderPercent = currentState.HP / currentState.HPMax;
        healthSlider.fillAmount = sliderPercent;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UIbar.forward = -cam.forward;
    }
}
