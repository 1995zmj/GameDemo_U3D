using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordMerchant : MonoBehaviour
{
    [SerializeField]
    private Text swordName;

    [SerializeField]
    private Text description;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text goldCost;

    [SerializeField]
    private Text AttackDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDisplayUI(SwordData swordData)
    {
        swordName.text = swordData.SwordName;
        Debug.Log("update display ui");
    }
}
