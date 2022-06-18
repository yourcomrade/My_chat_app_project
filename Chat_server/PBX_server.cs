using System;
using Ozeki;
using Ozeki.Network;
using Ozeki.VoIP;

namespace Test_server
{
    class PBXServer : PBXBase
    {
        string localAddress;
 
        public PBXServer(string localAddress, int minPortRange, int maxPortRange)
            : base(minPortRange, maxPortRange)
        {
            this.localAddress = localAddress;
            Console.WriteLine("PBX starting...");
            Console.WriteLine("Local address: " + localAddress);
        }
 
        protected override void OnStart()
        {
            SetListenPort(localAddress, 5060, TransportType.Udp);
            Console.WriteLine("Listened port: 5060(UDP)");
 
            Console.WriteLine("PBX started.");
            base.OnStart();
        }
 
        protected override RegisterResult OnRegisterReceived(ISIPExtension extension, SIPAddress from, int expires)
        {
            Console.WriteLine("Register received from: " + extension.ExtensionID);
            return base.OnRegisterReceived(extension, from, expires);
        }
 
        protected override void OnUnregisterReceived(ISIPExtension extension)
        {
            Console.WriteLine("Unregister received from: " + extension.ExtensionID);
            base.OnUnregisterReceived(extension);
        }
 
        protected override void OnCallRequestReceived(ISessionCall call)
        {
            Console.WriteLine("Call request received. Caller: " + call.DialInfo.CallerID + " callee: " + call.DialInfo.Dialed);
            base.OnCallRequestReceived(call);
        }
    }
}