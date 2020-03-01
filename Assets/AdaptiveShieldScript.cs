using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveShieldScript : MonoBehaviour
{
    string shaderFadeAmount = "Vector1_90F8D24A";
    string shaderSphereRadius = "Vector1_C495D893";
    string shaderSphereHitNormalized = "Vector3_3535B637";


    public float adaptiveShieldMoveTime = 0.5f;
    public float adaptiveShieldDecay = 0.05f;

    Material shieldMaterial;
    public bool shieldOn;

    public bool is3DShield;

    Sequence hitShowSequence;

    // Start is called before the first frame update
    void Start()
    {
        shieldMaterial = this.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        float fadeAmount = shieldMaterial.GetFloat(shaderFadeAmount);
        if (fadeAmount > 0f)
        {
            if (shieldOn)
            {
                shieldMaterial.SetFloat(shaderFadeAmount, fadeAmount - adaptiveShieldDecay);
            }
            else
            {
                shieldMaterial.SetFloat(shaderFadeAmount, fadeAmount - 0.1f);
            }

        }
    }

    public void OnHit(Vector3 normalizedHit, float damage)
    {
        if(shieldOn)
        {
            if (hitShowSequence != null)
            {
                hitShowSequence.Kill();
            }


            hitShowSequence = DOTween.Sequence();
            if(!is3DShield)
            {
                normalizedHit = new Vector3(normalizedHit.x, 0f, normalizedHit.z);
            }
            hitShowSequence.Insert(0f, shieldMaterial.DOVector(normalizedHit, shaderSphereHitNormalized, adaptiveShieldMoveTime));
            shieldMaterial.SetFloat(shaderSphereRadius, 0f);
            shieldMaterial.SetFloat(shaderFadeAmount, 1f);
            hitShowSequence.Insert(0f, shieldMaterial.DOFloat(0.75f, shaderSphereRadius, adaptiveShieldMoveTime));
            hitShowSequence.Insert(0f, shieldMaterial.DOFloat(1.4f, shaderFadeAmount, adaptiveShieldMoveTime));
        }
    }
}
