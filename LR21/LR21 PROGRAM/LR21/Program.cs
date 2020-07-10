using System.IO;
using System;

class Program
{

    public enum Method
    {
        Method1,
        Method2
    }

    class PolybiusSquare
    {
        char[,] square;
        string alphabet;
        Method encryptMethod;

        public PolybiusSquare(string alphabet = null, Method cipherMethod = Method.Method1)
        {
            this.alphabet = alphabet ?? "ABCDEFGHIJKLMNOPQRSTUVWXYZ ";
            encryptMethod = cipherMethod;
        }

        char[,] GetSquare(string key)
        {
            var newAlphabet = alphabet;
            for (int i = 0; i < key.Length; i++)
            {
                newAlphabet = newAlphabet.Replace(key[i].ToString(), "");
            }


            newAlphabet = key + newAlphabet + "0123456789!@#$%^&*)_+-=<>?,.";


            var n = (int)Math.Ceiling(Math.Sqrt(alphabet.Length));


            square = new char[n, n];
            var index = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (index < newAlphabet.Length)
                    {
                        square[i, j] = newAlphabet[index];
                        index++;
                    }
                }
            }

            return square;
        }


        bool FindSymbol(char[,] symbolsTable, char symbol, out int column, out int row)
        {
            var l = symbolsTable.GetUpperBound(0) + 1;
            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    if (symbolsTable[i, j] == symbol)
                    {

                        row = i;
                        column = j;
                        return true;
                    }
                }
            }


            row = -1;
            column = -1;
            return false;
        }

        public string PolibiusEncrypt(string text, string password)
        {
            var outputText = "";
            var square = GetSquare(password);
            switch (encryptMethod)
            {
                case Method.Method1:
                    for (int i = 0; i < text.Length; i++)
                    {
                        if (FindSymbol(square, text[i], out int columnIndex, out int rowIndex))
                        {
                            var newRowIndex = rowIndex == square.GetUpperBound(1)
                                ? 0
                                : rowIndex + 1;
                            outputText += square[newRowIndex, columnIndex].ToString();
                        }
                    }
                    break;

                case Method.Method2:
                    var m = text.Length;
                    var coordinates = new int[m * 2];
                    for (int i = 0; i < m; i++)
                    {
                        if (FindSymbol(square, text[i], out int columnIndex, out int rowIndex))
                        {
                            coordinates[i] = columnIndex;
                            coordinates[i + m] = rowIndex;
                        }
                    }

                    for (int i = 0; i < m * 2; i += 2)
                    {
                        outputText += square[coordinates[i + 1], coordinates[i]];
                    }
                    break;
            }

            return outputText;
        }

        public string PolybiusDecrypt(string text, string password)
        {
            var outputText = "";
            var square = GetSquare(password);
            var m = text.Length;
            switch (encryptMethod)
            {
                case Method.Method1:
                    for (int i = 0; i < m; i++)
                    {
                        if (FindSymbol(square, text[i], out int columnIndex, out int rowIndex))
                        {
                            var newRowIndex = rowIndex == 0
                                ? square.GetUpperBound(1)
                                : rowIndex - 1;
                            outputText += square[newRowIndex, columnIndex].ToString();
                        }
                    }
                    break;

                case Method.Method2:
                    var coordinates = new int[m * 2];
                    var j = 0;
                    for (int i = 0; i < m; i++)
                    {
                        if (FindSymbol(square, text[i], out int columnIndex, out int rowIndex))
                        {
                            coordinates[j] = columnIndex;
                            coordinates[j + 1] = rowIndex;
                            j += 2;
                        }
                    }

                    for (int i = 0; i < m; i++)
                    {
                        outputText += square[coordinates[i + m], coordinates[i]];
                    }
                    break;
            }

            return outputText;
        }
    }
    static string input1 = @"C:\Users\Levi\Desktop\LR21\input1.txt";
    static string input2 = @"C:\Users\Levi\Desktop\LR21\input2.txt";
    static string output1 = @"C:\Users\Levi\Desktop\LR21\output1.txt";
    static string key = @"C:\Users\Levi\Desktop\LR21\key.txt";
    static void Main(string[] args)
    {
        Program Method = new Program();
        Method.menu();
        while (true)
        {
            string command = Console.ReadLine();

            switch (command)
            {
                case "1":
                    {
                        Console.Clear();
                        Method.Encrypt();
                        Console.ReadKey();
                        Console.Clear();
                        Method.menu();
                        break;
                    }
                case "2":
                    {
                        Console.Clear();
                        Method.decrypt();
                        Console.ReadKey();
                        Console.Clear();
                        Method.menu();
                        break;
                    }
            }
        }
    }
    public void decrypt()
    {
        var polybius = new PolybiusSquare();
        var message = System.IO.File.ReadAllText(input2);
        Console.WriteLine("Зашифрованный текст: " + message);
        Console.Write("Введите ключ: ");
        var pass = Console.ReadLine().ToUpper();
        var cipherText = polybius.PolybiusDecrypt(message, pass);
        Console.WriteLine("Расшифрованный текст: {0}", cipherText);
        System.IO.File.WriteAllText(output1, cipherText);
    }
    public void Encrypt()
    {
        var polybius = new PolybiusSquare();
        var message = System.IO.File.ReadAllText(input1);
        Console.WriteLine("Шифруемый текст: " + message);
        Console.Write("Введите ключ: ");
        var pass = Console.ReadLine().ToUpper();
        System.IO.File.WriteAllText(key, pass);
        var cipherText = polybius.PolibiusEncrypt(message, pass);
        Console.WriteLine("Зашифрованный текст: {0}", cipherText);
        System.IO.File.WriteAllText(input2, cipherText);
    }
    public void menu()
    {
        Console.WriteLine("Выберите действие");
        Console.WriteLine("1) Зашифровать текст");
        Console.WriteLine("2) Расшифровать текст");
    }

}