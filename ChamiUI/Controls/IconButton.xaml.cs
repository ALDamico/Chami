using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AsyncAwaitBestPractices.MVVM;
using MahApps.Metro.IconPacks;

namespace ChamiUI.Controls;

public partial class IconButton : UserControl
{
    public IconButton()
    {
        InitializeComponent();
    }

    public PackIconFontAwesomeKind Icon
    {
        get => (PackIconFontAwesomeKind) GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public Dock IconPosition
    {
        get => (Dock) GetValue(IconPositionProperty);
        set => SetValue(IconPositionProperty, value);
    }

    public SolidColorBrush IconColor
    {
        get
        {
            if (!IsEnabled)
            {
                return Brushes.DimGray;
            }

            return (SolidColorBrush) GetValue(IconColorProperty);
        }
        set => SetValue(IconColorProperty, value);
    }

    public string ButtonText
    {
        get => (string) GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }
    
    public IAsyncCommand ClickCommand
    {
        get => (IAsyncCommand) GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly DependencyProperty IconPositionProperty =
        DependencyProperty.Register("IconPosition", typeof(Dock), typeof(IconButton), new PropertyMetadata(Dock.Left, OnIconPositionChanged));

    public static readonly DependencyProperty IconColorProperty =
        DependencyProperty.Register("IconColor", typeof(SolidColorBrush), typeof(IconButton), new PropertyMetadata(Brushes.Black, OnIconColorChanged));
    public static readonly DependencyProperty ButtonTextProperty =
        DependencyProperty.Register("ButtonText", typeof(string), typeof(IconButton), new PropertyMetadata(null, OnButtonTextChanged));
    
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(name: "Icon", typeof(PackIconFontAwesomeKind), typeof(IconButton), new PropertyMetadata(default(PackIconFontAwesomeKind), OnIconPropertyChanged));

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("CommandName",
        typeof(IAsyncCommand), typeof(IconButton), new PropertyMetadata(null, OnCommandChanged));

    private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is IconButton iconButton)
        {
            iconButton.OnCommandChanged(e);
        }
    }

    private void OnCommandChanged(DependencyPropertyChangedEventArgs e)
    {
        var command = (IAsyncCommand) e.NewValue;
        this.ClickCommand = command;
        ButtonElement.Command = command;
    }

    private static void OnIconColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is IconButton iconButton)
        {
            iconButton.OnIconColorChanged(e);
        }
    }

    private void OnIconColorChanged(DependencyPropertyChangedEventArgs e)
    {
        var value = (SolidColorBrush) e.NewValue;
        IconColor = value;
        IconElement.Foreground = value;
    }

    

    private static void OnButtonTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is IconButton iconButton)
        {
            iconButton.OnButtonTextChanged(e);
        }
    }

    private void OnButtonTextChanged(DependencyPropertyChangedEventArgs e)
    {
        var value = (string) e.NewValue;
        ButtonText = value;
        TextBlockElement.Text = value;
    }


    private static void OnIconPositionChanged(DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs args)
    {
        if (dependencyObject is IconButton iconButton)
        {
            iconButton.OnIconPositionChanged(args);
        }
    }

    private void OnIconPositionChanged(DependencyPropertyChangedEventArgs args)
    {
        IconPosition = (Dock) args.NewValue;
        DockPanel.SetDock(IconElement, IconPosition);
    }
    
    private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is IconButton iconButton)
        {
            iconButton.OnIconPropertyChanged(e);
        }
    }

    private void OnIconPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        var value =(PackIconFontAwesomeKind) e.NewValue;
        Icon = value;
        IconElement.Kind = value;
    }

    private void IconButton_OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        IconElement.Foreground = IconColor;
    }

    public SolidColorBrush GetActualForegroundColor()
    {
        return (SolidColorBrush) GetValue(IconColorProperty);
    }
}