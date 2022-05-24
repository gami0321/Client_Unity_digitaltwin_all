using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Newtonsoft.Json;
using WebSocketSharp;

public class wsTest : MonoBehaviour ,IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private WebSocket ws;
    //private float fDestroyTime = 2f;
    //private float fTickTime;
    float m_fSpeed = 5.0f;
    public int x_1;
    public int y_1;

    private Image bgImg;
    private Image joystickImg;
    private Vector3 inputVector;

    void Start()
    {
        ws = new WebSocket("ws://220.69.241.222:8888");

        ws.OnMessage += ws_OnMessage;
        ws.OnOpen += ws_OnOpen;
        ws.OnClose += ws_OnClose;
        ws.Connect();
        ws.Send("Hello Arduino!");

        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
    }

    void ws_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log(e.Data);
        ws.Send("hello Arduino!");
    }


    void ws_OnOpen(object sender, System.EventArgs e) { Debug.Log("open_Arduino"); }

    void ws_OnClose(object sender, CloseEventArgs e) { Debug.Log("close_Arduino"); }

    public void Send()
    {
        Debug.Log("x:" + x_1 + ", y:" + y_1);
        ws.Send(x_1 + "," + y_1);
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            float x_2 = pos.x;
            float y_2 = pos.y;

            x_1 = (int)x_2;
            y_1 = (int)y_2;

            if (-255 < x_1 && x_1 < 255 && -255 < y_1 && y_1 < 255) {
                if (y_1 > 0)
                {
                    if (x_1 < 0)
                    {
                        x_1 *= (-1);
            
                        if (x_1 < 50) 
                        {
                            x_1 = y_1;
                            Send();
                        }

                        else if (x_1 > y_1)
                        {
                            y_1 += x_1 - y_1;
                            x_1 -= 50;
                            Send();
                        }

                        else
                        {
                            Send();
                        }
                    }
                    else
                    {
                        if (y_1 > x_1)
                        {
                            x_1 += y_1 - x_1;
                            y_1 -= 50;
                            Send();
                        }
                        else if (x_1 < 50)
                        {
                            x_1 = y_1;
                            Send();
                        }
                        else
                        {
                            ws.Send(x_1 + "," + y_1);
                            Send();
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
                            x_1 += (y_1 + (x_1 * -1));
                            y_1 += 50;
                            Send();
                        }
                        else if (-50 < x_1)
                        {
                            x_1 = y_1;
                            Send();
                        }
                        else
                        {
                            Send();
                        }
                    }
                    else 
                    {
                        if (x_1 < y_1)
                        {
                            y_1 += x_1 + (y_1 * -1);
                            x_1 += 50;
                            Send();
                        }
                        else if (-50 < x_1)
                        {
                            x_1 = y_1;
                            Send();
                        }
                        else
                        {
                            Send();
                        }

                    }
                }
            }

            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);
            
            inputVector = new Vector3(pos.x , pos.y , 0);

            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
            joystickImg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 13)* m_fSpeed
                                                                    , inputVector.y * (bgImg.rectTransform.sizeDelta.y / 13)* m_fSpeed);
        }
    }

    public virtual void OnPointerDown(PointerEventData ped) { OnDrag(ped);}

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }
}
