using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanMan.Shared.StateInterfaces;

public interface IUiState : INotifyPropertyChanged, INotifyPropertyChanging
{
    RenderFragment? FlyoutContent { get; set; }
    bool FlyoutVisible { get; set; }
}
