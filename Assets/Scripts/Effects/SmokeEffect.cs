using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class SmokeEffect : MonoBehaviour
{

    VisualEffect visualEffect;
    // Start is called before the first frame update
    void Start()
    {
        visualEffect = this.GetComponent<VisualEffect>();
    }

    public void FadeIn()
    {
        if (visualEffect == null)
        {
            visualEffect = this.GetComponent<VisualEffect>();
        }

        visualEffect.Play();
        visualEffect.enabled = true;
    }

    void FadeAway()
    {

    }

    void OnDisable()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
