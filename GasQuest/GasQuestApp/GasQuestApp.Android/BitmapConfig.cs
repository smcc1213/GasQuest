using GasQuestApp.Droid;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Android.Factories;
using AndroidBitmapDescriptor = Android.Gms.Maps.Model.BitmapDescriptor;
using AndroidBitmapDescriptorFactory = Android.Gms.Maps.Model.BitmapDescriptorFactory;

namespace GasQuestApp.Droid
{
    public sealed class BitmapConfig : IBitmapDescriptorFactory
    {
        public AndroidBitmapDescriptor ToNative(BitmapDescriptor descriptor)
        {
            int iconId = 0;
            switch (descriptor.Id)
            {
                case "gas_station":
                    iconId = Resource.Drawable.gas_station;
                    break;
            }
            return AndroidBitmapDescriptorFactory.FromResource(iconId);
        }

    }
}