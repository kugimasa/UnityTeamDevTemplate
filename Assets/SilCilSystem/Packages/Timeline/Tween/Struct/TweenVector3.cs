﻿using SilCilSystem.Variables;
using SilCilSystem.Variables.Generic;
using UnityEngine;

namespace SilCilSystem.Timeline
{
    public class TweenVector3 : TweenVariableAsset<Vector3, TweenVector3Behaviour>
    {
        private enum LerpType
        {
            Lerp,
            SLerp,
        }

        [SerializeField] private LerpType m_lerpType = default;
        [SerializeField] private VariableVector3 m_variable = default;

        protected override Variable<Vector3> GetVariable()
        {
            return m_variable;
        }

        protected override Vector3 Lerp(Vector3 start, Vector3 end, float t)
        {
            switch (m_lerpType)
            {
                default:
                case LerpType.Lerp:
                    return Vector3.LerpUnclamped(start, end, t);
                case LerpType.SLerp:
                    return Vector3.SlerpUnclamped(start, end, t);
            }
        }
    }

    public class TweenVector3Behaviour : TweenVariableBehaviour<Vector3> { }
}