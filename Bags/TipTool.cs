using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipTool : MonoBehaviour
{
    
    private Text text1;
    private Text text2;
    private CanvasGroup canvasGroup;

    void Start()
    {
        text1 = GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
        text2 = transform.GetChild(1).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(string text)
    {
        text1.text = text;
        text2.text = text;
        canvasGroup.alpha = 1.0f;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0.0f;
    }

    public void SetPosition(Vector3 v3)
    {
        transform.position = v3;
    }
}
