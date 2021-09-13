using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1.ViewModel
{
    public class MainWindowViewModel
    {
        public string ButtonContent
        {
            get { return "click me"; }
        }


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return false;
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
        public void getOptionsTimesAndSales()
        {
            Console.WriteLine("OKAY");
        }

    }
}
