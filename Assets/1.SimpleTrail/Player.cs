using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.EditorCoroutines.Editor;

[ExecuteInEditMode]
public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public Transform shootingPoint;
    public GameObject bulletObject;
    public float shootForce;
    public bool pressedK;
    public GameObject TestBulletObject;
    [Range(0.1f, 2f)] public float Timeline=0.1f;


    bool particlesSimulation;
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }
    GameObject projectile;
    ParticleSystem ps;
    private void Update()
    {
        if (pressedK)
        {
            projectile = (GameObject)Instantiate(
            bulletObject, shootingPoint.position, shootingPoint.rotation);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * shootForce);
            ps = projectile.GetComponentInChildren<ParticleSystem>();
            EditorCoroutineUtility.StartCoroutine(IEDelayEditor(projectile), this);
            pressedK = false;
        }

        

        if (ps)
        {
            //ps.Simulate(Timeline);
            ps.Simulate(Timeline, true, true);
            ps.time = Timeline * Time.deltaTime;
            Timeline += Timeline * Time.deltaTime;
            if (Timeline > 2)
            {
                Timeline = 0.1f;
            }
        }


    }

    IEnumerator IEDelayEditor(GameObject projectile)
    {
        particlesSimulation = true;
        yield return new EditorWaitForSeconds(2f);
        DestroyImmediate(projectile.gameObject);
    }
}
