using Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ATM
{
    /// <summary>
    /// Логика взаимодействия для Deposit.xaml
    /// </summary>
    public partial class Deposit : Window
    {

        public class CashItem
        {
            public int Denomination { get; set; }
            public int Quantity { get; set; }
        }
        public Deposit()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public void UpdateListBox(ref ATMMachine atm)
        {
            var cashList = atm.Cash.Select(kvp => new CashItem { Denomination = kvp.Key, Quantity = 0 }).ToList();
            CashListBox.ItemsSource = cashList;
        }

        public Dictionary<int, int> getQuantityCash()
        {

            var cashDictionary = CashListBox.Items
                .Cast<CashItem>()
                .ToDictionary(cashItem => cashItem.Denomination, cashItem => cashItem.Quantity);


            return cashDictionary;
        }
    }
}

