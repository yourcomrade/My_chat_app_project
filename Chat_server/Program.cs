

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Ozeki.Network;

namespace Test_server
{
    internal class Program
    {

        private static TcpListener m_listener;
        private static List<Client> m_clients;
        private static int ID = 100;
        private static object m_lock = new object();
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Start");
            var myPBX = new PBXServer(NetworkAddressHelper.GetLocalIP().ToString(), 20000, 20500);
            Console.WriteLine("PBX server start at port 5060 and tcp server start at port 2003");
            m_listener = new TcpListener(IPAddress.Any, 2003);
            m_clients = new List<Client>();
            try
            {
                m_listener.Start();
                myPBX.Start();
                while (true)
                {
                   
                        var client =  await m_listener.AcceptTcpClientAsync().ConfigureAwait(false);
                        if (client != null)
                        {

                            ThreadPool.QueueUserWorkItem(async state => await Serve(new Client(client, ID)));

                        }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            finally
            {
                myPBX.Stop();
                m_listener.Stop();
            }
            

        }

        public static async Task Serve(Client my_cl)
        {
            Monitor.Enter(m_lock);
            try
            {
                
                m_clients.Add(my_cl);
                ID++;
                Monitor.Pulse(m_lock);
            }
            finally
            {
                Monitor.Exit(m_lock);
            }
            Console.WriteLine($"Member of clients: {m_clients.Count}");
            while (my_cl.Stillhere)
            {
                byte code = await my_cl.my_pack.Read_codeAsync();
                switch (code)
                {
                    case 1:
                        my_cl.username = await my_cl.my_pack.Read_msgAsync();
                        Console.WriteLine($"New user: {my_cl.username}");
                        await my_cl.my_pack.Send_msgAsync(my_cl.userId+" 5060", 1);
                        
                        break;
                    case 2:
                        my_cl.lastest_mes = await my_cl.my_pack.Read_msgAsync();
                        await Broadcast(my_cl.username, my_cl.userId, my_cl.lastest_mes);
                        
                        break;
                        
                    case 4:
                        string file_name = await my_cl.my_pack.Recv_FileAsync();
                        Console.WriteLine($"New {file_name} upload");
                        await Broadcast(my_cl.username, my_cl.userId, file_name);
                        break;
                    case 10:
                        Console.WriteLine($"{my_cl.username} has left");
                        break;
                }

                if (code == 10)
                {
                    my_cl.m_tcpClient.Close();
                    my_cl.m_tcpClient.Dispose();
                    
                    my_cl.Stillhere = false;
                    Monitor.Enter(m_lock);
                    try
                    {
                        m_clients.Remove(my_cl);
                        Monitor.Pulse(m_lock);
                    }
                    finally
                    {
                        Monitor.Exit(m_lock);
                    }
                    break;
                }
               
            }

           
        }

        public static async Task Broadcast(string username, string userID, string unknown)
        {
            
            try
            {
               
                //var user_send = m_clients.Where(o => o.userId.Equals(userID)).GetEnumerator().Current;
                foreach (var user in m_clients)
                {
                    
                    
                        if (File.Exists(unknown))
                        {
                            string msg_to_user = username + " " + DateTime.Now.ToShortDateString();
                            await user.my_pack.Send_msgAsync(msg_to_user, 3);
                            await user.my_pack.Send_FileAsync(unknown, 5);
                        }
                        else
                        {
                            string msg_to_user=username+" "+ DateTime.Now.ToShortDateString()+ " "+unknown;
                            await user.my_pack.Send_msgAsync(msg_to_user, 3);

                        }
                   
                }

                
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error from here: {e}");
            }

        }

    }
}