using System;
using System.Windows.Navigation;
using Facebook.Client;

namespace UniversityBelt.WindowsPhone
{
    public class CustomUriMapper : UriMapperBase
    {
        public override Uri MapUri(Uri uri)
        {
            //If URI is callback from facebook then go ot MainPage.xaml
            //If it's not then just go whereever the URI is pointing
            return !AppAuthenticationHelper.IsFacebookLoginResponse(uri) ?
                uri : new Uri("/MainPage.xaml", UriKind.Relative);
        }
    }
}