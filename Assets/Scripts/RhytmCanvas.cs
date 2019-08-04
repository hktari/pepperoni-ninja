using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class RhytmCanvas : MonoBehaviour
{
    public float PanelColorResetTime = 0.25f;

    private float m_colorResetTimer;

    [SerializeField] private GameObject background;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject runningPanel;
    private RhytmManager.RhytmAction m_curRhytmState;
    private Image panelImage;
    private Image runningPanelImage;
    private Image backgroundImage;
    public Sprite greenBackground;
    public Sprite redBackground;
    public Sprite defaultBackground;

    // Start is called before the first frame update
    void Start()
    {
        panelImage = panel.GetComponent<Image>();
        runningPanelImage = runningPanel.GetComponent<Image>();
        backgroundImage = background.GetComponent<Image>();
    }

    void Update()
    {
        m_colorResetTimer -= Time.deltaTime;
        if (m_colorResetTimer <= 0.0f)
        {
            m_curRhytmState = RhytmManager.RhytmAction.None;
        }

        switch (m_curRhytmState)
        {
            case RhytmManager.RhytmAction.None:
                backgroundImage.sprite = defaultBackground;
                break;
            case RhytmManager.RhytmAction.Failed:
                backgroundImage.sprite = redBackground;
                break;
            case RhytmManager.RhytmAction.Success:
                backgroundImage.sprite = greenBackground;
                break;
            default:
                break;
        }
    }

    public bool TryExecuteAction()
    {
        var success = runningPanel.GetComponent<TriggerHandler>().FirstCollider != null;
        var panelRect = panel.GetComponent<RectTransform>();
        if (success)
        {
            var trns = panelRect.parent.transform;
            trns.localScale = new Vector2(
                Mathf.Max(0.10f, trns.localScale.x * 0.75f),
                1.0f);
        }
        else
        {
            panelRect.transform.parent.transform.localScale = Vector2.one;
        }

        return success;
    }

    public void UpdateCanvas(float perc)
    {
        Rect backgroundRect = background.GetComponent<RectTransform>().rect;
        var newX = background.transform.position.x + backgroundRect.width * perc;
        var panelRect = runningPanel.GetComponent<RectTransform>().rect;
        runningPanel.transform.position = new Vector3(newX, runningPanel.transform.position.y, runningPanel.transform.position.z);
    }

    public void DisplayAction(RhytmManager.RhytmAction rhytmAction)
    {
        m_colorResetTimer = PanelColorResetTime;
        m_curRhytmState = rhytmAction;
    }
}
