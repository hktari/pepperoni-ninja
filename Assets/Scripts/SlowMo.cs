using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMo : MonoBehaviour
{
    bool init = false;
    static int falling = 0;
    public int numOfCollisionsOnPlayer = 2;

    private void Start()
    {
        init = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (init)
        {
            // when player hits the trigger, slow down time, then resume it after hitting a different trigger.
            if (falling == 0)
            {
                Debug.Log("Time slowed");
                falling++;
                Time.timeScale = 0.3f;
            }
            else if (falling >= numOfCollisionsOnPlayer) 
            {
                Debug.Log("Time resumed");
                Time.timeScale = 1f;
            }
            else
            {
                falling++;

            }
        }
    }

    public static void Pause()
    {
        Time.timeScale = 0f;
    }
    public static void Unpause()
    {
        Time.timeScale = 1f;
    }
}
