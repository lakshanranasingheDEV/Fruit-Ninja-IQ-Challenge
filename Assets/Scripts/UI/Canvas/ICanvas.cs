using Yunash.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yunash.UI
{
    public interface ICanvas
    {
        public void Open();
        public void Close();
    }

    public abstract class CanvasBase : MonoBehaviour, ICanvas
    {
        public IUIService uiService;
        private bool isInitialized;

        private void Start()
        {
            if (!isInitialized)
            {
                Initialize();
            }

            OnStartAsync();
        }
        public virtual void Initialize()
        {
            uiService = GameManager.Instance.UIService;
            isInitialized = true;
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        protected virtual async void OnStartAsync()
        {

        }
    }
}

