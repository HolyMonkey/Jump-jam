using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam.Extensions
{
    public static class MathExtensions
    {
        public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
        }
    }
}
