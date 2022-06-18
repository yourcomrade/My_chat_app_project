﻿using System;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Chat_app_client.ViewModel;
using Microsoft.Win32;

namespace Chat_app_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //Button to OpenFileDiaglog to load avatar
        //Link: https://social.msdn.microsoft.com/Forums/vstudio/en-US/ab245116-547a-451f-a362-97cf17a524cf/how-to-set-background-image-in-the-button-at-runtime-in-wpf?forum=wpf
        private async void Button_loadImage(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Title = "Select a picture";
                op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                            "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                            "Portable Network Graphic (*.png)|*.png";
                if (op.ShowDialog() == true)
                {
                    
                    Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        BitmapImage bitimg= new BitmapImage();
                        bitimg.BeginInit();
                        bitimg.CacheOption = BitmapCacheOption.OnLoad;
                        bitimg.UriSource = new Uri(op.FileName);
                        bitimg.EndInit();
                        Image img = new Image();
                        img.Stretch = Stretch.Fill;
                        img.Source = bitimg;
                        ButtonLoadAva.Content = img;
                        ButtonLoadAva.Background = new ImageBrush(bitimg);
                        using (Stream fs = op.OpenFile())
                        {
                            var vm = DataContext as MyMainViewModel;
                            vm.Load_ava.Execute(fs);
                        }
                    },DispatcherPriority.Background);
                    
                    
                }

            });

        }
    }
}