using MounterApp.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace MounterApp.Helpers {
    public class FrameColorConverter : IValueConverter {
        public object Convert(object value,Type targetType,object parameter,CultureInfo culture) {
            //throw new NotImplementedException(); 
            if(value != null) {
                NewServiceorderExtensionBase obj = value as NewServiceorderExtensionBase;
                if(obj.NewIncome != null)
                    return Color.Yellow;
                else
                    return Color.White;
            }
            else
                return Color.White;
        }

        public object ConvertBack(object value,Type targetType,object parameter,CultureInfo culture) {
            return Color.White;
        }
    }
}
