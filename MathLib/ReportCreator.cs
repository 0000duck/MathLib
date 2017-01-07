using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace MathLib
{
    abstract class ReportCreator    //абстрактный класс для создания отчетов
    {
        protected StreamWriter sWriter;     //Поле потока записи в файл
        protected StringBuilder content;    //Поле, отвечающее за текстовое представление данных отчета 
        protected string fileName;          //Название файла, в котором генерируется отчет
        public abstract void WriteLine(string text);    //Метод для записи текста в отчет
        public abstract void WriteMatrix(Matrix matrix);    //Метод для записи матрицы в отчет
    }
}
