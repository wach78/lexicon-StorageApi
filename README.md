## DEL 5

## Vilka metoder har genererats i `ProductsController`?

Scaffoldingen har genererat en enkel CRUD-controller för `Product`.

CRUD betyder:

| Begrepp | Betydelse                |
| ------- | ------------------------ |
| Create  | Skapa ny data            |
| Read    | Läsa/hämta data          |
| Update  | Uppdatera befintlig data |
| Delete  | Ta bort data             |

### Genererade metoder

| Metod                                 | HTTP-metod       | Route                | Vad den gör                                  |
| ------------------------------------- | ---------------- | -------------------- | -------------------------------------------- |
| `GetProduct()`                        | `GET`            | `/api/Products`      | Hämtar alla produkter                        |
| `GetProduct(int id)`                  | `GET`            | `/api/Products/{id}` | Hämtar en produkt med ett specifikt id       |
| `PostProduct(Product product)`        | `POST`           | `/api/Products`      | Skapar en ny produkt                         |
| `PutProduct(int id, Product product)` | `PUT`            | `/api/Products/{id}` | Uppdaterar en befintlig produkt              |
| `DeleteProduct(int id)`               | `DELETE`         | `/api/Products/{id}` | Tar bort en produkt                          |
| `ProductExists(int id)`               | Ingen HTTP-route | Intern metod         | Kontrollerar om en produkt finns i databasen |


# Hur används StorageContext?

`StorageContext` används som en **EF Core DbContext**.

Det är klassen som kopplar C#-koden till databasen. Den används för att:

* hämta data från databasen
* lägga till ny data
* uppdatera befintlig data
* ta bort data
* spara ändringar med `SaveChangesAsync()`

En `DbContext` kan ses som en session mot databasen. Den håller reda på vilka entities som hämtas, ändras eller ska sparas.

Exempel:

```csharp
private readonly StorageApiContext _context;

public ProductsController(StorageApiContext context)
{
    _context = context;
}
```

Här får controllern tillgång till databasen genom `_context`.

Exempel på användning:

```csharp
return await _context.Product.ToListAsync();
```

Det hämtar alla produkter från databasen.

---

# Hur fungerar `CreatedAtAction`, `Ok`, `NotFound` osv?

Metoder som `Ok()`, `NotFound()`, `BadRequest()`, `NoContent()` och `CreatedAtAction()` används för att skapa HTTP-svar från API:t.

De bestämmer vilken HTTP-statuskod klienten får tillbaka.

---

## `Ok()`

```csharp
return Ok(product);
```

Skickar:

```http
200 OK
```

Används när requesten lyckades och API:t returnerar data.

Exempel:

```text
Produkten hittades och skickas tillbaka som JSON.
```

---

## `NotFound()`

```csharp
return NotFound();
```

Skickar:

```http
404 Not Found
```

Används när requesten är korrekt, men resursen inte finns.

Exempel:

```text
Produkten med detta id finns inte.
```

---

## `BadRequest()`

```csharp
return BadRequest();
```

Skickar:

```http
400 Bad Request
```

Används när klienten skickar felaktig data.

Exempel:

```text
Id i URL:en matchar inte id i JSON-body.
```

---

## `NoContent()`

```csharp
return NoContent();
```

Skickar:

```http
204 No Content
```

Används när operationen lyckades, men API:t inte behöver returnera någon data.

Vanligt vid:

* `PUT`
* `DELETE`

Exempel:

```text
Produkten togs bort, men inget objekt skickas tillbaka.
```

---

## `CreatedAtAction()`

`CreatedAtAction()` används oftast vid `POST`, när API:t har skapat en ny resurs.

Exempel:

```csharp
return CreatedAtAction(
    nameof(GetProduct),
    new { id = product.Id },
    product
);
```

Den gör tre saker:

1. Returnerar HTTP `201 Created`
2. Skickar tillbaka det skapade objektet som JSON
3. Skapar en `Location`-header till den nya resursen

Exempel på svar:

```http
201 Created
Location: https://localhost:7252/api/Products/6
```

Det betyder att produkten skapades och kan hämtas via länken i `Location`.

---
