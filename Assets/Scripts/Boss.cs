using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemy
{
    [SerializeField]
    GameObject _hpBarPrefab;
    [SerializeField]
    GameObject _cautionPrefab;
    [SerializeField]
    GameObject _winPrefab;

    int _maxHP;
    Slider _hpBarSlider;

    protected new void Start()
    {
        base.Start();

        // HPバーをインスタン化
        GameObject canvas = GameObject.Find("Canvas");
        GameObject go = Instantiate(_hpBarPrefab, canvas.transform);
        _hpBarSlider = go.GetComponent<Slider>();

        // Cautionを表示
        Instantiate(_cautionPrefab);
    }

    public new void Set(Vector3 position, int hp, int score)
    {
        base.Set(position, hp, score);
        _maxHP = hp;
    }

    protected override void Damaged2(int damage)
    {
        _hp -= damage;
        _hpBarSlider.value = (float)_hp / (float)_maxHP;

        if (_hp <= 0)
        {
            Destroy(gameObject);
            _gameController.EnemyIsDestroyed(_score);

            MakeExplosion(transform.position + new Vector3(0.3f, 0, 0));
            MakeExplosion(transform.position + new Vector3(-0.3f, 0, 0.1f));
            MakeExplosion(transform.position + new Vector3(-0.2f, 0, -0.2f));

            Instantiate(_winPrefab);
        }
    }
}