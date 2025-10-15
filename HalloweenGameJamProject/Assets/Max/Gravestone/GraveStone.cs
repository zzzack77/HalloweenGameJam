using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
public class GraveStone : MonoBehaviour , IPayLighting
{
    [SerializeField] private Sprite usedGravestone;
    [SerializeField] private Canvas promptCanvas;
    [SerializeField] private Canvas rewardCanvas;
    [SerializeField] private Image rewardImage;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private Sprite[] items;
    
    
    [SerializeField] protected AudioSource audioSource;      // reference to AudioSource
    [SerializeField] protected AudioClip equipClip;          // sound for equipping
    [SerializeField] protected AudioClip randomizeClip; 
    
    
    
    [SerializeField] private float RandomizerTime= 0.2f;
    public float lightingCost { get; } = 10;
    public bool bActive{get;set;}
    public bool PlayerInRange { get; private set; }
    public bool canInteract = true;
    private bool canInteractEquip = false;
    private int SelectedEquipment; // change variable type to whatever's appropriate
    private Coroutine RandomizeImageHandle;
   
    public bool CanActivate(PlayerStats playerStats)
    {
        if (playerStats.LightHP >= playerStats.GraveStoneCost && bActive == false)
        {
            playerStats.LightHP -= playerStats.GraveStoneCost;
            bActive = true;
            canInteract = false;
            promptCanvas.gameObject.SetActive(false);
            Activate();
            return true;
        }
        
        if (canInteractEquip && bActive)
        {
           
            //------- equip logic here -------
            
            //player.new ability(selectedEquipment) example
            
            
            // sounds
            if (audioSource && equipClip)
            {
                audioSource.PlayOneShot(equipClip);
            }
            
            rewardCanvas.gameObject.SetActive(false);
            canInteractEquip = false;
            return true;
        }
        return false;
    }

    public void Activate()
    {
        if (rewardImage != null)
        {
            rewardCanvas.gameObject.SetActive(true);
            RandomizeImageHandle = StartCoroutine(RandomizeImage());
        }

        if (audioSource && randomizeClip)
        {
            audioSource.PlayOneShot(randomizeClip);
        }

        GameManager.Instance.IncreaseScore(10); //don't know if this is the amount we want to increase score by? -maxb
        Invoke(nameof(DelayedReward) , 2f);
    }

    private void DelayedReward()
    {
        canInteractEquip = true;
        StopCoroutine(RandomizeImageHandle);
        
        int i = Random.Range(0, items.Length);
        switch (i)
        {
            case 1 :
                rewardText.text = "Press Space to Equip Augment...";
                
                break;
            case 2 :
                rewardText.text = "Press Space to Equip Weapon...";
                break;
            case 3 :
                rewardText.text = "Press Space to Equip ...";
                break;
            case 4 :
                rewardText.text = "Press Space to Equip ...";
                break;
            case 5 :
                rewardText.text = "Press Space to Equip ...";
                break;
            case 6 :
                rewardText.text = "Press Space to Equip ...";
                break;
            default:
                break;
        }
        rewardImage.sprite = items[i];
        SelectedEquipment = i;
        if(rewardText != null)
        {
            rewardText.gameObject.SetActive(true);
        }
        

    FinishActivation();
    }

    IEnumerator RandomizeImage()
    {
        
        while (true && rewardImage)
        {
            int i =  Random.Range(0, items.Length);
            rewardImage.sprite = items[i];
            yield return new WaitForSeconds(RandomizerTime);
        }
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || !canInteract && !canInteractEquip) return;
        PlayerInRange = true;
        BAPlayer player = other.GetComponent<BAPlayer>();
        if (player)
        {
            player.SetInteractable(this);
            if (promptCanvas && canInteract)
            {
                promptCanvas.gameObject.SetActive(true);
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || !canInteract && !canInteractEquip) return;
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
        
        if (usedGravestone)
        {
            GetComponent<SpriteRenderer>().sprite = usedGravestone;
        }
        
    }
}
