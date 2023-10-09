using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProgressUI : MonoBehaviour
{
    public Slider slider;
    public Text progressText;
    public GameObject loading;

    private static ProgressUI instance;
    private ProgressUI() { }
    public static ProgressUI Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        // loading ����ѭ�����ŷŴ�
        loading.transform.DOScale(Vector3.one * 1.1f, 1).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// ���õ�ǰ�Ľ���
    /// </summary>
    public void SetProgress(float progress)
    {
        slider.value = progress;
    }

    public void SetProgressText(string downloadSpeed, string size)
    {
        progressText.text = downloadSpeed + "/S " + size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
