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
using System.Windows.Shapes;

namespace Shesh_Besh
{
    /// <summary>
    /// Interaction logic for Instructions.xaml
    /// </summary>
    public partial class Instructions : Window
    {
        public Instructions()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            Growing.Grow_Up((TextBlock)sender);
        }
        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            Growing.Grow_Down((TextBlock)sender);
        }
        private void OnBackMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            this.Close();
            mainwindow.Show();
        }
    }
}
