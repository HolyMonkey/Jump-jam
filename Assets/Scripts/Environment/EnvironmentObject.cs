using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class EnvironmentObject : MonoBehaviour
    {
        [SerializeField] private int _level;
        [SerializeField] private int _reward;

        public int Level => _level;
        public int Reward => _reward;
    }
}
