using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace Project_9.ViewModels;

public class ViewModelBase : ReactiveObject, INotifyPropertyChanged
{
	public new event PropertyChangedEventHandler? PropertyChanged;

	protected void RaiseAndSetIfChanged<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
		if (!EqualityComparer<T>.Default.Equals(field, value)) {
			field = value;
			RaisePropertyChanged(propertyName);
		}
	}

	protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}