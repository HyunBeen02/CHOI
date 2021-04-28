using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    ParticleSystem currentEffect; //현재 폭발 이펙트
    TrailRenderer trail;
    public int damage;
    public float speed;
    LayerMask targetLayer;
    GameObject currentTarget; //현재 타겟 오브젝트
    float ExplosionRange = 1.5f; //폭발 공격 반경

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        targetLayer = 1 << LayerMask.NameToLayer("Enemy");
    }
    void Update()
    {
        rotateToTarget(currentTarget.transform);
        move();
        explodeAttack();
    }
    //받은 타겟이 위치한 방향으로 회전 선형보간으로 자연스럽게 구현
    void rotateToTarget(Transform target)
    {
        if (currentTarget == null || !currentTarget.activeSelf)
            return;

        Vector3 dirVec = target.position - transform.position;
        Quaternion driQua = Quaternion.LookRotation(dirVec);
        driQua = Quaternion.Euler(driQua.eulerAngles.x, driQua.eulerAngles.y, driQua.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, driQua, 0.1f);
    }
    //전방으로 이동
    void move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    //현재 타겟을 업데이트해줌
    public void setTarget(GameObject target_)
    {
        currentTarget = target_;
    }
    //현재 폭발 이펙트를 업데이트해줌
    public void setEffect(ParticleSystem effect_)
    {
        currentEffect = effect_;
    }
    //폭발 범위 안에 있는 적을 찾아 적의 피격 함수를 실행시켜 공격함
    public void explodeAttack()
    {
        if(transform.position.y <= 0.15f)
        {
            Collider[] targetCols = Physics.OverlapSphere(transform.position, ExplosionRange, targetLayer);
            foreach(Collider col in targetCols)
            {
                col.GetComponent<Zombie>().hit(damage);
            }
            startEffect();
            GameObject.Find("ExplosionSound").GetComponent<AudioSource>().Play();
            gameObject.SetActive(false);
            trail.Clear(); //트레일 초기화
        }
    }
    //폭발 파티클 미사일 위치로 변경 후 활성화 및 실행
    void startEffect()
    {
        currentEffect.gameObject.transform.position = transform.position;
        currentEffect.gameObject.SetActive(true);
        currentEffect.Play();
    }
    //오브젝트 비활성화시 타겟, 위치 초기화
    private void OnDisable()
    {
        currentTarget = null;
        transform.position = new Vector3(0, 100, 0);
    }
    //디버깅
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ExplosionRange);
    }
    */
}
