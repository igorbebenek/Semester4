import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from sklearn import datasets
from sklearn.metrics import accuracy_score, recall_score, precision_score, f1_score, roc_auc_score, roc_curve
from sklearn.model_selection import train_test_split
from sklearn.neighbors import KNeighborsClassifier
from sklearn.naive_bayes import GaussianNB
from sklearn.svm import SVC
from sklearn.tree import DecisionTreeClassifier
from sklearn.discriminant_analysis import QuadraticDiscriminantAnalysis

# Zadanie 1.1: Generowanie przykładowych danych
X, y = datasets.make_classification(
    n_samples=300,
    n_features=2,
    n_redundant=0,
    n_informative=2,
    random_state = 3,
    n_classes=2,
    n_clusters_per_class=2
)
# Zadanie 1.2: Wizualizacja wygenerowanych danych
plt.scatter(X[:, 0], X[:, 1], c=y, cmap='rainbow')
plt.title("Wykres punktowy wygenerowanych danych")
plt.show()

# Zadanie 1.3: Utworzenie listy klasyfikatorów z domyślnymi parametrami
models = {
    'GaussianNB': GaussianNB(),
    'QuadraticDiscriminantAnalysis': QuadraticDiscriminantAnalysis(),
    'KNeighborsClassifier': KNeighborsClassifier(),
    'SVC': SVC(probability=True),
    'DecisionTreeClassifier': DecisionTreeClassifier()
}

# słowniki do przechowywania metryk
dokladnosc, recall, precision, f1, rocauc = {}, {}, {}, {}, {}
la, lr, lp, lf1, lrocauc = [], [], [], [], []

# Zadanie 1.4: Podział danych, trening i ocena klasyfikatorów
for key in models.keys():
    for i in range(100):
        X_train, X_test, y_train, y_test = train_test_split(X, y, random_state=2)
        models[key].fit(X_train, y_train)
        y_pred = models[key].predict(X_test)
        la.append(accuracy_score(y_test, y_pred))
        lr.append(recall_score(y_test, y_pred))
        lp.append(precision_score(y_test, y_pred))
        lf1.append(f1_score(y_test, y_pred))
        lrocauc.append(roc_auc_score(y_test, y_pred))

        # Zadanie 1.5: Wizualizacja błędów klasyfikacji i krzywej ROC dla ostatniej iteracji
        if i == 99:
            roznica, ytest, ypred = [], [], []
            for j in range(len(X_test)):
                if y_pred[j] != y_test[j]:
                    roznica.append("red")
                else:
                    roznica.append("darkgreen")
            for j in range(len(y_test)):
                if y_test[j] == 0:
                    ytest.append('darkblue')
                elif y_test[j] == 1:
                    ytest.append('violet')
            for j in range(len(y_pred)):
                if y_pred[j] == 0:
                    ypred.append('darkblue')
                elif y_pred[j] == 1:
                    ypred.append('violet')

            fig, (ax1, ax2, ax3) = plt.subplots(1, 3, figsize=(15, 5))
            ax1.scatter(X_test[:, 0], X_test[:, 1], c=ytest, alpha=0.5)
            ax1.set_title('Oczekiwane')
            ax2.scatter(X_test[:, 0], X_test[:, 1], c=ypred, alpha=0.5)
            ax2.set_title('Obliczone')
            ax3.scatter(X_test[:, 0], X_test[:, 1], c=roznica, alpha=0.5)
            ax3.set_title('Różnice')
            plt.show()

            pr = models[key].predict_proba(X_test)
            fpr, tpr, thresholds = roc_curve(y_test, pr[:, 1])
            plt.plot([0, 1], [0, 1], '--', color='red')
            plt.plot(fpr, tpr)
            plt.xlabel('False Positive Rate')
            plt.ylabel('True Positive Rate')
            plt.title(f'Krzywa ROC dla {key}')
            plt.show()

            x_min, x_max = X[:, 0].min() - .5, X[:, 0].max() + .5
            y_min, y_max = X[:, 1].min() - .5, X[:, 1].max() + .5
            xx, yy = np.meshgrid(np.arange(x_min, x_max, 0.02), np.arange(y_min, y_max, 0.02))
            Z = models[key].predict(np.c_[xx.ravel(), yy.ravel()])
            Z = Z.reshape(xx.shape)
            plt.contourf(xx, yy, Z, alpha=0.8, cmap=plt.cm.Paired)
            plt.scatter(X[:, 0], X[:, 1], c=y, edgecolor='k', cmap=plt.cm.Paired)
            plt.title(f'Granica decyzyjna dla {key}')
            plt.show()

    dokladnosc[key] = np.mean(la)
    recall[key] = np.mean(lr)
    precision[key] = np.mean(lp)
    f1[key] = np.mean(lf1)
    rocauc[key] = np.mean(lrocauc)

df = pd.DataFrame(index=models.keys(), columns=['dokladnosc', 'recall', 'precision', 'f1', 'rocauc'])
df['dokladnosc'] = dokladnosc.values()
df['recall'] = recall.values()
df['precision'] = precision.values()
df['f1'] = f1.values()
df['rocauc'] = rocauc.values()

print(df)

ax = df.plot.bar()
ax.set_xlabel('Model')
ax.set_ylabel('Wynik')
plt.title('Metryki wydajności różnych modeli')
plt.tight_layout()
plt.show()
