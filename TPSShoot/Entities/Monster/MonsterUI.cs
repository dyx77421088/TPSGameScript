using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShoot
{
    public class MonsterUI : MonoBehaviour
    {
        public Transform HP;
        public Image HPImage;
        public Text HPText;

        private MonsterBehaviour mb;
        private Transform pb;
        void Start()
        {
            mb = GetComponent<MonsterBehaviour>();
            pb = PlayerBehaviour.Instance.transform;

            mb.onMonsterHPChange += OnChangeHP;
            mb.onMonsterDied += OnDied;
        }

        private void OnDestroy()
        {
            mb.onMonsterHPChange -= OnChangeHP;
            mb.onMonsterDied -= OnDied;
        }

        void Update()
        {
            UpdateRotate();
            OnChangeHP();
        }
        /// <summary>
        /// 只改变y轴的旋转
        /// </summary>
        private void UpdateRotate()
        {
            Vector3 direction = pb.position - HP.position;
            direction.y = 0f; // 将 y 分量设为 0

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            HP.rotation = targetRotation;
        }
        private void OnDied()
        {
            HP.gameObject.SetActive(false);
        }

        private void OnChangeHP()
        {
            HPImage.fillAmount = mb.GetHPPercentage();
            HPText.text = mb.GetCurrentHP() + "/" + mb.GetMaxHP();
        }
    }
}
