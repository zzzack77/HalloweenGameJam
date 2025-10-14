using System.Collections;
using UnityEngine;

public class CooldownHelper : MonoBehaviour
{
    // Cooldown Coroutine that takes in a bool, sets it to false then true again after cooldown 
    public static IEnumerator CooldownRoutine(System.Action<bool> setFlag, float cooldownTime)
    {
        setFlag(false);
        yield return new WaitForSeconds(cooldownTime);
        setFlag(true);
    }
}
