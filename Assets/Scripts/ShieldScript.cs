using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    Material shieldMaterial;

    string shaderFadeParam = "Vector1_12DFC281";

    bool shieldOn;
    // Start is called before the first frame update
    void Start()
    {
        shieldMaterial = this.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TurnOnShield()
    {
        if (shieldMaterial == null)
        {
            shieldMaterial = this.GetComponent<MeshRenderer>().material;
        }
        shieldMaterial.SetFloat(shaderFadeParam, 0f);
        shieldMaterial.DOFloat(1f, shaderFadeParam, GameConstants.ShieldAppearTime).SetEase(Ease.OutBack);
        shieldOn = true;
    }

    public void TurnOffShield()
    {
        if(shieldMaterial == null)
        {
            shieldMaterial = this.GetComponent<MeshRenderer>().material;
        }
        shieldOn = false;
        shieldMaterial.DOFloat(0f, shaderFadeParam, GameConstants.ShieldAppearTime).SetEase(Ease.OutBack);
    }

    public void LowPowerShield()
    {
        if(shieldOn)
        {
            TurnOffShield();
        }
    }
}
