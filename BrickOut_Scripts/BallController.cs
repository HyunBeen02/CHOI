using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float startSpeed;
    Rigidbody2D rigid2D;
    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        rigid2D.AddForce(Vector2.up * startSpeed);
    }
    void Update()
    {
        if (GameManager.isEnd)
        {
            rigid2D.velocity = Vector2.zero;
            rigid2D.AddForce(Vector2.zero);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            collision.gameObject.SetActive(false);

            GameManager.brickCount--;

            if (GameManager.brickCount <= 0)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().SetEndGame();
            }

            GameObject.Find("brick").GetComponent<AudioSource>().Play();
        }
        else if (collision.gameObject.CompareTag("Bottom"))
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().SetEndGame();
            GameObject.Find("pad").GetComponent<AudioSource>().Play();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            GameObject.Find("pad").GetComponent<AudioSource>().Play();
        }
    }
}
