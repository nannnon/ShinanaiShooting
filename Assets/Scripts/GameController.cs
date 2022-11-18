using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class GameController : MonoBehaviour
{
    public static readonly Vector3 ScreenPoint0 = new Vector3(-3, 0, 0);
    public static readonly Vector3 ScreenPoint1 = new Vector3(+3, 0, 10);

    [SerializeField]
    TextAsset _csvFile;
    [SerializeField]
    GameObject _enemy1Prefab;
    [SerializeField]
    GameObject _enemy2Prefab;
    [SerializeField]
    GameObject _enemy3Prefab;
    [SerializeField]
    GameObject _bossPrefab;

    struct EnemyData
    {
        public GameObject enemyType;
        public bool isBoss;
        public float appearTime;
        public Vector3 position;
        public float moveSpeedCoef;
        public Enemy.MovePattern movePattern;
        public Enemy.ShootPattern shootPattern;
        public float timeToStartShooting;
        public float shootingCycleTime;
        public float bulletSpeed;
        public int physicalStrength;
    }

    List<EnemyData> _enemyData;
    float _startTime;
    CameraController _cameraController;
    TextMeshProUGUI _hitNumTMP;
    int _hitCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        _startTime = Time.time;
        _cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();  
        _hitNumTMP = GameObject.Find("HitNum").GetComponent<TextMeshProUGUI>();

        LoadCSV();
    }

    void LoadCSV()
    {
        StringReader reader = new StringReader(_csvFile.text);
        reader.ReadLine(); // ヘッダをスキップ
        _enemyData = new List<EnemyData>();
        while (reader.Peek() != -1)
        {
            string[] items = reader.ReadLine().Split(',');
            EnemyData ed = new EnemyData();

            ed.isBoss = false;
            switch (int.Parse(items[0]))
            {
                case 0:
                    ed.enemyType = _bossPrefab;
                    ed.isBoss = true;
                    break;
                case 1:
                    ed.enemyType = _enemy1Prefab;
                    break;
                case 2:
                    ed.enemyType = _enemy2Prefab;
                    break;
                case 3:
                    ed.enemyType = _enemy3Prefab;
                    break;
            }

            ed.appearTime = float.Parse(items[1]);
            ed.position = new Vector3(float.Parse(items[2]), 0, float.Parse(items[3]));

            if (ed.isBoss)
            {
                _enemyData.Add(ed);
                continue;
            }

            ed.moveSpeedCoef = float.Parse(items[4]);
            ed.movePattern = Enum.Parse<Enemy.MovePattern>(items[5]);
            ed.shootPattern = Enum.Parse<Enemy.ShootPattern>(items[6]);
            ed.timeToStartShooting = float.Parse(items[7]);
            ed.shootingCycleTime = float.Parse(items[8]);
            ed.bulletSpeed = float.Parse(items[9]);
            ed.physicalStrength = int.Parse(items[10]);

            _enemyData.Add(ed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float elapsedTime = Time.time - _startTime;
        for (int i = _enemyData.Count - 1; i >= 0; --i)
        {
            EnemyData ed = _enemyData[i];

            if (elapsedTime >= ed.appearTime)
            {
                GameObject go = Instantiate(ed.enemyType);

                if (ed.isBoss)
                {
                    go.transform.position = ed.position;
                }
                else
                {
                    Enemy e = go.GetComponent<Enemy>();
                    e.Set(ed.position, ed.moveSpeedCoef, ed.movePattern, ed.shootPattern, ed.timeToStartShooting, ed.shootingCycleTime, ed.bulletSpeed, ed.physicalStrength);
                }

                _enemyData.RemoveAt(i);
            }
        }
    }

    public void PlayerIsHit()
    {
        ++_hitCounter;
        _hitNumTMP.text = "被弾数：" + _hitCounter;

        _cameraController.Shake();
    }
}
