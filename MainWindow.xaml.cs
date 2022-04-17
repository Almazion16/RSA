using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
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
            try
            {
                int bitSize = int.Parse(BitSize_Textbox.Text);
                string text = ForEncrypt_TextBox.Text;
                Rsa rsa = new Rsa(BigInteger.Parse(N_TextBox.Text), BigInteger.Parse(e_TextBox.Text), BigInteger.Parse(f_TextBox.Text), BigInteger.Parse(d_TextBox.Text));
                
                if (ForEncrypt_TextBox.Text.Length*4 > bitSize)
                {
                    string[] subsrtingsArray = new string[ForEncrypt_TextBox.Text.Length*4/bitSize+1];
                    for (int i = 0 , j = 0; i < text.Length; i+=bitSize/4, j++)
                    {
                        if(i+ bitSize / 4 < text.Length)
                            subsrtingsArray[j] = text.Substring(i, bitSize/4);
                        else
                            subsrtingsArray[j] = text.Substring(i, text.Length-i);
                    }
                    string resultText = "";
                    for(int i = 0; i < subsrtingsArray.Length&&subsrtingsArray[i]!=null; i++)
                    {
                        resultText += rsa.RSAEncryptAlgorithm(subsrtingsArray[i]) + "$";
                    }
                    EncryptingResult_TextBox.Text = resultText;
                    return;


                }
                
                text = rsa.RSAEncryptAlgorithm(text);
                EncryptingResult_TextBox.Text = text;
            }
            catch
            {
                MessageBox.Show("Вы неверно заполнили поля");
            }
        }

        private void ToDecrypt_Button_Click(object sender, RoutedEventArgs e)
        {
            if (EncryptingResult_TextBox.Text == "")
                return;
            string text = EncryptingResult_TextBox.Text;
            Rsa rsa = new Rsa(BigInteger.Parse(N_TextBox.Text), BigInteger.Parse(e_TextBox.Text), BigInteger.Parse(f_TextBox.Text), BigInteger.Parse(d_TextBox.Text));
            if (Regex.IsMatch(text, "[$]"))
            {
                string[] substringArray = text.Split('$');
                string resultText="";
                for (int i = 0; i < substringArray.Length-1 ; i++)
                {
                    resultText += rsa.RSADecryptAlgorithm(substringArray[i]);
                }
                DecryptingResult_TextBox.Text = resultText;
                return;
            }
            text = rsa.RSADecryptAlgorithm(text);
            DecryptingResult_TextBox.Text = text;
        }

        private void GenerateKeys_Button_Click(object sender, RoutedEventArgs e)
        {
            if (BitSize_Textbox.Text == "")
                return;
            Rsa a = new Rsa();
            a.GenerateKeys(int.Parse(BitSize_Textbox.Text));
            e_TextBox.Text = a.e.ToString();
            d_TextBox.Text = a.d.ToString();
            f_TextBox.Text = a.f.ToString();
            N_TextBox.Text = a.n.ToString();
        }

        private void BitSize_Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[^0-9]"))
                e.Handled = true;
        }

        private void BitSize_Textbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
                e.Handled = true;
        }

        private void ForEncrypt_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[^0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZабвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ ,.\-!\n?()]"))
                e.Handled = true;
        }

        private void BitSize_Textbox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            string TextFotChecking = (string)e.DataObject.GetData(typeof(String));
            if (Regex.IsMatch(TextFotChecking, @"[^0-9]"))
                e.CancelCommand();
        }
        
        private void ForEncrypt_TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            string TextFotChecking = (string)e.DataObject.GetData(typeof(String));
            if (Regex.IsMatch(TextFotChecking, @"[^0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZабвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ ,.\-!\n?()]"))
                e.CancelCommand();
        }

        private void test_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
