using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Enigma.Resources;
using Enigma.Ciphers;
using System.Text.RegularExpressions;
using Microsoft.Phone.Tasks;

namespace Enigma
{
    public partial class MainPage : PhoneApplicationPage
    {
        private static List<string> _bases;
        private static List<string> _ciphers;

        static MainPage()
        {
            _bases = new List<string>();

            foreach (int i in Enumerable.Range(2, (36 - 2)))
            {
                string name = "base " + i;
                if (i == 2)
                {
                    name += " (binary)";
                }
                else if (i == 8)
                {
                    name += " (octal)";
                }
                else if (i == 10)
                {
                    name += " (decimal)";
                }
                else if (i == 16)
                {
                    name += " (hexadecimal)";
                }

                _bases.Add(name);
            }

            _ciphers = new List<string>();

            _ciphers.Add("Binary");
            _ciphers.Add("Hexadecimal");
            _ciphers.Add("ROT-13");
            _ciphers.Add("Five Letter");
            _ciphers.Add("Disemvowel");
        }

        #region Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = this;

            // Set the initial state of the pickers
            //SourceBaseListPicker.SelectedIndex = _bases.FindIndex(item => item.StartsWith("base 10"));
            //CipherListPicker.SelectedIndex = _ciphers.FindIndex(item => item.StartsWith("Hexadecimal"));
        }
        #endregion

        #region Properties
        public List<string> SourceBases
        {
            get
            {
                return _bases;
            }
        }

        public List<string> TargetBases
        {
            get
            {
                return _bases;
            }
        }

        public List<string> Ciphers
        {
            get
            {
                return _ciphers;
            }
        }
        #endregion

        #region Methods
        private void DoConversion()
        {
            try
            {
                int base10 = BaseConverter.ConvertBaseToBase10(SourceTextBox.Text, GetRadixFromPicker((string)SourceBaseListPicker.SelectedItem));
                if (base10 < 0)
                {
                    throw new ArgumentOutOfRangeException("Number to be converted is out of range.  Largest supported base-10 number is " + Int32.MaxValue + ".");
                }

                TargetTextBox.Text = BaseConverter.ConvertBase10ToBase(base10, GetRadixFromPicker((string)TargetBaseListPicker.SelectedItem));
            }
            catch (Exception anyException)
            {
                MessageBox.Show(anyException.Message, "Error", MessageBoxButton.OK);
            }
        }

        private int GetRadixFromPicker(string radixText)
        {
            Regex regex = new Regex(@"base (\d+)");
            Match match = regex.Match(radixText);
            if (match.Success)
            {
                return Int32.Parse(match.Groups[1].Value);
            }

            throw new InvalidOperationException("Unknown base value selected.");
        }
        #endregion

        #region Event Handlers
        private void OnConvertButtonClick(object sender, RoutedEventArgs e)
        {
            DoConversion();
        }

        private void OnSourceTextBoxKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                DoConversion();
            }
        }

        private void OnSwapButtonClick(object sender, RoutedEventArgs e)
        {
            int sourceIndex = SourceBaseListPicker.SelectedIndex;
            int targetIndex = TargetBaseListPicker.SelectedIndex;

            SourceBaseListPicker.SelectedIndex = targetIndex;
            TargetBaseListPicker.SelectedIndex = sourceIndex;
        }
        #endregion

        private void OnEncodeButtonClick(object sender, RoutedEventArgs e)
        {
            DoEncode();
        }

        private void OnDecodeButtonClick(object sender, RoutedEventArgs e)
        {
            DoDecode();
        }

        private void DoEncode()
        {
            try
            {
                ICipher codec = GetSelectedCipher();
                if (codec != null)
                {
                    CipherOutputTextBox.Text = codec.Encode(CipherInputTextBox.Text);
                }

            }
            catch (Exception anyException)
            {
                MessageBox.Show(anyException.Message, "Error", MessageBoxButton.OK);
            }
        }

        private void DoDecode()
        {
            try
            {
                ICipher codec = GetSelectedCipher();
                if (codec != null)
                {
                    CipherOutputTextBox.Text = codec.Decode(CipherInputTextBox.Text);
                }

            }
            catch (Exception anyException)
            {
                MessageBox.Show(anyException.Message, "Error", MessageBoxButton.OK);
            }
        }

        private ICipher GetSelectedCipher()
        {
            switch ((string)CipherListPicker.SelectedItem)
            {
                case "ROT-13":
                    return new RotationCipher(13);

                case "Binary":
                    return GetBinaryCipher();

                case "Hexadecimal":
                    return GetHexadecimalCipher();

                case "Stuffing":
                    return GetStuffingCipher();

                case "Disemvowel":
                    return GetDisemvowelCipher();

                default:
                    throw new InvalidOperationException("Unsupported cipher");
            }
        }

        private ICipher GetBinaryCipher()
        {
            AggregateCipher cipher = new AggregateCipher();
            cipher.AddCipher(new RadixCipher(2, 8));
            cipher.AddCipher(new GroupingCipher(8));

            return cipher;
        }

        private ICipher GetHexadecimalCipher()
        {
            AggregateCipher cipher = new AggregateCipher();
            cipher.AddCipher(new RadixCipher(16, 2));
            cipher.AddCipher(new GroupingCipher(2));

            return cipher;
        }

        private ICipher GetStuffingCipher()
        {
            AggregateCipher cipher = new AggregateCipher();
            cipher.AddCipher(new StuffingCipher(1));
            cipher.AddCipher(new GroupingCipher(5));

            return cipher;
        }

        private ICipher GetDisemvowelCipher()
        {
            MapCipher cipher = new MapCipher(new Func<char, bool, string>((c, encode) =>
            {
                char[] charsToRemove = { 'a', 'e', 'i', 'o', 'u' };

                if (encode && charsToRemove.Contains(Char.ToLower(c)))
                {
                    return string.Empty;
                }

                return c.ToString();
            }));

            return cipher;
        }

        private void OnClearInputButtonClick(object sender, RoutedEventArgs e)
        {
            CipherInputTextBox.Text = string.Empty;
        }

        private void OnCopyOutputButtonClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(CipherOutputTextBox.Text);
        }

        private void OnSendEmailButtonClick(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "Message from Enigma";
            emailComposeTask.Body = CipherOutputTextBox.Text;

            emailComposeTask.Show();
        }
    }
}