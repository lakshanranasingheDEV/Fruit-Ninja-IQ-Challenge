using Yunash.Audio;
using Yunash.Data;
using Yunash.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yunash.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private DataManager dataManager;

        private static GameManager instance;
        public static GameManager Instance => instance;
        public IUIService UIService;
        public IAudioService AudioService;
        public IDataService DataService;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(instance);
                instance = this;
            }
            else
                instance = this;

            if (uiManager == null || audioManager == null || dataManager == null)
            {
                throw new NullReferenceException("GameManager: Initialization: One or More Managers are missing.");
            }

            UIService = uiManager;
            AudioService = audioManager;
            DataService = dataManager;
        }
    }
}

