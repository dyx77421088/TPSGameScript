using DG.Tweening;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ����������࣬�����б����򿪶�����״̬
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
