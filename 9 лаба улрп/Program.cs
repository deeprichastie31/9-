/*
Интерфейс:
    Перевод в рубли
    Разбиение получаемой валюты на копейки и рубли. 


*/


using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

void PrintInstructions()
{

}
Purse purse = new Purse("Ruble", 0); // заданы начальные значен
bool marker = true;
while (marker)
{
    Console.WriteLine("\tМЕНЮ\n**" + new string('-', 25) + "**"); // Данная конструкция позволяет размножить черторчку на определенную длину
    Console.WriteLine("1) Совершить перевод в другую валюту \n2) Пополнить кошелек \n3) Снять со счета\n4) Вывести информацию о кошельке");
    Console.WriteLine("**" + new string('-', 25) + "**");
    int chose = int.Parse(Console.ReadLine());
    switch (chose)
    {
        case 1:
            Console.WriteLine("Из какой валюты вы бы хотели перевести?");
            string valute = Console.ReadLine();
            Console.WriteLine("Вам сейчас по очереди потребуется ввести следующие данные для осуществления перевода:\n" +
                $"Количество {valute}\nКурс валюты, в которую вы собираетесь переводить относительно {valute}\nВалюта для перевода");
            double count = double.Parse(Console.ReadLine());
            double course = double.Parse(Console.ReadLine());
            string secondValute = Console.ReadLine();
            if (valute[0] == 'd' || valute[0] == 'D' || valute[0] == 'д' || valute[0] == 'Д') // случай для перевода денег из доллара
            {
                Dollar dollar = new Dollar(count, course, secondValute); // провелись расчеты
                purse.dollar -= count; // Вычитает из кошелька сумму, взятую для перевода в другую валюту
                if (purse.CheckMinus(purse.dollar, count)  == true)
                {
                    purse.dollar += count;
                    break;
                }
                purse.AddOrTakeoff(secondValute, dollar.GetMoney(), "Пополнить");
                dollar.Print();
            }
            if (valute[0] == 'e' || valute[0] == 'E' || valute[0] == 'е' || valute[0] == 'Е')
            {
                Euro euro = new Euro(count, course, secondValute);
                if (purse.CheckMinus(purse.euro, count) == true)
                {
                    purse.euro += count;
                    break;
                }
                purse.AddOrTakeoff(secondValute, euro.GetMoney(), "Попоплнить");
                euro.Print();
            }
            if (valute[0] == 'R' || valute[0] == 'r' || valute[0] == 'р' || valute[0] == 'Р')
            {
                Euro euro = new Euro(count, course, secondValute);
                if (purse.CheckMinus(purse.euro, count) == true)
                {
                    purse.euro += count;
                    break;
                }
                euro.Print();
            }
            if (valute[0] == 'P' || valute[0] == 'p' || valute[0] == 'ф' || valute[0] == 'Ф')
            {
                Pound pound = new Pound(count, course, secondValute);
                if (purse.CheckMinus(purse.pound, count) == true)
                {
                    purse.pound += count;
                    break;
                }
                purse.AddOrTakeoff(secondValute, pound.GetMoney(), "Пополнить");
                pound.Print();
            }
            break;
        case 2:
            purse.AddOrTakeoff("0", 0, "Пополнить");
            break;
        case 3:
            purse.AddOrTakeoff("0", 0, "Снять");
            break;
        case 4:
            purse.ShowInfo();
            break;
        case 5:

            break;
    }
}
interface ICurrency 
{
    void ConvertToAnyValute();
    void Print();
    double GetMoney();
}
abstract class Currency : ICurrency
{
    public double course;
    public double count;
    public string nameOfValue;
    protected double result;
    protected int Valute;
    protected int Copeck;

    public void ConvertToAnyValute() // собирает отдельно рубли и отдельно копейки. Центы и доллары. Именно поэтому я вставил реализацию в абстрактный класс
    {
        string str = Convert.ToString(Math.Round(result, 2));
        string[] parts = str.Split(',');

        if (Convert.ToInt32(result) == Math.Round(result, 2))
        {
            Valute = Convert.ToInt32(parts[0]);
            Copeck = Convert.ToInt32(0);
            return;
        }
        Valute = Convert.ToInt32(parts[0]);
        Copeck = Convert.ToInt32(parts[1]);
    }
    public double GetMoney()
    {
        return Math.Round(result, 2);
    }
    public abstract void Print();
}
class Dollar : Currency
{
    public Dollar(double count, double course, string nameofValue) // конструктор, который принимает значения извне и передает их в свойства, которые в свою очередь присваивают себе значения и с ними проводятся операции
    {
        result = count * course;
        this.course = course;
        this.count = count;
        nameOfValue = nameofValue; // имя валюты, которая будет также участвовать в переводе
        ConvertToAnyValute();
    }
    public Dollar()
    {

    }
    public override void Print()
    {
        if (nameOfValue[0] == 'р' ||  nameOfValue[0] == 'Р' || nameOfValue[0] == 'r' || nameOfValue[0] == 'е' || nameOfValue[0] == 'ф')
            Console.WriteLine($"{count}  это: " + "{0} {1} : {2}", Valute, Copeck, nameOfValue); // Переводит из трех возможных валют только в доллары!!
    }
}
class Euro : Currency
{
    public Euro(double count, double course, string nameofValue) // конструктор, который принимает значения извне и передает их в свойства, которые в свою очередь присваивают себе значения и с ними проводятся операции
    {
        result = count * course;
        this.course = course;
        this.count = count;
        nameOfValue = nameofValue;
        ConvertToAnyValute();
    }
    public Euro() 
    {

    }
    public override void Print()
    {
        if (nameOfValue[0] == 'р' || nameOfValue[0] == 'д' || nameOfValue[0] == 'ф')
            Console.WriteLine($"{count} Евро это: " + "{0},{1} : {2}", Valute, Copeck, nameOfValue); // Переводит из трех возможных валют только в доллары!!
    }
}
class Pound : Currency
{
    public Pound(double count, double course, string nameofValue) // конструктор, который принимает значения извне и передает их в свойства, которые в свою очередь присваивают себе значения и с ними проводятся операции
    // Реализацию конструктора писать не нужно, потому что те же самые переменные наследуются от класса доллар
    {
        result = count * course;
        this.course = course;
        this.count = count;
        nameOfValue = nameofValue;
        ConvertToAnyValute();
    }
    public Pound()
    {

    }
    public override void Print()
    {
        result = count * course;
        if (nameOfValue[0] == 'р' || nameOfValue[0] == 'д' || nameOfValue[0] == 'Е')
            Console.WriteLine($"{count} Фунтов это: " + "{0},{1} : {2}", Valute, Copeck, nameOfValue);
        
    }
}
class Purse
{
    private double value;
    private string wallet;
    public double dollar = 100;
    public double ruble = 0;
    public double euro = 90;
    public double pound = 80;
    Stack<double> actions = new Stack<double>();
    public List<Dollar> Dollars = new List<Dollar>();
    public List<Euro> Euros = new List<Euro>();
    public List<Pound> Pounds = new List<Pound>();
    public Purse(string wallet, double value)
    {
        this.wallet = wallet;
        this.value = value;
        AddOrTakeoff(wallet, value, "0");
    }
    public bool CheckMinus(double derp,double value)
    {
        try
        {
            if (derp - value < 0)
                throw new Exception("Вы не можете перевести больше, чем есть у вам на счету. ОПЕРАЦИЯ ОТМЕНЕНА!");
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return true;
        }
    }
    private double CatchExeption()
    {
        try 
        {
            throw new Exception("Вы не можете снять больше, чем есть на счету");
        }
        catch (Exception e) 
                {
            Console.WriteLine(e);
            return actions.Pop();
        }

    }
    public void AddOrTakeoff(string wallet, double value, string action) // занесу все инструкции сразу сюда в метод. Предоставлю пользователю при попаданиии сюда двигаться по интерфейсу, как выше.
    {
        if (value != 0)
        {
            goto metka;
        }
        if (action == "Пополнить")
        {
            Console.WriteLine("Выберите счет для пополнения: ");
            wallet = Console.ReadLine();
            Console.WriteLine("Введите сумму для пополнения");
            value = double.Parse(Console.ReadLine());
        }
        if (action == "Снять")
        {
            Console.WriteLine("Выберите счет для снятия: ");
            wallet = Console.ReadLine();
            Console.WriteLine("Введите сумму для снятия");
            value = double.Parse(Console.ReadLine());
        }
    metka:
        switch (wallet)
        {
            case "dollar" or "Долларовый" or "долларовый" or "Доллар" or "долларовый":
                actions.Push(dollar);
                this.dollar += value;
                if (dollar < 0)
                    dollar = CatchExeption();
                Dollars.Add(new Dollar());
                break;
            case "Euro" or "Евро" or "euro" or "евровый" or "Евровый":
                actions.Push(euro);
                this.euro += value;
                if (euro < 0)
                    euro = CatchExeption();
                Euros.Add(new Euro());
                break;
            case "pound" or "Фунт" or "Английский" or "фунтовый":
                actions.Push(pound);
                this.pound += value;
                if (pound < 0)
                    pound = CatchExeption();
                Pounds.Add(new Pound());
                break;
            case "Ruble" or "Рублевый" or "Неустойчивый" or "рублевый": // тут не надо добавлять, потому что у меня нет такого класса 
                actions.Push(ruble);
                this.ruble += value;
                if (ruble < 0)
                    ruble = CatchExeption();
                break;
        }
    }
    public void ShowInfo()
    {  
        Console.WriteLine("МЕНЮ\n**" + new string('~', 25) + "**");
        Console.WriteLine("Сумма валют на Вашем кошельке составляет: ");
        Console.WriteLine("На долларовом счету такой номинал:" + dollar);
        Console.WriteLine("На Евровом счету такой номинал:" + euro);
        Console.WriteLine("На рублевом счету такой номинал:" + ruble);
        Console.WriteLine("На фунтовом счету такой номинал:" + pound);
        Console.WriteLine("МЕНЮ\n**" + new string('~', 25) + "**");
    }

}