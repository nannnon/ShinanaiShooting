using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject _playerBulletPrefab;

    float _elapsedTimeForShooting = 0;

    void Start()
    {
        
    }

    void Update()
    {
        Move();
        ShootBullet();
    }

    void Move()
    {
        // 移動可能な範囲
        const float x0 = -3;
        const float x1 = x0 + 6;
        const float z0 = 0;
        const float z1 = z0 + 10;

        float moveSpeed = 3 * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x - moveSpeed >= x0)
        {
            transform.Translate(-moveSpeed, 0, 0, Space.World);
        }
        if (Input.GetKey(KeyCode.RightArrow) && transform.position.x + moveSpeed <= x1)
        {
            transform.Translate(+moveSpeed, 0, 0, Space.World);
        }
        if (Input.GetKey(KeyCode.DownArrow) && transform.position.z - moveSpeed >= z0)
        {
            transform.Translate(0, 0, -moveSpeed, Space.World);
        }
        if (Input.GetKey(KeyCode.UpArrow) && transform.position.z + moveSpeed <= z1)
        {
            transform.Translate(0, 0, +moveSpeed, Space.World);
        }
    }

    void ShootBullet()
    {
        _elapsedTimeForShooting += Time.deltaTime;
        const float shootingCycle = 0.3f;

        if (_elapsedTimeForShooting >= shootingCycle)
        {
            // 弾を発射
            GameObject go = Instantiate(_playerBulletPrefab);
            Bullet b = go.GetComponent<Bullet>();
            Vector3 pos = transform.position + new Vector3(0, 0, 0.5f);
            Vector3 vel = new Vector3(0, 0, 6f);
            b.Set(pos, vel);

            _elapsedTimeForShooting = 0;
        }
    }
}
