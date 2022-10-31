using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideUIController : MonoBehaviour
{
    public GameObject guideUI;
    GameObject UIHolder;
    public Sprite R;
    public Sprite mouse0;
    
    
    Image guideImage;
    Text guideText;

    private void Awake()
    {
        UIHolder = guideUI.transform.GetChild(0).gameObject;
        guideImage = UIHolder.transform.GetChild(1).gameObject.GetComponent<Image>();
        guideText= UIHolder.transform.GetChild(0).gameObject.GetComponent<Text>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "NPC")
        {
            guideUI.SetActive(true);
            guideText.text = "ª·‘í§π§Î";
            guideImage.sprite = R;
        }
        else if (other.tag == "SavePoint")
        {
            guideUI.SetActive(true);
            guideText.text = "–›Ì¨§π§Î";
            guideImage.sprite = R;
        }
        else if (other.tag == "Portal")
        {
            guideUI.SetActive(true);
            guideText.text = "ÅªÀÕ§π§Î";
            guideImage.sprite = R;
        }
        else if (other.tag == "Item")
        {
            guideUI.SetActive(true);
            guideText.text = " ∞§¶";
            guideImage.sprite = R;
        }
        else if (other.tag == "Caster")
        {
            if (other.gameObject.GetComponent<EventCasterManager>().active)
            {
                guideUI.SetActive(true);
                if (other.transform.parent.tag == "Enemy")
                {
                    guideText.text = "ÑI–Ã§π§Î";
                }
                else
                {
                    guideText.text = "È_§±§Î";
                }
                guideImage.sprite = mouse0;
            }
            else
            {
                guideUI.SetActive(false);
            }
        }
    }
    private void Update()
    {
        if (!GameMnager.instance.am.ac.inputEnabled)
        {
            guideUI.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        guideUI.SetActive(false);
        UIHolder.SetActive(true);
    }
}
