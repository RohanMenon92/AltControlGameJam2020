using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public bool testHit;
    private ParticleSystem particle;
    private MeshRenderer renderer;
    private AsteroidPool pool;
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        pool = FindObjectOfType<AsteroidPool>();
        renderer = GetComponent<MeshRenderer>();
        particle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if(testHit)
        {
            OnHit();
            testHit = false;
        }
    }

    public void OnHit()
    {
        // play explosion animation
        
        particle.Play();
        StartCoroutine(waitForAnimation());
        GetComponent<MeshCollider>().enabled = false;

    }

    IEnumerator waitForAnimation()
    {
        yield return new WaitForSeconds(0.15f);
        renderer.enabled = false;
        yield return new WaitForSeconds(0.25f);
        particle.Stop();
        yield return new WaitForSeconds(2.0f);
        pool.unusedAsteroid.Add(this);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Bullet"))
        {
            OnHit();
            other.gameObject.GetComponent<BulletScript>().OnHit();
        }
    }
}
