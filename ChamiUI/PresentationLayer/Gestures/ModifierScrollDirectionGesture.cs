using System.Windows.Input;
using ChamiUI.PresentationLayer.Gestures.Enums;

namespace ChamiUI.PresentationLayer.Gestures;

public class ModifierScrollDirectionGesture : MouseGesture
{
    public ModifierScrollDirectionGesture()
    {
    }

    public ModifierScrollDirectionGesture(MouseAction mouseAction, ModifierKeys modifierKeys) : base(mouseAction,
        modifierKeys)
    {
    }

    public ScrollDirection ScrollDirection { get; set; }

    public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
    {
        var baseMatches = base.Matches(targetElement, inputEventArgs);

        if (!baseMatches)
        {
            return false;
        }

        if (inputEventArgs is MouseWheelEventArgs mouseWheelEventArgs)
        {
            return ScrollDirection switch
            {
                ScrollDirection.None => mouseWheelEventArgs.Delta == 0,
                ScrollDirection.Down => mouseWheelEventArgs.Delta < 0,
                ScrollDirection.Up => mouseWheelEventArgs.Delta > 0,
                _ => false
            };
        }

        return false;
    }
}