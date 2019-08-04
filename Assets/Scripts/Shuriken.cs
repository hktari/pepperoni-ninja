using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public float bulletSpeed = 0.5f;
    public GameObject Splash;
    public GameObject Player;
    private float direction;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        direction = Mathf.Sign(Player.transform.localScale.x);
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(direction*bulletSpeed, 0));
    }
}
