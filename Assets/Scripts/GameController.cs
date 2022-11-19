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
    [SerializeField]
    GameObject _result;
    [SerializeField]
    String _nextStageName;

    struct EnemyDataPlus
    {
        public GameObject enemyType;
        public bool isBoss;
        public float appearTime;
        public Vector3 position;
        public EnemyData enemyData;
    }

    List<EnemyDataPlus> _enemyDataPlus;
    float _startTime;
    CameraController _cameraController;
    int _playerHitCounter = 0;
    TextMeshProUGUI _hitNumTMP;
    int _score = 0;
    TextMeshProUGUI _scoreTMP;
    GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _startTime = Time.time;
        _cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();  
        _hitNumTMP = GameObject.Find("HitNum").GetComponent<TextMeshProUGUI>();
        _scoreTMP = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        _player = GameObject.FindGameObjectWithTag("Player");

        LoadCSV();
    }

    void LoadCSV()
    {
        StringReader reader = new StringReader(_csvFile.text);
        reader.ReadLine(); // ヘッダをスキップ
        _enemyDataPlus = new List<EnemyDataPlus>();
        while (reader.Peek() != -1)
        {
            string[] items = reader.ReadLine().Split(',');
            EnemyDataPlus edp = new EnemyDataPlus();

            edp.isBoss = false;
            switch (int.Parse(items[0]))
            {
                case 0:
                    edp.enemyType = _bossPrefab;
                    edp.isBoss = true;
                    break;
                case 1:
                    edp.enemyType = _enemy1Prefab;
                    break;
                case 2:
                    edp.enemyType = _enemy2Prefab;
                    break;
                case 3:
                    edp.enemyType = _enemy3Prefab;
                    break;
            }

            edp.appearTime = float.Parse(items[1]);
            edp.position = new Vector3(float.Parse(items[2]), 0, float.Parse(items[3]));

            if (edp.isBoss)
            {
                _enemyDataPlus.Add(edp);
                continue;
            }

            edp.enemyData.moveSpeedCoef = float.Parse(items[4]);
            edp.enemyData.movePattern = Enum.Parse<MovePattern>(items[5]);
            edp.enemyData.shootPattern = Enum.Parse<ShootPattern>(items[6]);
            edp.enemyData.timeToStartShooting = float.Parse(items[7]);
            edp.enemyData.shootingCycleTime = float.Parse(items[8]);
            edp.enemyData.bulletSpeed = float.Parse(items[9]);
            edp.enemyData.physicalStrength = int.Parse(items[10]);
            edp.enemyData.score = int.Parse(items[11]);

            _enemyDataPlus.Add(edp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float elapsedTime = Time.time - _startTime;
        for (int i = _enemyDataPlus.Count - 1; i >= 0; --i)
        {
            EnemyDataPlus edp = _enemyDataPlus[i];

            if (elapsedTime >= edp.appearTime)
            {
                GameObject go = Instantiate(edp.enemyType);

                if (edp.isBoss)
                {
                    go.transform.position = edp.position;
                }
                else
                {
                    Enemy e = go.GetComponent<Enemy>();
                    e.Set(edp.position, edp.enemyData);
                }

                _enemyDataPlus.RemoveAt(i);
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

        int bombBonusScore = 3 * 10;
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
}
