﻿using cuddly_chainsaw.Models;
using cuddly_chainsaw.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace cuddly_chainsaw
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        AssignmentViewModel AssignmentModel;
        UserViewModel UserModel;
        public LoginPage()
        {
            //this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.InitializeComponent();
        }

        private async void login_Click(object sender, RoutedEventArgs e)
        {
            UserModel = new UserViewModel();
            string user = username.Text;
            string psw = password.Password;
            Boolean isLogin = await UserModel.logIn(user, psw);
            if (isLogin)
            {
                //login.Visibility = Visibility.Collapsed;
                //sign.Visibility = Visibility.Collapsed;
                //proRring.Visibility = Visibility.Visible;

                AssignmentModel = new AssignmentViewModel();
                var updator = TileUpdateManager.CreateTileUpdaterForApplication();
                updator.Clear();
                updator.EnableNotificationQueue(true);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(File.ReadAllText("Tile.xml"));
                XmlNodeList texts = xmlDoc.GetElementsByTagName("text");
                foreach (Assignment ass in AssignmentModel.AllAssignments)
                {
                    if (!ass.isEnded())
                    {
                        int count = 0;

                        // Small
                        ((XmlElement)texts[count]).InnerText = ass.Title;
                        count++;

                        // Medium
                        ((XmlElement)texts[count]).InnerText = ass.Title;
                        count++;
                        ((XmlElement)texts[count]).InnerText = ass.DDL.ToString();
                        count++;

                        // Wide
                        ((XmlElement)texts[count]).InnerText = ass.Title;
                        count++;
                        ((XmlElement)texts[count]).InnerText = ass.DDL.ToString();
                        count++;

                        //Large
                        ((XmlElement)texts[count]).InnerText = ass.Title;
                        count++;
                        ((XmlElement)texts[count]).InnerText = ass.DDL.ToString();


                        TileNotification notification = new TileNotification(xmlDoc);
                        updator.Update(notification);
                    }
                }
                this.Frame.Navigate(typeof(MainPage), UserModel);
            }
            //login.Visibility = Visibility.Visible;
            //sign.Visibility = Visibility.Visible;
            //proRring.Visibility = Visibility.Collapsed;
            
        }

        private void sign_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SignUpPage));
        }
    }
}