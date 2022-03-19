using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.EditorCoroutines.Editor;

[CustomEditor(typeof(Player))]
public class PlayerInspector : Editor
{

    Player cube;
    Vector3 direction;
    bool sceneRepaintAll;
    float slideVelocity;
    float velocity = 0;
    bool[] wasdBool = new bool[4];

    public float slideJumpForce = 0;
    public float jumpForce = 0;
    public float leftForce = 0;
    public float forwardForce = 0;

    public bool playParticles;

    private void OnEnable()
    {
        cube = (Player)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Reset"))
        {
            cube.transform.position = Vector3.zero;
            direction = Vector3.zero;
            jumpForce = 0;
            cube.rb.velocity = Vector3.zero;
        }
        slideVelocity = EditorGUILayout.Slider("velocity ", slideVelocity, 0, 5);
        slideJumpForce = EditorGUILayout.Slider("jumpForce  ", slideJumpForce, 0, 20);
        sceneRepaintAll = EditorGUILayout.Toggle("SceneView Repaint ", sceneRepaintAll);
        playParticles = EditorGUILayout.Toggle("Particles Play  ", playParticles);


        //showVelocity = EditorGUILayout.Foldout(showVelocity, "Velocity direction");
        if (GUILayout.Button("Run Physics"))
        {
            StepPhysics();
        }

        if (GUILayout.Button("Set Running Value"))
        {
            cube.shootForce = 140;
            slideVelocity = 1.8f;
            slideJumpForce = 9;
        }



    }

    private void StepPhysics()
    {
        //https://answers.unity.com/questions/158766/physics-in-scene-editor-mode.html?childToView=1672642#answer-1672642
        Physics.autoSimulation = false;
        Physics.Simulate(Time.fixedDeltaTime);
        Physics.autoSimulation = true;
    }

    private void OnSceneGUI()
    {
        if (sceneRepaintAll)
        {
            StepPhysics();
            SceneView.RepaintAll();
        }
        Handles.color = Color.blue;
        //Handles.DrawLine(Vector3.zero, cube.transform.position);

        Event e = Event.current;
        switch (e.type)
        {
            case EventType.KeyDown:
                {
                    if (Event.current.keyCode == (KeyCode.W))
                    {
                        direction = Vector3.forward;
                        velocity = slideVelocity;
                        wasdBool[0] = true;
                    }
                    else if (Event.current.keyCode == (KeyCode.A))
                    {
                        direction = Vector3.left;
                        velocity = slideVelocity;
                        wasdBool[1] = true;

                    }
                    else if (Event.current.keyCode == (KeyCode.S))
                    {
                        direction = -Vector3.forward;
                        velocity = slideVelocity;
                        wasdBool[2] = true;

                    }
                    else if (Event.current.keyCode == (KeyCode.D))
                    {
                        direction = Vector3.right;
                        velocity = slideVelocity;
                        wasdBool[3] = true;
                    }
                    else if (Event.current.keyCode == (KeyCode.Space))
                    {
                        jumpForce = slideJumpForce;
                        if (wasdBool[0]) forwardForce = 2;
                        if (wasdBool[1]) leftForce = -2;
                        if (wasdBool[2]) forwardForce = -2;
                        if (wasdBool[3]) leftForce = 2;


                    }
                    else if (Event.current.keyCode == (KeyCode.K))
                    {

                        cube.pressedK = true;

                    }
                    else
                    {
                    }

                    break;
                }

            case EventType.KeyUp:
                {
                    if (Event.current.keyCode == (KeyCode.W))
                    {
                        wasdBool[0] = false;
                        if (countWASDBool() == 0)
                        {
                            velocity = 0;
                        }
                    }
                    else if (Event.current.keyCode == (KeyCode.A))
                    {
                        wasdBool[1] = false;
                        if (countWASDBool() == 0)
                        {
                            velocity = 0;
                        }
                    }
                    else if (Event.current.keyCode == (KeyCode.S))
                    {
                        wasdBool[2] = false;
                        if (countWASDBool() == 0)
                        {
                            velocity = 0;
                        }
                    }
                    else if (Event.current.keyCode == (KeyCode.D))
                    {
                        wasdBool[3] = false;
                        if (countWASDBool() == 0)
                        {
                            velocity = 0;
                        }
                    }


                    break;
                }
        }

        cube.rb.AddForce(new Vector3(leftForce, jumpForce, forwardForce), ForceMode.Impulse);
        leftForce = 0;
        forwardForce = 0;
        jumpForce = 0;
        Handles.color = Color.red;
        Handles.Label(cube.transform.position + (2 * Vector3.one), "Velocity: " + velocity);
        //Handles.Label(cube.transform.position + (2 * new Vector3(1, 0, 1)), "Disatance: " + cube.transform.position.magnitude);


        cube.transform.position += direction * velocity * Time.deltaTime;

    }

    int countWASDBool()
    {
        int count = 0;
        for (int i = 0; i < wasdBool.Length; i++)
        {
            if (wasdBool[i] == true)
            {
                count++;
            }
        }
        return count;
    }


    static IEnumerator IEDelayEditor(GameObject projectile)
    {
        ParticleSystem ps = projectile.GetComponent<ParticleSystem>();
        if (ps)
        {
            ps.Play();
        }
        else
        {
            Debug.Log("we Don't able to find particles");
        }
        yield return new EditorWaitForSeconds(2f);
        DestroyImmediate(projectile.gameObject);
    }

}
