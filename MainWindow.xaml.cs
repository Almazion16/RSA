using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

namespace RSA
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToEncrypt_Button_Click(object sender, RoutedEventArgs e)
        {
            string text = ForEncrypt_TextBox.Text;
            Rsa rsa = new Rsa(BigInteger.Parse(N_TextBox.Text), BigInteger.Parse(e_TextBox.Text), BigInteger.Parse(f_TextBox.Text), BigInteger.Parse(d_TextBox.Text));
            text = rsa.RSAEncryptAlgorithm(text);
            EncryptingResult_TextBox.Text = text;
        }

        private void ToDecrypt_Button_Click(object sender, RoutedEventArgs e)
        {
            string text = EncryptingResult_TextBox.Text;
            Rsa rsa = new Rsa(BigInteger.Parse(N_TextBox.Text), BigInteger.Parse(e_TextBox.Text), BigInteger.Parse(f_TextBox.Text), BigInteger.Parse(d_TextBox.Text));
            text = rsa.RSADecryptAlgorithm(text);
            DecryptingResult_TextBox.Text = text;
        }

        private void GenerateKeys_Button_Click(object sender, RoutedEventArgs e)
        {
            Rsa a = new Rsa();
            a.GenerateKeys();
            e_TextBox.Text = a.e.ToString();
            d_TextBox.Text = a.d.ToString();
            f_TextBox.Text = a.f.ToString();
            N_TextBox.Text = a.n.ToString();
        }
    }
}
