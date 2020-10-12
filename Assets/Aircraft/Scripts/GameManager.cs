﻿using System.Collections;
using System.Collections.Generic;
//using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aircraft
{
    public enum GameState
    {
        Default,
        MainMenu,
        Preparing,
        Playing,
        Paused,
        Gameover
    }

    public enum GameDifficulty
    {
        Normal,
        Hard
    }

    public delegate void OnStateChangeHandler();

    public class GameManager : MonoBehaviour
    {
        public event OnStateChangeHandler OnStateChange;

        private GameState gameState;

        public GameState GameState
        {
            get
            {
                return gameState;
            }

            set
            {
                gameState = value;
                if (OnStateChange != null) OnStateChange();
            }

        }

        public GameDifficulty GameDifficulty { get; set; }

        public static GameManager Instance
        {
            get; private set;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);

            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void OnApplicationQuit()
        {
            Instance = null;
        }

        public void LoadLevel(string levelName, GameState newState)
        {
            StartCoroutine(LoadLevelAsync(levelName, newState));

        }

        private IEnumerator LoadLevelAsync(string levelName, GameState newState)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);
            while(operation.isDone == false)
            {
                yield return null;
            }

            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);

            GameState = newState;

        }
        

    }
}
