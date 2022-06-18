﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Chat_app_client.Model;

namespace Chat_app_client.ViewModel
{
    public class UserInfoViewModel:INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private UserInfo m_userinfo = new UserInfo();

        public string Username
        {
            get => m_userinfo.Username;
            set
            {
                try
                {
                    m_userinfo.Username = value;
                    OnPropertyChanged("Username");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"e");
                }
                
            }
        }

        public string UserID
        {
            get => m_userinfo.UserID;
            set
            {
                try
                {
                    m_userinfo.UserID = value;
                    OnPropertyChanged("UserID");
                }catch (Exception e)
                {
                    Console.WriteLine($"e");
                }
                
            }
        }

        public byte[] AvatarSource
        {
            get => m_userinfo.AvatarSource;
            set
            {
                try
                {
                    m_userinfo.AvatarSource = value;
                    OnPropertyChanged("AvatarSource");
                }catch (Exception e)
                {
                    Console.WriteLine($"e");
                }
                
            }
        }
    }
}

