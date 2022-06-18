using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Chat_app_client.Connect;
using Chat_app_client.Model;

namespace Chat_app_client.ViewModel
{
    public class MyMainViewModel : INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<UserInfoViewModel> users_list { get; set; }
        public ObservableCollection<MessageInfoViewModel> chat_bubbles { get; set; }
        public UserInfoViewModel m_UserInfoViewModel { get; set; }
        public myServer_connect MyServerConnect { get; set; }
        private readonly object m_lock = new object();
        public string Register
        {
            get =>MyServerConnect?.status;
            set
            {
                MyServerConnect.status = value;
                OnPropertyChanged("Register");
            }
        }

        public async event Action Recvmess;
        public UserInfo myProfile { get; set; }
        public ICommand Init_connect_to_server { get; set; }
        public ICommand  Send_message { get; set; }
        public ICommand  Recv_message { get; set; }
        public ICommand  Send_ava_img { get; set; }
        public ICommand  Recv_ava_img { get; set; }
        public ICommand  Send_file { get; set; }
        public ICommand  Recv_file { get; set; }
        public ICommand  Disconnect { get; set; }
        public ICommand  Load_ava { get; set; }
        //public myServer_connect MyServerConnect { get; set; }
        private string m_IPaddress="0";
        public string IPaddress
        {
            get => m_IPaddress;
            set
            {
                m_IPaddress = value;
                OnPropertyChanged("IPaddress");
            }
        }

        private string m_TextMessage=String.Empty;

        public string TextMessage
        {
            get => m_TextMessage;
            set
            {
                m_TextMessage = value;
                OnPropertyChanged("TextMessage");
            }
        }

        private string m_portnum="0";
        public string portnum
        {
            get => m_portnum;
            set
            {
                m_portnum = value;
                OnPropertyChanged("portnum");
            }
        }

        public MyMainViewModel()
        {
            
            users_list = new ObservableCollection<UserInfoViewModel>();
            chat_bubbles = new ObservableCollection<MessageInfoViewModel>();
            
            myProfile = new UserInfo
            {
                Username = "Minh",
                UserID = "100",
                // AvatarSource = @"Elaina.jpg"
            };
            m_UserInfoViewModel = new UserInfoViewModel
            {
                Username = myProfile.Username,
                AvatarSource = myProfile.AvatarSource,
                UserID = myProfile.UserID
            };
            

            users_list.Add(m_UserInfoViewModel);
            
            Load_ava = new RelayCommand<Stream>(async (p) => await Save_Ava(p), (p) =>
            {
                if (p == null)
                {
                    return false;
                }
                else
                    return true;
            } );
            Init_connect_to_server = new RelayCommand(async (p) => await Task.Run(async () => { await InitConnect();}),
                p => !string.IsNullOrEmpty(myProfile.Username));
            Disconnect = new RelayCommand(async p => await Task .Run(async () => { await DisconnectServer();}),
                p => !string.IsNullOrEmpty(myProfile.Username));
            Send_message = new RelayCommand(async p => await SendMessage(),
                o => !string.IsNullOrEmpty(myProfile.Username));
            Recvmess += (async () => await MessageReceived());

        }
        

        private async Task Save_Ava(Stream fs)
        {
           
            var arr = new byte[fs.Length];
            if (await fs.ReadAsync(arr, 0, arr.Length) > 0)
            {
                m_UserInfoViewModel.AvatarSource = arr;
            }
        }

        private async Task InitConnect()
        {
            MyServerConnect=new myServer_connect(IPaddress,Convert.ToInt32(portnum));
            MyServerConnect.my_user_name = m_UserInfoViewModel.Username;
            (bool, string ) res = await MyServerConnect.Try_connect_server_init(myProfile.Username);
            if (res.Item1)
            {
                Register = "Register successfull";
                await Waiting();
            }
            else
            {
                Register = res.Item2;
            }
        }

        private async Task SendMessage()
        {
            if (!string.IsNullOrEmpty(TextMessage) && !string.IsNullOrWhiteSpace(TextMessage))
            {
                if (!MyServerConnect.status.Equals("Disconnect"))
                {
                    await MyServerConnect.my_pack.Send_msgAsync(TextMessage, 2);
                }
                
            }

        }

        private async Task Waiting()
        {
            while (true)
            { byte code = await MyServerConnect.my_pack.Read_codeAsync();
               
                   switch (code)
                   {
                       case 3:
                           TextMessage = await MyServerConnect.my_pack.Read_msgAsync();
                            Recvmess?.Invoke();
                           break;
                       default:
                           MessageBox.Show(Convert.ToString(code));
                           break;
                   }
                   
                
                
            }
            
        }
        
        

        private async Task DisconnectServer()
        {
            MyServerConnect.status = "Disconnect";
            await MyServerConnect.my_pack.Send_msgAsync("", 10);
        }
        private async Task MessageReceived()
        {
            
                Application.Current.Dispatcher.InvokeAsync(async () =>
                {
                    var msg_arr = TextMessage.Split('$').ToArray();
                    chat_bubbles.Add(new MessageInfoViewModel
                    {
                        Date_time = DateTime.Now.ToShortDateString(),
                        Is_mess = true,
                        Message = msg_arr[2],
                        Username = msg_arr[0],
                        UserID = msg_arr[1]
                    });
                });
               
            
            
           
               
                
                
        }
       
    }
}