import numpy as np
import matplotlib.pyplot as plt
from numpy.core._multiarray_umath import ndarray
from numpy.lib.stride_tricks import as_strided


#rozklad_normalny = np.random.normal(loc=0.0,scale=1.0,size=20) #loc to srodek rozkladu, scale to odchylenie standardowe (defaultowe)
#print(rozklad_normalny)
#a=np.array([1,2,3,4,5,6,7])
#print(a)
#array 2D
#b=np.array([[1,2,3,4,5], [6,7,8,9,10]])
#print(b)
#b_po_transponowaniu = b.transpose()
#print(b_po_transponowaniu)
#print(np.arange(100))
#print(np.linspace(0,2,10))
#print(np.arange(0,100,5))
#print(np.random.randint(1,1000))
#print(np.zeros((3,2))) #(3,2) jako pierwszy argument
#print(np.ones((3,2)))
#print(np.random.randint(1,1000,(5,5),np.int32)) #losowa macierz 5x5 od 1 do 1000, typ np.int32
#tablica_random = np.random.uniform(0,10,size=10) #randomowa tablica liczb zmiennoprzecinkowych od 0 do 10, rozmiar 10
#print(tablica_random)
#print(ndarray.astype(tablica_random,int)) # ta sama tablica do inta (potrzebny byl inny import)
#print(np.rint(tablica_random)) #zaokraglij do najblizszego inta

#Selekcja danych

#b=np.array([[1,2,3,4,5], [6,7,8,9,10]],dtype=np.int32)
#print(np.ndim(b)) #sprawdzenie ile wymiarów ma tablica b

#print(np.size(b)) #sprawdzenie rozmiaru tablicy b

#print(np.take(b,(1,3))) #wypisz indeksy 1 i 3 z tablicy b

#print(b[0]) #wypisz pierwszy row z tablicy b

#print(b[:,0]) #wypisz wiersze z kolumny 1, : oznacza wszystkie

#random_array2 = np.random.randint(10,100,(20,7))
#print(random_array2)
#print(random_array2[:,:4]) #wypisz wszystkie wiersze dla pierwszych 4 kolumn (:4 oznacza, że wszystkie dla indeksu od 0 do 4)

#Działania na macierzach
#macierz1 = np.random.randint(0,10,(3,3)) #stworzenie macierzy
#print(macierz1)
#macierz2 = np.random.randint(0,10,(3,3))
#print(macierz2)
#print(np.matmul(macierz1,macierz2)) # mnożenie macierzy przez siebie
#print(np.add(macierz1,macierz2)) # dodawanie macierzy do siebie
#print(np.divide(macierz1,macierz2)) # dzielenie macierzy przez siebie
#print(np.power(macierz1,macierz2)) # "potęowanie" macierzy przez siebie
#print(macierz1>=4) #porownanie wartosci macierzy 1 >= 4
#print(macierz1!=1) #sprawdzenie czy wartosci sa nierowne 1, zwraca macierz booolowska
#print(macierz1!=4)
#print(np.trace(macierz2)) #obliczanie sumy głównej przekątnej macierzy 2

# ZADANIA PODSUMOWUJĄCE

#ZAD 1
matrix = np.random.randint(0,1000,(10,5))
print("Macierz: \n", matrix)
diagonal_sum = np.trace(matrix)
print("Suma przekątnej: \n", diagonal_sum)
print("Przekątna: \n", np.diag(matrix))

#ZAD 2
array1 = np.random.normal(0,1,(10,5))
print("Pierwsza tablica: \n", array1)
array2 = np.random.normal(0,1,(10,5))
print("Druga tablica: \n",array2)
print("Iloczyn: \n", np.multiply(array1,array2))

#ZAD 3
matrix2 = np.random.randint(1, 101, (7, 5))
print("Macierz 2: \n", matrix2)
matrix3 = np.random.randint(1, 101, (7, 5))
print("Macierz 3: \n", matrix3)
print("Suma macierzy: \n:",np.add(matrix2,matrix3))

#ZAD 4
matrix4 = np.random.randint(1,101,(4,5))
print("Macierz 4: \n", matrix4)
matrix5 = np.random.randint(1,101,(5,4))
print("Macierz 5: \n", matrix5)
print("Suma macierzy 4 i 5 po transponowaniu: \n", np.add(matrix4,matrix5.transpose()))

#ZAD 5
matrix6 = np.random.randint(1,101,(5,5))
print("Macierz 6: \n", matrix6)
matrix7 = np.random.randint(1,101,(5,5))
print("Macierz 7: \n", matrix7)
print("Mnożenie kolumn 3 i 4:\n", np.multiply(matrix6[:, 2:4], matrix7[:, 2:4]))

#ZAD 6
matrix8 = np.random.normal(0,1,(5,5)) #loc to srodek rozkladu, scale to odchylenie standardowe (defaultowe)
matrix9 = np.random.uniform(0,1,(5,5))
print("Średnie dla macierzy 8 i 9: \n",np.mean(matrix8),np.mean(matrix9))
print("Odchylenie standardowe dla macierzy 8 i 9: \n", np.std(matrix8), np.std(matrix9))
print("Wariancja dla macierzy 8 i 9: \n", np.var(matrix8), np.var(matrix9))

#Zad 7
macierz_a = np.random.randint(1, 20, (5, 5))
print("Macierz a: \n", macierz_a)
macierz_b = np.random.randint(1, 20, (5, 5))
print("Macierz b: \n", macierz_b)
print("Iloczyn macierzy używając *: \n", macierz_a * macierz_b)
print("Iloczyn macierzy a i b używając np.dot(): \n", np.dot(macierz_a,macierz_b))

#Różnica jest taka, że macierz_a * macierz_b nie jest zgodne z mnożeniem matematycznym, a po prostu mnożeniem odpowiadających sobie elementów

#Zad 8
#użycie atrybutu .strides oraz funkcji as_strided
macierz_przyklad = np.random.randint(1, 20, (5, 5))
print("Macierz przyklad: \n", macierz_przyklad)
print("macierz_przyklad.strides: \n", macierz_przyklad.strides) # printuje (20,4) czyli żeby przesunąć się o jeden wiersz trzeba użyć 20 bajtów, o jedną kolumnę 4 bajty
strided = as_strided(macierz_przyklad, shape=(3,5), strides=macierz_przyklad.strides) #pokazuje naszą macierz z widocznością 3x5, funkcja potrzebuje informacji ze .strides żeby zadziałała
print("Pięć kolumn z pierwszych trzech wierszy: \n",strided)

#Zad 9
macierz_c = np.random.randint(1, 20, (5, 5))
print("Macierz c: \n,", macierz_c)
macierz_d = np.random.randint(1,20,(5,5))
print("Macierz d: \n", macierz_d)
print("Użycie np.vstack: \n", np.vstack((macierz_c, macierz_d))) #łączy dwie macierze w jedną i zwiększa liczbę wierszy

print("Użycie np.hstack: \n", np.hstack((macierz_c, macierz_d))) #też łączy dwie macierze, ale zwiększa liczbę kolumn, dodaje macierz d z prawej strony do macierzy c

#np.vstack może być przydatne gdy ktoś chce dodać wiersze do istniejącego już arraya, a hstack kolumn


























