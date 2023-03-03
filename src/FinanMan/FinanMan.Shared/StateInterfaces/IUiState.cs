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
    event Func<Task>? ActiveLanguageChanged;

    RenderFragment? FlyoutContent { get; set; }
    bool FlyoutVisible { get; set; }
    string ActiveLanguage { get; }

    void SetLanguage(string language);
}
