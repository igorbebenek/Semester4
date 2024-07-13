import numpy as np
import pandas as pd

sciezka = "C:\\Users\\igorb\\Downloads\\zoo.csv"
dane = pd.read_csv(sciezka)
# Zad 1
def freq(x, prob=True):
    counts = x.value_counts(normalize=prob).sort_index() #gdy normalize jest na true, to zwraca prawdopodobienstwa wystapienia kazdej wartosc, na false zwraca bezwzgledne liczby wystapien
    xi = counts.index.tolist()
    ni = counts.values.tolist()
    return xi, ni

print("Zadanie 1")

x = pd.Series(np.random.randint(1, 11, size=50))  # pd.series to jak hashtable
xi_freq, ni_freq = freq(x, prob=False) #lista unikalnych wartosci i lista liczby wystapien tych wartosci
xi_prob, ni_prob = freq(x, prob=True) #ni_prob - p wystapienia xi

print("\nkolumna danych x")
print("unikalne wartości xi:", xi_freq)
print("prawdopodobieństwo wystąpienia każdej z tych wartości:", ni_prob)
print("częstość występowania tych wartości:", ni_freq)

#Zad2
def freq2(x, y, prob=True):
    ni = pd.crosstab(x, y) #crosstab tworzy dataframe, x indeks y wartosci
    pi = ni / ni.values.sum() if prob else ni
    xi = ni.index.values    #pobiera wartosci x
    yi = ni.columns.values
    ni_values = ni.to_numpy() #dajemy do tablicy
    pi_values = pi.to_numpy()
    return xi, yi, pi_values if prob else ni_values


print("Zadanie 2")

x = pd.array(np.random.randint(1, 11, size=100))
y = pd.array(np.random.randint(1, 11, size=100))

[xi, yi, ni] = freq2(x, y, prob=True)

# Wypisanie wyników
print("kolumna x:", x[:10])
print("kolumna y:", y[:10])
print("\n xi unikalne wartości x:", xi)
print("yi unikalne wartości y:", yi)
print("ni:")
print(ni)

#Zadanie 3
def entropy(probabilities):
    probabilities = np.array(probabilities)
    probabilities = probabilities[probabilities > 0]  # usuwamy zerowe probabilities
    entropy = -np.sum(probabilities * np.log2(probabilities)) #wzor
    return entropy

def infogain(x, y):
    _, px = freq(x, prob=True) # _ czyli ignorujemy pierwsza wartosc ktora zwraca funkcja
    _, py = freq(y, prob=True)
    _, _, pxy = freq2(x, y, prob=True)

    pxy = pxy.flatten() #splaszczenie dwuwymiarowej tablicy na jednowymairowa
    Hx = entropy(px)
    Hy = entropy(py)
    Hxy = entropy(pxy)
    infogain = Hx + Hy - Hxy
    return infogain


print("Zadanie 3")
x = pd.Series([1, 5, 7, 5, 2])
y = pd.Series(['a', 'a', 'a', 'b', 'b'])

print("Entropia dla x:", entropy(freq(x, prob=True)[1]))
print("Entropia dla y:", entropy(freq(y, prob=True)[1]))

print("Przyrost informacji dla x, y:", infogain(x, y))

print("Zadanie 4")

for kolumna in dane.columns:
    _, prawdopodobienstwa = freq(dane[kolumna], prob=True)
    entropia_kolumny = entropy(prawdopodobienstwa)
    print(f"Entropia dla {kolumna}: {entropia_kolumny:.6f}")
    przyrost_informacji = infogain(dane[kolumna], dane['type']) #wzgledem type
    print(f"Przyrost informacji dla {kolumna}: {przyrost_informacji:.4f}")



