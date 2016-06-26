using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace MathLib
{
    class DelegateGenerator
    {
        #region static variables
        // статическая переменная для создания уникальных имен
        private static Int32 m_classIndex = 0;
        #endregion


        #region internal types
        /// <summary>
        /// класс, который содержит всю информацию о делегате
        /// </summary>
        public class DelegateInfo<T>
        {
            // исходный код функции
            public String Code { get; set; }
            // информация о созданной функции
            public MethodInfo MethodInfo { get; set; }
            // скомпелированный делегат
            public Delegate Delegate { get; set; }
            public T Exec { get { return (T)Convert.ChangeType(this.Delegate, typeof(T)); } }
            // текст ошибки
            public String ErrorText { get; set; }
            // была ли ошибка компиляции
            public bool WasError { get { return !String.IsNullOrEmpty(this.ErrorText); } }
        }
        #endregion


        #region management
        /// <summary>
        /// Создаем делегат по коду
        /// </summary>
        /// <typeparam name="T">тип делегата</typeparam>
        /// <param name="name">Имя создаваемой функции. Должно быть таким же, как в тексте функции</param>
        /// <param name="code">Текст функции</param>
        public static T CreateDelegate<T>(String funcName, String code)
        {
            // заполняем информацию о функции
            DelegateInfo<T> del = new DelegateInfo<T>();
            del.Code = code;
            del.MethodInfo = null;
            del.Delegate = null;
            del.ErrorText = String.Empty;

            // компилируем функцию
            CompileDelegate<T>(funcName, del);
            if (!del.WasError)
                return (T)Convert.ChangeType(del.Delegate, typeof(T));
            return default(T);
        }


        /// <summary>
        /// Создаем делегат по коду и возвращаем структуру с информацией о нем
        /// </summary>
        /// <typeparam name="T">тип делегата</typeparam>
        /// <param name="name">Имя создаваемой функции. Должно быть таким же, как в тексте функции</param>
        /// <param name="code">Текст функции</param>
        public static DelegateInfo<T> CreateDelegateInfo<T>(String funcName, String code)
        {
            // заполняем информацию о функции
            DelegateInfo<T> del = new DelegateInfo<T>();
            del.Code = code;
            del.MethodInfo = null;
            del.Delegate = null;
            del.ErrorText = String.Empty;

            // компилируем функцию
            CompileDelegate<T>(funcName, del);
            return del;
        }
        #endregion


        #region compile functions
        /// <summary>
        /// Компилируем делегат
        /// </summary>
        private static void CompileDelegate<T>(String name, DelegateInfo<T> del)
        {
            // перечисляем все библиотеки, от которых может зависеть текст функции
            String[] referenceAssemblies =
            {
                                "System.dll",
                                "System.Data.dll",
                                "System.Design.dll",
                                "System.Drawing.dll",
                                "System.Windows.Forms.dll",
                                "System.Xml.dll"
                        };

            String className = "C" + name + m_classIndex.ToString();
            m_classIndex++;

            // создаем полный текст класса с функцией
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using System.Design;");
            sb.AppendLine("using System.Drawing;");
            sb.AppendLine("using System.Windows.Forms;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("namespace DelegateGenerator");
            sb.AppendLine("{");
            sb.Append("    public class "); sb.AppendLine(className);
            sb.AppendLine("        {");
            sb.AppendLine(del.Code);
            sb.AppendLine("        }");
            sb.AppendLine("}");

            // компилируем класс
            
            CompilerParameters codeParams = new CompilerParameters(referenceAssemblies);
            codeParams.GenerateExecutable = false;
            codeParams.GenerateInMemory = true;
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            CompilerResults codeResult = codeProvider.CompileAssemblyFromSource(codeParams, sb.ToString());

            // проверяем результат на ошибку
            if (codeResult.Errors.HasErrors)
            {
                StringBuilder err = new StringBuilder();
                for (int i = 0; i < codeResult.Errors.Count; ++i)
                    err.AppendLine(codeResult.Errors[i].ToString());
                del.ErrorText = err.ToString();
                return;
            }

            // получаем функцию созданного класса по имени
            Type type = codeResult.CompiledAssembly.GetType("DelegateGenerator." + className);
            del.MethodInfo = type.GetMethod(name);
            if (null == del.MethodInfo)
            {
                del.ErrorText = String.Format("Delegate name '{0}' error", name);
            }
            else
            {
                del.Delegate = Delegate.CreateDelegate(typeof(T), del.MethodInfo);
                if (null == del.Delegate)
                    del.ErrorText = String.Format("Delegate type '{0}' error", name);
            }
        }
    }
    #endregion
}
