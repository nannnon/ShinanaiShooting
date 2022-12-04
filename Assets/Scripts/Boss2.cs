using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss2 : Boss
{
    Vector3 _targetPos;
    Vector3 _step;

    new void Start()
    {
        base.Start();
        _targetPos = new Vector3(0, 0, GameController.ScreenPoint1.z);
        _step = CalculateStep(_targetPos);
    }

    new void Update()
    {
        base.Update();
        Move();
        ShootBullet();
    }

    void Move()
    {
        Vector3 t = _step * Time.deltaTime;
        transform.Translate(t, Space.World);

        float x0 = GameController.ScreenPoint0.x;
        float x1 = GameController.ScreenPoint1.x;
        float z0 = GameController.ScreenPoint0.z;
        float z1 = GameController.ScreenPoint1.z;

        if (_moveState == MoveState.State0)
        {
            if (transform.position.z <= _targetPos.z)
            {
                _moveState = MoveState.State1;
                _targetPos = new Vector3(x0, 0, (z1 - z0) / 2);
                _step = CalculateStep(_targetPos);
            }
        }
        else if (_moveState == MoveState.State1)
        {
            if (transform.position.x <= _targetPos.x)
            {
                _moveState = MoveState.State2;
                _targetPos = new Vector3(x1, 0, (z1 - z0) / 2);
                _step = CalculateStep(_targetPos);
            }
        }
        else if (_moveState == MoveState.State2)
        {
            if (transform.position.x >= _targetPos.x)
            {
                _moveState = MoveState.State3;
                _targetPos = new Vector3(0, 0, z1);
                _step = CalculateStep(_targetPos);
            }
        }
        else if (_moveState == MoveState.State3)
        {
            if (transform.position.z >= _targetPos.z)
            {
                _moveState = MoveState.State1;
                _targetPos = new Vector3(x0, 0, (z1 - z0) / 2);
                _step = CalculateStep(_targetPos);
            }
        }
    }

    Vector3 CalculateStep(Vector3 targetPos)
    {
        Vector3 d = targetPos - transform.position;
        float theta = Mathf.Atan2(d.z, d.x);

        const float moveSpeed = 1.5f;
        Vector3 step = new Vector3(moveSpeed * Mathf.Cos(theta), 0, moveSpeed * Mathf.Sin(theta));

        return step;
    }

    void ShootBullet()
    {
        _timeForShooting += Time.deltaTime;
        const float shootingCycleTime = 0.3f;
        const float bulletSpeed = 6;

        if (_timeForShooting >= shootingCycleTime)
        {
            // 扇の範囲内でRandom
            {
                float center = 3 * Mathf.PI / 2;
                float theta = Random.Range(center - Mathf.PI / 4, center + Mathf.PI / 4);
                Vector3 vel = new Vector3(bulletSpeed * Mathf.Cos(theta), 0, bulletSpeed * Mathf.Sin(theta));
                Vector3 rpos = new Vector3(-0.5f, 0, 0);
                GenerateBullet(rpos, vel);
            }

            // 両翼から
            {
                Vector3 vel = new Vector3(0, 0, -bulletSpeed);
                GenerateBullet(new Vector3(-1.5f, 0, 0), vel);
                GenerateBullet(new Vector3(+1.5f, 0, 0), vel);
            }

            _timeForShooting = 0;
        }
    }
}