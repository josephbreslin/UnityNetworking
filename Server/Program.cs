using System;
using System.Net;   
using System.Net.Sockets;
using System.Text;

namespace Server
{
    static public class Program
    {
        static int maxPlayerCount;
        static int port = 1510;

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the Maximum number of players that can sign into this server, Between 2 and 20");
            string str = Console.ReadLine();
            maxPlayerCount = Convert.ToInt32(str);

            if(maxPlayerCount >20)
                maxPlayerCount = 20;
            if(maxPlayerCount < 2)
                maxPlayerCount = 2;
            
            ActivateServer();
        }

        static void ActivateServer()
        {
            Console.WriteLine("Server activated! \nClient capacity is set to {0}", maxPlayerCount);

            //Get host address
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);

            //Open Socket
            Socket listener = new Socket(ipAddr.AddressFamily,SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(maxPlayerCount);

                while(true)
                {
                    Console.WriteLine("Waiting for connection");
                    Socket clientSocket = listener.Accept();

                    string data = null;
                    Byte[] bytes = new Byte[1024];

                    while(true)
                    {
                        int numBytes = clientSocket.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, numBytes);

                        if(data.IndexOf("<EOF>") > -1)
                            break;
                    }
                    Console.WriteLine("Text Recieved --> {0}", data);
                    Random random = new Random();
                    int gridOutput = random.Next(0, 100);
                    byte[] message = Encoding.ASCII.GetBytes(gridOutput.ToString());
                    clientSocket.Send(message);

                    //Experiment without this feature, or with this feature on command from the client
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch(Exception e)
            {
                //Log errors
                Console.WriteLine(e.ToString());
            }
        }
    }
}
