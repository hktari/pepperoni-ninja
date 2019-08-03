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

    [SerializeField]
    public float RhytmTimeMax = 0.5f;

    [SerializeField]
    public float RhytmActionLeewayInSec = 0.1f;

    public float CurRhytmTime { get; set; }

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
            CurRhytmTime = Mathf.Clamp(CurRhytmTime, 0, RhytmTimeMax);
        }

        Canvas?.UpdateCanvas(CurRhytmTime / RhytmTimeMax);
    }

    public bool TryPerformAction()
    {
        var success = Mathf.Abs(CurRhytmTime - RhytmTimeMax / 2.0f) < RhytmActionLeewayInSec;
        Debug.Log("SuCCESS: " + success);
        Canvas.DisplayAction(success ? RhytmAction.Success : RhytmAction.Failed);
        return success;
    }
}
