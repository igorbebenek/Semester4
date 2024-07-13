import librosa
import numpy as np
import sounddevice as sd
import soundfile as sf
import matplotlib.pyplot as plt
from scipy.fftpack import fft
from sklearn import preprocessing

#----------------------SYGNAŁ AUDIO------------------------------------------------------------------------

s, fs = sf.read('test.wav', dtype='float32')

# sd.play(s,fs)
# status = sd.wait()

time = np.linspace(0, len(s) / fs, num=len(s)) * 1000
X = len(s)
dim = 1

Y = s / np.amin(s)

plt.figure()
plt.plot(time, Y)
plt.xlabel('Czas [ms]')
plt.ylabel('Amplituda')
plt.title('Sygnał audio')
plt.show()

# Zadanie 4
# Dynamika jest odpowiednia, tak samo zakres amplitudy. Nie występują szumy czy przesterowania, bo nagranie zostało stworzone przez syntezator.

#----------------------ZASTOSOWANIE OKIEN KROCZĄCYCH------------------------------------------------------------------------


#Zad 1 i 2
def f(dlugosc_okna_ms, nakladanie_procent=0):
    E = []
    Z = []
    ramka = int(fs * dlugosc_okna_ms / 1000)
    przesuniecie = int(ramka * (1 - nakladanie_procent / 100))
    n = int((X - ramka) / przesuniecie) + 1
    j = 0

    if len(s.shape) == 1:
        s1 = s
    else:
        s1 = s[:, 0]

    for i in range(n):
        E.append(np.sum(np.power(s1[j:j + ramka], 2)))
        j += przesuniecie

    E = preprocessing.normalize([E])
    E = E.T
    E = E / np.amax(E)
    t_okno = np.zeros(n)

    for i in range(n):
        t_okno[i] = i * przesuniecie * 1000 / fs

    suma = 0

    for i in range(X - 1):
        if s1[i] * s1[i + 1] >= 0:
            z = 0
        else:
            z = 1
        suma += z
        if (i + 1) % przesuniecie == 0:
            Z.append(suma)
            suma = 0

    Z = preprocessing.normalize([Z])
    Z = Z.T
    Z = Z / np.amax(Z)
    t_z = np.zeros(len(Z))

    for i in range(len(Z)):
        t_z[i] = i * przesuniecie * 1000 / fs

    return t_okno, E, t_z, Z

t_okno_10ms, E_10ms, t_z_10ms, Z_10ms = f(dlugosc_okna_ms=10)

plt.figure()
plt.plot(time, Y, c="green")
plt.plot(t_okno_10ms, E_10ms, c='red')
plt.plot(t_z_10ms, Z_10ms, c='blue')
plt.title('Sygnał z zastosowaniem okien kroczących 10 ms')
plt.show()

#Zad 3
#Maksima funkcji E wskazuja na głosne dzwieki, a minima na cisze
#Maksima funkcji Z wskazuja na szumy i bezdzwieczne fragmenty, a minima na dzwieczne - takie jak samogloski
#Mozna je uzyc do podzialu sygnalu na segmenty dzwieczne i bezdzwieczne


#Zad 4

t_okno_5ms, E_5ms, t_z_5ms, Z_5ms = f(dlugosc_okna_ms=5)
t_okno_20ms, E_20ms, t_z_20ms, Z_20ms = f(dlugosc_okna_ms=20)
t_okno_50ms, E_50ms, t_z_50ms, Z_50ms = f(dlugosc_okna_ms=50)

plt.figure()
plt.plot(time, Y)
plt.xlabel('Czas [ms]')
plt.plot(t_okno_5ms, E_5ms, color='orange')
plt.plot(t_z_5ms, Z_5ms, color='purple')
plt.title("5 ms")
plt.show()

plt.figure()
plt.plot(time, Y)
plt.xlabel('Czas [ms]')
plt.plot(t_okno_20ms, E_20ms, color='orange')
plt.plot(t_z_20ms, Z_20ms, color='purple')
plt.title("20 ms")
plt.show()

plt.figure()
plt.plot(time, Y)
plt.xlabel('Czas [ms]')
plt.plot(t_okno_50ms, E_50ms, color='orange')
plt.plot(t_z_50ms, Z_50ms, color='purple')
plt.title("50 ms")
plt.show()

#Zad 5
t_okno_10ms, E_10ms, t_z_10ms, Z_10ms = f(10,50)


plt.figure()
plt.plot(time, Y, c="green")
plt.plot(t_okno_10ms, E_10ms, c='red')
plt.plot(t_z_10ms, Z_10ms, c='blue')
plt.title('10 ms, 50% nakładanie')
plt.show()

#--------------------------------ANALIZA CZESTOTLIWOSCIOWA--------------------------------------------------------------------------------------

#Zadanie 1
start_time = 1.15
end_time = 1.256
start_sample = int(start_time * fs)
end_sample = start_sample + 2048
s2048 = s[start_sample:end_sample]

czas_ms = len(s2048) / fs * 1000
x = np.linspace(0, czas_ms, len(s2048))

#Zadanie 2
hamming_window = np.hamming(len(s2048))
s2048_hamming = s2048 * hamming_window

#Zadanie 3
yf = fft(s2048_hamming)
widmo = np.log(np.abs(yf))

#Zadanie 4
plt.figure(figsize=(12, 6))
plt.plot(x, s2048_hamming, c='blue')
plt.title("Sygnał zamaskowany oknem Hamminga")
plt.show()

frequencies = np.fft.fftfreq(len(s2048), 1/fs)
plt.figure()
plt.plot(frequencies[:len(widmo)//2], widmo[:len(widmo)//2], c='red')
plt.xlim(0, 10000)
plt.title("Widmo w zakresie 0-10000 Hz")
plt.show()

#----------------------------ROZPOZNAWANIE SAMOGŁOSEK USTNYCH /a,e,i,o,u,y/-----------------------------------------------------------------


#Zadanie 1
okno = s[start_sample:end_sample]

#Zadanie 2
p = 20
a = librosa.lpc(okno,order=p)

#Zadanie 3
# https://ccrma.stanford.edu/~hskim08/lpc/
# LPC to technika stosowana w przetwarzaniu sygnałów audio, szczególnie mowy. Służy do kompresji sygnału, przewidując jego przyszłe wartości na podstawie przeszłych próbek, co pozwala na efektywną reprezentację obwiedni widma sygnału.

#Zadanie 4
a = np.pad(a, (0, 2048 - len(a)), 'constant')

#Zadanie 5
y = fft(a)
widmo_2 = np.log(np.abs(y)) * (-1)

frequencies = np.linspace(0, fs, len(widmo))
plt.figure()
plt.plot(frequencies[:len(widmo)//2], widmo[:len(widmo)//2], c='navy')
plt.plot(frequencies[:len(widmo_2) // 2], widmo_2[:len(widmo_2) // 2] - 2.9, c='red')
plt.xlim(0, 10000)
plt.title('widmo amplitudowe')
plt.legend()
plt.show()

