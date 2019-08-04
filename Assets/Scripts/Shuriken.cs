using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public float bulletSpeed = 0.5f;
    public GameObject Splash;
    public GameObject Splash2;
    public GameObject Splash3;
    private GameObject Player;
    private float direction;
    private Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        direction = Mathf.Sign(Player.transform.localScale.x);
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        rigid.MovePosition((Vector2)transform.position+new Vector2(direction*bulletSpeed, 0));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy killed");
            Destroy(collision.gameObject);
            Instantiate(Splash);
            Destroy(this.gameObject);
        }

    }
}
