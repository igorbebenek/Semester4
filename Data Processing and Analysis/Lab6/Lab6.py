import numpy as np
import matplotlib.pyplot as plt
from sklearn import datasets
from sklearn.decomposition import PCA


def draw_vector(v0, v1, ax=None):
    ax = ax or plt.gca()
    arrowprops = dict(arrowstyle='->', linewidth=2, shrinkA=0, shrinkB=0, color='red')
    ax.annotate('', v1, v0, arrowprops=arrowprops)

def moj_pca(dane, n_składowych=2):
    srednia = np.mean(dane, axis=0)
    dane_centrowane = dane - srednia
    macierz_kowariancji = np.cov(dane_centrowane, rowvar=False)
    wartosci_wlasne, wektory_wlasne = np.linalg.eigh(macierz_kowariancji)
    indeksy_sort = np.argsort(wartosci_wlasne)[::-1]
    wektory_wlasne = wektory_wlasne[:, indeksy_sort][:, :n_składowych]
    wartosci_wlasne = wartosci_wlasne[indeksy_sort][:n_składowych]
    dane_transformowane = np.dot(dane_centrowane, wektory_wlasne)
    dane_rekonstruowane = np.dot(dane_transformowane, wektory_wlasne.T) + srednia
    return srednia, wektory_wlasne, wartosci_wlasne, dane_transformowane, dane_rekonstruowane

np.random.seed(1)
dane = np.dot(np.random.rand(2, 2), np.random.randn(2, 200)).T

srednia, wektory_wlasne, wartosci_wlasne, dane_projekcja, dane_rekonstruowane = moj_pca(dane, n_składowych=2)

plt.scatter(dane[:, 0], dane[:, 1], alpha=0.2)
for i in range(len(wartosci_wlasne)):
    koniec_wektora = srednia + np.sqrt(wartosci_wlasne[i]) * wektory_wlasne[:, i] * 3
    draw_vector(srednia, koniec_wektora)

srednia, wektory_wlasne, _, dane_projekcja, dane_rekonstruowane = moj_pca(dane, n_składowych=1)
dane_projekcja = np.dot(dane - srednia, wektory_wlasne) * wektory_wlasne.T + srednia
plt.scatter(dane_projekcja[:, 0], dane_projekcja[:, 1], alpha=0.8, color='orange')

plt.axis('equal')
plt.title('Wizualizacja PCA')
plt.grid(True)
plt.legend()
plt.show()


#Zadanie 2
iris = datasets.load_iris()
X = iris.data
y = iris.target

srednia, wektory_wlasne, wartosci_wlasne, dane_projekcja,dane_rekonstruowane = moj_pca(X, n_składowych=2)

plt.figure(figsize=(8, 6))
scatter = plt.scatter(dane_projekcja[:, 0], dane_projekcja[:, 1], c=y, cmap='viridis', edgecolor='none', alpha=0.7)
plt.title('PCA na Iris')
plt.show()

#Zadanie 3
digits = datasets.load_digits()
X = digits.data
y = digits.target

srednia, wektory_wlasne, wartosci_wlasne, dane_projekcja, dane_rekonstruowane = moj_pca(X, n_składowych=2)

plt.figure(figsize=(10, 8)) # wizualizacja po redukcji wymiarowości
scatter = plt.scatter(dane_projekcja[:, 0], dane_projekcja[:, 1], c=y, cmap='viridis', edgecolor='none', alpha=0.7)
plt.title('PCA na Digits')
plt.show()
#krzywa wariancji
pca = PCA().fit(X)
plt.plot(np.cumsum(pca.explained_variance_ratio_))
plt.title('Krzywa wariancji')
plt.grid(True)
plt.show()

def oblicz_bledy_rekonstrukcji(dane, max_n_składowych):
    bledy = []
    for n in range(1, max_n_składowych + 1):
        _, _, _, _, dane_rekonstruowane = moj_pca(dane, n_składowych=n)
        blad = np.mean(np.sqrt(np.sum((dane - dane_rekonstruowane) ** 2, axis=1)))
        bledy.append(blad)
    return bledy

bledy_rekonstrukcji = oblicz_bledy_rekonstrukcji(X, 64)  # 64 to maksymalna liczba wymiarów w digits

plt.figure(figsize=(10, 6))
plt.plot(range(1, 65), bledy_rekonstrukcji)
plt.title('Błąd rekonstrukcji')
plt.grid(True)
plt.show()