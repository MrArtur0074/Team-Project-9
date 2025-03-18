using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Project_9.Models;
using Project_9.Services;

namespace Project_9.ViewModels;

public class WingGeometryViewModel : ViewModelBase, INotifyDataErrorInfo
{
	private readonly Wing                             _wing;
	private readonly UndoRedoService                  _undoRedo;
	private readonly Dictionary<string, List<string>> _errors = new();

	public WingGeometryViewModel(Wing wing, UndoRedoService undoRedo) {
		_wing = wing;
		_undoRedo = undoRedo;
	}

	public string Name {
		get => _wing.Name;
		set => _undoRedo.Execute(new SetPropertyCommand<string>(_wing, nameof(Wing.Name), value));
	}

	public int Span {
		get => _wing.Span;
		set {
			try {
				_undoRedo.Execute(new SetPropertyCommand<int>(_wing, nameof(Wing.Span), value));
			}
			catch (Exception exception) {
				AddError(nameof(Span), exception.Message);
				return;
			}
			RemoveError(nameof(Span));
		}
	}

	public int RootChord {
		get => _wing.RootChord;
		set {
			try {
				_undoRedo.Execute(new SetPropertyCommand<int>(_wing, nameof(Wing.RootChord), value));
			}
			catch (Exception exception) {
				AddError(nameof(RootChord), exception.Message);
			}
			RemoveError(nameof(RootChord));
		}
	}
	
	public double IncidenceAngle {
		get => _wing.IncidenceAngle;
		set {
			try {
				_undoRedo.Execute(new SetPropertyCommand<double>(_wing, nameof(Wing.IncidenceAngle), value));
			}
			catch (Exception exception) {
				AddError(nameof(IncidenceAngle), exception.Message);
			}
			RemoveError(nameof(IncidenceAngle));
		}
	}
	
	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
	public bool HasErrors => _errors.Values.Any(errors => errors.Any());

	public IEnumerable GetErrors(string propertyName) {
		return _errors.TryGetValue(propertyName, out var errors) ? errors : null;
	}

	private void AddError(string propertyName, string error) {
		if (!_errors.ContainsKey(propertyName)) {
			_errors[propertyName] = [];
		}
		_errors[propertyName].Add(error);
		ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
	}

	private void RemoveError(string propertyName) {
		if (_errors.Remove(propertyName)) {
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}
	}
}