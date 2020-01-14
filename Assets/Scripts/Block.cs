using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Block
{
    bool Active { get; set; }

    void OnActivate(Collider2D col);
}
