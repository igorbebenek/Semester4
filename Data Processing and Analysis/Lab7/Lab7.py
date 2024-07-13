from collections import Counter
from sklearn.metrics import accuracy_score
import matplotlib.pyplot as plt
import numpy as np
from sklearn import datasets
from sklearn.decomposition import PCA

def euclidean_distance(a, b):
    return np.sqrt(sum((a - b) ** 2))

class KNN:
    def __init__(self, k=3):
        self.k = k

    def fit(self, X, y):
        self.X_train = X
        self.y_train = y

    def predict(self, X):
        predictions = [self._predict(x) for x in X]
        return predictions

    def _predict(self, x):
        distances = [euclidean_distance(x, x_train) for x_train in self.X_train]
        k_indices = np.argsort(distances)[:self.k]

        k_nearest_labels = [self.y_train[i] for i in k_indices]
        most_common = Counter(k_nearest_labels).most_common()
        return most_common[0][0]

    def score(self, X, y):
        return accuracy_score(y, self.predict(X))

# 3.1
X, y = datasets.make_classification(n_samples=100, n_features=2, n_informative=2, n_redundant=0, n_repeated=0, random_state=3)

X_przyklady = 100
X_min, X_max = np.min(X, axis=0), np.max(X, axis=0)
y_min, y_max = np.min(X, axis=0), np.max(X, axis=0)

X_przyklad1 = np.random.uniform(X_min[0], X_max[0], (X_przyklady, 1))
y_przyklad2 = np.random.uniform(y_min[1], y_max[1], (X_przyklady, 1))
X_przyklad = np.hstack((X_przyklad1, y_przyklad2))

plt.plot(X[y == 0, 0], X[y == 0, 1], 'go')
plt.plot(X[y == 1, 0], X[y == 1, 1], 'ro')

plt.title('Zbiór danych')
plt.show()

# 3.2
model = KNN(5)
model.fit(X, y)
wynik = model.predict(X_przyklad)
print("Wynik", wynik)
dokladnosc = model.score(X_przyklad, wynik)
print("Dokładność:", dokladnosc)

# 3.3
x0 = np.linspace(X_przyklad[:, 0].min(), X_przyklad[:, 0].max(), 50)
x1 = np.linspace(X_przyklad[:, 1].min(), X_przyklad[:, 1].max(), 50)
XX, YY = np.meshgrid(x0, x1)

siatka = np.vstack((XX.ravel(), YY.ravel())).T

wynik = model.predict(siatka)
wynik = np.array(wynik)
wynik = wynik.reshape(XX.shape)

plt.plot(X[:, 0][y == 0], X[:, 1][y == 0], 'o')
plt.plot(X[:, 0][y == 1], X[:, 1][y == 1], 'ro')

plt.contour(XX, YY, wynik, colors=['blue', 'red'])


plt.show()

# 3.4
iris = datasets.load_iris()
X_iris = iris.data
y_iris = iris.target

# Klasyfikacja
model_iris = KNN(7)
model_iris.fit(X_iris, y_iris)
wynik_iris = model_iris.predict(X_iris)
print("Wynik iris", wynik_iris)
print("Model score: ", model_iris.score(X_iris, y_iris))

# 3.5

transformacja = PCA(n_components=2)
dane_przeksztalcone = transformacja.fit_transform(X_iris)

kolory = np.array(['b', 'g', 'r'])[y_iris]
plt.scatter(dane_przeksztalcone[:, 0], dane_przeksztalcone[:, 1], c=kolory)

x_min, x_max = dane_przeksztalcone[:, 0].min(), dane_przeksztalcone[:, 0].max()
y_min, y_max = dane_przeksztalcone[:, 1].min(), dane_przeksztalcone[:, 1].max()

xx, yy = np.meshgrid(np.linspace(x_min, x_max, 100), np.linspace(y_min, y_max, 100))

punkty_siatki = np.c_[xx.ravel(), yy.ravel()]
punkty_orbitalne = transformacja.inverse_transform(punkty_siatki)

Z = model_iris.predict(punkty_orbitalne)
Z = np.array(Z)
Z = Z.reshape(xx.shape)

plt.contour(xx, yy, Z)

plt.title('Iris z PCA')
plt.show()
