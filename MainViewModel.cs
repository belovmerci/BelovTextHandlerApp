using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Win32;
using System.Collections.Generic;

namespace BelovTextHandlerApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        // public TextHandler textHandler = new TextHandler();
        // proper mvvm ?

        private string _inputFilePath;
        private string _outputFilePath;
        private int _minWordLength;
        private bool _removePunctuation;

        private RelayCommand _processFilesCommand;

        private List<string> _selectedFromFiles = new List<string>();
        public List<string> SelectedFromFiles
        {
            get { return _selectedFromFiles; }
            set
            {
                _selectedFromFiles = value;
                OnPropertyChanged(nameof(SelectedFromFiles));
            }
        }

        private List<string> _selectedToFiles = new List<string>();
        public List<string> SelectedToFiles
        {
            get { return _selectedToFiles; }
            set
            {
                _selectedToFiles = value;
                OnPropertyChanged(nameof(SelectedToFiles));
            }
        }

        
        private int selectedFilesCounter = 0;
        private string _selectedFilesCount = "Выбрано файлов: 0";
        public string SelectedFilesCount
        {
            get { return _selectedFilesCount; }
            set
            {
                _selectedFilesCount = value;
                OnPropertyChanged(nameof(SelectedFilesCount));
            }
        }

        public string InputFilePath
        {
            get { return _inputFilePath; }
            set
            {
                _inputFilePath = value;
                OnPropertyChanged(nameof(InputFilePath));
            }
        }

        public string OutputFilePath
        {
            get { return _outputFilePath; }
            set
            {
                _outputFilePath = value;
                OnPropertyChanged(nameof(OutputFilePath));
            }
        }

        public int MinWordLength
        {
            get { return _minWordLength; }
            set
            {
                _minWordLength = value;
                OnPropertyChanged(nameof(MinWordLength));
            }
        }

        public bool RemovePunctuation
        {
            get { return _removePunctuation; }
            set
            {
                _removePunctuation = value;
                OnPropertyChanged(nameof(RemovePunctuation));
            }
        }


        public ICommand ProcessFilesCommand => _processFilesCommand ?? (_processFilesCommand = new RelayCommand(ProcessFiles));

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void ProcessFiles()
        {
            TextHandler textHandler = new TextHandler();
            await textHandler.ProcessFileListAsync(SelectedFromFiles, SelectedToFiles, MinWordLength, RemovePunctuation);
            // await textHandler.ProcessFilesConcurrentlyAsync(SelectedFromFiles, SelectedToFiles, MinWordLength, RemovePunctuation);
        }


        // 1 command?
        public ICommand AddFromFilesCommand => new RelayCommand(AddFromFiles);
        public ICommand AddToFilesCommand => new RelayCommand(AddToFiles);

        private void AddFromFiles()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedFromFiles.AddRange(openFileDialog.FileNames);
                RecountFilesNumber();
                OnPropertyChanged(nameof(SelectedFromFiles));
            }
        }

        private void AddToFiles()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedToFiles.AddRange(openFileDialog.FileNames);
                RecountFilesNumber();
                OnPropertyChanged(nameof(SelectedToFiles));
            }
        }

        private void RecountFilesNumber()
        {
            if (_selectedFromFiles.Count > _selectedToFiles.Count)
            {
                selectedFilesCounter = _selectedFromFiles.Count;
            }
            else
            {
                selectedFilesCounter = _selectedToFiles.Count;
            }
            // SelectedFilesCount = $"Файлов выбрано: {0}", selectedFilesCounter;
            SelectedFilesCount = "Файлов выбрано: " + selectedFilesCounter;
            OnPropertyChanged(nameof(SelectedToFiles));
        }
    }
}