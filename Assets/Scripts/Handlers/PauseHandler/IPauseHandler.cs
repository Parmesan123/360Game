using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPauseHandler
{
    public bool IsPaused { get; }

    public void Pause(bool value);
}
