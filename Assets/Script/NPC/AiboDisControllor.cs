using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiboDisControllor : MonoBehaviour
{
    Material[] bodyMaterials;
    Material[] clothMaterials;
    Material[] hairMaterials;
    public DialogueData_SO targetDialogue;
    public Shader disShader;
    private bool isDisShader = false;
    float currentWeight = 0;
    public Texture noise1;
    public Texture noise2;
    public Texture shape;
    public QuestData_SO targetQuest;
    public float destoryTime;
    private bool goDis = false;
    // Start is called before the first frame update
    void Start()
    {
        var renderer = transform.GetComponentsInChildren<Renderer>();
        {
            bodyMaterials=renderer[0].materials;
            clothMaterials = renderer[1].materials;
            hairMaterials = renderer[2].materials;
        }
        //dialogueController = GetComponentInParent<DialogueController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetDialogue != null)
        {
            goDis = targetDialogue.isDialogueEnd;
        }
        else
        {
            goDis= GetComponentInParent<QuestGiver>().IsFinsihed;
        }
        if (goDis)
        {
            if (isDisShader == false)
            {
                ChangeShader();
                isDisShader = true;
            }
            currentWeight = Mathf.Lerp(currentWeight, 1.0f, 0.3f * Time.deltaTime);
            SetShaderWeight();

            Destroy(this.transform.parent.gameObject, destoryTime);
        }
        if (targetQuest != null && QuestManager.instance.GetTask(targetQuest)!=null && QuestManager.instance.GetTask(targetQuest).IsFinished)
        {
            Destroy(this.transform.parent.gameObject);
        }
    }


    void ChangeShader()
    {
        for (int i = 0; i < bodyMaterials.Length; i++)
        {
            bodyMaterials[i].shader = disShader;
            bodyMaterials[i].SetTexture("_DissolveTexture", noise1);
            bodyMaterials[i].SetTexture("_FlowMap", noise2);
            bodyMaterials[i].SetTexture("_ParticleShape", shape);
            bodyMaterials[i].SetFloat("_ParticleRadius", 0.03f);
            bodyMaterials[i].SetVector("_Direction", new Vector4(3, 3, 3, 0));
        }
        for (int i = 0; i < clothMaterials.Length; i++)
        {
            clothMaterials[i].shader = disShader;
            clothMaterials[i].SetTexture("_DissolveTexture", noise1);
            clothMaterials[i].SetTexture("_FlowMap", noise2);
            clothMaterials[i].SetTexture("_ParticleShape", shape);
            clothMaterials[i].SetFloat("_ParticleRadius", 0.03f);
            clothMaterials[i].SetVector("_Direction", new Vector4(3, 3, 3, 0));
        }
        for (int i = 0; i < hairMaterials.Length; i++)
        {
            hairMaterials[i].shader = disShader;
            hairMaterials[i].SetTexture("_DissolveTexture", noise1);
            hairMaterials[i].SetTexture("_FlowMap", noise2);
            hairMaterials[i].SetTexture("_ParticleShape", shape);
            hairMaterials[i].SetFloat("_ParticleRadius", 0.03f);
            hairMaterials[i].SetVector("_Direction", new Vector4(3, 3, 3, 0));
        }
    }

    void SetShaderWeight()
    {
        for (int i = 0; i < bodyMaterials.Length; i++)
        {
            bodyMaterials[i].SetFloat("_Weight", currentWeight); ;
        }
        for (int i = 0; i < clothMaterials.Length; i++)
        {
            clothMaterials[i].SetFloat("_Weight", currentWeight); ;
        }
        for (int i = 0; i < hairMaterials.Length; i++)
        {
            hairMaterials[i].SetFloat("_Weight", currentWeight); ;
        }
    }
}
