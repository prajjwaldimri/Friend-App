//Only for production purposes. Should not be edited. 
// However if you want to check your API Keys create two files named TwitterConsumerKey and TwitterConsumerSecret and add your API Keys

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace BeFriend
{
    class AuthTokens
    {

        public static string TwitterConsumerKey;
        public static string TwitterConsumerSecret;
        public static string FacebookAppID = "1831229747103982";

        public static async Task KeyRetriever()
        {
            try
            {
                var consumerKey = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///TwitterConsumerKey.txt"));
                using (var sRead = new StreamReader(await consumerKey.OpenStreamForReadAsync()))
                    TwitterConsumerKey = await sRead.ReadToEndAsync();
                var consumerSecret = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///TwitterConsumerSecret.txt"));
                using (var sRead = new StreamReader(await consumerSecret.OpenStreamForReadAsync()))
                    TwitterConsumerSecret = await sRead.ReadToEndAsync();
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("No API Keys Found. Please check AuthTokens class for further details");
            }
            
        }
    }
}
