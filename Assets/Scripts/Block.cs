using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Block
{
    bool Active { get; set; }

    void OnActivate(Collision2D col);
}
