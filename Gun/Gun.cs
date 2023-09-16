using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletHole; // ����
    [SerializeField] private GameObject spark; // ����Ч
    [SerializeField] private ParticleSystem shootParticle; // �ӵ����������Ч��
    [SerializeField] private Light shootLight; // �ƹ�
    [SerializeField] private GameObject[] Prefabs;
    [SerializeField] private int prefab = 1;
    [SerializeField] private Transform firePosition;

    private AudioSource gunAudio; // �������ŵ�����
    public AudioClip shootClip; // fire����
    public AudioClip buttleLeftClip; // �����е�����������û��
    public AudioClip buttleOutClip; // �����е����������п���

    public int remainBullet = 120; // ���õĵ�����
    public int oneClipBullet = 30; // һ���ӵ����ĵ�����
    public float maxRange = 100; // ������
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
                //PlayParticle(); // ����
                //ShootEffect();  // �ӵ�ը����Ч
                Instantiate(Prefabs[prefab], firePosition.position, firePosition.rotation);
                break;
            case GunAudio.ButtleOut:
                Debug.Log("��������");
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
    /// ������������Ч�����׺ͱ�ը�ģ�
    /// </summary>
    public void ShootEffect()
    {
        // ����һ������
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
