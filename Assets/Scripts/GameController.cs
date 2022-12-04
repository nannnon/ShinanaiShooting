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
    GameObject _enemy4Prefab;
    [SerializeField]
    GameObject _enemy5Prefab;
    [SerializeField]
    GameObject _bossPrefab;
    [SerializeField]
    GameObject _result;
    [SerializeField]
    String _nextStageName;

    struct EnemyData
    {
        public GameObject enemyType;
        public bool isBoss;
        public float appearTime;
        public Vector3 position;
        public int hp;
        public int score;
        public NormalEnemyData normalEnemyData;
    }

    List<EnemyData> _enemyData;
    float _startTime;
    CameraController _cameraController;
    int _playerHitCounter = 0;
    int _score = 0;
    GameObject _player;
    TextMeshProUGUI _hitNumTMP;
    TextMeshProUGUI _scoreTMP;
    TextMeshProUGUI _bombsNumTMP;

    void Start()
    {
        _startTime = Time.time;
        _cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();  
        _player = GameObject.FindGameObjectWithTag("Player");
        _hitNumTMP = GameObject.Find("HitNum").GetComponent<TextMeshProUGUI>();
        _scoreTMP = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        _bombsNumTMP = GameObject.Find("BombNum").GetComponent<TextMeshProUGUI>();

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
                case 4:
                    ed.enemyType = _enemy4Prefab;
                    break;
                case 5:
                    ed.enemyType = _enemy5Prefab;
                    break;
                default:
                    throw new Exception("Not found");
            }

            ed.appearTime = float.Parse(items[1]);
            ed.position = new Vector3(float.Parse(items[2]), 0, float.Parse(items[3]));
            ed.hp = int.Parse(items[4]);
            ed.score = int.Parse(items[5]);

            if (ed.isBoss)
            {
                _enemyData.Add(ed);
                continue;
            }

            ed.normalEnemyData.moveSpeedCoef = float.Parse(items[6]);
            ed.normalEnemyData.movePattern = Enum.Parse<MovePattern>(items[7]);
            ed.normalEnemyData.shootPattern = Enum.Parse<ShootPattern>(items[8]);
            ed.normalEnemyData.timeToStartShooting = float.Parse(items[9]);
            ed.normalEnemyData.shootingCycleTime = float.Parse(items[10]);
            ed.normalEnemyData.bulletSpeed = float.Parse(items[11]);
            ed.normalEnemyData.isItemHolder = bool.Parse(items[12]);

            _enemyData.Add(ed);
        }
    }

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
                    Boss b = go.GetComponent<Boss>();
                    b.Set(ed.position, ed.hp, ed.score);
                }
                else
                {
                    NormalEnemy e = go.GetComponent<NormalEnemy>();
                    e.Set(ed.position, ed.hp, ed.score, ed.normalEnemyData);
                }

                _enemyData.RemoveAt(i);
            }
        }
    }

    public void PlayerIsHit()
    {
        ++_playerHitCounter;
        _hitNumTMP.text = "被弾数：" + _playerHitCounter;

        _cameraController.Shake();
    }

    public void EnemyIsDestroyed(int score)
    {
        _score += score;
        _scoreTMP.text = "スコア：" + _score;
    }

    public Vector3 GetPlayerPosition()
    {
        return _player.transform.position;
    }

    public void StageClear()
    {
        _result.SetActive(true);

        var resultText = GameObject.Find("ResultText").GetComponent<TextMeshProUGUI>();

        resultText.text = "";
        resultText.text += "スコア：" + _score + "<br>";

        int hitPenaltyScore = _playerHitCounter * -3;
        resultText.text += "被弾によるスコア減少：" + hitPenaltyScore + "<br>";

        int bombBonusScore = _player.GetComponent<Player>().GetBombsNum() * 10;
        resultText.text += "ボム残弾によるスコア増加：" + bombBonusScore + "<br>";

        int finalScore = _score + hitPenaltyScore + bombBonusScore;
        resultText.text += "最終スコア：" + finalScore + "<br>";

        String eval = "";
        if (finalScore <= 20)
        {
            eval = "残念な感じ";
        }
        else if (finalScore <= 60)
        {
            eval = "良い感じ";
        }
        else
        {
            eval = "素晴らしい";
        }
        resultText.text += "評価：" + eval;
    }

    public void LoadNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_nextStageName);
    }

    public void PlayerUsedBomb(int bombsNum)
    {
        // ボム数表示反映
        _bombsNumTMP.text = "ボム数：" + bombsNum;

        // 全ての敵弾を消す
        {
            var gos = GameObject.FindGameObjectsWithTag("EnemyBullet");
            foreach (GameObject go in gos)
            {
                Destroy(go);
            }
        }

        // 全ての敵にダメージを与える
        {
            var gos = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject go in gos)
            {
                const int damage = 5;
                go.GetComponent<Enemy>().Damaged(damage);
            }
        }
    }
}
