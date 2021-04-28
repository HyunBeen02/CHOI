using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMissileTurret : Turret, ITurretFunction
{
    protected override void Start()
    {
        base.Start();
        StartCoroutine(attack());
    }
    //현재 타겟에 비활성화 되어있는 미사일과 폭발 이펙트를 찾아 위치와 회전값을 조정하고 활성화해줌
    public void attackToTarget()
    {
        if (currentTarget == null || !currentTarget.activeSelf || GameManager.instance.isDay)
            return;
        for (int idx = 0; idx < turretProjectileGenerator.TMissiles.Length; idx++)
        {
            //idx번째 TMissiles이 사용가능한 상태인지 체크
            if (!turretProjectileGenerator.TMissiles[idx].activeSelf)
            {
                //공격
                turretProjectileGenerator.TMissiles[idx].GetComponent<Missile>().setTarget(currentTarget);
                ParticleSystem currentEffect = turretProjectileGenerator.TMissileEffects[idx].GetComponent<ParticleSystem>();
                turretProjectileGenerator.TMissiles[idx].GetComponent<Missile>().setEffect(currentEffect);

                turretProjectileGenerator.TMissiles[idx].transform.position = transform.position + new Vector3(0, 1.39f, 0);
                turretProjectileGenerator.TMissiles[idx].transform.rotation = Quaternion.Euler(-40, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                turretProjectileGenerator.TMissiles[idx].SetActive(true);
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