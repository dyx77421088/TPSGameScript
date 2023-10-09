using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TPSShoot.Bags;
using UnityEngine;

namespace TPSShoot.Manger
{
    public class GameManager : MonoBehaviour
    {
        [Tooltip("���ֻ����뻹�ǵ��Զ˵�")]
        public bool isMobileInput;
        public bool IsGamePause {  get; private set; }
        private static GameManager instance;
        public static GameManager Instance { get => instance; }

        public void Awake()
        {
            instance = this;
            IsGamePause = false;

            Events.GamePauseRequest += OnPauseGameRequest;
            Events.GameResumeRequest += OnResumeGameRequest;
            Events.PlayerLoaded += initStory;
        }
        private void OnDestroy()
        {
            Events.GamePauseRequest -= OnPauseGameRequest;
            Events.GameResumeRequest -= OnResumeGameRequest;
            Events.PlayerLoaded -= initStory;
        }
        private bool temp;
        private void Start()
        {
            temp = !isMobileInput;
        }
        private void Update()
        {
            if (temp != isMobileInput)
            {
                temp = isMobileInput;
                // �л�Ϊ�ֻ�����ģʽ���������ģʽ
                if (isMobileInput) Events.MobileInputMode.Call();
                else Events.DesktopInputMode.Call();
            }
        }
        //public bool IsCursorVisible()
        //{
        //    return Cursor.visible;
        //}
        private void OnPauseGameRequest()
        {
            if (!IsGamePause) PauseGame();
        }
        private void OnResumeGameRequest()
        {
            if (IsGamePause) ResumeGame();
        }

        private void PauseGame()
        {
            //Time.timeScale = 0;

            IsGamePause = true;
            Events.GamePause.Call();
        }
        private void ResumeGame()
        {
            //Time.timeScale = 1;

            IsGamePause = false;
            Events.GameResume.Call();
        }

        #region ��ʼ��װ������
        /// <summary>
        /// ��ʼ������
        /// </summary>
        private void initStory()
        {
            List<Item> items = new();
            JObject jo = JObject.Parse(Resources.Load<TextAsset>("Items").text);
            JArray ja = JArray.Parse(jo["data"].ToString());
            foreach (JObject item in ja)
            {
                //Debug.Log(item["type"].ToString());
                Item.ItemType it = Enum.Parse<Item.ItemType>(item["type"].ToString());
                switch (it)
                {
                    case Item.ItemType.Material:
                        Bags.Material mt = item.ToObject<Bags.Material>();
                        items.Add(mt);
                        break;
                    case Item.ItemType.Consumable:
                        Consumable c = item.ToObject<Consumable>();
                        items.Add(c);
                        break;
                    case Item.ItemType.Equipment:
                        Equipment eq = item.ToObject<Equipment>();
                        items.Add(eq);
                        break;
                    case Item.ItemType.Weapon:
                        Weapon weapon = item.ToObject<Weapon>();
                        items.Add(weapon);
                        break;
                    case Item.ItemType.Bullet:
                        Bullet bullet = item.ToObject<Bullet>();
                        items.Add(bullet);
                        break;
                }
            }
            // ��ʼ���ɹ�
            Events.ItemsJsonLoaded.Call(items);
        }
        #endregion

    }
}
