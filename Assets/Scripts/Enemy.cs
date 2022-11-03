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
        GoThrough
    }

    float _appearTime;
    MovePattern _movePattern;
    int _physicalStrength;
    float _elapsedTimeForShooting = 0;
    bool _damaged = false;
    SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _appearTime = Time.time;
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
        float elapsedTime = Time.time - _appearTime;
        float moveSpeed = 3 * Time.deltaTime;

        if (_movePattern == MovePattern.AppearAndShootAndBye)
        {
            if (elapsedTime < 2)
            {
                Vector3 t = new Vector3(0, 0, -moveSpeed);
                transform.Translate(t, Space.World);
            }
            else if (5 <= elapsedTime)
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

        // 画面外の特定範囲まで移動したら削除
        const float x0 = -5;
        const float x1 = +5;
        const float z0 = -3;
        const float z1 = 14;
        if (transform.position.x < x0 ||
            transform.position.x > x1 ||
            transform.position.z < z0 ||
            transform.position.z > z1 )
        {
            Destroy(gameObject);
        }
    }

    void ShootBullet()
    {
        _elapsedTimeForShooting += Time.deltaTime;
        const float shootingCycle = 1.2f;

        if (_elapsedTimeForShooting >= shootingCycle)
        {
            // 弾を発射
            GameObject go = Instantiate(_enemyBulletPrefab);
            Bullet b = go.GetComponent<Bullet>();
            Vector3 pos = transform.position + new Vector3(0, 0, -0.5f);
            Vector3 vel = new Vector3(0, 0, -5f);
            b.Set(pos, vel);

            _elapsedTimeForShooting = 0;
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
