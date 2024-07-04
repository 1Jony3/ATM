using Domain;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ATM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ATMMachine atm;

        public MainWindow()
        {
            InitializeComponent();
            atm = new ATMMachine();
            UpdateBalanceDisplay();
            UpdateCashListBox();
        }

        private void UpdateBalanceDisplay() => BalanceTextBlock.Text = $"{atm.TotalCash}₽";
        
        private void UpdateUsersCash()
        {
            if (!ShowBalance.Content.Equals("Показать баланс")) ShowBalance.Content = $"{atm.UserCash}₽";
            
        }
        private void UpdateCashListBox()
        {
            var cashList = atm.Cash.Select(kvp => new { Denomination = $"₽{kvp.Key}", Quantity = kvp.Value.ToString() }).ToList();
            CashListBox.ItemsSource = cashList;
        }

        private void ShowBalance_Click(object sender, RoutedEventArgs e)
        {
            if (ShowBalance.Content.Equals("Показать баланс")) ShowBalance.Content = $"{atm.UserCash}₽";
            else ShowBalance.Content = "Показать баланс";
        }

        private void WithdrawButton_Click(object sender, RoutedEventArgs e)
        {
            bool largeDenominationsFirst = true;
            if (DenominationComboBox.SelectedIndex == 1) largeDenominationsFirst = false;


            if (int.TryParse(AmountInput.Text, out int amount))
            {
                if (atm.Withdraw(amount, out Dictionary<int, int> cashToDispense, largeDenominationsFirst))
                {
                    MessageBox.Show($"Выдано: {string.Join(", ", cashToDispense.Select(c => $"{c.Value} x {c.Key}₽"))}", "Успешно");
                    UpdateBalanceDisplay();
                    UpdateCashListBox();
                    UpdateUsersCash();
                }
                else
                {
                    MessageBox.Show("Недостаточно средств или невозможно выдать запрошенную сумму.", "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Введите корректную сумму.", "Ошибка");
            }
        }

        private void DepositButton_Click(object sender, RoutedEventArgs e)
        {
            Deposit deposit = new Deposit();

            deposit.UpdateListBox(ref atm);

            if (deposit.ShowDialog() == true)
            {
                atm.Deposit(deposit.getQuantityCash());
                UpdateBalanceDisplay();
                UpdateCashListBox();
                UpdateUsersCash();


            }
           
        }
    }
}