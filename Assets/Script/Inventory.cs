using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Burst.Intrinsics.X86.Avx;

public class Inventory : Singleton<Inventory>
{
    /* Gem Text to modify */
    [SerializeField] private TextMeshProUGUI numberOfGemText = null;

    [SerializeField] private int gems = 0;
    [SerializeField] private int heart = 3;

    public int Gems { get => gems; set => gems = value; }
    public int Heart { get => heart; set => heart = value; }

    protected override void Awake()
    {
        base.Awake();
        GameObject gemTextTMP = GameObject.Find("Gem Text (TMP)");

        if (gemTextTMP == null)
        {
            Debug.LogError("EventManager inexistant inside the scene !");
        }

        numberOfGemText = gemTextTMP.GetComponent<TextMeshProUGUI>();

        if (numberOfGemText == null)
        {
            Debug.LogError("EventManager inexistant inside the scene !");
        }
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
            CanvasScript.Instance.AddHeart(nbHeart);
        }
        else
        {
            CanvasScript.Instance.RemoveHeart(-nbHeart);
        }

    }
}
