using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] Transform zombieParent; //좀비들의 부모가 될 객체
    [SerializeField] GameObject zombie1;
    [SerializeField] GameObject zombie2;

    [SerializeField] Transform bossParent; //보스급 적들의 부모가 될 객체
    [SerializeField] GameObject boss;

    int zombiesSize = 80;
    public GameObject[] zombies;
    int bossesSize = 40;
    public GameObject[] bosses;

    void Start()
    {
        generateEnemy();
    }
    //오브젝트 풀링을 위한 적 객체 시작시 미리 생성
    void generateEnemy()
    {
        zombies = new GameObject[zombiesSize];
        bosses = new GameObject[bossesSize];
        //두 종류인 좀비 모델을 랜덤으로 하나의 모델 생성 (모델 이외 동일)
        //또한 보스급 적 생성 보스 개수까지
        for (int idx = 0; idx < bossesSize; idx++)
        {
            int ranZombieType = Random.Range(0, 2);
            if (ranZombieType == 0)
            {
                zombies[idx] = Instantiate(zombie1, zombieParent);
                zombies[idx].SetActive(false);
            }
            else
            {
                zombies[idx] = Instantiate(zombie2, zombieParent);
                zombies[idx].SetActive(false);
            }
            //보스 생성
            bosses[idx] = Instantiate(boss, bossParent);
            bosses[idx].SetActive(false);
        }
        //두 종류인 좀비 모델을 랜덤으로 하나의 모델 생성 (모델 이외 동일)
        //이어서 좀비 개수까지 좀비 생성
        for (int idx = bossesSize; idx < zombiesSize; idx++)
        {
            int ranZombieType = Random.Range(0, 2);
            if (ranZombieType == 0)
            {
                zombies[idx] = Instantiate(zombie1, zombieParent);
                zombies[idx].SetActive(false);
            }
            else
            {
                zombies[idx] = Instantiate(zombie2, zombieParent);
                zombies[idx].SetActive(false);
            }
        }
    }
}
