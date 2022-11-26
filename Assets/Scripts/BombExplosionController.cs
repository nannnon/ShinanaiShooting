using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionController : MonoBehaviour
{
    void Start()
    {
        Invoke("BreakEffect", 1.0f);
    }

    void BreakEffect()
    {
        Destroy(gameObject);
    }
}
