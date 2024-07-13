import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

#ladowanie danych
def load_dataset():
    data = pd.read_csv('autos.csv')
    width = data['width'].values
    length = data['length'].values
    dataset = np.array(list(zip(width, length)))
    return dataset

#odleglosc euklidesowa
def distp(a, b):
    return np.sqrt(np.sum((a - b) ** 2))


#obliczanie odległosci Mahalanobisa
def distm(a, b, VI):
    delta = a - b
    dist = np.dot(np.dot(delta, VI), delta)
    return np.sqrt(dist)


# k-srodki
def k_means(k, dist_func, VI=None):
    dataset = load_dataset()
    centroids = dataset[np.random.choice(dataset.shape[0], size=k, replace=False)]

    while True:
        if dist_func == distp:
            distances = np.array([distp(point, centroid) for point in dataset for centroid in centroids])
        elif dist_func == distm:
            distances = np.array([distm(point, centroid, VI) for point in dataset for centroid in centroids])

        assignments = np.argmin(distances.reshape(dataset.shape[0], k), axis=1)
        new_centroids = []
        for i in range(k):
            if np.any(assignments == i):
                new_centroids.append(dataset[assignments == i].mean(axis=0))
            else:
                new_centroids.append(dataset[np.random.choice(dataset.shape[0])])
        new_centroids = np.array(new_centroids)
        if np.allclose(centroids, new_centroids):
            break
        centroids = new_centroids
    return centroids, assignments


# wykres
def zwizualizuj(centroids, assignments, dataset, title):
    plt.scatter(dataset[:, 0], dataset[:, 1], c=assignments, cmap='viridis')
    plt.scatter(centroids[:, 0], centroids[:, 1], c='red')
    plt.title(title)
    plt.show()




#  F(C)
def licz_FC(centroids, assignments, dataset, dist_func, VI=None):
    sum_distances_between_centroids = np.sum(
        [dist_func(centroids[i], centroids[j], VI) if dist_func == distm
         else dist_func(centroids[i], centroids[j])
         for i in range(len(centroids)) for j in range(i + 1, len(centroids))])

    ssd = np.sum([np.sum(dist_func(point, centroids[i], VI) ** 2 if dist_func == distm
                         else np.sum((point - centroids[i]) ** 2)) for i in range(len(centroids)) for point in
                  dataset[assignments == i]])

    F_C = sum_distances_between_centroids / ssd
    return F_C


#wywolanie
k = 4
dataset = load_dataset()
plt.scatter(dataset[:, 0], dataset[:, 1], c='blue', marker='o')
plt.title('Initial Data')
plt.show()

cov_matrix = np.cov(dataset.T)
VI = np.linalg.inv(cov_matrix)
# z odlegloscia euklidesowa
centroids_euc, assignments_euc = k_means(k, distp)
zwizualizuj(centroids_euc, assignments_euc, dataset, 'K-Means Euclidean')
F_C_euc = licz_FC(centroids_euc, assignments_euc, dataset, distp)
print("Wartość F(C) :\n", F_C_euc)
print("Centroids:\n", centroids_euc, "\n")
print("Assignments:\n", assignments_euc, "\n")

# z z odległościa Mahalanobisa
centroids_mah, assignments_mah = k_means(k, distm, VI=VI)
zwizualizuj(centroids_mah, assignments_mah, dataset, 'K-Means Mahalanobis')
F_C_mah = licz_FC(centroids_mah, assignments_mah, dataset, distm, VI=VI)
print("Wartość F(C) :\n", F_C_mah)
print("Centroids:\n", centroids_mah, "\n")
print("Assignments:\n", assignments_mah, "\n")


