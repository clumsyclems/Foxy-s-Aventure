using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /* Gem Text to modify */
    public TextMeshProUGUI numberOfGemText = null;

    private int gems = 0;
    private int heart = 3;

    public int Gems { get => gems; set => gems = value; }
    public int Heart { get => heart; set => heart = value; }

    public static Inventory instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("They have more than inventory inside the project");
            return;
        }
        instance = this;
    }

    public void UpdateGems(int nbGems)
    { 
        gems += nbGems;

        numberOfGemText.text = gems.ToString();

    }

    public void UpdateHeart(int nbHeart) 
    {
        heart += nbHeart;
        if (nbHeart > 0)
        {
            CanvasScript.instance.AddHeart(nbHeart);
        }
        else
        {
            CanvasScript.instance.RemoveHeart(-nbHeart);
        }

    }
}
