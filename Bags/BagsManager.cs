using DG.Tweening;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 背包管理的类，包括有背包打开动画及状态
/// </summary>
public class BagsManager : MyMonoInstance<BagsManager>
{
    
    
    private bool showBag = false;

    

    private void Start()
    {
        
        

    }
    private void Update()
    {
        
    }
    

    public bool IsOpenBag()
    {
        return showBag;
    }

    
}
