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

 public struct EnemyData
{
    public float moveSpeedCoef;
    public MovePattern movePattern;
    public ShootPattern shootPattern;
    public float timeToStartShooting;
    public float shootingCycleTime;
    public float bulletSpeed;
    public int physicalStrength;
    public int score;
}

public class Enemy : MonoBehaviour
{
    [SerializeField]
    GameObject _enemyBulletPrefab;
    [SerializeField]
    GameObject _explosionPrefab;

    enum MoveState
    {
        State0,
        State1,
        State2,
        State3,
    }

    EnemyData _enemyData;

    MoveState _moveState = MoveState.State0;
    float _timeForMoving;
    float _appearTime;
    float _timeForShooting = 0;
    bool _hit = false;
    SpriteRenderer _spriteRenderer;
    GameController _gameController;

    // Start is called before the first frame update
    void Start()
    {
        _appearTime = Time.time;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        ShootBullet();

        if (_hit)
        {
            float level = Mathf.Abs(Mathf.Sin(Time.time * 10));
            _spriteRenderer.color = new Color(1f, 1f, 1f, level);
        }
    }

    public void Set(Vector3 position, EnemyData enemyData)
    {
        transform.position = position;
        _enemyData = enemyData;
    }

    void Move()
    {
        float moveSpeed = _enemyData.moveSpeedCoef * Time.deltaTime;
        float x0 = GameController.ScreenPoint0.x;
        float x1 = GameController.ScreenPoint1.x;
        float z0 = GameController.ScreenPoint0.z;
        float z1 = GameController.ScreenPoint1.z;

        if (_enemyData.movePattern == MovePattern.AppearAndShootAndBye)
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
        else if (_enemyData.movePattern == MovePattern.GoThrough)
        {
            Vector3 t = new Vector3(0, 0, -moveSpeed);
            transform.Translate(t, Space.World);
        }
        else if (_enemyData.movePattern == MovePattern.SouthWest)
        {
            const float theta = 3 * Mathf.PI / 2 - Mathf.PI / 8;
            Vector3 t = new Vector3(moveSpeed * Mathf.Cos(theta), 0, moveSpeed * Mathf.Sin(theta));
            transform.Translate(t, Space.World);
        }
        else if (_enemyData.movePattern == MovePattern.SouthEast)
        {
            const float theta = 3 * Mathf.PI / 2 + Mathf.PI / 8;
            Vector3 t = new Vector3(moveSpeed * Mathf.Cos(theta), 0, moveSpeed * Mathf.Sin(theta));
            transform.Translate(t, Space.World);
        }
        else if (_enemyData.movePattern == MovePattern.UFromLeft)
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
        else if (_enemyData.movePattern == MovePattern.UFromRight)
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
        else if (_enemyData.movePattern == MovePattern.NInvert)
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
        else if (_enemyData.movePattern == MovePattern.N)
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

        if (elapsedTime >= _enemyData.timeToStartShooting && _timeForShooting >= _enemyData.shootingCycleTime)
        {
            if (_enemyData.shootPattern == ShootPattern.NotShooting)
            {
                // 何もしない
            }
            else if (_enemyData.shootPattern == ShootPattern.Straight)
            {
                GenerateBullet(new Vector3(0, 0, -0.5f), new Vector3(0, 0, -_enemyData.bulletSpeed));
            }
            else if (_enemyData.shootPattern == ShootPattern.Random)
            {
                float theta = Random.Range(0, 2 * Mathf.PI);
                Vector3 vel = new Vector3(_enemyData.bulletSpeed * Mathf.Cos(theta), 0, _enemyData.bulletSpeed * Mathf.Sin(theta));
                GenerateBullet(Vector3.zero, vel);
            }
            else if (_enemyData.shootPattern == ShootPattern.ToPlayer)
            {
                var delta = _gameController.GetPlayerPosition() - transform.position;
                float theta = Mathf.Atan2(delta.z, delta.x);
                Vector3 vel = new Vector3(_enemyData.bulletSpeed * Mathf.Cos(theta), 0, _enemyData.bulletSpeed * Mathf.Sin(theta));
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
            _hit = true;
            StartCoroutine(WaitAndBack());

            --_enemyData.physicalStrength;
            if (_enemyData.physicalStrength <= 0)
            {
                Destroy(gameObject);
                Explode();
                _gameController.EnemyIsDestroyed(_enemyData.score);
            }
        }
    }

    IEnumerator WaitAndBack()
    {
        yield return new WaitForSeconds(1);
        _hit = false;
        _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    void Explode()
    {
        GameObject go = Instantiate(_explosionPrefab);
        go.transform.position = transform.position;
    }
}
