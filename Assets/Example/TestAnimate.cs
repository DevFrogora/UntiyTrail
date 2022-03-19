using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[ExecuteInEditMode]
public class TestAnimate : MonoBehaviour
{
    [Range(1,3f)] public float Timeline =1;
    private ParticleSystem Ps;

    void Update()
    {
        Ps = GetComponent<ParticleSystem>();
        Ps.Simulate(Timeline, true, true);
        //Ps.Pause(true);
        Ps.time = Timeline * Time.deltaTime;
        Timeline += Timeline* Time.deltaTime;
        if(Timeline > 3)
        {
            Timeline = 0.1f;
        }
    }
}