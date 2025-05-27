using UnityEngine;
using UnityEngine.UI;
using TMPro;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/RCC Detachable Part")]
public class Damage : MonoBehaviour
{
    public RCC_DetachablePart bumperFPart; // Assign your RCC_DetachablePart for bumper_F in the Unity Inspector.
    public RCC_DetachablePart bumperRPart; // Assign your RCC_DetachablePart for bumper_R in the Unity Inspector.
    public RCC_DetachablePart hood,trunk,doorR,doorL;
    
    public TMP_Text strengthText; // Assign your Text GameObject in the Unity Inspector.


    public int myDamage = 0;
    public myDamageScore damageScore;
    public SaturationChanger sc;
    public GameObject crashUI;

    public int demageImpactScore = 10;
    private int prevDamage;
    void Start()
    {
        prevDamage = -1;
        damageScore = new myDamageScore();
    }

    void Update()
    {
        // Update other code


        // Get the damage from bumper_F and bumper_R
        float bumperFStrength = bumperFPart.strength;
        float bumperRStrength = bumperRPart.strength;
        float hoodStrength = hood.strength;
        float trunkStrength = trunk.strength;
        float doorRStrength = doorR.strength;
        float doorLStrength = doorL.strength;

        // Calculate the total damage
        float totalDamage = bumperFStrength + bumperRStrength + hoodStrength + trunkStrength + doorRStrength + doorLStrength;

        // Calculate myDamage
        myDamage = (int)(totalDamage / 6);

        // Set the text in the Text UI as an integer
        strengthText.text = myDamage.ToString(); // No format needed for integer.
        if (prevDamage == -1)
        {
            prevDamage = myDamage;
        }
        else if (myDamage!=prevDamage)
        {
            sc.ChangeSaturation();
            crashUI.SetActive(true);
            Invoke("CrashUIActive", 3f);
            prevDamage = myDamage;

        }
    }

    void CrashUIActive()
    {
        crashUI.SetActive(false);
      
    }

    [System.Serializable]
    public class myDamageScore
    {
        public int score = 0;
    }
}
