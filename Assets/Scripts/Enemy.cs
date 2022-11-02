using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    GameObject _enemyBulletPrefab;

    public enum MovePattern
    {
        AppearAndShootAndBye
    }

    float _appearTime;
    MovePattern _movePattern;
    int _physicalStrength;
    float _elapsedTimeForShooting = 0;

    // Start is called before the first frame update
    void Start()
    {
        _appearTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        ShootBullet();
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

        if (_movePattern == MovePattern.AppearAndShootAndBye)
        {
            if (elapsedTime < 2)
            {
                Vector3 t = new Vector3(0, 0, -3 * Time.deltaTime);
                transform.Translate(t, Space.World);
            }
            else if (5 <= elapsedTime)
            {
                Vector3 t = new Vector3(0, 0, +3 * Time.deltaTime);
                transform.Translate(t, Space.World);
            }
        }

        // 画面外の特定範囲まで移動したら削除
        const float x0 = -4;
        const float x1 = 4;
        const float z0 = -2;
        const float z1 = 13;
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
            --_physicalStrength;
            if (_physicalStrength <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
