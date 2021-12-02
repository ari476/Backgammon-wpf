using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shesh_Besh
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void lbl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == lbl_Exit)
            {
                Window.GetWindow(this).Close();
            }
            if (e.Source == lbl_StartGame)
            {
                StartGame StartGame = new StartGame();
              
                StartGame.Show();
                this.Close();
            }
            if (e.Source == lbl_About)
            {

                About about = new About();
                about.Show();
                this.Close();
            }

            if (e.Source == lbl_HowToPlay)
            {
                Instructions inst = new Instructions();
                inst.Show();
                this.Close();
            }
        }
    }
}
