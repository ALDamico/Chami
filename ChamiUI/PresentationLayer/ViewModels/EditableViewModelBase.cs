using System;
using System.ComponentModel;
using System.Reflection;

namespace ChamiUI.PresentationLayer.ViewModels;

public abstract class EditableViewModelBase : ViewModelBase, IEditableObject, ICloneable
{
    public bool ChangeInProgress { get; private set; }
    protected EditableViewModelBase MementoObject;
    public void BeginEdit()
    {
        ChangeInProgress = true;
        MementoObject = (EditableViewModelBase) Clone();
    }

    public void CancelEdit()
    {
        var properties = GetType().GetProperties(BindingFlags.Public);
        foreach (var property in properties)
        {
            property.SetValue(this, property.GetValue(MementoObject));
        }

        ChangeInProgress = false;
        MementoObject = null;
    }

    public void EndEdit()
    {
        ChangeInProgress = false;
        MementoObject = null;
    }

    public virtual object Clone()
    {
        var newObject = Activator.CreateInstance(GetType());
        var properties = newObject.GetType().GetProperties(BindingFlags.Public|BindingFlags.SetProperty);
        foreach (var property in properties)
        {
            property.SetValue(newObject, property.GetValue(this));
        }

        return newObject;
    }
}