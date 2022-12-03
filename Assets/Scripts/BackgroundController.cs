using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField]
    GameObject[] _terrains;

    void Update()
    {
        GameObject t0 = _terrains[0];
        GameObject t1 = _terrains[1];

        float scroll = -10 * Time.deltaTime;
        t0.transform.position += new Vector3(0, 0, scroll);
        t1.transform.position += new Vector3(0, 0, scroll);

        if (t0.transform.position.z <= -450)
        {
            Vector3 newPos = new Vector3(-150, -200, t1.transform.position.z + 300);
            t0.transform.position = newPos;
        }
        else if (t1.transform.position.z <= -450)
        {
            Vector3 newPos = new Vector3(-150, -200, t0.transform.position.z + 300);
            t1.transform.position = newPos;
        }
    }
}
