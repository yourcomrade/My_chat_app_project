using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using Test_app;
using Test_server;

namespace Chat_app_client.Connect
{
    public class myServer_connect
    {
        public string my_user_name { get; set; }
        public string my_user_ID { get; set; }
        public SIP_register my_SIP;
        private int PBXport;
        private TcpClient m_TcpClient;
        public string IP_server { get; set; }
        public string status { get; set; }
        public int numport { get; set; }
        public event Action InitialConnect;
        public event Action SendMessage;
        public event Action Disconnect;
        public event Action Recvmess;
        public event Action SendFile;
        public event Action RecvFile;
        public event Action newUserconnect;
        public Packet my_pack { get; set; }

        public myServer_connect(string IPaddress_server,int portnum)
        {
            IP_server = IPaddress_server;
            this.numport = portnum;
            this.m_TcpClient = new TcpClient(IPaddress_server, portnum);
            this.IP_server = IPaddress_server;
            my_pack = new Packet(m_TcpClient.GetStream());
        }

       
       
        public async Task<(bool,string)> Try_connect_server_init( string username)
        {
            if (!m_TcpClient.Connected)
            {
                this.m_TcpClient = new TcpClient(IP_server,numport);
                my_pack = new Packet(m_TcpClient.GetStream());
            }
            my_user_name = username;
            await my_pack.Send_msgAsync(my_user_name, 1);
            byte code = await my_pack.Read_codeAsync();
            if (code == 1)
            {
                string login = await my_pack.Read_msgAsync();
                my_user_ID = login.Split(' ').ToArray()[0];
                PBXport =Convert.ToInt32(login.Split(' ').ToArray()[1]);
                my_SIP = new SIP_register(my_user_ID, IP_server, PBXport);
                Console.WriteLine($"Success your ID is {my_user_ID} and PBXport is {PBXport}");
                status = "Register successfully";
                return (true, login);
            }
            else
            {
                status = "Unsucessfully register ";
                return (false, "Unsuccess");
            }
        }
        

        public async Task Send()
        {
            while (true)
            {
                Console.WriteLine("Enter option: ");
                var opt = Console.ReadLine();
                switch (opt)
                {
                    case "2":
                        Console.Write("Enter mess: ");
                        var msg = Console.ReadLine();
                        if (msg != null)
                        {
                            await my_pack.Send_msgAsync(msg, 2);
                        }

                        break;
                    case "4":
                        Console.WriteLine("Enter file name: ");
                        var file = Console.ReadLine();
                        if (file != null)
                        {
                            await my_pack.Send_FileAsync(file, 4);
                        }


                        break;
                    case "10":
                        status = "Disconnect";
                        await my_pack.Send_msgAsync(" ", 10);
                        break;
                }
            }
            
        }

    }
}