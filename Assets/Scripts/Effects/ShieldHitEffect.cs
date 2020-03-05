using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class ShieldHitEffect : MonoBehaviour
{
    VisualEffect visualEffect;
    Sequence effectSequence;
    // Start is called before the first frame update

    void Start()
    {
        visualEffect = this.GetComponent<VisualEffect>();
    }

    public void FadeIn()
    {
        if(visualEffect == null)
        {
            visualEffect = this.GetComponent<VisualEffect>();
        }

        visualEffect.Stop();
        visualEffect.SetFloat("Rate", 0f);
        effectSequence = DOTween.Sequence();
        effectSequence.Insert(0f, DOTween.To(() => visualEffect.GetFloat("Rate"), x => visualEffect.SetFloat("Rate", x), 1.0f, GameConstants.ShieldHitLife * 0.2f));
        effectSequence.Append(DOTween.To(() => visualEffect.GetFloat("Rate"), x => visualEffect.SetFloat("Rate", x), 0f, GameConstants.ShieldHitLife * 0.2f).SetDelay(GameConstants.ShieldHitLife * 0.6f));
        effectSequence.AppendInterval(GameConstants.ShieldHitLife);
        effectSequence.OnComplete(() =>
        {
            FadeAway();
        });
        visualEffect.Play();
        effectSequence.Play();
    }

    void FadeAway()
    {
        FindObjectOfType<GameManager>().ReturnEffectToPool(gameObject, GameConstants.EffectTypes.ShieldHit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
