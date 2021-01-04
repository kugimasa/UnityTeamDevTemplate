﻿using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace SilCilSystem.Editors.Views
{
    public static class UICreator
    {
        [MenuItem(Constants.CreateGameObjectMenuPath + nameof(Canvas), false, Constants.CreateGameObjectMenuOrder)]
        public static Canvas CreateCanvas()
        {
            var instance = Instantiate<Canvas>(Constants.CanvasTemplateID);
            CreateEventSystem(false);
            return instance;
        }

        [MenuItem(Constants.CreateGameObjectMenuPath + nameof(EventSystem), false, Constants.CreateGameObjectMenuOrder)]
        public static EventSystem CreateEventSystem()
        {
            if (GameObject.FindObjectOfType<EventSystem>() != null) return null;
            return Instantiate<EventSystem>(Constants.EventSystemTemplateID, true);
        }
        
        [MenuItem(Constants.CreateGameObjectMenuPath + nameof(Text), false, Constants.CreateGameObjectMenuOrder)]
        public static Text CreateText()
        {
            return InstantiateElement<Text>(Constants.TextTemplateID);
        }

        [MenuItem(Constants.CreateGameObjectMenuPath + "Text - Text Mesh Pro", false, Constants.CreateGameObjectMenuOrder)]
        public static TextMeshProUGUI CreateTextTMP()
        {
            return InstantiateElement<TextMeshProUGUI>(Constants.TextTMPTemplateID);
        }

        [MenuItem(Constants.CreateGameObjectMenuPath + nameof(Slider), false, Constants.CreateGameObjectMenuOrder)]
        public static Slider CreateSlider()
        {
            return InstantiateElement<Slider>(Constants.SliderTemplateID);
        }

        [MenuItem(Constants.CreateGameObjectMenuPath + nameof(Toggle), false, Constants.CreateGameObjectMenuOrder)]
        public static Toggle CreateToggle()
        {
            return InstantiateElement<Toggle>(Constants.ToggleTemplateID);
        }

        private static T InstantiateElement<T>(string guid, bool selection = true) where T : Component
        {
            var canvas = GetCanvas();
            var instance = Instantiate<T>(guid, selection);
            instance.transform.SetParent(canvas.transform, false);
            return instance;
        }
        
        private static T Instantiate<T>(string guid, bool selection = true) where T : Object
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var prefab = AssetDatabase.LoadAssetAtPath<T>(path);
            var instance = Object.Instantiate(prefab);
            instance.name = prefab.name;
            if (selection) Selection.activeObject = instance;
            return instance;
        }

        private static EventSystem CreateEventSystem(bool selection)
        {
            if (GameObject.FindObjectOfType<EventSystem>() != null) return null;
            return Instantiate<EventSystem>(Constants.EventSystemTemplateID, selection);
        }

        private static Canvas GetCanvas()
        {
            Canvas canvas = null;
            foreach(var gameObj in Selection.gameObjects)
            {
                canvas = gameObj.GetComponent<Canvas>();
                if (canvas != null) return canvas;
            }

            canvas = GameObject.FindObjectOfType<Canvas>();
            if (canvas != null) return canvas;

            canvas = CreateCanvas();
            return canvas;
        }
    }
}