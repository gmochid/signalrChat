using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace SignalRClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // create hub connection to specified url
            var hubConnection = new HubConnection("http://localhost:8080/signalr");
            // create hub proxy to communicate with one of specified hub
            var chat = hubConnection.CreateHubProxy("MyHub");
            // register event and callback
            chat.On<string, string>(
                "addMessage",
                (name, message) => { Console.WriteLine("{0} : {1}", name, message); }
            );
            // start create connection
            hubConnection.Start().Wait();

            Console.WriteLine("Connection to server established");

            string displayName = null;
            string chatMessage = null;

            Console.Write("Display Name: ");
            displayName = Console.ReadLine();

            // adding the connection ID to the client list and notifying other clients about the arrival of this client
            chat.Invoke("Notify", displayName, hubConnection.ConnectionId);

            Console.Write("Write your message: ");
            while ((chatMessage = Console.ReadLine()) != null)
            {
                // send the message
                chat.Invoke("Send", displayName, chatMessage).Wait();
                Console.Write("Write your message: ");
            }
        }
    }
}
