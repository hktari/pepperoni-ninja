using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhytmCanvas : MonoBehaviour
{
    public float PanelColorResetTime = 0.25f;

    private float m_colorResetTimer;

    [SerializeField] private GameObject background;
    [SerializeField] private GameObject panel;
    private RhytmManager.RhytmAction m_curRhytmState;
    private Image panelImage;
    private Image backgroundImage;
    public Sprite greenbackground;
    public Sprite redbackground;

    // Start is called before the first frame update
    void Start()
    {
        panelImage = panel.GetComponent<Image>();
        backgroundImage = panel.GetComponent<Image>();
    }

    void Update()
    {
        m_colorResetTimer -= Time.deltaTime;
        if (m_colorResetTimer <= 0.0f)
        {
            m_curRhytmState = RhytmManager.RhytmAction.None;
        }

        Color panelColor = Color.white;
        Color backgroundColor = Color.white;

        switch (m_curRhytmState)
        {
            case RhytmManager.RhytmAction.None:
                panelColor = Color.white;
                backgroundColor = Color.white;
                break;
            case RhytmManager.RhytmAction.Failed:
                panelColor = backgroundColor = Color.red;
                break;
            case RhytmManager.RhytmAction.Success:
                panelColor = backgroundColor = Color.green;
                break;
            default:
                break;
        }

        panelImage.color = panelColor;
        backgroundImage.color = backgroundColor;
    }

    public void UpdateCanvas(float perc)
    {
        Rect backgroundRect = background.GetComponent<RectTransform>().rect;
        var newX = background.transform.position.x + backgroundRect.width * perc;
        var panelRect = panel.GetComponent<RectTransform>().rect;
        panel.transform.position = new Vector3(newX, panel.transform.position.y, panel.transform.position.z);
    }

    public void DisplayAction(RhytmManager.RhytmAction rhytmAction)
    {
        m_colorResetTimer = PanelColorResetTime;
        m_curRhytmState = rhytmAction;
    }
}
