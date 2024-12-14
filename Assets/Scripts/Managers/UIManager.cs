using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yunash.UI
{
    public interface IUIService
    {
        void OpenCanvas<ICanvasType>() where ICanvasType : class, ICanvas;
        void CloseCanvas<ICanvasType>() where ICanvasType : class, ICanvas;
    }
    public class UIManager : MonoBehaviour, IUIService
    {
        [SerializeField] List<CanvasBase> canvasBases;

        private List<ICanvas> canvases = new List<ICanvas>();
        private List<ICanvas> openCanvasStack = new List<ICanvas>();

        #region API
        void IUIService.CloseCanvas<ICanvasType>()
        {
            var canvas = canvases.Find(canvas => canvas is ICanvasType);

            if (openCanvasStack.Contains(canvas))
                openCanvasStack.Remove(canvas);

            if (canvas != null)
                canvas.Close();
            else
                throw new Exception($"UIManager: {typeof(ICanvasType)} is missing from UI Canvas registry.");
        }

        void IUIService.OpenCanvas<ICanvasType>()
        {
            var canvas = canvases.Find(canvas => canvas is ICanvasType);

            if (openCanvasStack.Contains(canvas))
                openCanvasStack.Remove(canvas);
            else
                openCanvasStack.Add(canvas);


            if (canvas != null)
                canvas.Open();
            else
                throw new Exception($"UIManager: {typeof(ICanvasType)} is missing from UI Canvas registry.");
        }
        #endregion

    }

}
