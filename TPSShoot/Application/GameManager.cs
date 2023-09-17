using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot.Manger
{
    public class GameManager : MonoBehaviour
    {
        public bool IsGamePause {  get; private set; }
        
        private static GameManager instance;
        public static GameManager Instance { get => instance; }

        public void Awake()
        {
            instance = this;
            IsGamePause = false;

            Events.GamePauseRequest += OnPauseGameRequest;
            Events.GameResumeRequest += OnResumeGameRequest;
        }
        private void OnDestroy()
        {
            Events.GamePauseRequest -= OnPauseGameRequest;
            Events.GameResumeRequest -= OnResumeGameRequest;
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
            Time.timeScale = 0;

            IsGamePause = true;
            Events.GamePause.Call();
        }
        private void ResumeGame()
        {
            Time.timeScale = 1;

            IsGamePause = false;
            Events.GameResume.Call();
        }
    }
}
