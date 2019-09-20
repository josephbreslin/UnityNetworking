using System;

namespace Server
{
    public class Program
    {
        int maxPlayerCount;

        static void main(string[] args)
        {
            if(args.Length == 0)
            {
                maxPlayerCount = 2;
            }
            else
            {
                if(args.Length > 0)
                {
                    maxPlayerCount = ((int)args[0] < 100 && (int)args[0] > 0) ? (int)args[0] : 2;
                }
            }
            Console.log("Server activated! \nClient capacity is set to {0}", maxPlayerCount);
            ActivateServer();
        }

        static void ActivateServer()
        {
            
        }
    }
    //Notes https://www.geeksforgeeks.org/socket-programming-in-c-sharp/
}
