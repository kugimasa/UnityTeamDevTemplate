﻿using System.Diagnostics;
using UnityEngine;
using SilCilSystem.Variables;
using SilCilSystem.Variables.Base;
using SilCilSystem.Editors;

namespace SilCilSystem.Internals.Variables
{
    [Variable("Variable")]
    internal class NotificationQuaternion : VariableQuaternion
    {
        [SerializeField] private Quaternion m_value = default;
        [SerializeField, NonEditable] private GameEventQuaternion m_onValueChanged = default;

        public override Quaternion Value
        {
            get => m_value;
            set
            {
                m_value = value;
                m_onValueChanged?.Publish(m_value);
            }
        }

        [OnAttached, Conditional("UNITY_EDITOR")]
        private void OnAttached(VariableAsset parent)
        {
            m_onValueChanged = parent.GetSubVariable<GameEventQuaternion>();
        }
    }
}