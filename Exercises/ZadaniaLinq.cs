using LinqConsoleLab.PL.Data;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    //W DANYCH NIE MA NULI
    /// <summary>
    /// Zadanie:
    /// Wyszukaj wszystkich studentów mieszkających w Warsaw.
    /// Zwróć numer indeksu, pełne imię i nazwisko oraz miasto.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko, Miasto
    /// FROM Studenci
    /// WHERE Miasto = 'Warsaw';
    /// </summary>
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    { //dziala
        return DaneUczelni.Studenci
            .Where(s => s.Miasto == "Warsaw")
            .Select(s => $"{s.NumerIndeksu} | {s.Imie} {s.Nazwisko} | {s.Miasto}");
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj listę adresów e-mail wszystkich studentów.
    /// Użyj projekcji, tak aby w wyniku nie zwracać całych obiektów.
    ///
    /// SQL:
    /// SELECT Email
    /// FROM Studenci;
    /// </summary>
    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        return DaneUczelni.Studenci.Select(s => s.Email);
    }

    /// <summary>
    /// Zadanie:
    /// Posortuj studentów alfabetycznie po nazwisku, a następnie po imieniu.
    /// Zwróć numer indeksu i pełne imię i nazwisko.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko
    /// FROM Studenci
    /// ORDER BY Nazwisko, Imie;
    /// </summary>
    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        return DaneUczelni.Studenci.OrderBy(s => s.Nazwisko).ThenBy(s => s.Imie)
            .Select(s => $"{s.NumerIndeksu} | {s.Imie} {s.Nazwisko} ");
    }

    /// <summary>
    /// Zadanie:
    /// Znajdź pierwszy przedmiot z kategorii Analytics.
    /// Jeżeli taki przedmiot nie istnieje, zwróć komunikat tekstowy.
    ///
    /// SQL:
    /// SELECT TOP 1 Nazwa, DataStartu
    /// FROM Przedmioty
    /// WHERE Kategoria = 'Analytics';
    /// </summary>
    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {//widze ze zadanie narzuca analitics w wyborze i wiem ze bedzie pozycja z danych ale zrobie z rozpatrzeniem nula
        var przedmiot = DaneUczelni.Przedmioty.FirstOrDefault(p => p.Kategoria == "Analytics");
        if (przedmiot == null)
        {
            return ["Brak przedmiotu z kategorii Analytics"];
        }

        return [$"{przedmiot.Nazwa} | start: {przedmiot.DataStartu:yy-MM-dd}"];
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy w danych istnieje przynajmniej jeden nieaktywny zapis.
    /// Zwróć jedno zdanie z odpowiedzią True/False albo Tak/Nie.
    ///
    /// SQL:
    /// SELECT CASE WHEN EXISTS (
    ///     SELECT 1
    ///     FROM Zapisy
    ///     WHERE CzyAktywny = 0
    /// ) THEN 1 ELSE 0 END;
    /// </summary>
    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        var czyIstniej = DaneUczelni.Zapisy.Any(z => !z.CzyAktywny);
        return [$"Czy istnieje chodz jednen niaktyny zapis: {czyIstniej}"];
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy każdy prowadzący ma uzupełnioną nazwę katedry.
    /// Warto użyć metody, która weryfikuje warunek dla całej kolekcji.
    /// moj kom: w szablonie każdy ma, ale sprawdzić trza
    /// SQL:
    /// SELECT CASE WHEN COUNT(*) = COUNT(Katedra)
    /// THEN 1 ELSE 0 END
    /// FROM Prowadzacy;
    /// </summary>
    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {//zamiast !null sprawdzamy też same spacje w katedrach
        var czyWszyscyeMajaKatedre = DaneUczelni.Prowadzacy
            .All(p => !string.IsNullOrWhiteSpace(p.Katedra));
        return [$"Czy wszyscy prowadzacy maja katedre (All): {czyWszyscyeMajaKatedre}"];
    }

    /// <summary>
    /// Zadanie:
    /// Policz, ile aktywnych zapisów znajduje się w systemie.
    ///
    /// SQL:
    /// SELECT COUNT(*)
    /// FROM Zapisy
    /// WHERE CzyAktywny = 1;
    /// </summary>
    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        var liczbaAktywnychZapisow = DaneUczelni.Zapisy.Count(z => z.CzyAktywny);
        return [$"Liczba aktywnych zapisow: {liczbaAktywnychZapisow}"];
    }

    /// <summary>
    /// Zadanie:
    /// Pobierz listę unikalnych miast studentów i posortuj ją rosnąco.
    ///
    /// SQL:
    /// SELECT DISTINCT Miasto
    /// FROM Studenci
    /// ORDER BY Miasto;
    /// </summary>
    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        return DaneUczelni.Studenci.Select(s => s.Miasto).Distinct()
            .OrderBy(miasto => miasto);
    }

    /// <summary>
    /// Zadanie:
    /// Zwróć trzy najnowsze zapisy na przedmioty. (sortowanie po dacie)
    /// W wyniku pokaż datę zapisu, identyfikator studenta i identyfikator przedmiotu.
    ///
    /// SQL:
    /// SELECT TOP 3 DataZapisu, StudentId, PrzedmiotId
    /// FROM Zapisy
    /// ORDER BY DataZapisu DESC;
    /// </summary>
    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        return DaneUczelni.Zapisy
            .OrderByDescending(z => z.DataZapisu)
            .Take(3)
            .Select(z => $"{z.DataZapisu:yy-MM-dd} | " +
                                 $"StudentId: {z.StudentId} " +
                                 $"| PrzedmiotId: {z.PrzedmiotId}");
    }

    /// <summary>
    /// Zadanie:
    /// Zaimplementuj prostą paginację dla listy przedmiotów.
    /// Załóż stronę o rozmiarze 2 i zwróć drugą stronę danych.
    ///
    /// SQL:
    /// SELECT Nazwa, Kategoria
    /// FROM Przedmioty
    /// ORDER BY Nazwa
    /// OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
        return DaneUczelni.Przedmioty
            .OrderBy(p => p.Nazwa)
            .Skip(2)
            .Take(2)
            .Select(p => $"{p.Nazwa} | {p.Kategoria}");
    }

    /// <summary>
    /// Zadanie:
    /// Połącz studentów z zapisami po StudentId.
    /// Zwróć pełne imię i nazwisko studenta oraz datę zapisu.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, z.DataZapisu
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId;
    /// </summary>
    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        return DaneUczelni.Studenci
            .Join(DaneUczelni.Zapisy,
                student => student.Id,
                zapis => zapis.StudentId,
                ((student, zapis) => $"{student.Imie} {student.Nazwisko} |" +
                                     $"PrzedmiotId: {zapis.PrzedmiotId}"));
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj wszystkie pary student-przedmiot na podstawie zapisów.
    /// Użyj podejścia, które pozwoli spłaszczyć dane do jednej sekwencji wyników.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, p.Nazwa
    /// FROM Zapisy z
    /// JOIN Studenci s ON s.Id = z.StudentId
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId;
    /// </summary>
    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
        return DaneUczelni.Studenci
            .SelectMany(student =>
                    DaneUczelni.Zapisy
                        .Where(z => z.StudentId == student.Id),
                (student, zapis) => new { student, zapis }
            )
            .Join(
                DaneUczelni.Przedmioty,
                temp => temp.zapis.PrzedmiotId,
                przedmiot => przedmiot.Id,
                (temp, przedmiot) =>
                    $"{temp.student.Imie} {temp.student.Nazwisko} | {przedmiot.Nazwa}"
            );
    }

    /// <summary>
    /// Zadanie:
    /// Pogrupuj zapisy według przedmiotu i zwróć nazwę przedmiotu oraz liczbę zapisów.
    ///
    /// SQL:
    /// SELECT p.Nazwa, COUNT(*)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        return DaneUczelni.Zapisy
            .Join(DaneUczelni.Przedmioty,
                zapis => zapis.PrzedmiotId,
                przedmiot => przedmiot.Id,
                (zapis, przedmiot) => new { przedmiot.Nazwa })
            .GroupBy(x => x.Nazwa)
            .Select(grupa => $"{grupa.Key} | Liczba zapisów: {grupa.Count()}");
    }

    /// <summary>
    /// Zadanie:
    /// Oblicz średnią ocenę końcową dla każdego przedmiotu.
    /// Pomiń rekordy, w których ocena końcowa ma wartość null.
    ///
    /// SQL:
    /// SELECT p.Nazwa, AVG(z.OcenaKoncowa)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        return DaneUczelni.Zapisy
            .Where(z => z.OcenaKoncowa.HasValue)
            .Join(
                DaneUczelni.Przedmioty,
                zapis => zapis.PrzedmiotId,
                przedmiot => przedmiot.Id,
                (zapis, przedmiot) => new
                {
                    przedmiot.Nazwa,
                    Ocena = zapis.OcenaKoncowa!.Value
                }
            )
            .GroupBy(x => x.Nazwa)
            .Select(grupa => $"{grupa.Key} | Średnia ocena: {grupa.Average(x => x.Ocena):0.00}");
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego prowadzącego policz liczbę przypisanych przedmiotów.
    /// W wyniku zwróć pełne imię i nazwisko oraz liczbę przedmiotów.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, COUNT(p.Id)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        return DaneUczelni.Prowadzacy
            .GroupJoin(
                DaneUczelni.Przedmioty,
                prowadzacy => prowadzacy.Id,
                przedmiot => przedmiot.ProwadzacyId,
                (prowadzacy, przedmioty) =>
                    $"{prowadzacy.Imie} {prowadzacy.Nazwisko} | Liczba przedmiotów: {przedmioty.Count()}"
            );
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego studenta znajdź jego najwyższą ocenę końcową.
    /// Pomiń studentów, którzy nie mają jeszcze żadnej oceny.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, MAX(z.OcenaKoncowa)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY s.Imie, s.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        return DaneUczelni.Zapisy
            .Where(z => z.OcenaKoncowa.HasValue)
            .Join(
                DaneUczelni.Studenci,
                zapis => zapis.StudentId,
                student => student.Id,
                (zapis, student) => new
                {
                    student.Imie,
                    student.Nazwisko,
                    Ocena = zapis.OcenaKoncowa!.Value
                }
            )
            .GroupBy(x => new { x.Imie, x.Nazwisko })
            .Select(grupa => $"{grupa.Key.Imie} {grupa.Key.Nazwisko} | Najwyższa ocena: {grupa.Max(x => x.Ocena):0.0}");
    }

    /// <summary>
    /// Wyzwanie:
    /// Znajdź studentów, którzy mają więcej niż jeden aktywny zapis.
    /// Zwróć pełne imię i nazwisko oraz liczbę aktywnych przedmiotów.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Imie, s.Nazwisko
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        return DaneUczelni.Zapisy
            .Where(z => z.CzyAktywny)
            .GroupBy(z => z.StudentId)
            .Where(grupa => grupa.Count() > 1)
            .Join(
                DaneUczelni.Studenci,
                grupa => grupa.Key,
                student => student.Id,
                (grupa, student) =>
                    $"{student.Imie} {student.Nazwisko} | Aktywne przedmioty: {grupa.Count()}"
            );
    }

    /// <summary>
    /// Wyzwanie:
    /// Wypisz przedmioty startujące w kwietniu 2026, dla których żaden zapis nie ma jeszcze oceny końcowej.
    ///
    /// SQL:
    /// SELECT p.Nazwa
    /// FROM Przedmioty p
    /// JOIN Zapisy z ON p.Id = z.PrzedmiotId
    /// WHERE MONTH(p.DataStartu) = 4 AND YEAR(p.DataStartu) = 2026
    /// GROUP BY p.Nazwa
    /// HAVING SUM(CASE WHEN z.OcenaKoncowa IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {//czyli szukam przedmiot-zapis, gdzie dla przedmiotu nie ma ani jednej ocenki koncowej (10min na interpretacje polecenia .xd)
        return DaneUczelni.Przedmioty
            .Where(p => p.DataStartu.Month == 4 && p.DataStartu.Year == 2026)
            .GroupJoin(
                DaneUczelni.Zapisy,
                p => p.Id,
                z => z.PrzedmiotId,
                (p, zapisy) => new {p, zapisy})
            .Where(x => x.zapisy.All(z => z.OcenaKoncowa == null))
            .Select(x => x.p.Nazwa);
        //x=p,zapis
    }

    /// <summary>
    /// Wyzwanie:
    /// Oblicz średnią ocen końcowych dla każdego prowadzącego na podstawie wszystkich jego przedmiotów.
    /// Pomiń brakujące oceny, ale pozostaw samych prowadzących w wyniku.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, AVG(z.OcenaKoncowa)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// LEFT JOIN Zapisy z ON z.PrzedmiotId = p.Id
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {//wszyskie oceny z wszystki przedmiotów jednego prowadzącego do jednego wora i średnia
        //1morawska (4,5+3,5+5+4,5)/4 = 4,375 sowa 
        return DaneUczelni.Prowadzacy
            .Join(
                DaneUczelni.Przedmioty,
                prowadzacy => prowadzacy.Id,
                przedmiot => przedmiot.ProwadzacyId,
                (prowadzacy, przedmiot) => new { prowadzacy, przedmiot }
            )
            .Join(
                DaneUczelni.Zapisy.Where(z => z.OcenaKoncowa.HasValue),
                x => x.przedmiot.Id,
                zapis => zapis.PrzedmiotId,
                (x, zapis) => new
                {
                    x.prowadzacy.Imie,
                    x.prowadzacy.Nazwisko,
                    Ocena = zapis.OcenaKoncowa!.Value
                }
            )
            .GroupBy(x => new { x.Imie, x.Nazwisko })
            .Select(grupa => $"{grupa.Key.Imie} {grupa.Key.Nazwisko} | Średnia: {grupa.Average(x => x.Ocena):0.00}");
    }

    /// <summary>
    /// Wyzwanie:
    /// Pokaż miasta studentów oraz liczbę aktywnych zapisów wykonanych przez studentów z danego miasta.
    /// Posortuj wynik malejąco po liczbie aktywnych zapisów.
    ///
    /// SQL:
    /// SELECT s.Miasto, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Miasto
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {//miasto-l zapisow (NIE STUDENTOW) z miasta w zapisach - sort malejąco po tej liczbie
        return DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy,
                s => s.Id,
                z => z.StudentId,
                (s, z) => new { s.Miasto, z.CzyAktywny }
            )
            .Where(x => x.CzyAktywny)
            .GroupBy(x => x.Miasto)
            .OrderByDescending(grupa => grupa.Count())
            .Select(grupa => $"{grupa.Key} | Aktywne zapisy: {grupa.Count()}");
    }

    private static NotImplementedException Niezaimplementowano(string nazwaMetody)
    {
        return new NotImplementedException(
            $"Uzupełnij metodę {nazwaMetody} w pliku Exercises/ZadaniaLinq.cs i uruchom polecenie ponownie.");
    }
}
