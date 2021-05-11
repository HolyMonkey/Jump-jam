using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MousePositionConverter
{
    protected Camera _camera;

    public MousePositionConverter(Camera camera)
    {
        _camera = camera;
    }

    public abstract Vector3 GetCursorPosition(Vector3 mousePosition, float distance);
}
