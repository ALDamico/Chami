using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using ChamiUI.PresentationLayer.Factories;
using Microsoft.Xaml.Behaviors;

namespace ChamiUI.PresentationLayer.Behaviours;

class ProgressBarAnimationBehavior : Behavior<ProgressBar>
{
    private bool _isAnimating;

    protected override void OnAttached()
    {
        base.OnAttached();
        var progressBar = AssociatedObject;
        progressBar.ValueChanged += ProgressBar_ValueChanged;
    }

    private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_isAnimating)
            return;

        _isAnimating = true;

        DoubleAnimation doubleAnimation = new DoubleAnimation
            (e.OldValue, e.NewValue, DurationFactory.FromMilliseconds(250), FillBehavior.Stop);
        doubleAnimation.Completed += Db_Completed;

        ((ProgressBar)sender).BeginAnimation(RangeBase.ValueProperty, doubleAnimation);

        e.Handled = true;
    }

    private void Db_Completed(object sender, EventArgs e)
    {
        _isAnimating = false;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        ProgressBar progressBar = this.AssociatedObject;
        progressBar.ValueChanged -= ProgressBar_ValueChanged;
    }
}