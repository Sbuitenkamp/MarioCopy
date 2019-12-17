using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Item
{
    int SizeIndex { get; set; }
    int ScoreWorth { get; set; }

    void OnReveal();
    void OnActivate();
}
