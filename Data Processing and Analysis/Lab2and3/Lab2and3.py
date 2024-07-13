import pandas as pd
import numpy as np
import scipy.stats
import scipy
import matplotlib.pyplot as plt
from scipy.stats import gaussian_kde
from scipy import stats
#
# indeks = pd.date_range('2020-01-01 00:00:00',periods=5,freq='D')
# print(indeks)
# df = pd.DataFrame(np.random.rand(5,3), index= indeks, columns = ['A','B','C'])
# print(df)
#
#
#
#
# #Wygeneruj tabele
# df_new = pd.DataFrame(np.random.randint(0, 100, size=(20, 3)), columns=list('ABC'))
# trzy_pierwsze_wiersze = df_new.head(3)
# ostatnie_trzy_wiersze = df_new.tail(3)
# print("Pełna tabela:\n", df_new)
# print("Pierwsze trzy wiersze: \n",trzy_pierwsze_wiersze )
# print("Ostatnie trzy wiersze: \n", ostatnie_trzy_wiersze)
# #Nazwa indeksu tabeli
# print("Nazwa indeksu tabeli: \n",df_new.index.name)
# #Wyświetl nazwy kolumn
# print("Nazwy kolumn tabeli:", df_new.columns.tolist())
# #Wyświetl tylko dane tabeli bez indeksów i nagłówków kolumn,
# print("Dane tabeli bez indeksów i nagłówków kolumn: \n",df_new.to_string(index=False,header=False))
# #Pięc losowych wierszy
# print("Pięc losowych wierszy: \n",df_new.sample(5))
# #Wybierz tylko wartości kolumny 'A' a nasatępnie tylko 'A' i 'B' (korzystanie z values)
# print("Wartosci kolumny a: \n", df['A'].values)
# print("Wartosci kolumny A i B: \n", df[['A','B']].values)
# #Zapoznaj się z funkcją iloc i wyświetl: trzy pierwsze wiersze i kolumny 'A' i 'B', wiersz piąty, wiersze 0,5,6,7 i kolumny 1 i 2
# print("Trzy pierwsze wiersze i kolumny \n", df.iloc[:3,:2])
# print("Piąty wiersz: \n",df.iloc[4])
# # DO OGARNIĘCIA
# #print("Wiersze 0,5,6,7 i kolumny 1 i 2", df.iloc[[0,5,6,7],[1,2]])
#
# #Zapoznanie się z funkcją describe, wyświetl podstawowe statystyki tabeli
#
# #sprawdz ktore dane wt tabeli sa wieksze od 0
# #wyświetlenie podstawowych statystyk z tabeli
# print( df.describe())
# #które większe od 0
# print("Większe od 0: \n",df>0)
# print("Wartosci wiekszych od 0: \n", df[df>0])
# #Wybierz z kolumny 'A' tylko dane większe od 0
# print("Dane większe od 0 tylko z kolumny A", df['A'][df['A'] > 0])
# #Policz średnią w kolumnach
# print("Średnia w kolumnach: \n",df.mean())
# #Policz średnią w wierszach
# print("Średnia w wierszach: \n", df.mean(axis=1))
#
#
#
# #Zad 9
# #Wskazówki
# #wykres dla probek -> 0x=len, y= kde(len)
# # wykres esytmatora -> (x=linspace, y= kde(linspace)

# ZADANIA PODSUMOWUJĄCE

#Zadanie 1 grupowanie po Y, srednia X w grupach wyznaczonych przez Y
df = pd.DataFrame({"x": [1, 2, 3, 4, 5], "y": ['a', 'b', 'a', 'b', 'b']})

# Grupowanie danych po 'y' i obliczanie średniej dla 'x'
print(df.groupby('y')['x'].mean()) #dzielimy grupy na podstawie wartosci w Y, obliczamy srednie dla kazdych z tych grup w kolumnie x

#Zadanie 2 rozklad licznosci atrybutow
print("Rozkład liczności dla x: ", df['x'].value_counts()) #liczy wystąpienia każdej unikalnej wartości w kolumnie 'x' i zwraca ile razy sie powtrzyla dana wartosc

print("Rozkład liczności dla y: ", df['y'].value_counts())

#Zadanie 3 wczytanie danych
sciezka_pliku = r'C:\Users\igorb\Desktop\autos.csv'

#read csv automatycznie rozpoznaje pierwszy wiersz jako naglowek, w loadtxt trzeba uzyc skiprows, read csv automatycznie przypisuej typy do roznych kolumn, lepsze do excela

print("Wczytanie za pomocą read_csv:", pd.read_csv(sciezka_pliku))

#dane jednego typu czyli string, delimiter - znak do rodzielenia kolumn w pliku tekstowym
print("Wczytanie za pomocą loadtxt: ", np.loadtxt(sciezka_pliku, delimiter=',', dtype=str,skiprows = 1))

#Zadanie 4 Zgrupowac po zmiennej make i wyznaczyc srednie zuzycie paliwa dla kazdego z producentow
df = pd.read_csv(sciezka_pliku)
srednie_zuzycie_w_miescie = df.groupby('make')['city-mpg'].mean() #grupujemy wedlug marki, a następnie liczymy średnie zużycie paliwa w mieście (city mpg) dla każdej marki.
print("Średnie zużycie paliwa w mieście dla każdej marki:")
print(srednie_zuzycie_w_miescie)
srednie_na_autostradzie = df.groupby('make')['highway-mpg'].mean()
print("Średnie zużycie na autostradzie: ", srednie_na_autostradzie)
srednie_ogolnie = (srednie_zuzycie_w_miescie + srednie_na_autostradzie) / 2
print("Srednia ogólna: ", srednie_ogolnie)

#Zadanie 5  Zgrupowac po zmiennej make licznosci dla fuel-type
licznosc_dla_paliwa = df.groupby(['make', 'fuel-type']).size()

print("Licznosc typow paliwa w markach: ", licznosc_dla_paliwa)

#Zadanie 6
length = df['length'].values
city_mpg = df['city-mpg'].values #values konwertowalo na tablice z numpy

dopas_1 = np.polyfit(length, city_mpg, 1) #polyfit - dopasowujemy wielomian (oś x, y i stopien)
#liczenie przewidywanych wartosci wielomianu 1 stopnia
przewidywane_wartosci_1 = np.polyval(dopas_1, length) #przyjmuje

dopas_2 = np.polyfit(length, city_mpg, 2)
#liczenie przewidywanych wartosci wielomianu 2 stopnia
przewidywane_wartosci_2 = np.polyval(dopas_2, length) #polyval przyjmuje wspolczynniki wielomianu, wartosci punktow

print("Współczynniki dla 1 stopnia: ", dopas_1)
print("Współczynniki dla 2 stopnia: ", dopas_2)
#print("Przewidywane wartosci 1", przewidywane_wartosci_1)
#print("Przewidywane wartosci 2", przewidywane_wartosci_2)

#Zadanie 7
wspolczynnik_korelacji, istotnosc = stats.pearsonr(length, city_mpg)
#zwraca wartość między -1 a 1, która określa siłę związku między zmiennymi i p-value czyli istotnosc
print("Współczynnik korelacji:", wspolczynnik_korelacji)
print("Istotnosc: ", istotnosc)

#Zadanie 8
plt.scatter(length, city_mpg, label='Próbki')
plt.plot(length, przewidywane_wartosci_1, color='blue', label='wielomian najlepszego dopasowania')
plt.xlabel('length')
plt.ylabel('city-mpg')
plt.title('Wizualizacja wynikow dopasowania')
plt.legend()
plt.show()

#Zadanie 9
kde = gaussian_kde(length) #tworzenie estymatora

x_values = np.linspace(min(length), max(length), 1000)
plt.figure(figsize=(8, 6)) #wielkosc 8 na 6 cali

plt.plot(x_values, kde(x_values), label='Estymator gęstości')

plt.scatter(length, kde(length), color='green', label='Próbki')

plt.legend()
plt.show()

# Zadanie 10
width = df['width'].values

estymator_gestosci_dlugosci = gaussian_kde(length)
estymator_dlugosci_szerokosci = gaussian_kde(width)

x_length = np.linspace(min(length), max(length), 1000) #zakresy dla estymatorow
x_width = np.linspace(min(width), max(width), 1000)

plt.figure(figsize=(8, 12)) #okno na dwa wykresy

# wykres length
plt.subplot(2, 1, 1) # 2 to liczba wierszoy w siatce wykresow
plt.plot(x_length, estymator_gestosci_dlugosci(x_length))
plt.scatter(length, estymator_gestosci_dlugosci(length), color='red')

# wykres width
plt.subplot(2, 1, 2)
plt.plot(x_width, estymator_dlugosci_szerokosci(x_width))
plt.scatter(width, estymator_dlugosci_szerokosci(width), color='green')

plt.tight_layout() #etykiety itp nie nachodza na siebie dzieki temu
plt.show()


# Zadanie 11
width_dane = df['width']
length_dane = df['length']

dwuwymiarowy_estymator = scipy.stats.gaussian_kde([length_dane, width_dane])

# tworzymy dwie macierze meshgridem
siatka_x, siatka_y = np.meshgrid(np.linspace(min(length_dane), max(length_dane), 100),
                                 np.linspace(min(width_dane), max(width_dane), 100))
wspolrzedne_siatki = np.vstack([siatka_x.ravel(), siatka_y.ravel()]) #ravel splaszcza macierz do jednego wymiaru, laczymy macierz w pionie vstackiem

# Obliczanie funkcji gęstości na siatce punktów
wartosci_gestosci = dwuwymiarowy_estymator(wspolrzedne_siatki) #obliczamy gestosc dla kazdego punktu na siatce
wartosci_gestosci = wartosci_gestosci.reshape(siatka_x.shape) #dopasowujemy kszalt wartosci_gestosci do ksztaltu macierzy siatka_x

plt.figure()

plt.contour(siatka_x, siatka_y, wartosci_gestosci, cmap='magma') # rysujemy wykres gestosci i uzywamy kolorow mapy magma

plt.plot(length_dane, width_dane, 'k.', markersize=2, label='Próbki') # k- czarny kolor  . - małe kropki

plt.savefig('gestosc_wykres.png')

plt.savefig('gestosc_wykres.pdf')
plt.show()
























