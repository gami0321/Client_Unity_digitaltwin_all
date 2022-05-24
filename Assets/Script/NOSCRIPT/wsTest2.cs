using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using WebSocketSharp;

public class wsTest2 : MonoBehaviour
{
    private WebSocket ws;
    public string user;
    public string user2;
    public string userId;
    public static string userIp;
    //public string userPort;
    public static string userInfo;
    void Start()
    {
        ws = new WebSocket("ws://3.35.92.132:3002");

        ws.OnMessage += ws_OnMessage;
        ws.OnOpen += ws_OnOpen;
        ws.OnClose += ws_OnClose;
        ws.Connect();
        ws.Send("hello Server!");

        userIp = "";
        Debug.Log(userIp);
    }

    
    [System.Serializable]
    public class userip 
    {
        public string user_id { get; set; }
        public string user_ip { get; set; }
    }

    void ws_OnMessage(object sender, MessageEventArgs e) {
        Debug.Log(e.Data);
        user = e.Data;
        
        userip user2 = JsonConvert.DeserializeObject<userip>(user);
        userId = user2.user_id;
        userIp = user2.user_ip;

        Debug.Log(userId);
        Debug.Log(userIp);

        userInfo = "ws://" + userIp+":8080";
        Debug.Log(userInfo);
    }

    void ws_OnOpen(object sender, System.EventArgs e) { Debug.Log("open2"); }

    void ws_OnClose(object sender, CloseEventArgs e) { Debug.Log("close2"); }
}
