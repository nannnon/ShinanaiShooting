using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CautionController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyMe", 2.0f);
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
