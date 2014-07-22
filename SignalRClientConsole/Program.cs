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
            var hubConnection = new HubConnection("http://localhost:8080/signalr");
            var chat = hubConnection.CreateHubProxy("MyHub");
            chat.On<string, string>(
                "addMessage",
                (name, message) => { Console.WriteLine("{0} : {1}", name, message); }
            );
            hubConnection.Start().Wait();

            chat.Invoke("Notify", "Client Console", hubConnection.ConnectionId);

            string chatMessage = null;

            while ((chatMessage = Console.ReadLine()) != null)
            {
                chat.Invoke("Send", "Client Console", chatMessage).Wait();
            }
        }
    }
}
