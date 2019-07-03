using Cryptography.Properties;
using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Cryptography
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        #region "モデル"
        /// <summary>
        /// 暗号化キー
        /// </summary>
        public string key { get; set; }

        /// <summary>
        ///  ベクトル
        /// </summary>
        public string iv { get; set; }

        /// <summary>
        ///  バリュー
        /// </summary>
        public string value { get; set; }

        /// <summary>
        ///  ファイル名
        /// </summary>
        public string filePath { get; set; }
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            key = Settings.Default.key;
            iv = Settings.Default.iv;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        /// <summary>
        /// オープンのボタンのクリックイベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void openBtn_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            var result = fileDialog.ShowDialog();
            if (result == true)
            {
                filePath = fileDialog.FileName;

                try
                {
                    value = AesCrypto.Decrypt(File.ReadAllText(filePath), key, iv);
                    PropertyChanged(this, new PropertyChangedEventArgs("value"));
                    saveBtn.IsEnabled = true;

                    MessageBox.Show("復号化処理が成功しました。", "復号化", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch
                {
                    MessageBox.Show("復号化処理が失敗しました。", "復号化", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// 保存のボタンのクリックイベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            // パスが指定されない場合、選択する
            if (string.IsNullOrWhiteSpace(filePath))
            {
                var sfd = new SaveFileDialog();
                if (sfd.ShowDialog() != true)
                {
                    return;
                }

                filePath = sfd.FileName;
            }

            try
            {
                var encryptedText = AesCrypto.Encrypt(value, key, iv);
                File.WriteAllText(filePath, encryptedText);

                MessageBox.Show("暗号化処理が成功しました。", "暗号化", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch
            {
                MessageBox.Show("暗号化処理が失敗しました。", "暗号化", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// テストチェンジイベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void textArea_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textArea.Text) || !string.IsNullOrWhiteSpace(filePath))
            {
                saveBtn.IsEnabled = true;
            }
            else
            {
                saveBtn.IsEnabled = false;
            }
        }
    }
}
