using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Client
{
    public class ViewModel: INotifyPropertyChanged
    {
        private ObservableCollection<Person> _persons;
        public ObservableCollection<Person> Persons { get => _persons; set { _persons = value; OnPropertyChanged(nameof(Persons)); } }

        private readonly Model model;

        public ViewModel() 
        {

            model = new Model();
            Persons = new ObservableCollection<Person>();

        }

        private Command updateDataInServer;
        public Command UpdateDataInServer
        {
            get => updateDataInServer ?? (updateDataInServer = new Command(obj =>
            {
                model.SaveData(Persons);
            }));
        }

        private Command getDataFromServer;
        public Command GetDataFromServer
        {
            get => getDataFromServer ?? (getDataFromServer = new Command(obj =>
            {
                Persons = model.GetDataFromServer();
            }));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
