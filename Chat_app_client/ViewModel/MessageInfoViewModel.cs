using System.ComponentModel;
using System.Runtime.CompilerServices;
using Chat_app_client.Model;

namespace Chat_app_client.ViewModel
{
    public class MessageInfoViewModel:INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private MessageInfo m_messageInfo=new MessageInfo();

        public string Username
        {
            get => m_messageInfo.Username;
            set
            {
                m_messageInfo.Username = value;
                OnPropertyChanged("Username");
            }
        }

        public string UserID
        {
            get => m_messageInfo.UserID;
            set
            {
                m_messageInfo.UserID = value;
                OnPropertyChanged("UserID");
            }
        }

        public string Date_time
        {
            get => m_messageInfo.Date_time;
            set
            {
                m_messageInfo.Date_time = value;
                OnPropertyChanged("Date_time");
            }
        }

        public string Message
        {
            get => m_messageInfo.Lastest_message;
            set
            {
                m_messageInfo.Lastest_message = value;
                OnPropertyChanged("Message");
            }
        }

        public bool Is_mess
        {
            get => m_messageInfo.Is_mess;
            set
            {
                m_messageInfo.Is_mess = value;
                OnPropertyChanged("Is_mess");
            }
        }
    }
}

