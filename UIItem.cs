using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI title;
    [SerializeField]
    Image image;
    [SerializeField]
    GameObject menu;
    RectTransform rt;
    int id;
    public void Construct(string text, Texture2D _texture, int _id)
    {
        rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, Screen.width / 2);
        title.text = text;
        Vector2 pivot = new Vector2(0, 0);
        Rect rec = new Rect(0,0, _texture.width, _texture.height);
        image.sprite = Sprite.Create(_texture, rec, pivot, 1);
        id = _id;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, Screen.width / 2);
    }
    public void OnButtonClicked()
    {
        menu = Instantiate(menu, FindObjectOfType<Canvas>().transform);
        menu.GetComponent<SizeWindow>().id = id;
    }
    
}
