using UnityEngine;

public class MapScroll : MonoBehaviour
{   //this is used at GameScene
    float speed = 3.2f;
    float endX = -14f;

    Vector2 startPos = new Vector2(28.5f, 0);

    void Update()
    {
        if (transform.position.x >= endX)
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        else
            transform.position = startPos;
    }
}
