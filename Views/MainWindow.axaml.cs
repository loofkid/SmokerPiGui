using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using JetBrains.Annotations;
using ReactiveUI;
using SmokerPiGui.Converters;

namespace SmokerPiGui.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
#if !DEBUG
            Cursor = new Cursor(StandardCursorType.None);
            ExtendClientAreaToDecorationsHint = true;
#endif
            // System.IObservable<IObservedChange<Avalonia.Platform.Screen, int>>? WindowHeight = Screens.Primary.ObservableForProperty(s => s.Bounds.Height);
            // System.IObservable<IObservedChange<Avalonia.Platform.Screen, int>>? WindowWidth = Screens.Primary.ObservableForProperty(s => s.Bounds.Width);

            // WindowHeight.Subscribe(s => NumberPanel.Height = s.Value * 0.8);
            // WindowWidth.Subscribe(s => NumberPanel.Width = s.Value * 0.5);
        }

        public RelativePanel NumberPanel => this.FindControl<RelativePanel>("NumberPanel");

        public void CloseWindow(object sender, RoutedEventArgs args)
        {
            Close();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}