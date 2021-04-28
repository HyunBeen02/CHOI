using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    IFire currentWeapon;

    void Start()
    {
        currentWeapon = new Pistol();
    }
    public void setWeapon(IFire iweapon)
    {
        if(currentWeapon == iweapon)
        {
            Debug.Log("Load failed, already same weapon");
            return;
        }
        currentWeapon = iweapon;
    }
    public void shoot(GameObject[] bullet, Transform startPos)
    {
        currentWeapon.fire(bullet, startPos);
    }
}
public interface IFire
{
    void fire(GameObject[] bullet, Transform startPos);
}
public class Pistol : MonoBehaviour, IFire
{
    public void fire(GameObject[] bullet, Transform startPos)
    {
        //총알 켠 후 위치 잡아주기
        for (int i = 0; i < bullet.Length; i++)
        {
            if(bullet[i].activeSelf == false)
            {
                bullet[i].SetActive(true);
                bullet[i].transform.position = startPos.position;
                bullet[i].transform.rotation = Quaternion.Euler(startPos.rotation.eulerAngles.x, 
                    startPos.rotation.eulerAngles.y - 90, startPos.rotation.eulerAngles.z);
                return;
            }
        }
    }
}
public class RifleGun : MonoBehaviour, IFire
{
    public void fire(GameObject[] bullet, Transform startPos)
    {
        //총알 켠 후 위치 잡아주기
        for (int i = 0; i < bullet.Length; i++)
        {
            if (bullet[i].activeSelf == false)
            {
                bullet[i].SetActive(true);
                bullet[i].transform.position = startPos.position;
                bullet[i].transform.rotation = Quaternion.Euler(startPos.rotation.eulerAngles.x,
                    startPos.rotation.eulerAngles.y - 90, startPos.rotation.eulerAngles.z);
                return;
            }
        }
    }
}
public class ShotGun : MonoBehaviour, IFire
{
    int count = 0;
    public void fire(GameObject[] bullet, Transform startPos)
    {
        //총알 켠 후 위치 잡아주기
        for (int i = 0; i < bullet.Length; i++)
        {
            int ranShotYRot = Random.Range(-6, 6);
            int ranShotZRot = Random.Range(-5, 5);
            if (bullet[i].activeSelf == false)
            {
                bullet[i].SetActive(true);
                bullet[i].transform.position = startPos.position;
                bullet[i].transform.rotation = Quaternion.Euler(startPos.rotation.eulerAngles.x,
                    startPos.rotation.eulerAngles.y + (ranShotYRot - 90), startPos.rotation.eulerAngles.z + ranShotZRot);
                count++;
                if (count > 4)
                {
                    count = 0;
                    return;
                }
                
            }
        }
    }
}

public class WeaponData
{
    public const float PISTOL_FIRE_RATE = 0.16f;
    public const float RIFLE_FIRE_RATE = 0.08f;
    public const float SHOTGUN_FIRE_RATE = 0.6f;

    public const float PISTOL_RELOAD_SPEED = 0.5f;
    public const float RIFLE_RELOAD_SPEED = 0.6f;
    public const float SHOTGUN_RELOAD_SPEED = 0.5f;

    public const int PISTOL_MAX_AMMO = 10;
    public const int RIFLE_MAX_AMMO = 30;
    public const int SHOTGUN_MAX_AMMO = 2;
}