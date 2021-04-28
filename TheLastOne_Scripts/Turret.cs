using UnityEngine;

public class Turret : MonoBehaviour
{
    public float fireRate;
    protected GameObject currentTarget;
    protected TurretProjectileGenerator turretProjectileGenerator;
    LayerMask targetLayer;
    float getTargetDelay = 1;
    protected virtual void Start()
    {
        turretProjectileGenerator = GameObject.Find("TurretProjectileGenerator").GetComponent<TurretProjectileGenerator>();
        targetLayer = 1 << LayerMask.NameToLayer("Enemy");
        getTarget();
    }
    void Update()
    {
        rotateToTarget();
    }
    //타겟을 향해 회전함
    public void rotateToTarget()
    {
        if (currentTarget == null || currentTarget.layer != LayerMask.NameToLayer("Enemy"))
        {
            currentTarget = null;
            return;
        }
        Vector3 dir = currentTarget.transform.position - transform.position;
        Quaternion targeRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, targeRot, 0.18f);
    }
    //가장 가까운 타겟의 오브젝트 정보를 현재 타겟에 넣어줌
    public void getTarget()
    {
        Collider[] targetCols = Physics.OverlapSphere(transform.position, 15, targetLayer);
        float dis = 999;
        foreach (Collider col in targetCols)
        {
            float targetDis = Vector3.Distance(transform.position, col.transform.position);
            if (dis > targetDis)
            {
                dis = targetDis;
                currentTarget = col.gameObject;
            }
        }
        //딜레이를 주어 반복 실행하여 낭비를 줄임
        Invoke("getTarget", getTargetDelay);
    }
}
//터렛마다 꼭 구현해야하는 함수 정의
public interface ITurretFunction
{
    void attackToTarget();
}