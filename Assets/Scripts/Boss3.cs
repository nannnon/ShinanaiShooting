
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss3 : Boss
{
    static readonly Vector3 _center = new Vector3(0, 0, 7.5f);
    const float _r = 2.5f;
    float _theta;

    new void Update()
    {
        base.Update();
        Move();
        ShootBullet();
    }

    void Move()
    {
        float x0 = GameController.ScreenPoint0.x;
        float x1 = GameController.ScreenPoint1.x;
        float z0 = GameController.ScreenPoint0.z;
        float z1 = GameController.ScreenPoint1.z;

        if (_moveState == MoveState.State0)
        {
            float moveSpeed = 1.5f * Time.deltaTime;
            Vector3 t = new Vector3(0, 0, -moveSpeed);
            transform.Translate(t, Space.World);

            if (transform.position.z <= (_center.z + _r))
            {
                _moveState = MoveState.State1;
                _theta = Mathf.PI / 2;
            }
        }
        else
        {
            // 円状に周る
            transform.position = _center + new Vector3(_r * Mathf.Cos(_theta), 0, _r * Mathf.Sin(_theta));

            _theta += Time.deltaTime;
        }
    }

    void ShootBullet()
    {
        _timeForShooting += Time.deltaTime;
        const float shootingCycleTime = 0.3f;

        if (_timeForShooting >= shootingCycleTime)
        {
            // 全方位へ
            const int bulletNum = 16;
            for (int i = 0; i < bulletNum; ++i)
            {
                float theta = i * 2 * Mathf.PI / bulletNum;
                Vector3 velocity = new Vector3();
                const float bulletSpeed = 6;
                velocity.x = bulletSpeed * Mathf.Cos(theta);
                velocity.z = bulletSpeed * Mathf.Sin(theta);
                GenerateBullet(Vector3.zero, velocity);
            }

            _timeForShooting = 0;
        }
    }
}