using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathLib
{
    class HtmlReportCreator : ReportCreator     //Класс для создания HTML-отчетов
    {

        public HtmlReportCreator(string fname)  //конструктор, инициализирующий HTML-отчет с заданным названием файла
        {
            string s = "<html>" +
                            "<head>" +
                                "<link rel='stylesheet' href='styles.css'>" +
                                "<meta charset = 'utf-8'/>" +
                                "<title>Отчет</title>" +
                            "</head>" +
                            " <body> {content} </body>" +
                        "</html>";
            this.sWriter = new StreamWriter(fname);
            this.fileName = fname;
            this.content = new StringBuilder(s);
        }

        public override void WriteLine(string text)     //Функция добавления текста в HTML-отчет
        {
            this.content.Replace("{content}", "<p>" + text + "</p>" + "{content}");
        }

        public override void WriteMatrix(Matrix matrix)     //Функция добавления матрицы в HTML-отчет
        {
            StringBuilder htmlTable = new StringBuilder("<table class='matrix matrix-body'>{mBody}</table> {mAns} <br/>");
            StringBuilder matrixBody = new StringBuilder("");
            StringBuilder matrixAns = new StringBuilder("");
            matrix = matrix.RoundElements(5);

            for (int i = 0; i < matrix.Rows; i++)
            {
                matrixBody.Append("<tr>");
                for (int j = 0; j < matrix.Columns; j++)
                {
                    matrixBody.Append("<td>" + matrix.MatrixBody[i, j] + "</td>");
                }
                matrixBody.Append("</tr>");
            }
            if (matrix.MatrixAns != null)
            {
                matrixAns.Append("<table class='matrix matrix-ans'>");
                for (int i = 0; i < matrix.Rows; i++)
                {
                    matrixAns.Append("<tr><td>" + matrix.MatrixAns[i] + "</td></tr>");
                }
                matrixAns.Append("</table>");
            }

            htmlTable.Replace("{mBody}", matrixBody.ToString());
            htmlTable.Replace("{mAns}", matrixAns.ToString());
            content.Replace("{content}", htmlTable.ToString() + "{content}");
        }

        public void GenerateReport()    //функция генерации HTML-отчета
        {
            //WebBrowser wb = new WebBrowser();
            //wb.Document.OpenNew();
            sWriter.WriteLine(content.ToString());
            this.sWriter.Close();
            Process.Start(fileName);
        }

    }
}
