using System;
using System.Collections.Generic;
using System.Linq;

interface IOrderStrategy
{
    object FormatOrder(List<(int count, int foodId)> order);
}

class OrderFormatter
{
    public void DisplayOrder(IOrderStrategy strategy, List<(int count, int foodId)> order)
    {
        var formattedOrder = strategy.FormatOrder(order);

        Console.WriteLine("Замовлення:");
        switch (formattedOrder)
        {
            case List<int[]> fastFoodOrder:
                Console.WriteLine("Замовлення фастфуду:");
                foreach (var item in fastFoodOrder)
                {
                    Console.WriteLine($"Айдi Страви: {item[0]}, Кiлькiсть: {item[1]}");
                }
                break;

            case (List<int> foodIds, List<int> counts):
                Console.WriteLine("Замовлення сушi:");
                Console.WriteLine("Айдi Страви: " + string.Join(", ", foodIds));
                Console.WriteLine("Кiлькiсть: " + string.Join(", ", counts));
                break;

            case List<int> ukrainianOrder:
                Console.WriteLine("Замовлення традицiйної української кухнi:");
                Console.WriteLine("Айдi Їжi: " + string.Join(", ", ukrainianOrder));
                break;

            default:
                Console.WriteLine("Невідомий формат замовлення.");
                break;
        }
    }
}

class OrderSystem
{
    private IOrderStrategy _orderStrategy;
    private OrderFormatter _formatter = new OrderFormatter();

    public void SetOrderStrategy(IOrderStrategy orderStrategy)
    {
        _orderStrategy = orderStrategy;
    }

    public void PlaceOrder(List<(int count, int foodId)> order)
    {
        if (_orderStrategy != null)
        {
            _formatter.DisplayOrder(_orderStrategy, order);
        }
        else
        {
            Console.WriteLine("Не встановлено стратегію замовлення.");
        }
    }
}

class FastFoodOrder : IOrderStrategy
{
    public object FormatOrder(List<(int count, int foodId)> order)
    {
        return order.Select(o => new int[] { o.foodId, o.count }).ToList();
    }
}

class SushiOrder : IOrderStrategy
{
    public object FormatOrder(List<(int count, int foodId)> order)
    {
        var foodIds = order.Select(o => o.foodId).ToList();
        var counts = order.Select(o => o.count).ToList();
        return (foodIds, counts);
    }
}

class UkrainianFoodOrder : IOrderStrategy
{
    public object FormatOrder(List<(int count, int foodId)> order)
    {
        var ukrainianOrder = new List<int>();
        foreach (var o in order)
        {
            ukrainianOrder.AddRange(Enumerable.Repeat(o.foodId, o.count));
        }
        return ukrainianOrder;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var order = new List<(int count, int foodId)>
        {
            (3, 123), (1, 500), (2, 42)
        };

        var orderSystem = new OrderSystem();

        Console.WriteLine("Виберiть тип їжi (a - Фастфуд, b - Сушi, c - Традицiйна українська кухня):");
        char foodType = Console.ReadLine()[0];

        switch (foodType)
        {
            case 'a':
                orderSystem.SetOrderStrategy(new FastFoodOrder());
                break;
            case 'b':
                orderSystem.SetOrderStrategy(new SushiOrder());
                break;
            case 'c':
                orderSystem.SetOrderStrategy(new UkrainianFoodOrder());
                break;
            default:
                Console.WriteLine("Невiдомий тип замовлення.");
                return;
        }

        orderSystem.PlaceOrder(order);
    }
}