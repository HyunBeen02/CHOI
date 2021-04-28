using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCreator : MonoBehaviour
{   //this is used at GameScene
    public GameObject[] spring_Obstacle = new GameObject[2];
    public GameObject[] summer_Obstacle = new GameObject[2];
    public GameObject[] autumn_Obstacle = new GameObject[3];
    public GameObject[] winter_Obstacle = new GameObject[3];

    public float[] wait_spring = new float[8];
    public float[] wait_Summer = new float[10];
    public float[] wait_Autumn = new float[13];
    public float[] wait_Winter = new float[12];

    GameManager gameManager;

    //distanceTime = 1.7f;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        startStageObstacle();
    }
    void startStageObstacle()//스테이지에 따른 코루틴 실행
    {
        switch (Manager.instance.selectedStage)
        {
            case 1: StartCoroutine(start_stage_Spring()); break;
            case 2: StartCoroutine(start_stage_Summer()); break;
            case 3: StartCoroutine(start_stage_Autumn()); break;
            case 4: StartCoroutine(start_stage_Winter()); break;
            default: Debug.Log("Failed to load stage number"); break;
        }
    }
    void createObstacle_Spring()//장애물 생성
    {
        GameObject obj = spring_Obstacle[Random.Range(0, 2)];
        obj = Instantiate(obj, obj.transform.position, Quaternion.identity);
        obj.SetActive(true);
    }
    void createObstacle_Summer()//장애물 생성
    {
        GameObject obj = summer_Obstacle[Random.Range(0, 2)];
        obj = Instantiate(obj, obj.transform.position, Quaternion.identity);
        obj.SetActive(true);
    }
    void createObstacle_Autumn()//장애물 생성
    {
        GameObject obj = autumn_Obstacle[Random.Range(0, 3)];
        obj = Instantiate(obj, obj.transform.position, Quaternion.identity);
        obj.SetActive(true);
    }
    void createObstacle_Winter()//장애물 생성
    {
        GameObject obj = winter_Obstacle[Random.Range(0, 3)];
        obj = Instantiate(obj, obj.transform.position, Quaternion.identity);
        obj.SetActive(true);
    }
    IEnumerator start_stage_Spring()//노래 시간에 따라 장애물 생성
    {
        yield return new WaitForSeconds(0.6f);
        for (int index = 0; index < wait_spring.Length; index++)
        {
            yield return new WaitForSeconds(wait_spring[index]);
            createObstacle_Spring();
        }

        yield return new WaitForSeconds(6);
        gameManager.runClear();
    }
    IEnumerator start_stage_Summer()//노래 시간에 따라 장애물 생성
    {
        yield return new WaitForSeconds(0.6f);
        for (int index = 0; index < wait_Summer.Length; index++)
        {
            yield return new WaitForSeconds(wait_Summer[index]);
            createObstacle_Summer();
        }

        yield return new WaitForSeconds(6);
        gameManager.runClear();
    }
    IEnumerator start_stage_Autumn()//노래 시간에 따라 장애물 생성
    {
        yield return new WaitForSeconds(0.6f);
        for (int index = 0; index < wait_Autumn.Length; index++)
        {
            yield return new WaitForSeconds(wait_Autumn[index]);
            createObstacle_Autumn();
        }

        yield return new WaitForSeconds(6);
        gameManager.runClear();
    }
    IEnumerator start_stage_Winter()//노래 시간에 따라 장애물 생성
    {
        yield return new WaitForSeconds(0.6f);
        for (int index = 0; index < wait_Winter.Length; index++)
        {
            yield return new WaitForSeconds(wait_Winter[index]);
            createObstacle_Winter();
        }

        yield return new WaitForSeconds(6);
        gameManager.runClear();
    }
}
