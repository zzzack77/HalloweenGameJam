using UnityEngine;

public interface IPayLighting
{
    
    [SerializeField] float lightingCost { get; }
    bool bActive{get;set;}
    
    public bool CanActivate(ref float playerLight);
    
    void Activate();
}
