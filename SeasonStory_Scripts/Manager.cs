using UnityEngine;
public class Manager : MonoBehaviour
{
    public static Manager instance;

    public int selectedStage = 0; // 0 default, 1 stage1, 2 stage2, 3 stage3, 4 stage4
    public float volumeSize = 0.5f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}