# Legacy Code — Анализ зависимостей и тестируемость кода

## 1. О проекте

Учебный проект демонстрирует одну из распространённых проблем Legacy Code — **жёсткую связанность классов (Tight Coupling)**.

Рассматривается пример обработки заказа (`OrderProcessor`), который самостоятельно создаёт необходимые объекты для выполнения своей работы:

- Валидатор заказа (`OrderValidator`).
- Репозиторий для сохранения заказа (`OrderRepository`).
- Сервис отправки уведомлений (`EmailNotificator`).

Такая реализация работает как пример простой архитектуры, но имеет существенный недостаток: класс `OrderProcessor` напрямую зависит от конкретных реализаций других классов.

В проекте рассматриваются два варианта реализации:

1. `Legacy Code` — первоначальная реализация с жёсткими зависимостями.
2. `Tastable` — улучшенная реализация с использованием интерфейсов и внедрения зависимостей через конструктор.

**Главная цель:** научиться находить жёсткие зависимости и изменять архитектуру программы таким образом, чтобы отдельные компоненты можно было заменять и тестировать независимо друг от друга.

---

## 2. Структура проекта

```text
Sample_0/
│
├── Legacy Code/
│   ├── Order.cs
│   ├── OrderProcessor.cs
│   ├── OrderValidator.cs
│   ├── OrderRepository.cs
│   └── EmailNotificator.cs
│
├── Tastable/
│   ├── IOrderValidator.cs
│   ├── IOrderRepository.cs
│   ├── INotificator.cs
│   ├── MyOrderValidator_1.cs
│   ├── MyOrderValidator_2.cs
│   ├── MyOrderRepository.cs
│   ├── MyNotificator_1.cs
│   └── TastableOrderProcessor.cs
│
├── Program.cs
└── Sample_0.csproj
```

### Назначение папок

**Legacy Code**

Первоначальная реализация программы. Здесь находятся классы, которые напрямую создают экземпляры других классов через `new`.

**Tastable**

Улучшенная реализация. Здесь используются интерфейсы, через которые передаются необходимые зависимости.

Название `Tastable` в проекте используется как название папки. По смыслу речь идёт о *Testable* — тестируемом коде.

---

## 3. Разбор первоначальной реализации (Legacy Code)

Рассмотрим класс `OrderProcessor.cs`.

```csharp
internal class OrderProcessor
{
    public void Process(Order order)
    {
        OrderValidator validator = new();

        if (!validator.Validate(order))
        {
            throw new Exception(
                "OrderProcessor.Process: order not validate!");
        }

        OrderRepository repository = new();
        repository.Save(order);

        EmailNotificator notificator = new();
        notificator.SendEmail(
            order.CustomerEmail,
            "Заказ обработан");
    }
}
```

### Что делает метод Process?

Метод выполняет три последовательных действия:

1. Проверяет корректность заказа.
2. Сохраняет заказ.
3. Отправляет клиенту уведомление.

### Как работает код?

При вызове:

```csharp
OrderProcessor orderProcessor = new();
orderProcessor.Process(new Order());
```

Внутри `Process()` самостоятельно создаются три объекта:

```csharp
OrderValidator validator = new();
OrderRepository repository = new();
EmailNotificator notificator = new();
```

Таким образом, `OrderProcessor` самостоятельно определяет, какие конкретно классы использовать.

### В чём проблема?

Предположим, мы хотим заменить отправку email на SMS.

Для этого потребуется изменять сам `OrderProcessor`, поскольку он напрямую создаёт `EmailNotificator`.

Аналогичная ситуация возникает при замене репозитория или валидатора.

Основные недостатки:

- Нельзя легко заменить реализацию зависимостей.
- Сложно тестировать `OrderProcessor` отдельно.
- Нельзя удобно подставить тестовый объект вместо настоящего.
- Изменение реализации одного компонента может потребовать изменения другого.

Это пример **Tight Coupling — жёсткой связанности компонентов**.

### Что такое жёсткая зависимость?

Жёсткая зависимость возникает, когда один класс напрямую зависит от конкретной реализации другого класса.

Например:

```csharp
OrderValidator validator = new();
```

Здесь `OrderProcessor` знает не только о том, что ему нужен валидатор, но и о том, какой именно класс должен выполнять проверку.

---

## 4. Что такое тестируемость кода (Testability)?

**Testability** — способность программного компонента быть проверенным с помощью автоматизированных тестов.

Хорошо тестируемый код позволяет:

- Проверять отдельные классы независимо от остальных.
- Подменять реальные зависимости тестовыми объектами.
- Проверять различные сценарии работы программы.
- Имитировать ошибки внешних компонентов.
- Проверять результаты выполнения методов.

### Почему первоначальный OrderProcessor сложно тестировать?

Допустим, мы хотим проверить, что заказ сохраняется только после успешной проверки.

Но `OrderProcessor` самостоятельно создаёт `OrderValidator` и `OrderRepository`.

Мы не можем просто передать ему специальный тестовый валидатор, который вернёт `false`, или тестовый репозиторий, который позволит проверить факт вызова `Save()`.

Для этого необходимо изменить способ получения зависимостей.

Именно эту задачу решает следующая часть проекта.

---

## 5. Интерфейсы (Interfaces)

**Интерфейс** — контракт, который определяет, какие методы должен реализовать класс.

Интерфейс не определяет конкретный алгоритм выполнения метода. Он только описывает, какие действия должны быть доступны.

В проекте используются три интерфейса.

### 5.1. IOrderValidator

Файл: `IOrderValidator.cs`

```csharp
internal interface IOrderValidator
{
    public bool Validate(Order order);
}
```

Описывает контракт проверки заказа.

Любой класс, реализующий `IOrderValidator`, должен предоставить метод:

```csharp
bool Validate(Order order);
```

### 5.2. IOrderRepository

Файл: `IOrderRepository.cs`

```csharp
internal interface IOrderRepository
{
    void Save(Order order);
}
```

Описывает контракт сохранения заказа.

Любой класс, реализующий этот интерфейс, должен предоставить метод `Save()`.

### 5.3. INotificator

Файл: `INotificator.cs`

```csharp
internal interface INotificator
{
    void SendEmail(string customerEmail, string message);
}
```

Описывает контракт отправки уведомлений по электронной почте.

### Зачем нужны интерфейсы?

Представим, что у нас есть два класса:

```csharp
internal class MyOrderValidator_1 : IOrderValidator
{
    public bool Validate(Order order)
    {
        return true;
    }
}
```

```csharp
internal class MyOrderValidator_2 : IOrderValidator
{
    public bool Validate(Order order)
    {
        return true;
    }
}
```

Оба класса реализуют один интерфейс.

Следовательно, оба могут использоваться там, где требуется `IOrderValidator`.

Для программы не принципиально, какой именно класс выполняет проверку. Главное — чтобы он соответствовал контракту.

**Важно:** интерфейс позволяет работать с абстракцией, а не с конкретной реализацией.

---

## 6. Реализации интерфейсов

В папке `Tastable` находятся классы, реализующие объявленные интерфейсы.

| Класс | Интерфейс | Назначение |
|---|---|---|
| MyOrderValidator_1 | IOrderValidator | Первая реализация проверки заказа |
| MyOrderValidator_2 | IOrderValidator | Вторая реализация проверки заказа |
| MyOrderRepository | IOrderRepository | Реализация сохранения заказа |
| MyNotificator_1 | INotificator | Реализация отправки уведомления |

Например:

```csharp
internal class MyOrderRepository : IOrderRepository
{
    public void Save(Order order)
    {
        // do some
        Console.WriteLine(
            "MyOrderRepository.Save: called!");
    }
}
```

Здесь класс `MyOrderRepository` реализует интерфейс `IOrderRepository`.

Это означает, что он обязан реализовать метод `Save()`.

В данном учебном примере вместо реального сохранения данных используется вывод сообщения в консоль.

Аналогично работают остальные реализации.

**Обрати внимание:** `MyOrderValidator_1` и `MyOrderValidator_2` демонстрируют возможность использовать разные реализации одного контракта. В текущем примере обе возвращают `true`, поэтому реальная логика проверки между ними не отличается.

---

## 7. Constructor Injection — внедрение зависимостей через конструктор

Это основная тема данного проекта.

**Dependency Injection (DI)** — подход, при котором объект получает необходимые ему зависимости извне, а не создаёт их самостоятельно.

В проекте используется разновидность DI — Constructor Injection.

Это означает, что необходимые объекты передаются через конструктор класса.

### TastableOrderProcessor.cs

Рассмотрим улучшенную реализацию:

```csharp
internal class TastableOrderProcessor
{
    private readonly IOrderValidator _validator;
    private readonly IOrderRepository _repository;
    private readonly INotificator _notificator;

    public TastableOrderProcessor(
        IOrderValidator validator,
        IOrderRepository repository,
        INotificator notificator)
    {
        _validator = validator;
        _repository = repository;
        _notificator = notificator;
    }

    public void Process(Order order)
    {
        if (!_validator.Validate(order))
        {
            throw new Exception(
                "OrderProcessor.Process: order not validate!");
        }

        _repository.Save(order);

        _notificator.SendEmail(
            order.CustomerEmail,
            "Заказ обработан");
    }
}
```

### Разберём по частям

**1. Поля класса**

```csharp
private readonly IOrderValidator _validator;
private readonly IOrderRepository _repository;
private readonly INotificator _notificator;
```

Класс хранит ссылки на необходимые зависимости.

Обрати внимание:

- Используются типы интерфейсов.
- Нет привязки к конкретным классам.
- `readonly` запрещает повторное присваивание полям после завершения конструктора.

**2. Конструктор**

```csharp
public TastableOrderProcessor(
    IOrderValidator validator,
    IOrderRepository repository,
    INotificator notificator)
{
    _validator = validator;
    _repository = repository;
    _notificator = notificator;
}
```

В конструктор передаются три объекта.

Затем они сохраняются в полях класса.

Теперь `TastableOrderProcessor` не создаёт валидатор, репозиторий и уведомитель самостоятельно.

Он получает их извне.

**3. Использование зависимостей**

```csharp
_validator.Validate(order);
_repository.Save(order);
_notificator.SendEmail(...);
```

Класс взаимодействует с объектами через интерфейсы.

Ему не важно, какая конкретно реализация находится за каждым интерфейсом.

### Главное отличие

Было:

```csharp
OrderValidator validator = new();
```

Стало:

```csharp
private readonly IOrderValidator _validator;
```

Теперь создание объекта и его использование разделены.

---

## 8. Сравнение двух архитектур

### Первоначальная реализация

```text
OrderProcessor
    |
    |-- new OrderValidator()
    |
    |-- new OrderRepository()
    |
    |-- new EmailNotificator()
```

`OrderProcessor` самостоятельно создаёт все зависимости.

### Улучшенная реализация

```text
                 TastableOrderProcessor
                         |
            +------------+------------+
            |            |            |
            v            v            v
       IOrderValidator  IOrderRepository  INotificator
            |            |            |
            v            v            v
       MyOrderValidator  MyOrderRepository  MyNotificator
```

`TastableOrderProcessor` работает через интерфейсы, а конкретные реализации передаются извне.

### Сравнительная таблица

| Критерий | OrderProcessor | TastableOrderProcessor |
|---|---|---|
| Создание зависимостей | Внутри класса | Извне |
| Использование интерфейсов | Нет | Да |
| Замена реализации | Сложнее | Проще |
| Тестирование | Затруднено | Удобнее |
| Связанность | Высокая | Ниже |
| Constructor Injection | Нет | Да |

---

## 9. Program.cs — точка сборки зависимостей

Теперь посмотрим, как создаются объекты в `Program.cs`.

### Первоначальный вариант

```csharp
OrderProcessor orderProcessor = new();
orderProcessor.Process(new Order());
```

Здесь создаётся `OrderProcessor`, который самостоятельно создаёт все необходимые зависимости.

### Новый вариант

```csharp
MyOrderValidator_1 myOrderValidator_1 = new();
MyOrderValidator_2 myOrderValidator_2 = new();
MyOrderRepository myOrderRepository = new();
MyNotificator_1 myNotificator_1 = new();

TastableOrderProcessor tastableOrderProcessor = new(
    myOrderValidator_1,
    myOrderRepository,
    myNotificator_1
);

tastableOrderProcessor.Process(new Order());
```

Здесь происходит следующее:

1. Создаётся конкретная реализация валидатора.
2. Создаётся конкретная реализация репозитория.
3. Создаётся конкретная реализация уведомителя.
4. Все три объекта передаются в конструктор `TastableOrderProcessor`.
5. Выполняется обработка заказа.

**Важный момент:** объект `myOrderValidator_2` создаётся, но в конструктор не передаётся и далее не используется. Это специально полезно заметить при анализе проекта: наличие двух реализаций показывает возможность замены валидатора, но фактически используется только первая.

### Что такое Composition Root?

`Program.cs` в данном примере выполняет роль Composition Root — места, в котором создаются конкретные реализации зависимостей и связываются между собой.

Именно здесь определяется, какие классы будут использоваться программой.

В больших приложениях эту задачу часто выполняет DI-контейнер, но в данном проекте зависимости создаются вручную.

---

## 10. Почему это делает код тестируемым?

Теперь мы можем передать в `TastableOrderProcessor` специальные тестовые реализации интерфейсов.

Например, создать валидатор, который всегда возвращает `false`.

```csharp
internal class FakeOrderValidator : IOrderValidator
{
    public bool Validate(Order order)
    {
        return false;
    }
}
```

Теперь можно проверить сценарий, когда заказ не прошёл проверку.

```csharp
var processor = new TastableOrderProcessor(
    new FakeOrderValidator(),
    new MyOrderRepository(),
    new MyNotificator_1()
);

processor.Process(new Order());
```

При выполнении программа выбросит исключение:

```text
OrderProcessor.Process: order not validate!
```

До сохранения заказа и отправки уведомления выполнение не дойдёт.

Это позволяет проверять различные сценарии без изменения самого `TastableOrderProcessor`.

### Что такое Fake и Mock?

Это понятия из тестирования.

**Fake** — упрощённая реализация зависимости, которая используется вместо настоящей.

Например, тестовый репозиторий, сохраняющий данные в список вместо базы данных.

**Mock** — тестовый объект, который позволяет задавать ожидаемое поведение и проверять взаимодействие с ним.

Например, проверить, что метод `Save()` был вызван ровно один раз.

В проекте готовые Mock-объекты и автоматические Unit Tests не реализованы. Здесь демонстрируется архитектурная подготовка к их использованию.

---

## 11. Dependency Inversion Principle (DIP)

В проекте также демонстрируется идея принципа инверсии зависимостей (Dependency Inversion Principle).

DIP — один из пяти принципов SOLID.

Основная идея:

- Модули высокого уровня не должны зависеть от конкретных модулей низкого уровня.
- Оба типа модулей должны зависеть от абстракций.
- Абстракции не должны зависеть от деталей реализации.

В нашем примере:

**Модуль высокого уровня:**

`TastableOrderProcessor` — управляет процессом обработки заказа.

**Модули низкого уровня:**

- `MyOrderValidator_1`.
- `MyOrderRepository`.
- `MyNotificator_1`.

**Абстракции:**

- `IOrderValidator`.
- `IOrderRepository`.
- `INotificator`.

Вместо зависимости от конкретных классов `TastableOrderProcessor` зависит от интерфейсов.

Важно понимать: DIP — архитектурный принцип, а DI — способ передачи зависимостей. Они связаны, но не являются одним и тем же.

---

## 12. Dependency Injection и Inversion of Control

Эти понятия часто встречаются вместе, но означают разное.

**Inversion of Control (IoC)** — принцип инверсии управления.

Объект не должен самостоятельно контролировать создание всех своих зависимостей. Управление этой частью процесса передаётся внешнему коду.

**Dependency Injection (DI)** — один из способов реализации IoC.

В нашем проекте используется:

```csharp
public TastableOrderProcessor(
    IOrderValidator validator,
    IOrderRepository repository,
    INotificator notificator)
```

Зависимости передаются через конструктор, а не создаются внутри класса.

### Основные способы внедрения зависимостей

| Способ | Описание |
|---|---|
| Constructor Injection | Передача зависимостей через конструктор |
| Property Injection | Передача зависимостей через свойства |
| Method Injection | Передача зависимостей через параметры метода |

В этом проекте используется Constructor Injection.

Для обязательных зависимостей он обычно является наиболее предпочтительным вариантом, поскольку позволяет получить полностью инициализированный объект.

---

## 13. Важные особенности и ограничения проекта

При изучении примера необходимо учитывать несколько моментов.

1. В `Legacy Code/OrderProcessor.cs` зависимости создаются через `new`, что демонстрирует жёсткую связанность.
2. В `Tastable/TastableOrderProcessor.cs` используются интерфейсы и Constructor Injection.
3. `OrderValidator` в старой реализации возвращает `true`, поэтому проверка всегда проходит.
4. `OrderRepository.Save()` и `EmailNotificator.SendEmail()` в старой реализации выбрасывают `NotImplementedException`.
5. Новые реализации используют `Console.WriteLine()` вместо настоящей работы с базой данных и электронной почтой.
6. Обе новые реализации валидатора возвращают `true`.
7. `Order` остаётся общим классом для обеих реализаций. Его расположение в `Legacy Code` показывает, что архитектура улучшена не полностью: модель данных пока не отделена от старой части проекта.
8. В проекте нет полноценного DI-контейнера, например Microsoft.Extensions.DependencyInjection. Все зависимости создаются вручную.
9. В проекте нет автоматических Unit Tests — демонстрируется возможность сделать код тестируемым.

---

## 14. Как запустить проект?

Требования:

- .NET SDK 9.0 или новее.
- Visual Studio, Rider или Visual Studio Code.

Запуск через терминал:

```bash
dotnet restore
dotnet run
```

После запуска должны появиться сообщения:

```text
MyOrderValidator_1.Validate: called!
MyOrderRepository.Save: called!
MyNotificator_1.SendEmail: called!
```

Это подтверждает последовательность выполнения операций:

1. Проверка заказа.
2. Сохранение заказа.
3. Отправка уведомления.

---

## 15. Что можно улучшить самостоятельно?

Для закрепления материала можно выполнить следующие дополнительные задания.

### Задание 1. Замена валидатора

В `Program.cs` замени:

```csharp
MyOrderValidator_1 myOrderValidator_1 = new();
```

на:

```csharp
MyOrderValidator_2 myOrderValidator_2 = new();
```

Передай вторую реализацию в конструктор `TastableOrderProcessor`.

Вопрос: потребовалось ли изменять сам `TastableOrderProcessor`?

### Задание 2. Реализация новой системы уведомлений

Создай класс:

```csharp
internal class SmsNotificator : INotificator
{
    public void SendEmail(string customerEmail, string message)
    {
        Console.WriteLine("Отправка SMS");
    }
}
```

Подключи его вместо `MyNotificator_1`.

Вопрос: почему новая реализация может использоваться без изменения `TastableOrderProcessor`?

### Задание 3. Отрицательная проверка заказа

Измени `MyOrderValidator_1`, чтобы он возвращал `false`.

Запусти программу.

Вопросы:

- Какое исключение возникло?
- Был ли вызван `MyOrderRepository.Save()`?
- Было ли отправлено уведомление?

### Задание 4. Создание тестового репозитория

Создай класс `FakeOrderRepository`, реализующий `IOrderRepository`.

Вместо реального сохранения пусть он выводит сообщение:

```text
Тестовый репозиторий: заказ сохранён!
```

Подключи его в `Program.cs`.

### Задание 5. Добавление нового способа уведомления

Создай ещё одну реализацию `INotificator`, например `TelegramNotificator`.

Подключи её без изменения класса `TastableOrderProcessor`.

Объясни, почему это возможно.

---

## 16. Вопросы для самопроверки

1. Что такое Legacy Code?
2. Что такое Tight Coupling?
3. Почему создание зависимостей через `new` внутри класса может усложнять тестирование?
4. Что такое интерфейс?
5. Для чего используются `IOrderValidator`, `IOrderRepository` и `INotificator`?
6. Может ли несколько классов реализовывать один интерфейс?
7. Что такое Dependency Injection?
8. Что такое Constructor Injection?
9. Почему поля зависимостей имеют тип интерфейса, а не конкретного класса?
10. Что такое Inversion of Control?
11. Чем DIP отличается от DI?
12. Что такое Composition Root?
13. В чём разница между Fake и Mock?
14. Почему `TastableOrderProcessor` проще тестировать, чем `OrderProcessor`?
15. Можно ли заменить репозиторий, не изменяя `TastableOrderProcessor`?

---

## 17. Краткая шпаргалка

| Понятие | Запомнить |
|---|---|
| Legacy Code | Существующий код, который приходится поддерживать и развивать |
| Tight Coupling | Жёсткая связанность классов |
| Interface | Контракт, определяющий доступные методы |
| Dependency | Зависимость одного класса от другого |
| DI | Передача зависимостей извне |
| Constructor Injection | Передача зависимостей через конструктор |
| IoC | Инверсия управления |
| DIP | Принцип зависимости от абстракций |
| Composition Root | Место создания и связывания зависимостей |
| Fake | Упрощённая тестовая реализация |
| Mock | Тестовый объект для проверки взаимодействий |
| Testability | Возможность удобно тестировать программный компонент |

### Главное, что нужно запомнить

**Плохой вариант:**

```csharp
public class OrderProcessor
{
    public void Process(Order order)
    {
        OrderValidator validator = new();
        OrderRepository repository = new();
        EmailNotificator notificator = new();
    }
}
```

Класс самостоятельно создаёт зависимости и привязан к конкретным реализациям.

**Улучшенный вариант:**

```csharp
public class OrderProcessor
{
    private readonly IOrderValidator _validator;

    public OrderProcessor(IOrderValidator validator)
    {
        _validator = validator;
    }
}
```

Класс получает зависимость извне и работает через интерфейс.

**Основная мысль проекта:**

Необходимо проектировать классы так, чтобы их можно было проверять, изменять и расширять независимо от конкретных реализаций других компонентов.

Это один из важнейших навыков при работе с Legacy Code и сопровождении существующих программных систем.