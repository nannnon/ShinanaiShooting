using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static readonly Vector3 ScreenPoint0 = new Vector3(-3, 0, 0);
    public static readonly Vector3 ScreenPoint1 = new Vector3(+3, 0, 10);

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
            GameObject go = Instantiate(_enemy2Prefab);
            Enemy e = go.GetComponent<Enemy>();
            Vector3 pos = new Vector3(2, 0, 12);
            e.Set(pos, Enemy.MovePattern.UFromRight, 5);

            _elapsedTime = 0;
        }
    }
}
