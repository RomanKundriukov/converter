using ReactiveUI;

namespace converter.ViewModels
{
    /// <summary>
    /// Basis ViewModel für alle ViewModels
    /// </summary>
    public class ViewModelBase : ReactiveObject
    {
        /// <summary>
        /// Enum für die Dateiformate
        /// </summary>
        public enum Format : byte
        {
            Pdf = 1,
            Docx = 2,
            Xlsx = 3
        }
    }
}
