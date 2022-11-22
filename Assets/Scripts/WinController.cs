using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndStageClear());
    }

    IEnumerator WaitAndStageClear()
    {
        yield return new WaitForSeconds(2);

        var gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        gameController.StageClear();

        Destroy(gameObject);
    }
}
