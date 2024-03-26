using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WPF.ComponentBindingEnum
{
    partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        PlotModelType modelType;
    }
}
