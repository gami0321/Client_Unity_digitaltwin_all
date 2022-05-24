using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using WebSocketSharp;
using System.Diagnostics;
using System;

public class arduinotest : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private WebSocket ws;
    private int x_1;
    private int y_1;
    private int a;
    private int b;
    float m_fSpeed = 5.0f;

    private Image bgImg;
    private Image joystickImg;
    private Vector3 inputVector;

    
    void Start()
    {
        UnityEngine.Debug.Log(IPTest.userIp);
        ws = new WebSocket("ws://"+IPTest.userIp+":8888");

        ws.OnMessage += ws_OnMessage;
        ws.OnOpen += ws_OnOpen;
        ws.OnClose += ws_OnClose;
        ws.Connect();
        //ws.Send("hello Arduino!");

        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();

        InvokeRepeating("Send", 0.5f, 0.5f);
    }
    void ws_OnMessage(object sender, MessageEventArgs e)
    {
        UnityEngine.Debug.Log(e.Data);
        //string sNow2;
        //sNow2 = DateTime.Now.ToString("Receive : "+"yyyy-MM-dd HH:mm:ss.fff");
        //UnityEngine.Debug.Log(sNow2);
    }

    void ws_OnOpen(object sender, System.EventArgs e) { /*UnityEngine.Debug.Log("open_Arduino");*/ }

    void ws_OnClose(object sender, CloseEventArgs e) { /*UnityEngine.Debug.Log("close_Arduino");*/ }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            float x_2 = pos.x;
            float y_2 = pos.y;

            x_1 = (int)x_2;
            y_1 = (int)y_2;

            if (-255 < x_1 && x_1 < 255 && -255 < y_1 && y_1 < 255)
            {
                if (y_1 > 0)
                {
                    if (x_1 < 0)
                    {
                        x_1 *= (-1);

                        if (x_1 < 50)
                        {
                            a = y_1-10;
                            b = y_1;
                        }

                        else if (x_1 > y_1)
                        {
                            b = x_1;
                            a = y_1-10;
                        }

                        else
                        {
                            a = x_1-10;
                            b = y_1;
                        }
                    }
                    else
                    {
                        if (y_1 > x_1)
                        {
                            b = x_1;
                            a = y_1-10;
                        }
                        
                        else if (x_1 < 50)
                        {
                            a = y_1-10;
                            b = y_1;
                        }
                        
                        else
                        {
                            a = x_1-10;
                            b = y_1;
                        }
                    }
                }

                else
                {
                    if (x_1 > 0)
                    {
                        x_1 *= (-1);
                        if (x_1 > y_2)
                        {
                            b = x_1;
                            a = y_1+10;
                        }
                        else if (-50 < x_1)
                        {
                            a = y_1+10;
                            b = y_1;
                        }
                        else
                        {
                            a = x_1+10;
                            b = y_1;
                        }
                    }
                    else
                    {
                        if (x_1 < y_1)
                        {
                            b = x_1;
                            a = y_1+10;
                        }
                        
                        else if (-50 < x_1)
                        {
                            a = y_1+10;
                            b = y_1;
                        }

                        else
                        {
                            a = x_1+10;
                            b = y_1;
                        }

                    }
                }
            }

            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x, pos.y, 0);

            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
            joystickImg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 13) * m_fSpeed
                                                                    ,inputVector.y * (bgImg.rectTransform.sizeDelta.y / 13) * m_fSpeed);
        }
    }
    public void Send()
    {
        //string sNow;
        //sNow = DateTime.Now.ToString("Send : "+"yyyy-MM-dd HH:mm:ss.fff");
        //UnityEngine.Debug.Log(sNow);
        UnityEngine.Debug.Log("x:" + a + ", y:" + b);
        ws.Send(a + "," + b);
    }

    public void bn_stop() 
    {
        a = 0;
        b = 0;
        Send();
    }

    public virtual void OnPointerDown(PointerEventData ped) { OnDrag(ped); }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }
}
