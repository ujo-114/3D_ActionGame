using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    public GameObject Player;
    public StateManager playerSM;
    public Image PlayerHealthSlider;
    void Awake()
    {
        PlayerHealthSlider = this.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        playerSM = Player.GetComponent<StateManager>();
    }

    // Update is called once per frame
    void LateUpdate()
    {

        float sliderPercent = playerSM.HP/playerSM.HPMax;
        PlayerHealthSlider.fillAmount = sliderPercent;
    }
}
