using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ConnectToWeb : MonoBehaviour
{
    [SerializeField] private Text textDebug;
    [SerializeField] private SpriteRenderer spriteRenderDebug;

    public DebugPackControl debugPack;

    void Start()
    {
        //Debug addresses
        string url = "https://i.imgur.com/lKmmuI0.png"; //Debug HTML packet
        string url2 = "https://www.google.com/"; //Google
        string url3 = "https://www.google.com:1234"; //Error
        string url4 = "https://www.google.com/none"; //Google 404

        //Test connect to web address
        //Get(url4, (string error) => {
        //    //Error
        //    Debug.Log("Error: " + error);
        //    textDebug.text = "Error: " + error;
        //}, (string text) => {
        //    //Successful connection with URL
        //    Debug.Log("Received: " + text);
        //    textDebug.text = "Data returned: " + text;
        //});

        //GetTexture(url, (string error) => {
        //    //Error
        //    Debug.Log("Error: " + error);
        //    textDebug.text = "Error: " + error;
        //}, (Texture2D texture2D) => {
        //    //Successful connection with URL
        //    Debug.Log("Received Image Data");
        //    textDebug.text = "Image Data returned successfully!";
        //    Sprite sprite = Sprite.Create(texture2D, new Rect(0,0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        //    spriteRenderDebug.sprite = sprite;
        //});
    }

    public void ConnectAndRetrieve()
    {
        string url = "https://i.imgur.com/QPxeQ4I.png"; //Debug HTML packet
        bool result; 

        GetTexture(url, (string error) => {
            //Error
            Debug.Log("Error: " + error);
            textDebug.text = "Error: " + error;

            //inform debug packet
            debugPack.ErrorReturned();
        }, (Texture2D texture2D) => {
            //Successful connection with URL
            Debug.Log("Received Image Data");
            textDebug.text = "Image Data returned successfully!";
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            spriteRenderDebug.sprite = sprite;

            //inform debug packet
            debugPack.WinningsReturned();
        });

        //TODO Handle if the packet/session has already been won/completed
        //debugPack.NoWinnings();

    }

    //Attempt to get PAGE DATA from given URL
    private void Get(string url, Action<string> onError, Action<string> onSuccess)
    {
        StartCoroutine(GetCoroutine(url, onError, onSuccess));
    }

    //Attempt to get IMAGE DATA from given URL
    private void GetTexture(string url, Action<string> onError, Action<Texture2D> onSuccess)
    {
        StartCoroutine(GetTextureCoroutine(url, onError, onSuccess));
    }

    //Coroutine for retrieving data from given URL
    private IEnumerator GetCoroutine(string url, Action<string> onError, Action<string> onSuccess)
    {
        using (UnityWebRequest unityWebReq = UnityWebRequest.Get(url)) {
            yield return unityWebReq.SendWebRequest();

            //When request is completed
            if(unityWebReq.isNetworkError || unityWebReq.isHttpError) {
                onError(unityWebReq.error);
            } else {
                onSuccess(unityWebReq.downloadHandler.text);
            }
        }
    }
    
    //Coroutine for retrieving data from given URL
    private IEnumerator GetTextureCoroutine(string url, Action<string> onError, Action<Texture2D> onSuccess)
    {
        using (UnityWebRequest unityWebReqTexture = UnityWebRequestTexture.GetTexture(url)) {
            yield return unityWebReqTexture.SendWebRequest();

            //When request is completed
            if(unityWebReqTexture.isNetworkError || unityWebReqTexture.isHttpError) {
                onError(unityWebReqTexture.error);
            } else {
                DownloadHandlerTexture downloadHandlerTexture = unityWebReqTexture.downloadHandler as DownloadHandlerTexture;
                onSuccess(downloadHandlerTexture.texture);
            }
        }
    }
}