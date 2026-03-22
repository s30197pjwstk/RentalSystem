Wypożyczalnia Sprzętu Uczelnianego (APBD)

Aplikacja konsolowa w C# realizująca system zarządzania wypożyczalnią sprzętu.



Zgodnie z wymogami zadania kod jest podzielony w następujący sposób:

1. Wysoka Spójność (High Cohesion) i Podział Odpowiedzialności (Single Responsibility):
    - Domenowe modele (`Equipment`, `User`, `Rental`) przechowują tylko stan i podstawowe zachowania bezpośrednio z nimi związane (np. `Rental.MarkAsReturned()`).
    - `RentalService` zajmuje się wyłącznie orkiestracją procesów biznesowych (przypisywanie sprzętu do użytkownika, walidacja). Nie ma tam logiki konsolowej (odczytywania wejścia czy wypisywania raportów znak po znaku).
    - Obliczanie kar zostało wyciągnięte do osobnej klasy implementującej `IPenaltyCalculator`.

2. Niskie Sprzężenie (Low Coupling) i Dependency Inversion:
    - `RentalService` nie zależy od konkretnej, twardo zakodowanej logiki kar za opóźnienia. Wstrzykiwany jest przez konstruktor interfejs `IPenaltyCalculator`. Pozwala to w przyszłości łatwo podmienić politykę kar (np. na `WeekendFreePenaltyCalculator`) bez modyfikowania rdzennego serwisu.

3. Rozsądne użycie dziedziczenia i Polimorfizmu (Open/Closed Principle):
    - Limity wypożyczeń zależą od typu użytkownika. Zamiast tworzyć instrukcję `switch` lub `if (user.Type == "Student")` w serwisie (co łamałoby zasadę OCP przy dodawaniu nowego typu użytkownika), limit został zdefiniowany jako abstrakcyjna właściwość w klasie `User`. Klasy `Student` i `Employee` same definiują swoje limity.
    - Dziedziczenie po klasie `Equipment` wymusza implementację metody `GetDetails()` dla unikalnych cech sprzętu (np. Lumens dla projektora, RAM dla laptopa).

4. Jawna obsługa błędów:
    - Gdy warunki biznesowe nie są spełnione (brak wolnego sprzętu, przekroczony limit studenta), system przerywa działanie rzucając `DomainException`. Jest to przechwytywane w interfejsie użytkownika, zapobiegając nielegalnemu stanowi aplikacji.

  Instrukcja uruchomienia:
1. Sklonuj repozytorium.
2. Otwórz terminal w folderze projektu.
3. Wywołaj komendę: `dotnet run`
   