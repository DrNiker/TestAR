using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SizeWindow : MonoBehaviour
{
    [SerializeField]
    GameObject box;
    public int id;
    [SerializeField]
    TMP_InputField h;
    [SerializeField]
    TMP_InputField w;
    [SerializeField]
    TMP_InputField d;
    // Start is called before the first frame update
    public async void OnButtonClick()
    {
        Texture text = await FindObjectOfType<Data>().GetTexture(id);
        float height = float.Parse(h.text) / 10;
        float width = float.Parse(w.text) / 10;
        float depth = float.Parse(d.text) / 10;
        box = Instantiate(box);
        box.transform.localScale = new Vector3(width, height, depth);
        box.GetComponent<Renderer>().material.SetTexture("_MainTex", text);
        FindObjectOfType<Placement>().GenerateBox(box);
        FindObjectOfType<Canvas>().enabled = false;
    }
}
