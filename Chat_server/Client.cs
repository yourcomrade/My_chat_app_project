using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Ozeki.Network;

namespace Test_server
{
    public class Client
    {
        public Packet my_pack { get;}
        public TcpClient m_tcpClient { get; set; }
        public string username { get; set; }
        public string userId { get; set; }
        public int PBXport { get; set; }
        public string lastest_mes { get; set; }
        public bool Stillhere { get; set; }
        
        public Client(TcpClient mcl, int userID)
        {
            m_tcpClient = mcl;
            userId = userID.ToString();
            Stillhere = true;
            my_pack = new Packet(m_tcpClient.GetStream());
            
        }
        
        
    }
}