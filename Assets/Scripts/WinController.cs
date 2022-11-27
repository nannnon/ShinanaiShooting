using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndStageClear());

        Time.timeScale = 0.1f;
    }

    IEnumerator WaitAndStageClear()
    {
        yield return new WaitForSeconds(0.2f);

        var gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        gameController.StageClear();

        Destroy(gameObject);

        Time.timeScale = 1;
    }
}
