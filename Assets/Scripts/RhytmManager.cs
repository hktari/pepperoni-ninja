using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhytmManager : MonoBehaviour
{
    public enum RhytmAction
    {
        None,
        Failed,
        Success
    }

    public RhytmCanvas Canvas;

    public float RhytmTimeMax { get; set; } = 1.0f;

    public float CurRhytmTime { get; set; }
    public float RhytmActionLeewayInSec { get; set; } = 0.1f;

    private bool leftToRight = true;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (leftToRight)
        {
            CurRhytmTime += Time.deltaTime;
        }
        else
        {
            CurRhytmTime -= Time.deltaTime;
        }

        if (CurRhytmTime > RhytmTimeMax || CurRhytmTime < 0.0f)
        {
            leftToRight = !leftToRight;
        }

        Canvas?.UpdateCanvas(CurRhytmTime / RhytmTimeMax);
    }

    public bool CanPerformAction()
    {
        var success = Mathf.Abs(CurRhytmTime - 0.5f) < RhytmActionLeewayInSec;
        Canvas.DisplayAction(success ? RhytmAction.Success : RhytmAction.Failed);
        return success;
    }
}
