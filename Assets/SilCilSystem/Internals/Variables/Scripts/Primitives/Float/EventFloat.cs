﻿using System;
using System.Collections.Generic;
using SilCilSystem.Variables;
using SilCilSystem.Variables.Base;

namespace SilCilSystem.Internals
{
    internal class EventFloat : GameEventFloat
    {
        private event Action<float> m_event = default;

        public override void Publish(float value)
        {
            m_event?.Invoke(value);
        }

        public override IDisposable Subscribe(Action<float> action)
        {
            m_event += action;
            return DelegateDispose.Create(() => m_event -= action);
        }

        public override void GetAssetName(ref string name) => name = $"{name}_OnChanged";
        public override void OnAttached(IEnumerable<VariableAsset> variables) { }
    }
}