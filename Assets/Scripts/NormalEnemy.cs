using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MovePattern
{
    AppearAndShootAndBye,
    GoThrough,
    SouthWest,
    SouthEast,
    UFromLeft,
    UFromRight,
    NInvert,
    N,
}

public enum ShootPattern
{
    NotShooting,
    Straight,
    Random,
    ToPlayer,
}

 public struct NormalEnemyData
{
    public float moveSpeedCoef;
    public MovePattern movePattern;
    public ShootPattern shootPattern;
    public float timeToStartShooting;
    public float shootingCycleTime;
    public float bulletSpeed;
    public bool isItemHolder;
}

public class NormalEnemy : Enemy
{
    NormalEnemyData _normalEnemyData;
    float _timeForMoving;
    float _appearTime;

    new void Start()
    {
        base.Start();
        _appearTime = Time.time;
    }

    new void Update()
    {
        base.Update();
        Move();
        ShootBullet();
    }

    public void Set(Vector3 position, int hp, int score, NormalEnemyData normalEnemyData)
    {
        base.Set(position, hp, score);
        _normalEnemyData = normalEnemyData;
    }

    void Move()
    {
        float moveSpeed = _normalEnemyData.moveSpeedCoef * Time.deltaTime;
        float x0 = GameController.ScreenPoint0.x;
        float x1 = GameController.ScreenPoint1.x;
        float z0 = GameController.ScreenPoint0.z;
        float z1 = GameController.ScreenPoint1.z;

        if (_normalEnemyData.movePattern == MovePattern.AppearAndShootAndBye)
        {
            if (_moveState == MoveState.State0)
            {
                Vector3 t = new Vector3(0, 0, -moveSpeed);
                transform.Translate(t, Space.World);

                if (transform.position.z <= z1)
                {
                    _timeForMoving = Time.time;
                    _moveState = MoveState.State1;
                }
            }
            else if (_moveState == MoveState.State1)
            {
                if (Time.time - _timeForMoving > 5)
                {
                    _moveState = MoveState.State2;
                }
            }
            else if (_moveState == MoveState.State2)
            {
                Vector3 t = new Vector3(0, 0, moveSpeed);
                transform.Translate(t, Space.World);
            }
        }
        else if (_normalEnemyData.movePattern == MovePattern.GoThrough)
        {
            Vector3 t = new Vector3(0, 0, -moveSpeed);
            transform.Translate(t, Space.World);
        }
        else if (_normalEnemyData.movePattern == MovePattern.SouthWest)
        {
            const float theta = 3 * Mathf.PI / 2 - Mathf.PI / 8;
            Vector3 t = new Vector3(moveSpeed * Mathf.Cos(theta), 0, moveSpeed * Mathf.Sin(theta));
            transform.Translate(t, Space.World);
        }
        else if (_normalEnemyData.movePattern == MovePattern.SouthEast)
        {
            const float theta = 3 * Mathf.PI / 2 + Mathf.PI / 8;
            Vector3 t = new Vector3(moveSpeed * Mathf.Cos(theta), 0, moveSpeed * Mathf.Sin(theta));
            transform.Translate(t, Space.World);
        }
        else if (_normalEnemyData.movePattern == MovePattern.UFromLeft)
        {
            if (_moveState == MoveState.State0)
            {
                Vector3 t = new Vector3(0, 0, -moveSpeed);
                transform.Translate(t, Space.World);

                if (transform.position.z <= z1 - (z1 - z0) / 2)
                {
                    _moveState = MoveState.State1;
                }
            }
            else if (_moveState == MoveState.State1)
            {
                Vector3 t = new Vector3(moveSpeed, 0, 0);
                transform.Translate(t, Space.World);

                if (transform.position.x >= x1 - 1)
                {
                    _moveState = MoveState.State2;
                }
            }
            else if (_moveState == MoveState.State2)
            {
                Vector3 t = new Vector3(0, 0, moveSpeed);
                transform.Translate(t, Space.World);
            }
        }
        else if (_normalEnemyData.movePattern == MovePattern.UFromRight)
        {
            if (_moveState == MoveState.State0)
            {
                Vector3 t = new Vector3(0, 0, -moveSpeed);
                transform.Translate(t, Space.World);

                if (transform.position.z <= z1 - (z1 - z0) / 2)
                {
                    _moveState = MoveState.State1;
                }
            }
            else if (_moveState == MoveState.State1)
            {
                Vector3 t = new Vector3(-moveSpeed, 0, 0);
                transform.Translate(t, Space.World);

                if (transform.position.x <= x0 + 1)
                {
                    _moveState = MoveState.State2;
                }
            }
            else if (_moveState == MoveState.State2)
            {
                Vector3 t = new Vector3(0, 0, moveSpeed);
                transform.Translate(t, Space.World);
            }
        }
        else if (_normalEnemyData.movePattern == MovePattern.NInvert)
        {
            if (_moveState == MoveState.State0)
            {
                Vector3 t = new Vector3(0, 0, -moveSpeed);
                transform.Translate(t, Space.World);

                if (transform.position.z <= z1 - (z1 - z0) * 8 / 10)
                {
                    _moveState = MoveState.State1;
                }
            }
            else if (_moveState == MoveState.State1)
            {
                float theta = Mathf.Atan2(z1 - transform.position.z, x1 - transform.position.x);
                Vector3 t = new Vector3(moveSpeed * Mathf.Cos(theta), 0, moveSpeed * Mathf.Sin(theta));
                transform.Translate(t, Space.World);

                if (transform.position.x >= x1 - 1)
                {
                    _moveState = MoveState.State2;
                }
            }
            else if (_moveState == MoveState.State2)
            {
                Vector3 t = new Vector3(0, 0, -moveSpeed);
                transform.Translate(t, Space.World);
            }
        }
        else if (_normalEnemyData.movePattern == MovePattern.N)
        {
            if (_moveState == MoveState.State0)
            {
                Vector3 t = new Vector3(0, 0, -moveSpeed);
                transform.Translate(t, Space.World);

                if (transform.position.z <= z1 - (z1 - z0) * 8 / 10)
                {
                    _moveState = MoveState.State1;
                }
            }
            else if (_moveState == MoveState.State1)
            {
                float theta = Mathf.Atan2(z1 - transform.position.z, x0 - transform.position.x);
                Vector3 t = new Vector3(moveSpeed * Mathf.Cos(theta), 0, moveSpeed * Mathf.Sin(theta));
                transform.Translate(t, Space.World);

                if (transform.position.x <= x0 + 1)
                {
                    _moveState = MoveState.State2;
                }
            }
            else if (_moveState == MoveState.State2)
            {
                Vector3 t = new Vector3(0, 0, -moveSpeed);
                transform.Translate(t, Space.World);
            }
        }

        // 画面外の特定範囲まで移動したら削除
        if (transform.position.x < GameController.ScreenPoint0.x - 2 ||
            transform.position.x > GameController.ScreenPoint1.x + 2 ||
            transform.position.z < GameController.ScreenPoint0.z - 3 ||
            transform.position.z > GameController.ScreenPoint1.z + 5)
        {
            Destroy(gameObject);
        }
    }

    void ShootBullet()
    {
        float elapsedTime = Time.time - _appearTime;
        _timeForShooting += Time.deltaTime;

        if (elapsedTime >= _normalEnemyData.timeToStartShooting && _timeForShooting >= _normalEnemyData.shootingCycleTime)
        {
            if (_normalEnemyData.shootPattern == ShootPattern.NotShooting)
            {
                // 何もしない
            }
            else if (_normalEnemyData.shootPattern == ShootPattern.Straight)
            {
                GenerateBullet(new Vector3(0, 0, -0.5f), new Vector3(0, 0, -_normalEnemyData.bulletSpeed));
            }
            else if (_normalEnemyData.shootPattern == ShootPattern.Random)
            {
                float theta = Random.Range(0, 2 * Mathf.PI);
                Vector3 vel = new Vector3(_normalEnemyData.bulletSpeed * Mathf.Cos(theta), 0, _normalEnemyData.bulletSpeed * Mathf.Sin(theta));
                GenerateBullet(Vector3.zero, vel);
            }
            else if (_normalEnemyData.shootPattern == ShootPattern.ToPlayer)
            {
                var delta = _gameController.GetPlayerPosition() - transform.position;
                float theta = Mathf.Atan2(delta.z, delta.x);
                Vector3 vel = new Vector3(_normalEnemyData.bulletSpeed * Mathf.Cos(theta), 0, _normalEnemyData.bulletSpeed * Mathf.Sin(theta));
                GenerateBullet(Vector3.zero, vel);
            }

            _timeForShooting = 0;
        }
    }

    protected override void Damaged2(int damage)
    {
        _hp -= damage;

        if (_hp <= 0)
        {
            Destroy(gameObject);
            _gameController.EnemyIsDestroyed(_score);
            MakeExplosion(transform.position);
        }
    }
}
