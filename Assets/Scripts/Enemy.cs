using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Enemy
{
    bool Alive { get; set; }
    int ScoreWorth { get; set; }

    void OnJump(Collision2D col);
    void OnFireBall();
}
