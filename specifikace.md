# Daily Task Reminder

Jednalo by se o program, ve kterém by si uživatel nakonfiguroval nějaké úlohy,
které musí dělat pravidelně (třeba 1x za den, nebo každé pondělí a čtvrtek)
jako například nakrmení domácího mazlíčka.

Bude moct přes webové rozhraní (POST request) označit úlohu jako splněnou a
zobrazit status úlohy (jestli jí už nesplnil někdo jiný).

Bude také možné úlohu nakonfigurovat, tak aby v případě že je ještě nesplněná
X hodin před deadline, bylo posláno upozornění skrze Discord
nebo Telegram bota (případně další), kterého si uživatel vytvoří a poskytne
programu API Token.

## Server

Hlavní část programu. 

Načte úlohy ze souborů. Bude dostávat http dotazy na stav úloh, odpovídat na
ně, zaznamenávat jejich splnění a posílat upozornění.

## Konfigurace

Rozšíření pro snazší konfiguraci.

Grafické uživatelské rozhraní, kde si uživatel nakliká úlohy,
jak často se opakují, jestli chce dostávat upozornění, atd.
aby konfiguraci uživatel nemusel psát ručně.

Výstupem je konfigurační soubor.

## Generator html stránky

Vygeneruje html stránku, která pošle na server dotaz na úlohy a zobrazí
jejich stav. Pro nesplněné úlohy bude také mít tlačítko, které na server
pošle informaci o tom, že úloha byla splněna.

Uživatel si ale samozřejmě bude moct napsat vlastní stránku či aplikaci, která
bude se serverem komunikovat.