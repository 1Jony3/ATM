using System.Security.Policy;
using System.Windows;

namespace Domain
{
    public class ATMMachine
    {
        public Dictionary<int, int> Cash { get; set; }
        public int TotalCash => Cash.Sum(c => c.Key * c.Value);
        public int UserCash { get; set; }

        public ATMMachine()
        {
            Cash = new Dictionary<int, int>
            {
                { 10, 100 },
                { 50, 100 },
                { 100, 100 },
                { 500, 100 },
                { 1000, 100 },
                { 5000, 100 }
            };
            UserCash = 35600;
        }
        public bool Withdraw(int amount, out Dictionary<int, int> cashToDispense, bool largeDenominationsFirst)
        {
            cashToDispense = new Dictionary<int, int>();
            var denominations = Cash.Keys.ToList();
            // Сортировка номиналов в зависимости от предпочтения пользователя
            if (largeDenominationsFirst)
            {
                denominations.Sort((a, b) => b.CompareTo(a)); // Сортировка по убыванию для выдачи крупными купюрами
            }
            else
            {
                denominations.Sort(); // Сортировка по возрастанию для выдачи с разменом
            }

            var amountUserCach = amount;

            foreach (var denom in denominations)
            {
                int countNeeded = amount / denom;
                int countAvailable = Cash[denom];

                if (countNeeded > 0 && countAvailable > 0)
                {
                    int countToDispense = Math.Min(countNeeded, countAvailable);
                    cashToDispense.Add(denom, countToDispense);
                    amount -= countToDispense * denom;
                    Cash[denom] -= countToDispense;
                }
            }
            UserCash -= amountUserCach;

            if (amount > 0)
            {
                // Если запрошенная сумма не может быть выдана из-за недостатка купюр,
                // возвращаем купюры обратно в банкомат и возвращаем false.
                foreach (var kvp in cashToDispense)
                {
                    Cash[kvp.Key] += kvp.Value;
                }
                cashToDispense.Clear();
                UserCash += amountUserCach;
                return false;
            }

            return true;
        }
        public void Deposit(Dictionary<int, int> cashToDeposit)
        {


            foreach (var pair in cashToDeposit)
            {
                if (Cash.ContainsKey(pair.Key))
                {
                    Cash[pair.Key] += pair.Value;
                }
                else
                {
                    Cash.Add(pair.Key, pair.Value);
                }
            }

        }

    }

    

}
