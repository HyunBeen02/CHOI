using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour
{
    public float speed;
    [Range(-1, 1)]public int dir;
    void Start()
    {
        
    }
    void Update()
    {
        if (transform.position.x <= -6f)
            dir = 1;
        else if (transform.position.x >= 6f)
            dir = -1;

        transform.Translate(new Vector3(speed * dir * Time.deltaTime, 0, 0));
    }
}
