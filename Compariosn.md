# Сравнителен Анализ: Newtonsoft.Json vs System.Text.Json

## 📊 Обобщена Таблица

| Характеристика                | Newtonsoft.Json          | System.Text.Json         | Победител           |
| ----------------------------- | ------------------------ | ------------------------ | ------------------- |
| **Performance (Serialize)**   | Baseline                 | 2-3x по-бърз             | ✅ System.Text.Json |
| **Performance (Deserialize)** | Baseline                 | 2-3x по-бърз             | ✅ System.Text.Json |
| **Памет**                     | По-високо потребление    | 50-70% по-малко          | ✅ System.Text.Json |
| **Dynamic Objects**           | Отличен (native dynamic) | Ограничен (JsonDocument) | ✅ Newtonsoft.Json  |
| **Circular References**       | Лесно (1 настройка)      | Лесно от .NET 6+         | 🟰 Равни             |
| **Date Formatting**           | Много лесно              | Изисква custom converter | ✅ Newtonsoft.Json  |
| **Enum Serialization**        | Лесно (1 converter)      | Изисква converter        | 🟰 Равни             |
| **API Богатство**             | Много богат              | Минималистичен           | ✅ Newtonsoft.Json  |
| **Dependency**                | External NuGet           | Built-in .NET            | ✅ System.Text.Json |
| **Документация**              | Отлична                  | Отлична                  | 🟰 Равни             |
| **Learning Curve**            | Лесна                    | Малко по-стръмна         | ✅ Newtonsoft.Json  |

## 🔍 Детайлно Сравнение

### 1. Основни Операции (Serialization/Deserialization)

#### Newtonsoft.Json

```csharp
var settings = new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    NullValueHandling = NullValueHandling.Ignore
};
var json = JsonConvert.SerializeObject(obj, settings);
var obj = JsonConvert.DeserializeObject<T>(json);
```

**Плюсове:**

- ✅ Прост и интуитивен API
- ✅ Богати настройки
- ✅ Една линия код за повечето случаи

**Минуси:**

- ❌ По-бавен (2-3x)
- ❌ По-голямо потребление на памет

#### System.Text.Json

```csharp
var options = new JsonSerializerOptions
{
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};
var json = JsonSerializer.Serialize(obj, options);
var obj = JsonSerializer.Deserialize<T>(json);
```

**Плюсове:**

- ✅ Много бърз (2-3x по-бърз)
- ✅ Ниско потребление на памет
- ✅ Вграден в .NET

**Минуси:**

- ❌ По-малко флексибилен
- ❌ По-verbose за някои операции

---

### 2. Circular References

#### Newtonsoft.Json

```csharp
var settings = new JsonSerializerSettings
{
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
};
var json = JsonConvert.SerializeObject(obj, settings);
```

**Оценка:** ⭐⭐⭐⭐⭐

- Работи отлично
- 1 настройка
- Налично винаги

#### System.Text.Json

```csharp
var options = new JsonSerializerOptions
{
    ReferenceHandler = ReferenceHandler.IgnoreCycles  // .NET 6+
};
var json = JsonSerializer.Serialize(obj, options);
```

**Оценка:** ⭐⭐⭐⭐☆

- Работи отлично от .NET 6+
- Липсва в по-стари версии
- Подобен синтаксис

**Победител:** 🟰 Равни (от .NET 6+)

---

### 3. Dynamic Objects

#### Newtonsoft.Json

```csharp
dynamic obj = JsonConvert.DeserializeObject(json);
Console.WriteLine(obj.name);  // Работи директно!
```

**Оценка:** ⭐⭐⭐⭐⭐

- Native dynamic support
- Много лесно за използване
- Няма допълнителен код

#### System.Text.Json

```csharp
using var doc = JsonDocument.Parse(json);
var name = doc.RootElement.GetProperty("name").GetString();
```

**Оценка:** ⭐⭐☆☆☆

- Няма dynamic support
- Изисква JsonDocument/JsonNode
- По-verbose код
- По-сложно API

**Победител:** ✅ Newtonsoft.Json (явна победа)

---

### 4. Enum Serialization

#### Newtonsoft.Json

```csharp
var settings = new JsonSerializerSettings
{
    Converters = { new StringEnumConverter() }
};
```

#### System.Text.Json

```csharp
var options = new JsonSerializerOptions
{
    Converters = { new JsonStringEnumConverter() }
};
```

**Победител:** 🟰 Равни (почти идентични)

---

### 5. Date Formatting

#### Newtonsoft.Json

```csharp
var settings = new JsonSerializerSettings
{
    DateFormatString = "dd.MM.yyyy"
};
```

**Оценка:** ⭐⭐⭐⭐⭐

- Много лесно
- 1 настройка
- Всякакви формати

#### System.Text.Json

```csharp
// Изисква custom JsonConverter!
public class CustomDateConverter : JsonConverter<DateTime>
{
    public override DateTime Read(...) { /* code */ }
    public override void Write(...) { /* code */ }
}

var options = new JsonSerializerOptions
{
    Converters = { new CustomDateConverter() }
};
```

**Оценка:** ⭐⭐☆☆☆

- Сложно
- Много код
- Трябва custom converter

**Победител:** ✅ Newtonsoft.Json (явна победа)

---

### 6. Null Value Handling

#### Newtonsoft.Json

```csharp
NullValueHandling = NullValueHandling.Ignore
```

#### System.Text.Json

```csharp
DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
```

**Победител:** 🟰 Равни (различен синтаксис, същ ефект)

---

## 🏆 Performance Benchmarks

### Тестова конфигурация

- 100 Person обекта
- Всеки с Address, Skills (колекция)
- Nullable полета
- .NET 8.0

### Резултати (примерни - от BenchmarkDotNet)

| Операция             | Newtonsoft.Json | System.Text.Json | Разлика           |
| -------------------- | --------------- | ---------------- | ----------------- |
| Serialize            | ~850 μs         | ~280 μs          | **3.0x по-бързо** |
| Deserialize          | ~920 μs         | ~350 μs          | **2.6x по-бързо** |
| Round-trip           | ~1,770 μs       | ~630 μs          | **2.8x по-бързо** |
| Memory (Serialize)   | ~85 KB          | ~28 KB           | **67% по-малко**  |
| Memory (Deserialize) | ~120 KB         | ~45 KB           | **62% по-малко**  |

> **Забележка:** Точните цифри зависят от хардуера и данните.
> Пуснете JsonComparison.Benchmarks за вашите конкретни резултати.

---

## 🎯 Кога да използваме какво?

### Използвай **Newtonsoft.Json** когато:

✅ Работиш с **legacy код** (преди .NET Core 3.0)  
✅ Нуждаеш се от **dynamic objects**  
✅ Искаш **лесно форматиране на дати**  
✅ Нуждаеш се от **богат API** с много опции  
✅ **Compatibility** е по-важна от performance  
✅ Работиш с много **custom converters**  
✅ Имаш **сложни сериализационни нужди**

**Примери:**

- Стари проекти
- Dynamic JSON APIs
- Много custom формати

---

### Използвай **System.Text.Json** когато:

✅ Performance е **критичен**  
✅ Работиш с **.NET 6+**  
✅ Нуждаеш се от **минимална памет**  
✅ **Нов проект** (greenfield)  
✅ Не искаш **external dependencies**  
✅ Стандартно JSON (без екзотични случаи)  
✅ **High-throughput** приложения

**Примери:**

- REST APIs (.NET 6+)
- Microservices
- High-performance системи
- Cloud-native applications

---

## 📈 Миграция от Newtonsoft към System.Text.Json

### Лесни за мигриране:

- ✅ Основна сериализация/десериализация
- ✅ Null handling
- ✅ Enum като strings
- ✅ Property naming policies

### Изискват внимание:

- ⚠️ Dynamic objects → трябва да се пренапишат
- ⚠️ Custom date formats → custom converters
- ⚠️ Custom converters → различен API
- ⚠️ Circular references → работи само от .NET 6+

### Препоръка:

**Не мигрирай** legacy проекти без нужда!  
**Използвай System.Text.Json** за нови проекти.

---

## 🎓 Заключение

### Newtonsoft.Json е като Swiss Army Knife

- Има tool за всичко
- Зрял и стабилен
- Лесен за научаване
- Малко по-бавен

### System.Text.Json е като Race Car

- Фокусиран и бърз
- Модерен дизайн
- По-малко features
- По-добър за нови проекти

### Окончателна препоръка:

- **Нов проект .NET 6+** → System.Text.Json
- **Legacy код** → Остани с Newtonsoft
- **Не знаеш** → Започни с System.Text.Json, падни на Newtonsoft ако трябва

И двете библиотеки са отлични. Изборът зависи от конкретните нужди! 🎯
