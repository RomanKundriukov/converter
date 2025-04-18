
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using converter.Global;
using converter.Services;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using Tmds.DBus.Protocol;
using System.Threading.Tasks;
using System.IO;


namespace converter.ViewModels
{
    /// <summary>
    /// ViewModel für das Hauptfenster
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Variables
        /// <summary>
        /// Enum für die Dateiformate
        /// </summary>
        private ObservableCollection<Format> _formats;
        public ObservableCollection<Format> Formats
        {
            get => _formats;
            set => this.RaiseAndSetIfChanged(ref _formats, value);
        }

        /// <summary>
        /// Aktuell ausgewähltes Format From
        /// </summary>
        private Format _selectedFormatFrom;
        public Format SelectedFormatFrom
        {
            get => _selectedFormatFrom;
            set => this.RaiseAndSetIfChanged(ref _selectedFormatFrom, value);
        }

        /// <summary>
        /// Aktuell ausgewähltes Format To
        /// </summary>
        private Format _selectedFormatTo;
        public Format SelectedFormatTo
        {
            get => _selectedFormatTo;
            set => this.RaiseAndSetIfChanged(ref _selectedFormatTo, value);
        }

        /// <summary>
        /// Aktuell dropped File
        /// </summary>
        private string _isFileDropped = "Ziehen Sie Ihre Dateien hierher…";
        public string IsFileDropped
        {
            get => _isFileDropped;
            set => this.RaiseAndSetIfChanged(ref _isFileDropped, value);
            
        }
        #endregion

        #region Commands
        /// <summary>
        /// Command für den Drop Files
        /// </summary>
        public ICommand SaveFile { get; }


        #endregion

        #region Services
        private readonly FileBearbeitungService _fileService = new();
        #endregion

        /// <summary>
        /// Initialisieren ViewModel und Kommands mit Enum
        /// </summary>
        public MainWindowViewModel()
        {
            // Initialize Enum mit Formats
            Formats = new ObservableCollection<Format>(
                Enum.GetValues(typeof(Format)).Cast<Format>()
            );

            // Initialize Commands
            SaveFile = ReactiveCommand.Create(async () =>
            {
                if(SelectedFormatTo == 0)
                {
                    var box = MessageBoxManager
                        .GetMessageBoxStandard("Oooops", "Wählen Sie aus, zu welchem Format ich Ihr Dokument konvertieren soll!",
                        ButtonEnum.Ok);

                    var result = await box.ShowAsync();
                }
                else if(IsFileDropped.Equals("Ziehen Sie Ihre Dateien hierher…"))
                {
                    var box = MessageBoxManager
                        .GetMessageBoxStandard("Oooops", "Keine File gefunden!",
                        ButtonEnum.Ok);

                    var result = await box.ShowAsync();
                }
                else
                {
                    string fileExtension = Path.GetExtension(GlobalVariables.FilePath).ToLower();
                    //Konvertieren und speichern new File
                    string ergebnis = _fileService.KonvertFile(filePath: GlobalVariables.FilePath, formatTo: SelectedFormatTo.ToString());

                    IsFileDropped = ergebnis;
                }

            });
        }

       

    }

}
