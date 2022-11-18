using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss1 : MonoBehaviour
{
    [SerializeField]
    GameObject _enemyBulletPrefab;
    [SerializeField]
    GameObject _psBar;

    enum MoveState
    {
        State0,
        State1,
        State2,
    }

    const int _maxPhysicalStrength = 30;
    int _physicalStrength = _maxPhysicalStrength;
    MoveState _moveState = MoveState.State0;
    float _timeForShooting = 0;
    float _thetaForShooting = Mathf.PI * 5 / 4;
    bool _hit = false;
    SpriteRenderer _spriteRenderer;
    GameObject _player;
    Slider _psBarSlider;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player");

        // HPバーをインスタン化
        GameObject canvas = GameObject.Find("Canvas");
        GameObject go = Instantiate(_psBar, canvas.transform);

        _psBarSlider = go.GetComponent<Slider>();
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

    void Move()
    {
        float moveSpeed = 1.5f * Time.deltaTime;
        float x0 = GameController.ScreenPoint0.x;
        float x1 = GameController.ScreenPoint1.x;
        float z0 = GameController.ScreenPoint0.z;
        float z1 = GameController.ScreenPoint1.z;

        if (_moveState == MoveState.State0)
        {
            Vector3 t = new Vector3(0, 0, -moveSpeed);
            transform.Translate(t, Space.World);

            if (transform.position.z <= z1)
            {
                _moveState = MoveState.State1;
            }
        }
        else if (_moveState == MoveState.State1)
        {
            Vector3 t = new Vector3(moveSpeed, 0, 0);
            transform.Translate(t, Space.World);

            if (transform.position.x >= x1)
            {
                _moveState = MoveState.State2;
            }
        }
        else if (_moveState == MoveState.State2)
        {
            Vector3 t = new Vector3(-moveSpeed, 0, 0);
            transform.Translate(t, Space.World);

            if (transform.position.x <= x0)
            {
                _moveState = MoveState.State1;
            }
        }
    }

    void ShootBullet()
    {
        _timeForShooting += Time.deltaTime;
        const float shootingCycleTime = 0.3f;
        const float bulletSpeed = 6;

        if (_timeForShooting >= shootingCycleTime)
        {
            if (_moveState == MoveState.State1)
            {
                // Random
                {
                    float theta = Random.Range(0, 2 * Mathf.PI);
                    Vector3 vel = new Vector3(bulletSpeed * Mathf.Cos(theta), 0, bulletSpeed * Mathf.Sin(theta));
                    Vector3 rpos = new Vector3(-1, 0, 0);
                    GenerateBullet(rpos, vel);
                }
                // ToPlayer
                {
                    var delta = _player.transform.position - transform.position;
                    float theta = Mathf.Atan2(delta.z, delta.x);
                    Vector3 vel = new Vector3(bulletSpeed * Mathf.Cos(theta), 0, bulletSpeed * Mathf.Sin(theta));
                    Vector3 rpos = new Vector3(1, 0, 0);
                    GenerateBullet(rpos, vel);
                }
            }
            else if (_moveState == MoveState.State2)
            {
                Vector3 vel = new Vector3(bulletSpeed * Mathf.Cos(_thetaForShooting), 0, bulletSpeed * Mathf.Sin(_thetaForShooting));
                Vector3 rpos = new Vector3(0, 0, -1);
                GenerateBullet(rpos, vel);

                _thetaForShooting += 0.3f;
                if (_thetaForShooting >= Mathf.PI * 7 / 4)
                {
                    _thetaForShooting = Mathf.PI * 5 / 4;
                }
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

            --_physicalStrength;
            _psBarSlider.value = (float)_physicalStrength / (float)_maxPhysicalStrength;
            if (_physicalStrength <= 0)
            {
                Destroy(gameObject);

                // ToDo game clear
            }
        }
    }

    IEnumerator WaitAndBack()
    {
        yield return new WaitForSeconds(1);
        _hit = false;
        _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
}
