using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource[] Sounds = new AudioSource[9];

    void Start()
    {
        Sounds[0] = GameObject.Find("PistolFireSound").GetComponent<AudioSource>();
        Sounds[1] = GameObject.Find("GunTurretFireSound").GetComponent<AudioSource>();
        Sounds[2] = GameObject.Find("ReloadSound").GetComponent<AudioSource>();
        Sounds[3] = GameObject.Find("ScrollSound").GetComponent<AudioSource>();
        Sounds[4] = GameObject.Find("ButtonSound").GetComponent<AudioSource>();
        Sounds[5] = GameObject.Find("ExplosionSound").GetComponent<AudioSource>();
        Sounds[6] = GameObject.Find("BuildingSound").GetComponent<AudioSource>();
        Sounds[7] = GameObject.Find("ZombieSound").GetComponent<AudioSource>();
        Sounds[8] = GameObject.Find("NightBGMSound").GetComponent<AudioSource>();
        setSoundVolume();
    }
    //사운드들의 볼륨을 설정에서 조정한 볼륨 크기로 조정해줌
    public void setSoundVolume()
    {
        for (int idx = 0; idx < Sounds.Length; idx++)
        {
            Sounds[idx].volume = GameManager.instance.volume_val;
        }
    }
}
