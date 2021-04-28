using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    EnemyGenerator enemyGenerator;
    void Start()
    {
        enemyGenerator = GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>();
    }
    public void generateEnemy()
    {
        StartCoroutine(stageGenerateEnemy());
    }
    //좀비를 16번 + 일수 만큼 생성 만약 8일이 지나고 dayIdx가 8로 나눠 떨어지면
    //보스급 적을 생성함
    IEnumerator stageGenerateEnemy()
    {
        WaitForSeconds spownDelay = new WaitForSeconds(0.75f);
        for (int dayIdx = 0; dayIdx < 16 + StateManager.dayCount; dayIdx++)
        {
            if (StateManager.dayCount >= 8 && dayIdx % 8 == 0)
            {
                //사용가능한 보스급 적을 찾아 위치 조정후 활성화해줌
                for (int bossIdx = 0; bossIdx < enemyGenerator.bosses.Length; bossIdx++)
                {
                    if (!enemyGenerator.bosses[bossIdx].activeSelf)
                    {
                        enemyGenerator.bosses[bossIdx].transform.position = getSpownPoint();
                        enemyGenerator.bosses[bossIdx].SetActive(true);
                        EnemyChecker.enemyCount++;
                        yield return spownDelay;
                        break;
                    }
                }
            }
            //사용가능한 좀비를 찾아 위치 조정후 활성화해줌
            for (int zomIdx = 0; zomIdx < enemyGenerator.zombies.Length; zomIdx++)
            {
                if (!enemyGenerator.zombies[zomIdx].activeSelf)
                {
                    enemyGenerator.zombies[zomIdx].transform.position = getSpownPoint();
                    enemyGenerator.zombies[zomIdx].SetActive(true);
                    EnemyChecker.enemyCount++;
                    yield return spownDelay;
                    break;
                }
            }
        }
    }
    //맵의 -22 부터 22까지 사각형의 형태로 랜덤 위치 반환
    Vector3 getSpownPoint()
    {
        int PosType = Random.Range(0, 4);
        int ranX = Random.Range(-20, 21);
        int ranZ = Random.Range(-20, 21);
        if (PosType == 0)
            return new Vector3(20, 0, ranZ);
        else if (PosType == 1)
            return new Vector3(-20, 0, ranZ);
        else if (PosType == 2)
            return new Vector3(ranX, 0, 20);
        else if (PosType == 3)
            return new Vector3(ranX, 0, -20);
        else
            return new Vector3(20, 0, 20);
    }
}
