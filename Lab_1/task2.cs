using System;
using System.IO;

namespace Task2 {
    class task2 {
        static void Main(string[] args) {
            string inPath = "input.txt"; // Файл з вхідними даними, текстом
            string outPath = "output.txt"; // Файл з вихідними даними, результатом роботи програми
            string[] words = new string[0]; // Створюємо масив рядків для слів
            int[] counts = new int[0]; // Створюємо масив для підрахунку
            int[][] pages = new int[0][]; // Створюємо масив для сторінок
            int currentPage = 1;
            int length = 0, i, strCount = 0;
            int pageLinesCount = 45; // Кількість рядків на сторінці файлу, для розрізнення сторінок
            string word = ""; // Порожній рядок для зчитаного слова
            StreamReader reader = new StreamReader(inPath); // Читаємо вхідні дані
            Read_File_Label: { // Основна мітка читання і обробки файлу
                if (reader.EndOfStream) // Якщо кінець, то завершуємо читання
                    goto End_Reading_Label;
                string str = reader.ReadLine(); // Читаємо по рядку
                if (strCount == pageLinesCount) {
                    currentPage++; // Зміна кількості сторінок
                    strCount = 0;
                }
                strCount++;
                int j = 0;
                Basic_Label: { // Мітка обробки файлу
                    if (j == str.Length)
                        goto End_Basic_Label;
                    char symbol = str[j];
                    if ('Z' >= symbol && symbol >= 'A') { // По символу обробляємо слово, і зберігаємо його
                        word += ((char)(symbol + 32));
                        if (j + 1 < str.Length)
                            goto End_Basic_Label;
                    }
                    else if ('z' >= symbol && symbol >= 'a') {
                        word += symbol;
                        if (j + 1 < str.Length)
                            goto End_Basic_Label;
                    }
                    if (word != "" && symbol != '-' && symbol != '\'') {
                        i = 0;
                        Check_Words_Label: { // Мітка для перевірки чи це нове слово
                            if (i == length)
                                goto New_Word_Label;
                            if (word == words[i]) { // Якщо це слово не нове
                                word = "";
                                if (counts[i] > 100) { // Якщо це слово зустрічається менше 100 разів, то воно ігнорується
                                    goto End_Basic_Label;
                                }
                                counts[i]++;
                                if (counts[i] <= pages[i].Length) {
                                    pages[i][counts[i] - 1] = currentPage;
                                }
                                else {
                                    int[] pagesTmp = new int[counts[i] * 2];
                                    int p = 0;
                                    Copy_Pages_Label: { // Мітка копіювання сторінок
                                        pagesTmp[p] = pages[i][p];
                                        p++;
                                        if (p < counts[i] - 1)
                                            goto Copy_Pages_Label;
                                    }
                                    pages[i] = pagesTmp;
                                    pages[i][counts[i] - 1] = currentPage;
                                }
                                goto End_Basic_Label;
                            }
                            i++;
                            goto Check_Words_Label;
                        }
                        New_Word_Label: // Мітка для обробки появи нового слова
                        if (length == words.Length) {
                            string[] New_Word_Labels = new string[(length + 1) * 2];
                            int[] newCounts = new int[(length + 1) * 2];
                            int[][] newPages = new int[(length + 1) * 2][];
                            i = 0;
                            For_Copy_Label: { // Мітка для запису даних про нове слово
                                if (i == length) {
                                    words = New_Word_Labels;
                                    counts = newCounts;
                                    pages = newPages;
                                    goto End_Copy_Label;
                                }
                                New_Word_Labels[i] = words[i];
                                newCounts[i] = counts[i];
                                newPages[i] = pages[i];
                                i++;
                                goto For_Copy_Label;
                            }
                        }
                        End_Copy_Label: // Мітка для завершення запису даних про нове слово
                        words[length] = word;
                        counts[length] = 1;
                        pages[length] = new int[] { currentPage };
                        length++;
                        word = "";
                    }
                    End_Basic_Label: // Мітка для перезапуску
                    j++;
                    if(j < str.Length)
                        goto Basic_Label;
                }
                if (!reader.EndOfStream) // Якщо не кінець файлу, то продовжуємо все з початку основної мітки
                    goto Read_File_Label;
            }
            End_Reading_Label: // Мітка для завершення читання
            reader.Close();
            int current, c;
            int[] currentPages;
            i = 1;
            Sort_Label: { // Мітка початку сортування вставленням
                current = counts[i];
                word = words[i];
                currentPages = pages[i];
                c = i - 1;
                While_Sort_Label: { // Мітка основної частини сортування
                    if (c >= 0) {
                        int symbol = 0;
                        compWords: { // Мітка для порівняння слів
                            if (symbol == words[c].Length || words[c][symbol] < word[symbol])
                                goto End_While_Sort_Label;
                            if (symbol + 1 < word.Length && words[c][symbol] == word[symbol]) {
                                symbol++;
                                goto compWords;
                            }
                        }
                        counts[c + 1] = counts[c];
                        words[c + 1] = words[c];
                        pages[c + 1] = pages[c];
                        c--;
                        goto While_Sort_Label;
                    }
                }
                End_While_Sort_Label: // Мітка перезапуску сортування
                counts[c + 1] = current;
                words[c + 1] = word;
                pages[c + 1] = currentPages;
                i++;
                if (i < length)
                    goto Sort_Label;
            }
            StreamWriter writer = new StreamWriter(outPath); //  Виводимо вихідні дані в файл
            i = 0;
            Write_Label: { // Мітка для виведення даних в файл
                if (counts[i] <= 100) { // Якщо слово не було проігноровано
                    writer.Write(words[i] + " - " + pages[i][0]);
                    int j = 1;
                    outPages: { // Мітка для виведення номеру сторінки
                        if (j == counts[i])
                            goto endOutPages;
                        if(pages[i][j] != pages[i][j - 1])
                            writer.Write(", "+pages[i][j]);
                        j++;
                        goto outPages;
                    }
                    endOutPages:
                    writer.WriteLine();
                }
                i++;
                if (i < length)
                    goto Write_Label;
            }
            writer.Close(); // Завершення запису
        }
    }
}