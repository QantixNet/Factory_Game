using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Diiage2022.Factory_Games.Client.Services;
using System.Threading;
using Diiage2022.Factory_Games.Client.Entities;


// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace Diiage2022.Factory_Games.Client.UWP
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class PageTestCommunication : Page
    {
       

        public PageTestCommunication()
        {
            this.InitializeComponent();
            SynchronizationContext originalContext = SynchronizationContext.Current;
            //((App)App.Current).servicesClass.SetGameScreenInterface(this, originalContext);
        }

        public void DisplayPlayerChooseDeveloper(Developer developer, Company company)
        {
            txtest.Text = "ca marcheeeeeeeeeeeeeeeeee"; // developer.DeveloperName + " --- " + company.Username;
        }

        private void ButtonCom1_Click(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).servicesClass.SelectDeveloper(2);
        }

        private void ButtonCom2_Click(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).servicesClass.SelectProjet(2);
        }

        private void ButtonCom3_Click(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).servicesClass.AddDeveloperToProject(2,4);
        }

        private void ButtonCom4_Click(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).servicesClass.FireDeveloper(2);
        }

        private void ButtonCom5_Click(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).servicesClass.FinishTurn();
        }

        private void ButtonCom6_Click(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).servicesClass.AddDeveloperToSchool(2,3,4);
        }

        private void Txtest_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
