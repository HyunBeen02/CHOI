using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damageCount; //공격력
    public float speed; //총알 속도

    float delta = 0; //초세기
    float spanTime = 0.5f; //초간격

    void Start()
    {
        delta = 0;
        spanTime = 0.5f;
    }
    void Update()
    {
        //총알 이동 (총알 회전값에 따라 right를 곱해줌)
        transform.Translate(Vector3.right * Time.deltaTime * speed);
        //초세기값이 간격타임을 넘으면 비활성화
        delta += Time.deltaTime;
        if(spanTime < delta)
        {
            gameObject.SetActive(false);
        }
    }
    //비활성화시 트레일과 위치 초기화
    void OnDisable()
    {
        delta = 0; //초세기값 초기화
        GetComponent<TrailRenderer>().Clear();
        transform.position = new Vector3(0, 100, 0);
    }
    //적과 충돌시 비활성화 후  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie") || other.CompareTag("Boss"))
        {
            //적 피격 실행
            other.GetComponent<Zombie>().hit(damageCount);
            gameObject.SetActive(false);
        }
    }
}
