using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public class Player : MonoBehaviour, IInputPresenter
    {
        [SerializeField] private Joystick _joystick = null;

        public Vector2 GetCurrentInput()
        {
            return new Vector2(_joystick.Horizontal, _joystick.Vertical);
        }
    }
}