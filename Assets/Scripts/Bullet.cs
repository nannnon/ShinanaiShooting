using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 _velocity = Vector3.zero;

    public void Set(Vector3 pos, Vector3 vel)
    {
        transform.position = pos;
        _velocity = vel;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_velocity * Time.deltaTime, Space.World);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        bool destory = false;
        
        if (tag == "PlayerBullet" && other.tag == "Enemy")
        {
            destory = true;
        }
        else if (tag == "EnemyBullet" && other.tag == "Player")
        {
            destory = true;
        }

        if (destory)
        {
            Destroy(gameObject);
        }
    }
}
