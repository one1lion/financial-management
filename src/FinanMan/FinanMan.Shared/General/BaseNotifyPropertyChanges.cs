using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FinanMan.Shared.General;

public abstract class BaseNotifyPropertyChanges : INotifyPropertyChanged, INotifyPropertyChanging
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event PropertyChangingEventHandler? PropertyChanging;

    protected void RaisePropertyChanged([CallerMemberName] string? propertyName = default)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void RaisePropertyChanging([CallerMemberName] string? propertyName = default)
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = default)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        RaisePropertyChanging(propertyName);
        field = value;
        RaisePropertyChanged(propertyName);
        return true;
    }
}
