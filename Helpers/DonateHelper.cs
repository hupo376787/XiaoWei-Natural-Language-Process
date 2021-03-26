using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Store;
using XiaoweiNLP.Controls;

namespace XiaoweiNLP.Helpers
{
    public class DonateHelper
    {
        //static StoreContext context = null;
        public static void GetAddOnInfoAndShowDonateWindow()
        {
            //if (context == null)
            //{
            //    context = StoreContext.GetDefault();
            //    // If your app is a desktop app that uses the Desktop Bridge, you
            //    // may need additional code to configure the StoreContext object.
            //    // For more info, see https://aka.ms/storecontext-for-desktop.
            //}

            //// Specify the kinds of add-ons to retrieve.
            //string[] productKinds = { "Durable" };
            //List<String> filterList = new List<string>(productKinds);

            //StoreProductQueryResult queryResult = await context.GetAssociatedStoreProductsAsync(productKinds);

            //if (queryResult.ExtendedError != null)
            //{
            //    // The user may be offline or there might be some other server failure.
            //    //return;
            //}

            DonateControl msgPopup = new DonateControl();
            msgPopup.ShowWindow();
        }
    }
}
