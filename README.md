# JSON Libraries Comparison: Newtonsoft.Json vs System.Text.Json

Курсова работа по ПРС/РПЗ 2025  
Тема 3: Сравнение на Newtonsoft.Json и System.Text.Json

## 📁 Структура на проекта

```
JsonComparison/
├── JsonComparison.sln
├── JsonComparison.Common/          # Споделени модели (Person, Address, Product)
├── JsonComparison.Newtonsoft/      # Newtonsoft.Json демонстрации
├── JsonComparison.SystemText/      # System.Text.Json демонстрации
├── JsonComparison.Benchmarks/      # Performance тестове (BenchmarkDotNet)
├── DemoData/                       # JSON тестови файлове
│   ├── simple-data.json
│   └── complex-data.json
├── Comparison.md                   # Детайлно сравнение
└── README.md                       # Този файл
```

## 🚀 Как да пусна проекта?

### Предварителни изисквания

- .NET 8.0 SDK или по-нова версия
- Visual Studio 2022 / VS Code / Rider

### Стъпки

1. **Clone или разархивирай проекта**

```bash
   cd JsonComparison
```

2. **Restore NuGet packages**

```bash
   dotnet restore
```

3. **Build solution**

```bash
   dotnet build
```

4. **Пусни Newtonsoft примерите**

```bash
   cd JsonComparison.Newtonsoft
   dotnet run
```

5. **Пусни System.Text.Json примерите**

```bash
   cd JsonComparison.SystemText
   dotnet run
```

6. **Пусни Performance benchmarks** ⏱️ (отнема време!)

```bash
   cd JsonComparison.Benchmarks
   dotnet run -c Release
```

## 📊 Какво демонстрират примерите?

### JsonComparison.Newtonsoft

- ✅ Основни операции (serialize/deserialize)
- ✅ Circular references handling
- ✅ Dynamic objects support
- ✅ Enum serialization (string/number)
- ✅ Custom date formats
- ✅ Null value handling

### JsonComparison.SystemText

- ✅ Основни операции (serialize/deserialize)
- ✅ Circular references handling (.NET 6+)
- ✅ Dynamic objects via JsonDocument
- ✅ Enum serialization with converter
- ✅ Date handling (ISO 8601)
- ✅ Null value handling
- ✅ Performance comparison

### JsonComparison.Benchmarks

- ⚡ Serialization performance
- ⚡ Deserialization performance
- ⚡ Round-trip performance
- 💾 Memory allocation comparison

## 📈 Основни резултати

| Метрика             | Newtonsoft.Json | System.Text.Json | Разлика             |
| ------------------- | --------------- | ---------------- | ------------------- |
| Serialization Speed | Baseline        | ~3x по-бързо     | ✅ System.Text.Json |
| Memory Usage        | Baseline        | ~65% по-малко    | ✅ System.Text.Json |
| API Flexibility     | Много богат     | Минималистичен   | ✅ Newtonsoft.Json  |
| Dynamic Support     | Отличен         | Ограничен        | ✅ Newtonsoft.Json  |

**Пълни резултати:** Виж `Comparison.md`

## 🎯 Кога да използваме какво?

### Newtonsoft.Json

- Legacy проекти
- Dynamic JSON objects
- Custom date formats
- Много специални случаи

### System.Text.Json

- Нови проекти (.NET 6+)
- High performance нужди
- Минимална памет
- Standard JSON

## 📚 Използвани технологии

- .NET 8.0
- C# 12
- Newtonsoft.Json 13.0.3
- System.Text.Json (built-in)
- BenchmarkDotNet 0.13.x

## 👤 Автор

[Твоето име]  
Курсова работа по ПРС/РПЗ 2025  
[Дата]

## 📄 Лиценз

Образователен проект - ФМИ, СУ "Св. Климент Охридски"
