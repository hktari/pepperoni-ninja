using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhytmCanvas : MonoBehaviour
{
    public float PanelColorResetTime = 0.25f;

    private float m_colorResetTimer;

    private GameObject backgroundImg;
    private GameObject panelImg;
    private RhytmManager.RhytmAction m_curRhytmState;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        backgroundImg = transform.GetChild(0).gameObject;
        panelImg = transform.GetChild(1).gameObject;
        image = panelImg.GetComponent<Image>();
    }

    void Update()
    {
        m_colorResetTimer -= Time.deltaTime;
        if (m_colorResetTimer <= 0.0f)
        {
            m_curRhytmState = RhytmManager.RhytmAction.None;
        }

        Color panelColor = Color.white;
        switch (m_curRhytmState)
        {
            case RhytmManager.RhytmAction.None:
                panelColor = Color.white;
                break;
            case RhytmManager.RhytmAction.Failed:
                panelColor = Color.red;
                break;
            case RhytmManager.RhytmAction.Success:
                panelColor = Color.green;
                break;
            default:
                break;
        }

        image.color = panelColor;
    }

    public void UpdateCanvas(float perc)
    {
        Rect backgroundRect = backgroundImg.GetComponent<RectTransform>().rect;
        var newX = backgroundImg.transform.position.x + backgroundRect.width * perc;
        var panelRect = panelImg.GetComponent<RectTransform>().rect;
        panelImg.transform.position = new Vector3(newX, panelImg.transform.position.y, panelImg.transform.position.z);
    }

    public void DisplayAction(RhytmManager.RhytmAction rhytmAction)
    {
        m_colorResetTimer = PanelColorResetTime;
        m_curRhytmState = rhytmAction;
    }
}
