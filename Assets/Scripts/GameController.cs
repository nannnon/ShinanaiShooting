using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GameObject _enemy1Prefab;
    [SerializeField]
    GameObject _enemy2Prefab;
    [SerializeField]
    GameObject _enemy3Prefab;

    float _elapsedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= 3)
        {
            GameObject go = Instantiate(_enemy1Prefab);
            Enemy e = go.GetComponent<Enemy>();
            Vector3 pos = new Vector3(-2, 0, 12);
            e.Set(pos, Enemy.MovePattern.AppearAndShootAndBye, 1);

            _elapsedTime = 0;
        }
    }
}
