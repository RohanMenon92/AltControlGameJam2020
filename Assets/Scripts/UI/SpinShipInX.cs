using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinShipInX : MonoBehaviour
{
    Transform ship;
    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        ship = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        ship.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }
}
