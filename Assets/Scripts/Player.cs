using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject _playerBulletPrefab;
    [SerializeField]
    GameObject _bombExplosionPrefab;
    [SerializeField]
    AudioClip _damaged;
    [SerializeField]
    AudioClip _getItem;
    [SerializeField]
    Sprite _player2Sprite;
    [SerializeField]
    Sprite _player3Sprite;

    enum PowerUpStatus
    {
        SingleShot,
        ThreeWay,
        DoubleFunnel,
    }

    float _elapsedTimeForShooting = 0;
    bool _hit = false;
    SpriteRenderer _spriteRenderer;
    GameController _gameController;
    int _bombsNum = 3;
    AudioSource _audioSource;
    static PowerUpStatus s_powerUpStatus = PowerUpStatus.SingleShot;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        _audioSource = GetComponent<AudioSource>();

        switch (s_powerUpStatus)
        {
            case PowerUpStatus.ThreeWay:
                _spriteRenderer.sprite = _player2Sprite;
                break;
            case PowerUpStatus.DoubleFunnel:
                _spriteRenderer.sprite = _player3Sprite;
                break;
        }
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
            GenerateBullet(new Vector3(0, 0, 0.5f), new Vector3(0, 0, 6f));

            if (s_powerUpStatus == PowerUpStatus.ThreeWay || s_powerUpStatus == PowerUpStatus.DoubleFunnel)
            {
                // 左右斜め方向に弾を発射
                GenerateBullet(new Vector3(0, 0, 0.5f), new Vector3(-2, 0, 5f));
                GenerateBullet(new Vector3(0, 0, 0.5f), new Vector3(+2, 0, 5f));
            }

            if (s_powerUpStatus == PowerUpStatus.DoubleFunnel)
            {
                // 左右のファンネルから弾発射
                GenerateBullet(new Vector3(-0.5f, 0, 0), new Vector3(0, 0, 6f));
                GenerateBullet(new Vector3(+0.5f, 0, 0), new Vector3(0, 0, 6f));
            }

            _elapsedTimeForShooting = 0;
        }
    }

    void GenerateBullet(Vector3 relativePos, Vector3 vel)
    {
        GameObject go = Instantiate(_playerBulletPrefab);
        Bullet b = go.GetComponent<Bullet>();
        Vector3 pos = transform.position + relativePos;
        b.Set(pos, vel);
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

            _audioSource.PlayOneShot(_damaged);
        }
        else if (other.tag == "Item")
        {
            switch (s_powerUpStatus)
            {
                case PowerUpStatus.SingleShot:
                    s_powerUpStatus = PowerUpStatus.ThreeWay;
                    _spriteRenderer.sprite = _player2Sprite;
                    break;
                case PowerUpStatus.ThreeWay:
                    s_powerUpStatus = PowerUpStatus.DoubleFunnel;
                    _spriteRenderer.sprite = _player3Sprite;
                    break;
            }

            _audioSource.PlayOneShot(_getItem);
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
