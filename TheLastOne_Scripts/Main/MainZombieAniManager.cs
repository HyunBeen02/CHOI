using UnityEngine;

public class MainZombieAniManager : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        setAni();
    }
    void setAni()
    {
        int ran = Random.Range(0, 2);
        float ranTime = Random.Range(0.5f, 3f);
        if (ran == 0)
        {
            animator.SetTrigger("idle");
        }
        else
        {
            animator.SetTrigger("walk");
        }
        Invoke("setAni", ranTime);
    }
}
