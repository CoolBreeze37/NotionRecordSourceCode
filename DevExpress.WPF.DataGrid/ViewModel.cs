using System.Collections.ObjectModel;
using System.Data.Entity;
using DevExpress.Mvvm;

namespace DevExpress.WPF.DataGrid
{
    public class ViewModel:ViewModelBase
    {
        NorthwindEntities northwindeDbContext;
        public ViewModel()
        {
            if (IsInDesignMode)
            {
                Orders = new ObservableCollection<Order>();
                Shippers = new ObservableCollection<Shipper>();
                Employees = new ObservableCollection<Employee>();
            }
            else
            {
                northwindeDbContext = new NorthwindEntities();

                northwindeDbContext.Orders.Load();
                Orders = northwindeDbContext.Orders.Local;
                northwindeDbContext.Shippers.Load();
                Shippers = northwindeDbContext.Shippers.Local;
                northwindeDbContext.Employees.Load();
                Employees = northwindeDbContext.Employees.Local;
            }
        }
        public ObservableCollection<Order> Orders
        {
            get => GetValue<ObservableCollection<Order>>();
            private set { SetValue(value); }
        }
        public ObservableCollection<Shipper> Shippers
        {
            get => GetValue<ObservableCollection<Shipper>>();
            private set { SetValue(value); }
        }
        public ObservableCollection<Employee> Employees
        {
            get => GetValue<ObservableCollection<Employee>>();
            private set { SetValue(value); }
        }

    }
}
