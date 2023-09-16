using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletHole; // 弹孔
    [SerializeField] private GameObject spark; // 火花特效
    [SerializeField] private ParticleSystem shootParticle; // 子弹发射的粒子效果
    [SerializeField] private Light shootLight; // 灯光
    [SerializeField] private GameObject[] Prefabs;
    [SerializeField] private int prefab = 1;
    [SerializeField] private Transform firePosition;

    private AudioSource gunAudio; // 武器播放的音乐
    public AudioClip shootClip; // fire声音
    public AudioClip buttleLeftClip; // 换弹夹的声音，弹夹没空
    public AudioClip buttleOutClip; // 换弹夹的声音，弹夹空了

    public int remainBullet = 120; // 备用的弹夹数
    public int oneClipBullet = 30; // 一轮子弹最大的弹夹数
    public float maxRange = 100; // 最大射程
    private void Start()
    {
        gunAudio = GetComponent<AudioSource>();

    }

    public void PlayGunAudio(GunAudio audio = GunAudio.Fire)
    {
        switch (audio)
        {
            case GunAudio.Fire:
                gunAudio.volume = 0.2f;
                gunAudio.clip = shootClip;
                //PlayParticle(); // 弹坑
                //ShootEffect();  // 子弹炸开特效
                Instantiate(Prefabs[prefab], firePosition.position, firePosition.rotation);
                break;
            case GunAudio.ButtleOut:
                Debug.Log("声音来了");
                gunAudio.volume = 1f;
                gunAudio.clip = buttleOutClip;
                gunAudio.Play();
                break;
            case GunAudio.ButtleLeft:
                gunAudio.volume = 1f;
                gunAudio.clip = buttleLeftClip;
                break;
            default:
                break;
        }
        gunAudio.Play();
    }

    /// <summary>
    /// 播放射击后的特效（弹孔和爆炸的）
    /// </summary>
    public void ShootEffect()
    {
        // 发射一条射线
        Ray ray = new Ray(firePosition.position, firePosition.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxRange))
        {
            GameObject go1 = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            GameObject go2 = Instantiate(spark, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

            Destroy(go1, 3);
            Destroy(go2, 3);
        }
    }

    public void PlayParticle()
    {
        shootParticle.Play();
    }

    public void PlayLight(bool enable = true)
    {
        shootLight.enabled = enable;
    }

    public enum GunAudio
    {
        Fire,
        ButtleOut,
        ButtleLeft
    }
}
