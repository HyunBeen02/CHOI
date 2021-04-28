using UnityEngine;

public class Obstacle : MonoBehaviour
{   //this is used at GameScene
    public string objectName; //리소스 이미지명과 같음
    public Sprite redOutline; 

    float speed = 3f;
    float endX = -8f;

    Vector3 startPos = new Vector3(5f, 0, 0);

    void Start()
    {
        transform.position = startPos + transform.position;
    }
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x <= endX)
            Destroy(gameObject);
    }
    public void setColor()
    {
        GetComponent<SpriteRenderer>().sprite = redOutline;
    }
}
