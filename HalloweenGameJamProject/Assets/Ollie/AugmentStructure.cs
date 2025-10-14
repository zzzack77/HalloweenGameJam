using UnityEngine;

public class AugmentStructure
{
    public bool freeze;
    public bool burn;
    public bool explode;

    public AugmentStructure()
    {
        freeze = false;
        burn = false;
    }
    public void setAug(int index, bool x)
    {
        if (index == 0) { freeze = x; }
        else if (index == 1) { burn = x; }
        else if(index == 2) { explode = x; }
    }
}
