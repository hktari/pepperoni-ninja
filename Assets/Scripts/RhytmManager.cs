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

    public float SpeedUpgradeFactor = 0.75f;

    public float CurRhytmTime { get; set; }

    private bool leftToRight = true;

    private float m_origRhytmTime;
    // Start is called before the first frame update
    void Start()
    {
        m_origRhytmTime = RhytmTimeMax;
    }

    float timeOfLastAction;
    public float ComboBreaksInSec = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timeOfLastAction > ComboBreaksInSec)
        {
            RhytmTimeMax = m_origRhytmTime;
            Canvas.ResetPanelSize();
        }

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
        //var success = Mathf.Abs(CurRhytmTime - RhytmTimeMax / 2.0f) < RhytmActionLeewayInSec;
        //Debug.Log("SuCCESS: " + success);
        var success = Canvas.TryExecuteAction();
        if (success)
        {
            RhytmTimeMax *= SpeedUpgradeFactor;
            timeOfLastAction = Time.time;
        }
        else
        {
            RhytmTimeMax = m_origRhytmTime; 
        }
        Canvas.DisplayAction(success ? RhytmAction.Success : RhytmAction.Failed);
        return success;
    }
}
