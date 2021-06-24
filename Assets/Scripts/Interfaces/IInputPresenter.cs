using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    public interface IInputPresenter
    {
        Vector2 GetCurrentInput();
    }
}
