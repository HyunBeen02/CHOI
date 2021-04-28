using UnityEngine;

public class BarController : MonoBehaviour
{
    float[] ranX = { 200, 150, 100 };
    float[] ranY = { 400, 500, 600 };

    void Start()
    {
        
    }
    void Update()
    {
        MoveBar();
    }
    void MoveBar()
    {
        transform.position = new Vector2(Mathf.Clamp(GetMousePos().x, -7.6f, 7.6f), -4f);
    }
    Vector2 GetMousePos()
    {
        Vector2 pos = Input.mousePosition;
        pos = Camera.main.ScreenToWorldPoint(pos);
        return pos;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Vector2 reflectBall = collision.transform.position - transform.position;
            collision.rigidbody.velocity = Vector2.zero;
            int ran = Random.Range(0, 3);
            if (reflectBall.x > 0)
            {
                collision.rigidbody.AddForce(new Vector2(ranX[ran] * 1f, ranY[ran]));
            }
            else if (reflectBall.x < 0)
            {
                collision.rigidbody.AddForce(new Vector2(ranX[ran] * -1f, ranY[ran]));
            }
            GameObject.Find("pad").GetComponent<AudioSource>().Play();
        }
    }
}
