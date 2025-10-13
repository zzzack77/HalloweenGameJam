using System;
using Unity.VisualScripting;
using UnityEngine;

public enum Ability
{
    None,
    Mine,
    Placeholder,
    Placeholder2
}
public class AbilityManager : MonoBehaviour
{
    public Ability ability;

    public static event Action OnMineAbility;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && ability != Ability.None)
        {
            switch (ability)
            {
                case Ability.Mine:
                    OnMineAbility?.Invoke();
                    break;
            }
        }
    }

}
