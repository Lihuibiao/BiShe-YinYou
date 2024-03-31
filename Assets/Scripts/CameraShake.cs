using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Animator camAnim;

    [InspectorButton]
    public void Shake()
    {
        camAnim.SetTrigger("Shake");
    }
}
