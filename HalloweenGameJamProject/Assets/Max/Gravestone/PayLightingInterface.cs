using UnityEngine;

public interface IPayLighting
{
    
    [SerializeField] float lightingCost { get; }
    bool bActive{get;set;}
    
    public bool CanActivate(PlayerStats playerStats);
    
    void Activate();
}
