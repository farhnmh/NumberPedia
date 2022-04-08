using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] Vector3 rotator;
    [SerializeField] float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotator * speed * Time.deltaTime);
    }
}
