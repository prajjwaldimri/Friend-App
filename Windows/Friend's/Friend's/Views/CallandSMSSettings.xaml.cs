using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Friend_s
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CallandSmsSettings : Page
    {
        public CallandSmsSettings()
        {
            InitializeComponent();
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            DatabaseConnector();
            await ThumbnailRetriever();
        }

        private async Task ThumbnailRetriever()
        {
            for(var i=0;i<5;i++)
            try
            {
                switch (i)
                {
                    case 0:
                    {
                        var image1 = new BitmapImage();
                        var storagefile =
                            await ApplicationData.Current.RoamingFolder.GetFileAsync("firstcontactthumb.jpg");
                        if (storagefile == null) return;
                        var stream = await storagefile.OpenAsync(FileAccessMode.Read);
                        //Converts to a IRandomAccessStream that can be set to an image
                        await image1.SetSourceAsync(stream);
                        FirstEllipseimg.ImageSource = image1;
                    }
                        break;
                    case 1:
                    {
                        var image1 = new BitmapImage();
                        var storagefile =
                            await ApplicationData.Current.RoamingFolder.GetFileAsync("secondcontactthumb.jpg");
                        if (storagefile == null) return;
                        var stream = await storagefile.OpenAsync(FileAccessMode.Read);
                        //Converts to a IRandomAccessStream that can be set to an image
                        await image1.SetSourceAsync(stream);
                        SecondEllipseimg.ImageSource = image1;
                    }
                        break;
                    case 2:
                    {
                        var image1 = new BitmapImage();
                        var storagefile =
                            await ApplicationData.Current.RoamingFolder.GetFileAsync("thirdcontactthumb.jpg");
                        if (storagefile == null) return;
                        var stream = await storagefile.OpenAsync(FileAccessMode.Read);
                        //Converts to a IRandomAccessStream that can be set to an image
                        await image1.SetSourceAsync(stream);
                        ThirdEllipseimg.ImageSource = image1;
                    }
                        break;
                    case 3:
                    {
                        var image1 = new BitmapImage();
                        var storagefile =
                            await ApplicationData.Current.RoamingFolder.GetFileAsync("forthcontactthumb.jpg");
                        if (storagefile == null) return;
                        var stream = await storagefile.OpenAsync(FileAccessMode.Read);
                        //Converts to a IRandomAccessStream that can be set to an image
                        await image1.SetSourceAsync(stream);
                        ForthEllipseimg.ImageSource = image1;
                    }
                        break;
                    case 4:
                    {
                        var image1 = new BitmapImage();
                        var storagefile =
                            await ApplicationData.Current.RoamingFolder.GetFileAsync("fifthcontactthumb.jpg");
                        if (storagefile == null) return;
                        var stream = await storagefile.OpenAsync(FileAccessMode.Read);
                        //Converts to a IRandomAccessStream that can be set to an image
                        await image1.SetSourceAsync(stream);
                        FirstEllipseimg.ImageSource = image1;
                    }
                        break;
                }
                }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private void DatabaseConnector()
        {
            try
            {
                var arr = CreateDatabase.GetValues(@"SELECT Number FROM Details");
                var brr = CreateDatabase.GetValues(@"SELECT Name FROM Details");
                FirstContactName.Text = brr[0];
                FirstContactNumber.Text = arr[0];
                SecondContactName.Text = brr[1];
                SecondContactNumber.Text = arr[1];
                ThirdContactName.Text = brr[2];
                ThirdContactNumber.Text = arr[2];
                ForthContactName.Text = brr[3];
                ForthContactNumber.Text = arr[3];
                FifthContactName.Text = brr[4];
                FifthContactNumber.Text = arr[4];
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        

        private async void ContactSetter(Contact contact, TextBlock contactnameBlock,
            TextBlock contactnumberBlock, ImageBrush ellipseImageBrush,string id, string filename)
        {
            var name = contact.FullName;
            var number = SpineClass.GetContactResult(contact);
            contactnameBlock.Text = name;
            contactnumberBlock.Text = number;

            CreateDatabase.InsertData(id,name,number);
            CreateDatabase.UpdateData(id,name,number);

            //Have to get these two permissions or the System returns null on the thumbnails
            var contactStore = await ContactManager.RequestStoreAsync();
            var contact2 = await contactStore.GetContactAsync(contact.Id);
            if (contact2.Thumbnail == null) return;
            var stream = await contact2.Thumbnail.OpenReadAsync();
            if (stream == null || stream.Size <= 0) return;
            var bmp = new BitmapImage();
            await bmp.SetSourceAsync(stream);
            ellipseImageBrush.ImageSource = bmp;
            
            SpineClass.ImagetoIsolatedStorageSaver(stream,filename);
        }

        private async void FirstContactButton_OnClick(object sender, RoutedEventArgs e)
        {
            var contactPicker = new ContactPicker();
            // Ask the user to pick contact phone numbers.
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
            var contacts = await contactPicker.PickContactAsync();
            ContactSetter(contacts,FirstContactName,FirstContactNumber,FirstEllipseimg,"1","firstcontactthumb.jpg");
        }

        private async void SecondContactButton_OnClick(object sender, RoutedEventArgs e)
        {
            var contactPicker = new ContactPicker();
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
            var contacts = await contactPicker.PickContactAsync();
            ContactSetter(contacts, SecondContactName, SecondContactNumber, SecondEllipseimg,"2","secondcontactthumb.jpg");
        }

        private async void ThirdContactButton_OnClick(object sender, RoutedEventArgs e)
        {
            var contactPicker = new ContactPicker();
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
            var contacts = await contactPicker.PickContactAsync();
            ContactSetter(contacts, ThirdContactName, ThirdContactNumber, ThirdEllipseimg,"3","thirdcontactthumb.jpg");
        }

        private async void ForthContactButton_OnClick(object sender, RoutedEventArgs e)
        {
            var contactPicker = new ContactPicker();
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
            var contacts = await contactPicker.PickContactAsync();
            ContactSetter(contacts, ForthContactName,ForthContactNumber,ForthEllipseimg,"4","forthcontactthumb.jpg");
        }

        private async void FifthContactButton_OnClick(object sender, RoutedEventArgs e)
        {
            var contactPicker = new ContactPicker();
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
            var contacts = await contactPicker.PickContactAsync();
            ContactSetter(contacts, FifthContactName,FifthContactNumber,FifthEllipseimg,"5","fifthcontactthumb.jpg");
        }
    }
}
