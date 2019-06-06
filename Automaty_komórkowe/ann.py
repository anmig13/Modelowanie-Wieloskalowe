from tkinter import *
import numpy
import matplotlib.pyplot as plt

def swapB(a, start, end):
    while(start < end):
        tmp=a[start]
        a[start]=a[end]
        a[end]=tmp
        start+=1
        end-=1
    #print(a)


def toBinary(number):
    binary=[]

    for i in range(8):
        binary.append(0)
    #print(binary)

    x=0
    while(number > 0):
        binary[x]=int(number%2)
        number = int(number/2)
        x+=1
    # print(binary)
    #print("after swap")
    swapB(binary, 0 ,7)
    return binary


def methods(number, w, sz):
    method_nr=number
    tab=toBinary(method_nr)

    columns = sz
    rows = w
    matrix = numpy.zeros([rows, columns ])
    matrix[0, int(columns / 2) ] = 1

    for i in range(len(matrix)):
        for j in range(len(matrix[i])):
            if(i==0):
                i=i+1
            if(j==columns-1):
                if (matrix[i - 1][j - 1] == 1 and matrix[i - 1][j] == 1 and matrix[i - 1][0] == 1):
                    matrix[i][j] = tab[0]
                elif (matrix[i - 1][j - 1] == 1 and matrix[i - 1][j] == 1 and matrix[i - 1][0] == 0):
                    matrix[i][j] = tab[1]
                elif (matrix[i - 1][j - 1] == 1 and matrix[i - 1][j] == 0 and matrix[i - 1][0] == 1):
                    matrix[i][j] = tab[2]
                elif (matrix[i - 1][j - 1] == 1 and matrix[i - 1][j] == 0 and matrix[i - 1][0] == 0):
                    matrix[i][j] = tab[3]
                elif (matrix[i - 1][j - 1] == 0 and matrix[i - 1][j] == 1 and matrix[i - 1][0] == 1):
                    matrix[i][j] = tab[4]
                elif (matrix[i - 1][j - 1] == 0 and matrix[i - 1][j] == 1 and matrix[i - 1][0] == 0):
                    matrix[i][j] = tab[5]
                elif (matrix[i - 1][j - 1] == 0 and matrix[i - 1][j] == 0 and matrix[i - 1][0] == 1):
                    matrix[i][j] = tab[6]
                elif (matrix[i - 1][j - 1] == 0 and matrix[i - 1][j] == 0 and matrix[i - 1][0] == 0):
                    matrix[i][j] = tab[7]
            else:
                if (matrix[i - 1][j - 1] == 1 and matrix[i - 1][j] == 1 and matrix[i - 1][j + 1] == 1):
                    matrix[i][j] = tab[0]
                elif (matrix[i - 1][j - 1] == 1 and matrix[i - 1][j] == 1 and matrix[i - 1][j + 1] == 0):
                    matrix[i][j] = tab[1]
                elif (matrix[i - 1][j - 1] == 1 and matrix[i - 1][j] == 0 and matrix[i - 1][j + 1] == 1):
                    matrix[i][j] = tab[2]
                elif (matrix[i - 1][j - 1] == 1 and matrix[i - 1][j] == 0 and matrix[i - 1][j + 1] == 0):
                    matrix[i][j] = tab[3]
                elif (matrix[i - 1][j - 1] == 0 and matrix[i - 1][j] == 1 and matrix[i - 1][j + 1] == 1):
                    matrix[i][j] = tab[4]
                elif (matrix[i - 1][j - 1] == 0 and matrix[i - 1][j] == 1 and matrix[i - 1][j + 1] == 0):
                    matrix[i][j] = tab[5]
                elif (matrix[i - 1][j - 1] == 0 and matrix[i - 1][j] == 0 and matrix[i - 1][j + 1] == 1):
                    matrix[i][j] = tab[6]
                elif (matrix[i - 1][j - 1] == 0 and matrix[i - 1][j] == 0 and matrix[i - 1][j + 1] == 0):
                    matrix[i][j] = tab[7]
    plt.imshow(matrix[:, 1:columns+1], cmap='Greys', interpolation='nearest')
    plt.show()



def parse_to_string(a):
    string=str(a)
    return string



def main():
    #t=[]
    #t=toBinary(30)
    #print(t)
    # b=parse_to_string(t)
    # print(b)
    num = int(input('Wpisz numer metody: '))
    print('Wybrales metode', num)

    wys = int(input('Wpisz wysokosc siatki: '))
    szer =  int(input('Wpisz szerokosc siatki: '))

    m=methods(num, wys, szer)




if __name__ == "__main__":
    main()