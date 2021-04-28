using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Entity
{
    public int maxHP; //초기 최대 체력
    Vector3 originPos; //초기 위치
    protected override void Start()
    {
        setHealthPoint(maxHP);
        originPos = transform.position;
    }
    void Update()
    {
        if(GameManager.instance.isDay)
            setHealthPoint(maxHP);
    }
    protected override void dieEntity()
    {
        if(currentHealhPoint <= 0)
        {
            StartCoroutine(dieEffect());
            //Invoke("DestroyDelay", 1f);
        }
    }
    //피격 기능
    public void hit(int damage)
    {
        StartCoroutine(hitEffect());
        decreaseHealthPoint(damage);
        dieEntity();//검사부터
    }
    //랜덤값으로 위치 조정하여 오래 흔들리며 죽는 이펙트 코루틴
    IEnumerator dieEffect()
    {
        for (int i = 0; i < 100; i++)
        {
            float ranX = Random.Range(-0.01f, 0.01f);
            float ranZ = Random.Range(-0.01f, 0.01f);
            transform.position = new Vector3(transform.position.x + ranX, 0, transform.position.z + ranZ);
            yield return new WaitForSeconds(0.01f); //just delay
        }
        //끝나면 파괴
        Destroy(gameObject);
    }
    //피격시 흔들림 위와 동일
    IEnumerator hitEffect()
    {
        for (int i = 0; i < 50; i++)
        {
            float ranX = Random.Range(-0.007f, 0.007f);
            float ranZ = Random.Range(-0.007f, 0.007f);
            transform.position = new Vector3(transform.position.x + ranX, 0, transform.position.z + ranZ);
            yield return new WaitForSeconds(0.01f); //just delay
        }
        transform.position = originPos;
    }
}
