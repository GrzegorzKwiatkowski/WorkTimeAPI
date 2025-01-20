# WorkTimeManagementAPI

Aplikacja do zarządzania czasem pracy pracowników.

## Uruchomienie aplikacji

1. **Zainstaluj Dockera**  
   Jeśli jeszcze tego nie zrobiłeś, pobierz i zainstaluj Dockera z [oficjalnej strony](https://www.docker.com/products/docker-desktop).

2. **Przygotowanie pliku `docker-compose.yml`**  
   W pliku `docker-compose.yml`, który znajduje się w głównym katalogu repozytorium, musisz podać ścieżkę do swojego pliku `init.sql`. Znajdziesz go w katalogu `WorkTimeManagementAPI\WorkTimeManagementAPI`.

   Przykład:
   ```yaml
   volumes:
     - ./WorkTimeManagementAPI/WorkTimeManagementAPI/init.sql:/docker-entrypoint-initdb.d/init.sql
3.Budowanie i uruchamianie aplikacji
Przejdź do katalogu, w którym znajduje się plik docker-compose.yml, a następnie uruchom poniższą komendę:
docker-compose up --build

4.Dostęp do aplikacji
Po uruchomieniu kontenerów, aplikacja będzie dostępna pod adresem:
http://localhost:5000/swagger

5.Gotowe!
Możesz teraz korzystać z API aplikacji.

Powyższa instrukcja powinna wystarczyć do uruchomienia aplikacji z repozytorium.
