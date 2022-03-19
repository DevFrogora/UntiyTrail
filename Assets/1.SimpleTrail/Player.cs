using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public Transform shootingPoint;
    public GameObject bulletObject;
    public float shootForce;
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }
}
