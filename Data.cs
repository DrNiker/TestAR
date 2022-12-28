using SimpleJSON;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using System.Net;
using System.Threading.Tasks;
public class Data : MonoBehaviour
{
    List<Item> items;

    [SerializeField]
    GameObject UIItemsParentLeft;
    [SerializeField]
    GameObject UIItemsParentRight;
    [SerializeField]
    GameObject UIItem;

    async void Start()
    {
        items = new List<Item>();
        var res = await ObtainSheetData();
        foreach (Item i in items)
        {
            res = await  AsyncGetImages(i);
        }
        SpawnItems();
    }

    public async Task<Texture> GetTexture(int id)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(items[id].textureURL);
        www.SendWebRequest();
        while (!www.isDone)
        {
            await Task.Yield();
        }
        if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error:" + www.error);
            return null;
        }
        else
        {
            items[id].mainTexture = DownloadHandlerTexture.GetContent(www);
            return items[id].mainTexture;
        }
    }
    public void SpawnItems()
    {
        int counter = 0;
        foreach(Item i in items)
        {
            GameObject obj = Instantiate(UIItem);
            Debug.Log(obj);
            if (counter % 2 == 0)
            {
                obj.transform.parent = UIItemsParentLeft.transform;
            } else
            {
                obj.transform.parent = UIItemsParentRight.transform;
            }
            if(i.title != null && i.previewTexture != null)
                obj.GetComponent<UIItem>().Construct(i.title, i.previewTexture, counter);
            counter++;
        }
    }

    private async Task<string> ObtainSheetData()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://sheets.googleapis.com/v4/spreadsheets/1Wj75QfY2F8PkNCTMYvOsL-FxYia2mdGvQITVti1xHMk/values/Sheet1?key=AIzaSyAfDwTdW42n8bg6IAOwzWMzNydubMkbDCk");
        www.SendWebRequest();
        while (!www.isDone)
        {
            await Task.Yield();
        }
        if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error:" + www.error);
            return www.error;
        }
        else
        {
            string json = www.downloadHandler.text;
            var o = JSON.Parse(json);
            int count = 0;
            foreach (var i in o["values"])
            {
                if (count == 0)
                {
                    count++;
                    continue;
                }
                var item = JSON.Parse(i.ToString());
                string title = item[0][0];
                string preview = item[0][1];
                string texture = item[0][2];
                float size = item[0][3];
                items.Add(new Item(title, preview, texture, size));
                count++;
            }
            return "Done";
        }
    }

    private async Task<string> AsyncGetImages(Item item)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(item.previewURL);
        www.SendWebRequest();
        while (!www.isDone)
        {
            await Task.Yield();
        }
        if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error:" + www.error);
            return www.error;
        }
        else
        {
            item.previewTexture = DownloadHandlerTexture.GetContent(www);
            item.loaded = true;
            return www.downloadHandler.text;
        }
    }
}

public class Item
{
    public string title;
    public string previewURL;
    public string textureURL;
    public float size;
    public Texture mainTexture;
    public Texture2D previewTexture;
    public bool loaded;

    public Item(string _Title, string _Preview, string _Texture, float _Size)
    {
        loaded = false;
        title = _Title;
        previewURL = _Preview;
        textureURL = _Texture;
        size = _Size;
    }
}