# KjbTech.Messaging.Sdk

It provides convenient abstractions for managing contacts, sending messages, and handling API errors in a consistent way.

---

## ✨ Features

- ✅ Simple, asynchronous API (`async/await`)
- ✅ Strongly-typed models (`ContactId`, `ExistingContact`, `Result<T>`, etc.)
- ✅ Unified error handling via `MessagingException` and `Result<T>`
- ✅ HTTP abstraction layer (`MessagingHttpApiBase`) with:
  - JSON serialization using `System.Text.Json`
- ✅ Support for dependency injection

---

## ⚙️ Configuration

You can register the SDK in your `Program.cs`:

```csharp
using KjbTech.Messaging.Sdk;

//IConfiguration configuration;

builder.Services.AddMessaging(configuration);
```

---

## 🚀 Usage

### Retrieve a contact

```csharp
var contact = await contactsApi.GetAsync(new ContactId("12345"));

if (contact is null)
{
    Console.WriteLine("Contact not found.");
}
else
{
    Console.WriteLine($"{contact.Name} ({contact.Phone})");
}
```

---

## ⚠️ Error Handling

All HTTP-level and deserialization errors are wrapped in `MessagingException`.

Example:
```csharp
try
{
    await contactsApi.ListAsync(new PaginationParameter(1, 50));
}
catch (MessagingException ex)
{
    logger.LogError(ex, "Unexpected failure while calling the messaging API");
}
```

Non-fatal business errors (e.g. validation) are returned as `Result.Error`.

---

## 🧰 Development

### Build
```bash
dotnet build
```

### Test
```bash
dotnet test
```

### Code style
- Nullable reference types: **enabled**
- Async naming: all async methods end with `Async`
- HTTP responses always wrapped in `Result<T, HttpResponseMessage>`
