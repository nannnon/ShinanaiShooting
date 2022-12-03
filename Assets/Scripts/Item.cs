using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    void Update()
    {
        Vector3 vel = new Vector3(0, 0, -2);
        transform.Translate(vel * Time.deltaTime, Space.World);

        if (transform.position.x < GameController.ScreenPoint0.x - 2 ||
            transform.position.x > GameController.ScreenPoint1.x + 2 ||
            transform.position.z < GameController.ScreenPoint0.z - 2 ||
            transform.position.z > GameController.ScreenPoint1.z + 2)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
