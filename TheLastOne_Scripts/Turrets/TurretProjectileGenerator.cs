using UnityEngine;

public class TurretProjectileGenerator : MonoBehaviour
{
    [SerializeField] GameObject turret_bullet; //포탑 총알 프리팹
    [SerializeField] GameObject turret_missile; //미사일 프리팹
    [SerializeField] GameObject missile_effect; //미사일 이펙트(파티클) 프리팹

    [SerializeField] Transform turret_bullet_parent; //포탑 총알 프리팹을 담을 부모 객체
    [SerializeField] Transform turret_missile_parent; //미사일 프리팹을 담을 부모 객체
    [SerializeField] Transform missile_effect_parent; //미사일 이펙트(파티클) 프리팹을 담을 부모 객체
    int bulletSize = 500;
    public GameObject[] TBullets;
    int missileSize = 200;
    public GameObject[] TMissiles;
    public GameObject[] TMissileEffects;

    void Awake()
    {
        generateProjectile();
    }
    //오브젝트 풀링을 위한 총알과 미사일을 미리 생성
    void generateProjectile()
    {
        TBullets = new GameObject[bulletSize];
        TMissiles = new GameObject[missileSize];
        TMissileEffects = new GameObject[missileSize];
        //포탑 총알과 포탑 미사일을 미사일 사이즈까지 생성
        for (int idx = 0; idx < missileSize; idx++)
        {
            TBullets[idx] = Instantiate(turret_bullet, turret_bullet_parent);
            TMissiles[idx] = Instantiate(turret_missile, turret_missile_parent);
            TMissileEffects[idx] = Instantiate(missile_effect, missile_effect_parent);
            TBullets[idx].SetActive(false);
            TMissiles[idx].SetActive(false);
            TMissileEffects[idx].SetActive(false);
        }
        //포탑 총알을 미사일 사이즈부터 총알 사이즈까지 생성
        for (int idx = missileSize; idx < bulletSize; idx++)
        {
            TBullets[idx] = Instantiate(turret_bullet, turret_bullet_parent.transform);
            TBullets[idx].SetActive(false);
        }
    }
}
