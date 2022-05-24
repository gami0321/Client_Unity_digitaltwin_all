using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using WebSocketSharp;
using System;

public class IPTest : MonoBehaviour
{
    private WebSocket ws;
    public string user;
    public string user2;
    public static string userId;
    public static string userIp;
    public static string userInfo;
    // Start is called before the first frame update
    void Start()
    {
        ws = new WebSocket("ws://3.35.92.132:3002");

        ws.OnMessage += ws_OnMessage;
        ws.OnOpen += ws_OnOpen;
        ws.OnClose += ws_OnClose;
        ws.Connect();
        ws.Send("hello Server form iptest!");

        userIp = "";
    }

    [System.Serializable]
    public class userip
    {
        public string user_id { get; set; }
        public string user_ip { get; set; }
    }

    void ws_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log(e.Data);
        user = e.Data;

        userip user2 = JsonConvert.DeserializeObject<userip>(user);
        userId = user2.user_id;
        userIp = user2.user_ip;
    }

    void ws_OnOpen(object sender, System.EventArgs e) { Debug.Log("IPTest_open"); }

    void ws_OnClose(object sender, CloseEventArgs e) { Debug.Log("IPTest_close"); }
}
