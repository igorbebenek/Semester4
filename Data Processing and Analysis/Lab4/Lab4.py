import matplotlib.pyplot as plt
import numpy as np
import pylab as py
from skimage import data
from skimage import filters
from skimage import exposure
from PIL import Image
import matplotlib.image
import matplotlib.image as mpimg

plt.close('all')
#Dyskretyzacja
#Zadanie 1 i 2

def sinus(f, Fs):
    duration = 1
    t = np.arange(0, duration, 1 / Fs)
    s = np.sin(2 * np.pi * f * t)
    return t, s

t, s = sinus(10, 50)

plt.plot(t, s)
plt.show()

#Zadanie 3
t, s = sinus(10, 20)
plt.plot(t, s)
plt.show()

t, s = sinus(10, 21)
plt.plot(t, s)
plt.show()

t, s = sinus(10, 30)
plt.plot(t, s)
plt.show()

t, s = sinus(10, 45)
plt.plot(t, s)
plt.show()

t, s = sinus(10, 50)
plt.plot(t, s)
plt.show()

t, s = sinus(10, 100)
plt.plot(t, s)
plt.show()

t, s = sinus(10, 150)
plt.plot(t, s)
plt.show()

t, s = sinus(10, 200)
plt.plot(t, s)
plt.show()

t, s = sinus(10, 250)
plt.plot(t, s)
plt.show()

t, s = sinus(10, 1000)
plt.plot(t, s)
plt.show()


#ZADANIE 4
#Twierdzenie Nyquista- Shannona. Mówi ono, że aby móc wiernie odtworzyć sygnał analogowy na podstawie jego próbek cyfrowych sygnał musi być prókowany z częstotliwośćią
#ponad dwa razy większa niz najwyzsza czestotliwosc wystepujaca w sygnale

#Zadanie 5
#Zjawisko to nazywamy aliasingiem

#Zadanie 6
sciezka = 'C:\\Users\\igorb\\Downloads\\robal.png'
obrazek = plt.imread(sciezka)

plt.xlim(200, 300)
plt.ylim(230, 300)
plt.imshow(obrazek)
plt.show()


# KWANTYZACJA

#Zadanie 1 i 2

sciezka = 'C:\\Users\\igorb\\Downloads\\robal.png'
obrazek = Image.open(sciezka)
obrazek_array = np.array(obrazek) #konwersja obrazka na macierz
wymiary_macierzy = obrazek_array.ndim #sprawdzenie liczby wymiarów tej macierzy
print("Liczba wymiarów macierzy obrazka: ", wymiary_macierzy)

#Zadanie 3

liczba_wartosci = obrazek_array.shape[2]

print("Liczba wartości przez które opisywany jest kazdy piksel: ", liczba_wartosci)

#Zadanie 4
obrazek = Image.open(sciezka).convert('RGB')
obrazek_array = np.array(obrazek)

#kanały rgb
r = obrazek_array[:,:,0]
g = obrazek_array[:,:,1]
b = obrazek_array[:,:,2]
jasnosc = (np.max(obrazek_array, axis=2) + np.min(obrazek_array, axis=2)) / 2

srednia = (r + g + b) / 3 #usrednianie wartosci piksela

luminancja = 0.21*r + 0.72*g + 0.07*b #wyznaczanie luminancji piksela

plt.imshow(jasnosc, cmap='gray')
plt.title('Jasność')
plt.show()

plt.imshow(srednia, cmap='gray')
plt.title('Średnia')
plt.show()

plt.imshow(luminancja, cmap='gray')
plt.title('Luminancja')
plt.show()

#Zadanie 5
#histogram jasnosc
plt.figure()
plt.hist(jasnosc.ravel(), bins=256, color='blue')
plt.show()

#histogram srednia
plt.figure()
plt.hist(srednia.ravel(), bins=256, color='blue')
plt.show()
#histogram luminacja
plt.figure()
plt.hist(luminancja.ravel(), bins=256, color='blue')
plt.show()

# Zadanie 6
hist, bins = np.histogram(luminancja.ravel(), bins=16)

srodek_przedzialu = (bins[:-1] + bins[1:]) / 2

for i in range(len(bins) - 1):
    print(f"{i+1}.\t{bins[i]:<19} - {bins[i + 1]}")

plt.hist(luminancja.ravel(), bins=16, color='blue')
plt.axvline(np.mean(bins), color='red', linestyle='dashed', linewidth=2)
plt.show()

# Zadanie 7
luminancja_kwantyzacja = np.copy(luminancja)

for i in range(len(bins) - 1):
    mask = (luminancja >= bins[i]) & (luminancja < bins[i+1])
    luminancja_kwantyzacja[mask] = srodek_przedzialu[i]

plt.imshow(luminancja_kwantyzacja, cmap='gray')
plt.title('Luminancja po kwantyzacji z użyciem środków')
plt.show()

#Zadanie 8 wykresy już zostały wyświetlone

#BINARYZACJA
# Zadanie 2
sciezka = 'C:\\Users\\igorb\\Downloads\\jablka.jpg'
obrazek = Image.open(sciezka)

obrazek_grey = obrazek.convert('L')  # konwersja do szarosci
plt.imshow(obrazek_grey, cmap='gray')
plt.title('Obraz w skali szarości')
plt.show()

histogram, krawedzie_przedzialow = np.histogram(np.array(obrazek_grey).ravel(), bins=256, range=[0, 256])

# Zadanie 3
def znajdz_prog(hist):
    szczyty = np.argpartition(hist, -2)[-2:]
    prawdziwe_minimum = np.min(hist[min(szczyty):max(szczyty)+1])
    indeks_minimum = np.where(hist == prawdziwe_minimum)[0][0]
    return indeks_minimum

prog = znajdz_prog(histogram)

plt.figure(figsize=(10, 6))
srodki_przedzialow = (krawedzie_przedzialow[:-1] + krawedzie_przedzialow[1:]) / 2  # liczenie srodkow przedialow
plt.bar(srodki_przedzialow, histogram, color='blue', width=1, align='center')
plt.axvline(x=prog, color='red', linestyle='--')
plt.title('Histogram z punktem progowania')
plt.show()

#Zadanie 4
obrazek_w_szarosci = obrazek.convert('L')

obrazek_array = np.array(obrazek_w_szarosci)
prog = 127
binaryzowany = np.where(obrazek_array > prog, 1, 0)

plt.imshow(binaryzowany, cmap='gray')
plt.title('Zadanie 4')
plt.show()

#Zadanie 5
segmentowany_obiekt = obrazek_w_szarosci * binaryzowany

plt.imshow(segmentowany_obiekt, cmap='gray')
plt.title('Wysegmentowany obiekt')
plt.show()














