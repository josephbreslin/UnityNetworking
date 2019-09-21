using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class LightGrid : MonoBehaviour
{
    public float cubeSize = .5f;
    List<GameObject> cubes = new List<GameObject>();

    //Client
    int port = 1510;

    void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localScale= new Vector3(cubeSize, cubeSize, cubeSize);
                cube.transform.position = new Vector3(i, j, 0);
                cubes.Add(cube);
            }
        }
        Camera.main.transform.position = new Vector3(5, 5, -8);
        SetCubeColors(45);
    }

    void SetCubeColors(int index)
    {
        for(int i = 0; i < cubes.Count; i++)
        {
            if(i == index)
            {
                cubes[i].GetComponent<Renderer>().material.color = Color.magenta;
            }
            else
            {
                cubes[i].GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }
    public void RequestNumber()
    {
        Debug.Log("Requesting number from server");
        ActivateClient(false);
    }


    public void ActivateClient(bool isShutDown)
    {
        try
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily,
                                        SocketType.Stream,
                                        ProtocolType.Tcp);

            try
            {
                sender.Connect(localEndPoint);
                Debug.Log("Socket Connected to " +sender.RemoteEndPoint.ToString());

                byte[] messageSent = Encoding.ASCII.GetBytes("Server please give me a random number? <EOF>");

                int byteSent = sender.Send(messageSent);

                byte[] messageReceived = new byte[1024];
                int byteRecv = sender.Receive(messageReceived);
                Debug.Log("Message from Server -> " + Encoding.ASCII.GetString(messageReceived, 0, byteRecv));

                int index = Convert.ToInt32(Encoding.ASCII.GetString(messageReceived, 0, byteRecv));
                SetCubeColors(index);

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
               
                
            }
            catch (ArgumentNullException ane)
            {
                Debug.Log("ArgumentNullException " + ane.ToString());
            }
            catch (SocketException se)
            {
                Debug.Log("SocketException " + se.ToString());
            }
            catch (Exception e)
            {
                Debug.Log("Unexpected exception " + e.ToString());
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
        isShutDown = false;
    }
}
