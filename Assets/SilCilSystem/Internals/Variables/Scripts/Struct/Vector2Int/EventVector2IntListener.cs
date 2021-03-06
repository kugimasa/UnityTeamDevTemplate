﻿using System;
using System.Diagnostics;
using UnityEngine;
using SilCilSystem.Variables;
using SilCilSystem.Variables.Base;
using SilCilSystem.Editors;

namespace SilCilSystem.Internals.Variables
{
    [Variable("Listener", Constants.ListenerMenuPath + "(Vector2Int)", typeof(GameEventVector2Int))]
    internal class EventVector2IntListener : GameEventVector2IntListener
    {
        [SerializeField, NonEditable] private GameEventVector2Int m_event = default;

        public override IDisposable Subscribe(Action<Vector2Int> action) => m_event?.Subscribe(action);

        [OnAttached, Conditional("UNITY_EDITOR")]
        private void OnAttached(VariableAsset parent)
        {
            m_event = parent.GetSubVariable<GameEventVector2Int>();
        }
    }
}