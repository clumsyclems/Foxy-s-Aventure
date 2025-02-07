using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : Abstract_Canvas<CanvasScript>
{
    [SerializeField] private Sprite heartSprite = null;
    [SerializeField] private List<GameObject> heartList = null;

    [SerializeField] private int maxHeart = 10;
    [SerializeField] private int maxHeartInLine = 10;

    public Vector2 customSizeDelta = Vector2.zero;

    private void Start()
    {
        AddHeart(Inventory.Instance.Heart);
    }

    public void RemoveHeart(int numberOfHeart)
    {
        for (int heartIndex = 0; (heartIndex < numberOfHeart) && (heartList.Count > 0); ++heartIndex)
        {
            Destroy(heartList[heartList.Count - 1]);
            heartList.RemoveAt(heartList.Count - 1);
        }
        
    }

    public void AddHeart(int numberOfHeart)
    { 
        float lastHeartPositionX = (heartList.LastOrDefault() != null) ? heartList.Last().GetComponent<RectTransform>().anchoredPosition.x : 0f;
        for (int heartIndex = 0; (heartIndex < numberOfHeart) && (heartList.Count < maxHeart); ++heartIndex)
        {
            /* Create a new game object for the life */
            GameObject newHeart = new("Heart_" + (heartList.Count + 1));

            /* Instanciate the Image to represent the life */
            Image newImage = newHeart.AddComponent<Image>();

            /* Copy the image given */
            newImage.sprite = heartSprite;
            newImage.preserveAspect = true;

            /* Add the new object inside the Canvas */
            newHeart.transform.SetParent(gameObject.transform, false);

            /* To place the new image */
            RectTransform newRect = newHeart.GetComponent<RectTransform>();

            /* to place the new image */
            newRect.sizeDelta = customSizeDelta;
            newRect.anchorMax = new Vector2(0f, 1f);
            newRect.anchorMin = new Vector2(0f, 1f);
            newRect.anchoredPosition = new Vector2(newRect.sizeDelta.x * (heartList.Count % maxHeartInLine + 1), newRect.sizeDelta.y * -((int)(heartList.Count/maxHeartInLine) + 1));

            heartList.Add(newHeart);
        }
    }
}
