using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    GameObject _enemyBulletPrefab;
    [SerializeField]
    GameObject _explosionPrefab;

    protected int _hp;
    protected int _score;
    bool _hit = false;
    SpriteRenderer _spriteRenderer;
    protected GameController _gameController;

    protected enum MoveState
    {
        State0,
        State1,
        State2,
        State3,
    }
    protected MoveState _moveState = MoveState.State0;
    protected float _timeForShooting = 0;

    protected void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
    }

    protected void Update()
    {
        if (_hit)
        {
            float level = Mathf.Abs(Mathf.Sin(Time.time * 10));
            _spriteRenderer.color = new Color(1f, 1f, 1f, level);
        }
    }

    public void Set(Vector3 position, int hp, int score)
    {
        transform.position = position;
        _hp = hp;
        _score = score;
    }

    protected void GenerateBullet(Vector3 relativePos, Vector3 vel)
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
            Damaged(1);
        }
    }

    protected void MakeExplosion(Vector3 pos)
    {
        GameObject go = Instantiate(_explosionPrefab);
        go.transform.position = pos;
    }

    public void Damaged(int damage)
    {
        if (!_hit)
        {
            _hit = true;
            StartCoroutine(WaitAndBack());
        }

        Damaged2(damage);
    }

    IEnumerator WaitAndBack()
    {
        yield return new WaitForSeconds(1);
        _hit = false;
        _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    protected abstract void Damaged2(int damage);
}