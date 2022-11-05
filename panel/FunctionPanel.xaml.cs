using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
using ImageProcessor.Tab;
using Microsoft.Win32;
using OpenCvSharp;

namespace ImageProcessor.Panel
{
    /// <summary>
    /// FunctionPanel.xaml 的互動邏輯
    /// </summary>
    public partial class FunctionPanel : Control.CustomCard
    {
        #region Fields
        private TcpClient _tcpClient;
        #endregion

        #region Properties
        public EngineerTab EngineerTab { get; set; }

        public ObservableCollections.ObservableStack<int> ObservableStack { get; } = new ObservableCollections.ObservableStack<int>();

        public ObservableCollections.ObservableQueue<int> ObservableQueue { get; } = new ObservableCollections.ObservableQueue<int>();

        public ObservableCollections.ObservableDictionary<int, int> ObservableDictionary { get; } = new ObservableCollections.ObservableDictionary<int, int>();

        private object _locker = new object();
        #endregion

        public FunctionPanel()
        {
            InitializeComponent();
        }

        private void CustomCard_Loaded(object sender, RoutedEventArgs e)
        {
            //EngineerTab.save
            ObservableStack.CollectionChanged += ObservableStack_CollectionChanged;

            ObservableDictionary.CollectionChanged += ObservableDictionary_CollectionChanged;

            ObservableQueue.CollectionChanged += ObservableQueue_CollectionChanged;

            //BindingOperations.EnableCollectionSynchronization(ObservableStack, _locker);
            BindingOperations.EnableCollectionSynchronization(ObservableQueue, _locker);
        }

        private void ObservableQueue_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"{e.Action} {e.NewItems?[0]} {e.OldItems?[0]} {e.NewStartingIndex} {e.OldStartingIndex}");
        }

        private void ObservableDictionary_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"{e.Action} {e.NewItems?[0]} {e.OldItems?[0]} {e.NewStartingIndex} {e.OldStartingIndex}");
        }

        private void ObservableStack_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"{e.Action} {e.NewItems?[0]} {e.OldItems?[0]} {e.NewStartingIndex} {e.OldStartingIndex}");
        }

        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            Mat mat = EngineerTab.Indicator.Image;

            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                FileName = string.Empty,
                Filter = "BMP Image(*.bmp)|*.bmp",
                InitialDirectory = $@"{Directory.GetCurrentDirectory()}"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                Cv2.ImWrite(saveFileDialog.FileName, mat);
            }
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (_tcpClient == null)
            {
                _tcpClient = new TcpClient("127.0.0.1", 8016);
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            //_tcpClient.Close();

            //return;

            if (_tcpClient != null)
            {
                NetworkStream networkStream = _tcpClient.GetStream();

                byte[] data = Encoding.UTF8.GetBytes("test");

                networkStream.Write(data, 0, data.Length);
                //System.Diagnostics.Debug.WriteLine($"{DateTime.Now:mm:ss.fff} 123");

                data = new byte[256];

                int i = networkStream.Read(data, 0, data.Length);

                string str = Encoding.UTF8.GetString(data, 0, i);

                System.Diagnostics.Debug.WriteLine($"Str: {str}");

                //networkStream.Close();
                //_tcpClient.Close();
            }
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                for (int i = 0; i <= 20; i++)
                {
                    if (i == 20)
                    {
                        ObservableQueue.Clear();
                        break;
                    }

                    if (ObservableQueue.Count > 5)
                    {
                        int j = ObservableQueue.Dequeue();
                        System.Diagnostics.Debug.WriteLine($"Dequeue: {j}");
                    }
                    else
                    {
                        ObservableQueue.Enqueue(i);
                    }
                    SpinWait.SpinUntil(() => false, 500);
                }
            });

            //Task.Run(() =>
            //{
            //    for (int i = 0; i <= 20; i++)
            //    {
            //        if (i == 20)
            //        {
            //            ObservableDictionary.Clear();
            //            break;
            //        }

            //        System.Diagnostics.Debug.WriteLine($"{ObservableDictionary.ContainsKey(i % 3)} {i % 3}");

            //        if (!ObservableDictionary.ContainsKey(i % 3))
            //        {
            //            ObservableDictionary.Add(i % 3, i);
            //        }
            //        else
            //        {
            //            ObservableDictionary.Remove(i % 3);
            //        }
            //        SpinWait.SpinUntil(() => false, 200);
            //    }

            //    ObservableDictionary[0] = 2;
            //    SpinWait.SpinUntil(() => false, 200);
            //    ObservableDictionary[0] = 3;
            //    SpinWait.SpinUntil(() => false, 200);
            //});
        }
    }
}
