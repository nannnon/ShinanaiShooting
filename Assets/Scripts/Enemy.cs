using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    GameObject _enemyBulletPrefab;

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
    enum MoveState
    {
        State0,
        State1,
        State2,
        State3,
    }

    MovePattern _movePattern;
    int _physicalStrength;
    MoveState _moveState = MoveState.State0;
    float _timeForMoving;
    float _timeForShooting = 0;
    bool _damaged = false;
    SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        ShootBullet();

        if (_damaged)
        {
            float level = Mathf.Abs(Mathf.Sin(Time.time * 10));
            _spriteRenderer.color = new Color(1f, 1f, 1f, level);
        }
    }

    public void Set(Vector3 position, MovePattern movePattern, int physicalStrength)
    {
        transform.position = position;
        _movePattern = movePattern;
        _physicalStrength = physicalStrength;
    }

    void Move()
    {
        float moveSpeed = 3 * Time.deltaTime;
        float x0 = GameController.ScreenPoint0.x;
        float x1 = GameController.ScreenPoint1.x;
        float z0 = GameController.ScreenPoint0.z;
        float z1 = GameController.ScreenPoint1.z;

        if (_movePattern == MovePattern.AppearAndShootAndBye)
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
        else if (_movePattern == MovePattern.GoThrough)
        {
            Vector3 t = new Vector3(0, 0, -moveSpeed);
            transform.Translate(t, Space.World);
        }
        else if (_movePattern == MovePattern.SouthWest)
        {
            const float theta = 3 * Mathf.PI / 2 - Mathf.PI / 8;
            Vector3 t = new Vector3(moveSpeed * Mathf.Cos(theta), 0, moveSpeed * Mathf.Sin(theta));
            transform.Translate(t, Space.World);
        }
        else if (_movePattern == MovePattern.SouthEast)
        {
            const float theta = 3 * Mathf.PI / 2 + Mathf.PI / 8;
            Vector3 t = new Vector3(moveSpeed * Mathf.Cos(theta), 0, moveSpeed * Mathf.Sin(theta));
            transform.Translate(t, Space.World);
        }
        else if (_movePattern == MovePattern.UFromLeft)
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
        else if (_movePattern == MovePattern.UFromRight)
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
        else if (_movePattern == MovePattern.NInvert)
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
        else if (_movePattern == MovePattern.N)
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
        _timeForShooting += Time.deltaTime;
        const float shootingCycle = 1.2f;

        if (_timeForShooting >= shootingCycle)
        {
            // 弾を発射
            GameObject go = Instantiate(_enemyBulletPrefab);
            Bullet b = go.GetComponent<Bullet>();
            Vector3 pos = transform.position + new Vector3(0, 0, -0.5f);
            Vector3 vel = new Vector3(0, 0, -5f);
            b.Set(pos, vel);

            _timeForShooting = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "PlayerBullet")
        {
            _damaged = true;
            StartCoroutine("WaitAndBack");

            --_physicalStrength;
            if (_physicalStrength <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator WaitAndBack()
    {
        yield return new WaitForSeconds(1);
        _damaged = false;
        _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
}
