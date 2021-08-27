using System;
using System.Collections.Generic;
using UnityEngine;

namespace Build1.UnityUI.Utils
{
    public static class ScreenUtil
    {
        private static ScreenResolutionChangedListener _listener;
        private static List<Action>                    _listeners;
        
        /*
         * Screen Values.
         */
        
        public static float GetAspectRatio()
        {
            return Mathf.Max((float)Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height); 
        }
        
        public static float GetDiagonalInches()
        {
            var screenWidth = Screen.width / Screen.dpi;
            var screenHeight = Screen.height / Screen.dpi;
            return Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
        }
        
        /*
         * Resolution Changed.
         */
        
        public static void SubscribeOnScreenResolutionChanged(Action onChanged)
        {
            if (_listeners != null && _listeners.Contains(onChanged))
                return;
            
            if (_listener == null && _listeners == null)
            {
                _listener = UnityUI.GetRoot().AddComponent<ScreenResolutionChangedListener>();
                _listener.OnChanged += OnScreenResolutionChanged;
            }
            
            _listeners ??= new List<Action>();
            _listeners.Add(onChanged);
        }
        
        public static void UnsubscribeFromScreenResolutionChanged(Action onChanged)
        {
            if (_listeners == null || !_listeners.Remove(onChanged))
                return;
            
            if (_listeners.Count > 0)
                return;
            
            UnityEngine.Object.Destroy(_listener);
            
            _listener.OnChanged -= OnScreenResolutionChanged;
            _listener = null;

            _listeners = null;
        }

        private static void OnScreenResolutionChanged()
        {
            foreach (var listener in _listeners)
                listener.Invoke();
        }
    }
}