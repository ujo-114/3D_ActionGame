using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDisController : MonoBehaviour
{
    public Renderer[] renderers;
    public Shader disShader;
    private bool isDisShader = false;
    float currentWeight = 0;
    public Texture noise1;
    public Texture noise2;
    public Texture shape;
    public ActorManager am;

    public List<Material> bodyMaterials = new List<Material>();//ÓÃlist´¢´æcastÊÂ¼þ
    // Start is called before the first frame update
    void Start()
    {
        renderers = transform.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            if (!bodyMaterials.Contains(renderer.material))
            {
                bodyMaterials.Add(renderer.material);
            }
        }
        //dialogueController = GetComponentInParent<DialogueController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (am.sm.isDie)
        {
            if (isDisShader == false)
            {
                ChangeShader();
                isDisShader = true;
            }
            currentWeight = Mathf.Lerp(currentWeight, 1.0f, 0.3f * Time.deltaTime);
            SetShaderWeight();
        }
    }


    void ChangeShader()
    {
        for (int i = 0; i < bodyMaterials.Count; i++)
        {
            bodyMaterials[i].shader = disShader;
            bodyMaterials[i].SetTexture("_DissolveTexture", noise1);
            bodyMaterials[i].SetTexture("_FlowMap", noise2);
            bodyMaterials[i].SetTexture("_ParticleShape", shape);
            bodyMaterials[i].SetFloat("_ParticleRadius", 0.03f);
            bodyMaterials[i].SetVector("_Direction", new Vector4(3, 3, 3, 0));
        }
    }

    void SetShaderWeight()
    {
        for (int i = 0; i < bodyMaterials.Count; i++)
        {
            bodyMaterials[i].SetFloat("_Weight", currentWeight);
        }
    }
}
