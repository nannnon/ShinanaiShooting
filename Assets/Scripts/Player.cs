using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject _playerBulletPrefab;
    [SerializeField]
    GameObject _bombExplosionPrefab;

    float _elapsedTimeForShooting = 0;
    bool _hit = false;
    SpriteRenderer _spriteRenderer;
    GameController _gameController;
    int _bombsNum = 3;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
    }

    void Update()
    {
        Move();
        ShootBullet();
        ShootBomb();

        if (_hit)
        {
            float level = Mathf.Abs(Mathf.Sin(Time.time * 10));
            _spriteRenderer.color = new Color(1f, 1f, 1f, level);
        }
    }

    void Move()
    {
        // 移動可能な範囲
        float x0 = GameController.ScreenPoint0.x;
        float x1 = GameController.ScreenPoint1.x;
        float z0 = GameController.ScreenPoint0.z;
        float z1 = GameController.ScreenPoint1.z;

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

    void ShootBomb()
    {
        if (_bombsNum > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            --_bombsNum;
            Instantiate(_bombExplosionPrefab);
            _gameController.PlayerUsedBomb(_bombsNum);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (_hit)
        {
            return;
        }

        if (other.tag == "Enemy" || other.tag == "EnemyBullet")
        {
            _hit = true;
            StartCoroutine(WaitAndBack());
            _gameController.PlayerIsHit();
        }
    }

    IEnumerator WaitAndBack()
    {
        yield return new WaitForSeconds(3);
        _hit = false;
        _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    public int GetBombsNum()
    {
        return _bombsNum;
    }
}
