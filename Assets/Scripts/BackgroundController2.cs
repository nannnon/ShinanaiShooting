using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController2 : MonoBehaviour
{
    [SerializeField]
    GameObject[] _cubes;

    void Update()
    {
        foreach (GameObject cube in _cubes)
        {
            float scroll = -20 * Time.deltaTime;
            cube.transform.position += new Vector3(0, 0, scroll);

            if (cube.transform.position.z <= -150)
            {
                var tmp = cube.transform.position;
                tmp.z = 150;
                cube.transform.position = tmp;
            }
        }
    }
}
