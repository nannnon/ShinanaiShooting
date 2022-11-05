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
    public enum ShootPattern
    {
        Straight,
        Random,
        ToPlayer,
    }

    MovePattern _movePattern;
    ShootPattern _shootPattern;
    float _shootingCycle;
    int _physicalStrength;

    MoveState _moveState = MoveState.State0;
    float _timeForMoving;
    float _timeForShooting = 0;
    bool _damaged = false;
    SpriteRenderer _spriteRenderer;
    GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.Find("Player");
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

    public void Set(Vector3 position, MovePattern movePattern, ShootPattern shootPattern, float shootingCycle, int physicalStrength)
    {
        transform.position = position;
        _movePattern = movePattern;
        _shootPattern = shootPattern;
        _shootingCycle = shootingCycle;
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

        if (_timeForShooting >= _shootingCycle)
        {
            const float bulletSpeed = 5f;

            if (_shootPattern == ShootPattern.Straight)
            {
                GenerateBullet(new Vector3(0, 0, -0.5f), new Vector3(0, 0, -bulletSpeed));
            }
            else if (_shootPattern == ShootPattern.Random)
            {
                float theta = Random.Range(0, 2 * Mathf.PI);
                Vector3 vel = new Vector3(bulletSpeed * Mathf.Cos(theta), 0, bulletSpeed * Mathf.Sin(theta));
                GenerateBullet(Vector3.zero, vel);
            }
            else if (_shootPattern == ShootPattern.ToPlayer)
            {
                float dx = _player.transform.position.x - transform.position.x;
                float dz = _player.transform.position.z - transform.position.z;
                float theta = Mathf.Atan2(dz, dx);
                Vector3 vel = new Vector3(bulletSpeed * Mathf.Cos(theta), 0, bulletSpeed * Mathf.Sin(theta));
                GenerateBullet(Vector3.zero, vel);
            }

            _timeForShooting = 0;
        }
    }

    void GenerateBullet(Vector3 relativePos, Vector3 vel)
    {
        GameObject go = Instantiate(_enemyBulletPrefab);
        Bullet b = go.GetComponent<Bullet>();
        Vector3 pos = transform.position + relativePos;
        b.Set(pos, vel);
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
