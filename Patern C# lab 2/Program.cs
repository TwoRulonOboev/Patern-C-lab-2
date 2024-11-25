using System;
using System.Collections.Generic;

// === Паттерн "State" ===
// Интерфейс состояния оптического элемента
public interface IOpticalElementState
{
    void ProcessRay(OpticalElement element);
    void Visualize(OpticalElement element);
}

// Конкретные состояния
public class TransparentState : IOpticalElementState
{
    public void ProcessRay(OpticalElement element)
    {
        Console.WriteLine($"{element.Name}: Пропускаю луч через элемент.");
    }

    public void Visualize(OpticalElement element)
    {
        Console.WriteLine($"{element.Name}: Отображаю прозрачный элемент.");
    }
}

public class ReflectiveState : IOpticalElementState
{
    public void ProcessRay(OpticalElement element)
    {
        Console.WriteLine($"{element.Name}: Отражаю луч от элемента.");
    }

    public void Visualize(OpticalElement element)
    {
        Console.WriteLine($"{element.Name}: Отображаю зеркальный элемент.");
    }
}

public class AbsorptiveState : IOpticalElementState
{
    public void ProcessRay(OpticalElement element)
    {
        Console.WriteLine($"{element.Name}: Поглощаю луч элементом.");
    }

    public void Visualize(OpticalElement element)
    {
        Console.WriteLine($"{element.Name}: Отображаю поглощающий элемент.");
    }
}

// === Паттерн "Iterator" ===
// Оптическая система (контейнер элементов)
public class OpticalSystem : IEnumerable<OpticalElement>
{
    private readonly List<OpticalElement> _elements = new();

    public void AddElement(OpticalElement element)
    {
        _elements.Add(element);
    }

    public IEnumerator<OpticalElement> GetEnumerator()
    {
        return _elements.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

// Оптический элемент
public class OpticalElement
{
    public string Name { get; }
    private IOpticalElementState _state;

    public OpticalElement(string name, IOpticalElementState initialState)
    {
        Name = name;
        _state = initialState;
    }

    public void SetState(IOpticalElementState state)
    {
        _state = state;
    }

    public void ProcessRay()
    {
        _state.ProcessRay(this);
    }

    public void Visualize()
    {
        _state.Visualize(this);
    }
}

// === Паттерн "Visitor" ===
// Интерфейс посетителя
public interface IVisitor
{
    void Visit(OpticalElement element);
}

// Конкретный посетитель для обработки лучей
public class RayProcessor : IVisitor
{
    public void Visit(OpticalElement element)
    {
        element.ProcessRay();
    }
}

// Конкретный посетитель для визуализации
public class Visualizer : IVisitor
{
    public void Visit(OpticalElement element)
    {
        element.Visualize();
    }
}

// Расширение контейнера для работы с посетителями
public static class OpticalSystemExtensions
{
    public static void Accept(this OpticalSystem system, IVisitor visitor)
    {
        foreach (var element in system)
        {
            visitor.Visit(element);
        }
    }
}

// === Пример использования ===
class Program
{
    static void Main()
    {
        // Создаем элементы с начальными состояниями
        var lens = new OpticalElement("Линза", new TransparentState());
        var mirror = new OpticalElement("Зеркало", new ReflectiveState());
        var absorber = new OpticalElement("Поглотитель", new AbsorptiveState());

        // Создаем оптическую систему и добавляем элементы
        var opticalSystem = new OpticalSystem();
        opticalSystem.AddElement(lens);
        opticalSystem.AddElement(mirror);
        opticalSystem.AddElement(absorber);

        // Используем посетителей
        Console.WriteLine("Расчет лучей:");
        opticalSystem.Accept(new RayProcessor());

        Console.WriteLine("\nВизуализация системы:");
        opticalSystem.Accept(new Visualizer());

        // Меняем состояние элемента
        Console.WriteLine("\nИзменение состояния элемента:");
        lens.SetState(new ReflectiveState());
        lens.ProcessRay();
        lens.Visualize();
    }
}
