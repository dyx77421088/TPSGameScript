using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public Transform gunAttackPos;
    public Transform gunIdlePos;
    public Transform gunPos;
    

    public void switchPos(GunPosition pos)
    {
        switch(pos)
        {
            case GunPosition.AttackPos:
                gunPos.parent = gunAttackPos;
                gunPos.DOLocalMove(Vector3.zero, 0.5f);
                //gunPos.localPosition = Vector3.zero;
                gunPos.DOLocalRotateQuaternion(Quaternion.identity, 0.5f);
                //gunPos.localRotation = Quaternion.identity;
                break;
            case GunPosition.IdlePos:
                gunPos.parent = gunIdlePos;
                gunPos.DOLocalMove(Vector3.zero, 0.5f);
                gunPos.DOLocalRotateQuaternion(Quaternion.identity, 0.5f);
                break;  
        }
    }



    public enum GunPosition
    {
        AttackPos,
        IdlePos,
    }
}
