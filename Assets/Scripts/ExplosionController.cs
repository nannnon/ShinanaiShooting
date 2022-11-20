using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("BreakEffect", 1.0f);
    }

    void BreakEffect()
    {
        Destroy(gameObject);
    }
}
