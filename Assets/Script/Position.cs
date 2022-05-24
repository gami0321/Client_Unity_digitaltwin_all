using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using WebSocketSharp;

public class Position : MonoBehaviour
{
    private WebSocket ws;
    private string user;
    private string user3;
    private string userID_2;
    private float userPosX_2;
    private float userPosY_2;

    private Text userPosText;
    public static Vector3 Npos;
    // Start is called before the first frame update
    void Start()
    {
        ws = new WebSocket("ws://3.35.92.132:3000");

        ws.OnMessage += ws_OnMessage;
        ws.OnOpen += ws_OnOpen;
        ws.OnClose += ws_OnClose;
        ws.Connect();
        //ws.Send("hello Server!");

        Npos = Vector3.zero;

        userPosText = GameObject.Find("Text_Value").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        userPosText.text = "UserID : 4" + " UserPosX : " + userPosX_2.ToString() + " UserPosY : " + userPosY_2.ToString();
    }

    [System.Serializable]
    public class userInfo
    {
        public string userID { get; set; }
        public float userPosX { get; set; }
        public float userPosY { get; set; }
    }

    void ws_OnMessage(object sender, MessageEventArgs e)
    {
        user = e.Data;
        userInfo user3 = JsonConvert.DeserializeObject<userInfo>(user);
        userID_2 = user3.userID;

        if (userID_2 == "4")
        {
            userID_2 = user3.userID;
            userPosX_2 = user3.userPosX;
            userPosY_2 = user3.userPosY;
            //Debug.Log(userID_2);
            //Debug.Log(userPosX_2);
            //Debug.Log(userPosY_2);
        }

        else
        {
            Debug.Log("error");
        }

        Npos = new Vector3(3*userPosX_2, 0, 3 * userPosY_2);
    }

    void ws_OnOpen(object sender, System.EventArgs e) { /*Debug.Log("open");*/ }

    void ws_OnClose(object sender, CloseEventArgs e) { /*Debug.Log("close");*/ }
}
