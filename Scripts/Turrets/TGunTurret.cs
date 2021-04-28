using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGunTurret : Turret, ITurretFunction
{
    protected override void Start()
    {
        base.Start();
        StartCoroutine(attack());
    }
    //현재 타겟에 비활성화 되어있는 총알을 찾아 위치와 회전값을 조정하고 활성화해줌
    public void attackToTarget()
    {
        if (currentTarget == null || !currentTarget.activeSelf || GameManager.instance.isDay)
            return;

        //idx번째 TBullets이 사용가능한 상태인지 체크
        for (int idx = 0; idx < turretProjectileGenerator.TBullets.Length; idx++)
        {
            if (!turretProjectileGenerator.TBullets[idx].activeSelf)
            {
                GameObject.Find("GunTurretFireSound").GetComponent<AudioSource>().Play();
                turretProjectileGenerator.TBullets[idx].transform.position = transform.position + new Vector3(0, 1.37f, 0);
                turretProjectileGenerator.TBullets[idx].transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 90, transform.rotation.eulerAngles.z);
                turretProjectileGenerator.TBullets[idx].SetActive(true);
                return;
            }
            
        }
    }
    //attackToTarget 함수를 fireRate만큼 딜레이를 주어 무한반복하게함
    IEnumerator attack()
    {
        while (true)
        {
            attackToTarget();
            yield return new WaitForSeconds(fireRate);
        }
    }
}
