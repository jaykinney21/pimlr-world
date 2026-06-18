using UnityEngine;
using UnityEngine.UI;

public class UIHorizontalScrollView : MonoBehaviour
{
    public GameObject itemPrefab;
    public int itemCount = 10;
    public float itemWidth = 100f;
    public float highlightScale = 1.2f;
    public Color highlightColor = Color.yellow;

    public Transform content;
    private RectTransform highlightTransform;
    private int currentIndex = 0;

    void Start()
    {
        /*content = transform.Find("Viewport/Content");

        // Instantiate items
        for (int i = 0; i < itemCount; i++)
        {
            GameObject item = Instantiate(itemPrefab, content);
            RectTransform itemRect = item.GetComponent<RectTransform>();
            itemRect.anchoredPosition = new Vector2(i * itemWidth, 0);
            item.GetComponent<Button>().onClick.AddListener(() => HighlightItem(itemRect));
        }

        highlightTransform = content.GetChild(0).GetComponent<RectTransform>();
        // Set initial highlight
        HighlightItem(highlightTransform);*/

        //StartCoroutine(ScrollViewFocusFunctions.FocusOnItemCoroutine(this.gameObject,));
    }


    //public void FocusOnItemCoroutine

    void HighlightItem(RectTransform itemRect)
    {
        // Reset previous highlight
        highlightTransform.localScale = Vector3.one;
        highlightTransform.GetComponent<Image>().color = Color.white;

        // Highlight the selected item
        highlightTransform = itemRect;
        highlightTransform.localScale = Vector3.one * highlightScale;
        highlightTransform.GetComponent<Image>().color = highlightColor;

        // Scroll to the selected item
        //currentIndex = content.GetChildIndex(itemRect);
        float targetPosition = -currentIndex * itemWidth;
        //content.anchoredPosition = new Vector2(targetPosition, 0);
    }
}
 