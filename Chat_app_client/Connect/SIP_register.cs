using System;
using Ozeki.VoIP;

namespace Test_app
{
    
    public class SIP_register
    {
        private static ISoftPhone m_softphone;   // softphone object
        private static IPhoneLine m_phoneLine;   // phoneline object

        public  SIP_register(string my_ID,string IPadd ,int port)
        {
            m_softphone = SoftPhoneFactory.CreateSoftPhone(12000, 20000);
            var registrationRequired = true;
            var userName = my_ID;
            var displayName = my_ID;
            var authenticationId = my_ID;
            var registerPassword = my_ID;
            var domainHost = IPadd;
            var domainPort = port;
            var account = new SIPAccount(registrationRequired, displayName, userName, authenticationId, registerPassword, domainHost, domainPort);
            RegisterAccount(account);
        }
        private void RegisterAccount(SIPAccount account)
        {
            try
            {
                m_phoneLine = m_softphone.CreatePhoneLine(account);
                m_phoneLine.RegistrationStateChanged += sipAccount_RegStateChanged;
                m_softphone.RegisterPhoneLine(m_phoneLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during SIP registration: " + ex);
            }
        }
        private void sipAccount_RegStateChanged(object sender, RegistrationStateChangedArgs e)
        {
            if (e.State == RegState.Error || e.State == RegState.NotRegistered)
                Console.WriteLine("Registration failed!");
 
            if (e.State == RegState.RegistrationSucceeded)
                Console.WriteLine("Registration succeeded - Online!");
        }
    } 
    
}