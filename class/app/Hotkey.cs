using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Controls;
using System;
using ImageProcessor.Tab;
using System.Globalization;

namespace ImageProcessor
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Always Can Excute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanExcute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void MainTabCanExcute(object sender, CanExecuteRoutedEventArgs e)
        {
            // MainTab is focused
            e.CanExecute = OnNavIndex == 0;
        }

        private void DeviceTabCanExcute(object sender, CanExecuteRoutedEventArgs e)
        {
            // DeviceTab is focused
            e.CanExecute = OnNavIndex == 1;
        }

        private void MotionTabCanExcute(object sender, CanExecuteRoutedEventArgs e)
        {
            // MotionTab is focused
            e.CanExecute = OnNavIndex == 2;
        }

        private void DatabaseTabCanExcute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = OnNavIndex == 3;
        }

        private void EngineerTabCanExcute(object sender, CanExecuteRoutedEventArgs e)
        {
            // EngineerTab is focused
            e.CanExecute = OnNavIndex == 4;
        }

#if false
        //private void MinCommand(object sender, ExecutedRoutedEventArgs e)
        //{
        //    Minbtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        //}

        //private void MaxCommand(object sender, ExecutedRoutedEventArgs e)
        //{
        //    Maxbtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        //}

        //private void QuitCommand(object sender, ExecutedRoutedEventArgs e)
        //{
        //    //Debug.WriteLine(BaslerCam.IsConnected);
        //    //foreach (var item in BaslerCams)
        //    //{
        //    //    Debug.WriteLine($"Connected {item.IsConnected}");
        //    //}
        //    Quitbtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        //}  
#endif

        private void OpenDeviceCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Grabber 不為啟動狀態 // CamSelector 有選擇相機
            //switch (OnTabIndex)
            //{
            //    case 3:
            //        e.CanExecute = EngineerTab.CamConnect.IsEnabled;
            //        break;
            //    default:
            //        break;
            //}
            e.CanExecute = OnNavIndex switch
            {
                4 => EngineerTab.CamConnect.IsEnabled,
                _ => false
            };
            //e.CanExecute = CamConnect.IsEnabled;
        }
        private void OpenDeviceCommand(object sender, ExecutedRoutedEventArgs e)
        {
            //EngineerTab tab = FindName("EngineerTab") as EngineerTab;
            EngineerTab.CamConnect.IsChecked = (bool)EngineerTab.CamConnect.IsChecked ? false : true;
            //tab.CamConnect.IsChecked = (bool)EngineerTab.CamConnect.IsChecked ? false : true;

            //if ((bool)EngineerTab.CamConnect.IsChecked)
            //{
            //    EngineerTab.CamConnect.IsChecked = false;
            //}
            //else
            //{
            //    EngineerTab.CamConnect.IsChecked = true;
            //}

            //switch (OnTabIndex)
            //{
            //    case 3: // Engineer Tab
            //        if ((bool)EngineerTab.CamConnect.IsChecked)
            //        {
            //            EngineerTab.CamConnect.IsChecked = false;
            //        }
            //        else
            //        {
            //            EngineerTab.CamConnect.IsChecked = true;
            //        }
            //        break;
            //    default:
            //        break;
            //}
        }

        [Obsolete("不須另外啟動")]
        private void StartGrabberCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // 相機已開啟
            // e.CanExecute = camera != null && camera.IsOpen;
            e.CanExecute = false;   // 此功能已過時
        }

        //private void StartGrabberCommand(object sender, ExecutedRoutedEventArgs e)
        //{
        //    GrabberStartBtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        //}
        private void SingleShotCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = OnNavIndex switch
            {
                4 => EngineerTab.SingleShot.IsEnabled,
                _ => false,
            };
        }

        private void SingleShotCommand(object sender, ExecutedRoutedEventArgs e)
        {
            EngineerTab.SingleShot.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            //switch (OnTabIndex)
            //{
            //    case 3:
            //        EngineerTab.SingleShot.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            //        break;
            //    default:
            //        break;
            //}
            //SingleShot.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void ContinousShotCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = OnNavIndex switch
            {
                4 => EngineerTab.ContinouseShot.IsEnabled,
                _ => false,
            };
            //e.CanExecute = ContinouseShot.IsEnabled;
        }

        private void ContinousShotCommand(object sender, ExecutedRoutedEventArgs e)
        {
            EngineerTab.ContinouseShot.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            //switch (OnTabIndex)
            //{
            //    case 3:
            //        EngineerTab.ContinouseShot.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            //        break;
            //    default:
            //        break;
            //}
            //ContinouseShot.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void ToggleStreamGrabberCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = OnNavIndex switch
            {
                4 => EngineerTab.ToggleStreamGrabber.IsEnabled,
                _ => false
            };
        }

        private void ToggleStreamGrabberCommand(object sender, ExecutedRoutedEventArgs e)
        {
            EngineerTab.ToggleStreamGrabber.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void RetrieveImageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = OnNavIndex switch
            {
                4 => EngineerTab.RetrieveImage.IsEnabled,
                _ => false
            };
        }

        private void RetrieveImageCommand(object sender, ExecutedRoutedEventArgs e)
        {
            EngineerTab.RetrieveImage.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void CrosshairOnCommnad(object sender, ExecutedRoutedEventArgs e)
        {
            EngineerTab.Crosshair.Enable = !EngineerTab.Crosshair.Enable;
        }

        public void AssisRectOnCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (!EngineerTab.AssistRect.IsMouseDown)
            {
                EngineerTab.AssistRect.Enable = !EngineerTab.AssistRect.Enable;

                EngineerTab.AssistPoints.Enable = false;
            }
        }

        public void AssistPointsOnCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (!EngineerTab.AssistPoints.IsMouseDown)
            {
                EngineerTab.AssistPoints.Enable = !EngineerTab.AssistPoints.Enable;

                EngineerTab.AssistRect.Enable = false;
            }
        }

        /// <summary>
        /// Switch Tab with parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchTabCommand(object sender, ExecutedRoutedEventArgs e)
        {
            byte idx = byte.Parse(e.Parameter as string, CultureInfo.CurrentCulture);

            idx = (byte)(idx == 255 ? AppTabControl.Items.Count - 1 : idx); 

            // 確保 idx 不會超過 TabItems 數目 // 確保 Enable // 確保 Content 不為 null
            OnNavIndex =
                idx < AppTabControl.Items.Count &&
                (AppTabControl.Items[idx] as TabItem).IsEnabled &&
                (AppTabControl.Items[idx] as TabItem).Content != null ? idx : OnNavIndex;
        }

        private void GlobalTest(object sender, ExecutedRoutedEventArgs e)
        {
            Debug.WriteLine($"{OnNavIndex}");

            Debug.WriteLine($"{EngineerTab.CamSelector.Visibility}");
            Debug.WriteLine($"{EngineerTab.SingleShot.Visibility}");
        }
    }
}
