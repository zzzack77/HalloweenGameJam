using System;
using Unity.VisualScripting;
using UnityEngine;

public enum Ability
{
    None,
    Mine,
    FireWheel,
    Placeholder2
}
public class AbilityManager : MonoBehaviour
{
    public Ability ability;

    public static event Action OnMineAbility;
    public static event Action OnFireWheelAbility;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && ability != Ability.None)
        {
            switch (ability)
            {
                case Ability.Mine:
                    OnMineAbility?.Invoke();
                    break;
                case Ability.FireWheel:
                    OnFireWheelAbility?.Invoke();
                    break;
            }
        }
    }

}
