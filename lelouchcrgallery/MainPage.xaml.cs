using lelouchcrgallery.page;
using lelouchcrgallery.proxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.System.UserProfile;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Networking.Connectivity;
using Windows.Graphics.Imaging;
using Windows.Graphics.Display;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace lelouchcrgallery
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //  private object information;

        public MainPage()
        {
            this.InitializeComponent();
            // onetext.Text = "请按下刷新按钮获取图片";
            //和网络结♂合失败⁄(⁄ ⁄•⁄ω⁄•⁄ ⁄)⁄
            if (thephoto != null)
            {
                ring.IsActive = false;
                ring.Visibility = Visibility.Collapsed;
                onetext.Text = "请按下刷新按钮获取图片";
            }
        }
        private void Listboxmenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (onepicture.IsSelected)
            {
                //myframe.Navigate(typeof(tangqiang));不存在
                mynemu.IsPaneOpen = !mynemu.IsPaneOpen;
            }

            else if (huandeng.IsSelected)
            {
                myframe.Navigate(typeof(allold));
                mynemu.IsPaneOpen = !mynemu.IsPaneOpen;

            }
        }

        private async void fresh_Click(object sender, RoutedEventArgs e)
        {
             if (thephoto != null)
            {
                RootObject myimage = await pixivproxy.Getimage();
                ring.IsActive = false;
                ring.Visibility = Visibility.Collapsed;
                twotext.Text = "高度" + myimage.p_ori_hight + "宽度" + myimage.p_ori_width;
                // BitmapImage貌似是用来接收uri来转成图片的，死国一得死
                BitmapImage bitmapImage = new BitmapImage(new Uri(myimage.p_ori));
                thephoto.Source = bitmapImage;
            }
             else
            {
                ring.IsEnabled = true; 
                ring.Visibility = Visibility.Visible;
            }  
           
        }

        private void hanbao_Click(object sender, RoutedEventArgs e)
        {
            mynemu.IsPaneOpen = !mynemu.IsPaneOpen;
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            myframe.Navigate(typeof(seting));
        }

        private async void ShowMessageDialog()
        {
            var msgDialog = new Windows.UI.Popups.MessageDialog("失败") { Title = "交易中断，来晚了吗？ #贝多芬.jpg" };
            msgDialog.Commands.Add(new Windows.UI.Popups.UICommand("确定"));
            msgDialog.Commands.Add(new Windows.UI.Popups.UICommand("取消"));
            await msgDialog.ShowAsync();
        }

        private async  void Download_Click(object sender, RoutedEventArgs e)
        {

             RootObject myimage = await pixivproxy.Getimage();
            var saveFile = new FileSavePicker();
            saveFile.SuggestedStartLocation = PickerLocationId.PicturesLibrary; //下拉列表的文件类型
            int zijia = 1;
            string  filename = "老婆们"+zijia ++;
            saveFile .FileTypeChoices.Add (filename , new List<string >() { ".png", ".jpg", ".jpeg", ".bmp" }); //文件命名，图片+数字自加。。。有机会换成获取api返回的id试试
           
            string filenam = myimage.p_mid ;
            int ss = 1;
            ss++;
            saveFile.SuggestedFileName = ss+filenam;
            StorageFile sFile = await saveFile.PickSaveFileAsync();

            if (sFile != null)
            {
                // 在用户完成更改并调用CompleteUpdatesAsync之前，阻止对文件的更新，此方法参考了http://blog.csdn.net/csdn_ergo/article/details/51281093的博客
                CachedFileManager.DeferUpdates(sFile);
                //image控件转换图像
                RenderTargetBitmap renderTargerBitemap = new RenderTargetBitmap();
                //传入image控件
                await renderTargerBitemap.RenderAsync(thephoto);

                var pixelBuffer = await renderTargerBitemap.GetPixelsAsync();
                //下面这段不明所以的说
                using (var fileStream = await sFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, fileStream);
                    encoder.SetPixelData(
                        BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Ignore,
                         (uint)renderTargerBitemap.PixelWidth,
                         (uint)renderTargerBitemap.PixelHeight,
                         DisplayInformation.GetForCurrentView().LogicalDpi,
                         DisplayInformation.GetForCurrentView().LogicalDpi,
                         pixelBuffer.ToArray()
                        );
                    //刷新图像？
                    await encoder.FlushAsync();
                }

            }
            else
            {
                ShowMessageDialog();
            }
               
            
    }

        //未完成


        /*     private async  void homeimg_Click(object sender, RoutedEventArgs e)
             {
                 RootObject myimage = await pixivproxy.Getimage();
                 UserProfilePersonalizationSettings seting = UserProfilePersonalizationSettings.Current; //实例化对象
                 bool p = await seting.TrySetLockScreenImageAsync(thephoto.Source);
             }
       
        public async static Task<IRandomAccessStream> SaveImage(IInputStream steam, string fileName)
        {
            try
            {
                StorageFile file = await tempData.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                using (var sourceInputStream = steam)
                {
                    using (var destinationStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        using (var destinationOutputStream = destinationStream.GetOutputStreamAt(0))
                        {
                            await RandomAccessStream.CopyAndCloseAsync(sourceInputStream, destinationStream);
                        }
                    }
                }
                return await file.OpenReadAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 */
    }
}
