using UnityEngine;

public class ExplosionParticle : MonoBehaviour
{
    ParticleSystem particleSystem;
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }
    private void OnDisable()
    {
        particleSystem.Stop();
    }
    private void OnEnable()
    {
        particleSystem.Play();
        Invoke("ActiveFalse", 2f);
    }
    void ActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
