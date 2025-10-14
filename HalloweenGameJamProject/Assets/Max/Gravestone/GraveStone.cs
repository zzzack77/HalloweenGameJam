using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Collider2D))]
public class GraveStone : MonoBehaviour , IPayLighting
{
    [SerializeField] private Sprite usedGravestone;
    [SerializeField] private Canvas promptCanvas;
    public float lightingCost { get; } = 10;
    public bool bActive{get;set;}
    public bool PlayerInRange { get; private set; }
    public bool canInteract = true;

    
   
    public bool CanActivate(ref float playerLight)
    {
        if (playerLight >= lightingCost && bActive == false)
        {
            playerLight -= lightingCost;;
            bActive = true;
            canInteract = false;
            promptCanvas.gameObject.SetActive(false);
            Activate();
            return true;
        }
        return false;
    }

    public void Activate()
    {
       Invoke(nameof(DelayedReward) , 2f);
    }
    private void DelayedReward()
    {
        
        FinishActivation();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || !canInteract) return;
        PlayerInRange = true;
        BAPlayer player = other.GetComponent<BAPlayer>();
        if (player)
        {
            player.SetInteractable(this);
            if (promptCanvas)
            {
                promptCanvas.gameObject.SetActive(true);
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerInRange = false;
        BAPlayer player = other.GetComponent<BAPlayer>();
        if (player)
        {
            player.SetInteractable(null); 
            if(promptCanvas)
            {
                promptCanvas.gameObject.SetActive(false);
            }
        }
        
    }

    private void FinishActivation()
    {
        canInteract = false;
        if (usedGravestone)
        {
            GetComponent<SpriteRenderer>().sprite = usedGravestone;
        }
        
    }
}
