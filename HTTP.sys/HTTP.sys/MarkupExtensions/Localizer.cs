using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
namespace HTTP.sys.MarkupExtensions
{
    [ContentProperty("Name")]
    public sealed class Localizer:IMarkupExtension
    {
        public string Name { get; set; }
        public object ProvideValue(IServiceProvider provider)
        {
            if(Resource.Culture is null)
            {
                Resource.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
            return Resource.ResourceManager.GetString(Name);
        }
    }
}
