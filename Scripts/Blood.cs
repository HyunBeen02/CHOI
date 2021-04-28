using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("setOffActive", 4f);
    }
    void setOffActive()
    {
        gameObject.SetActive(false);
    }
}
