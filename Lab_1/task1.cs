using System;
using System.IO;

namespace Task1 {
    class task1 {
        static void Main(string[] args) {
            string[] stopWords = new string[] {
				"i", "me", "my", "we", "our", "you", "your", "he", "him", "his", 
				"she", "her", "it", "they", "them", "their", "what", "which", "who", 
				"whom", "this", "that", "these", "those", "am", "is", "are", "was", 
				"were", "be", "have", "has", "had", "do", "does", "did", "a", "an", 
				"the", "and", "but", "if", "or", "because", "as", "until", "while", 
				"of", "at", "by", "for", "to", "then", "here", "there", "when", "where", 
				"why", "how", "each", "no", "nor", "not", "so", "than", "too", "can", 
				"will", "now" }; // Стоп слова обробка яких вказавна в завданні. Список може бути більшим
            string inPath = "input.txt"; // Файл з вхідними даними, текстом
            string outPath = "output.txt"; // Файл з вихідними даними, результатом роботи програми
            string[] words = new string[0]; // Створюємо масив рядків для слів
            int[] counts = new int[0]; // Створюємо масив для підрахунку
            int length = 0, i;
            int maxWordsCount = 25; // Кількість виведених слів 25
            string word = ""; // Порожній рядок для зчитаного слова
            StreamReader reader = new StreamReader(inPath); // Читаємо вхідні дані
            Basic_Label: { // Основна мітка
                if (reader.EndOfStream) // Якщо кінець, то завершуємо читання
                    goto End_Reading_Label;
                char symbol = (char)reader.Read(); // Читаємо по символу 
                if ('Z' >= symbol && symbol >= 'A') { // По символу обробляємо слово, і зберігаємо його
                    word += ((char)(symbol + 32)).ToString();
                    if (!reader.EndOfStream)
                        goto Basic_Label;
                }
                else if ('z' >= symbol && symbol >= 'a') {
                    word += symbol;
                    if (!reader.EndOfStream)
                        goto Basic_Label;
                }
                if (word != "" && symbol != '-' && symbol != '\'') {
                    i = 0;
                    Check_Stop_Words_Label: { // Мітка для перевірки чи являється слово стоп словом
                        if (word == stopWords[i]) {
                            word = "";
                            if (reader.EndOfStream)
                                goto End_Reading_Label;
                            goto Basic_Label;
                        }
                        i++;
                        if (i < stopWords.Length)
                            goto Check_Stop_Words_Label;
                    }
                    i = 0;
                    Check_Words_Label: { // Мітка для перевірки чи це нове слово
                        if (i == length)
                            goto New_Word_Label;
                        if (word == words[i]) {
                            counts[i]++;
                            word = "";
                            if (reader.EndOfStream)
                                goto End_Reading_Label;
                            goto Basic_Label;
                        }
                        i++;
                        goto Check_Words_Label;
                    }
                    New_Word_Label: // Мітка для обробки появи нового слова
                    if (length == words.Length) {
                        string[] New_Word_Labels = new string[(length + 1) * 2];
                        int[] newCounts = new int[(length + 1) * 2];
                        i = 0;
                        For_Copy_Label: { // Мітка для запису даних про нове слово
                            if (i == length) {
                                words = New_Word_Labels;
                                counts = newCounts;
                                goto End_Copy_Label;
                            }
                            New_Word_Labels[i] = words[i];
                            newCounts[i] = counts[i];
                            i++;
                            goto For_Copy_Label;
                        }
                    }
                    End_Copy_Label: // Мітка для завершення запису даних про нове слово
                    words[length] = word;
                    counts[length] = 1;
                    word = "";
                    length++;
                }
                if(!reader.EndOfStream) // Якщо не кінець файлу, то продовжуємо все з початку основної мітки
                    goto Basic_Label;
            }
            End_Reading_Label: // Мітка для завершення читання
            reader.Close();
            int  current, c;
            i = 1;
            Sort_Label: { // Мітка початку сортування вставленням
                current = counts[i];
                word = words[i];
                c = i - 1;
                While_Sort_Label: { // Мітка основної частини сортування
                    if (c >= 0 && counts[c] < current) {
                        counts[c + 1] = counts[c];
                        words[c + 1] = words[c];
                        c--;
                        goto While_Sort_Label;
                    }
                }
                counts[c + 1] = current;
                words[c + 1] = word;
                i++;
                if (i < length)
                    goto Sort_Label;
            }
            StreamWriter writer = new StreamWriter(outPath); //  Виводимо вихідні дані в файл
            i = 0;
            Write_Label: { // Мітка для виведення даних в файл
                writer.WriteLine(words[i] + " - " + counts[i]);
                i++;
                if (i < maxWordsCount && i < length) // Обмеження кількості результатів 25
                    goto Write_Label;
            }
            writer.Close(); // Завершення запису
        }
    }
}