using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using converter.Global;
using converter.Services;
using converter.ViewModels;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static converter.ViewModels.ViewModelBase;
using System.Threading.Tasks;

namespace converter.Views
{
    public partial class MainWindow : Window
    {
        #region Services
        private readonly FileBearbeitungService _fileService = new();
        #endregion

        #region Variables
        private ObservableCollection<Format> _formats;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Macht das Fenster blein und stellt bas Fenster Unten Rechts fest
        /// </summary>
        /// <param name="e"></param>
        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);

            //Window Einstellungen
            WindowState = WindowState.Normal;
            Width = 300;
            Height = 450;

            var screen = Screens.Primary;
            var screenBounds = screen.Bounds;

            var newX = screenBounds.Width - (int)Width - 50;
            var newY = screenBounds.Height - (int)Height - 100;

            Position = new PixelPoint(newX, newY);

            CanResize = false;
        }

        /// <summary>
        /// Kriegen und bearbeiten die Dateien
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_Drop(object? sender, DragEventArgs e)
        {
            var fileNames = e.Data.GetFileNames();
            if (fileNames is not null)
            {
                foreach (var fileName in fileNames)
                {
                    //prüfen ob das Dateiformat unterschtützt ist
                    bool isFormatunterschtützt = _fileService.GetFileFormat(fileName);
                    

                    if (isFormatunterschtützt)
                    {
                        //ändern die Text File nach dem Drop
                        if (DataContext is MainWindowViewModel vm)
                        {
                            vm.IsFileDropped = Path.GetFileName(fileName);

                            GlobalVariables.FilePath = fileName;

                        }
                        return;
                    }
                    else
                    {
                        //ändern die Text File nach dem Drop
                        if (DataContext is MainWindowViewModel vm)
                        {
                            vm.IsFileDropped = "Dateiformat ist nicht unterstützt!";
                        }

                        return;
                    }
                }
            }
            e.Handled = true;
        }

       
    }
}