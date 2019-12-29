using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Enemy
{
    bool Alive { get; set; }

    void Walk();
    void OnJump(Collision2D col);
}
