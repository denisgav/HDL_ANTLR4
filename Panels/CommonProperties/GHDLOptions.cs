using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Schematix.CommonProperties
{
    [Serializable()]
    public class GHDLOptions : IOptions, ISerializable
    {
        /// <summary>
        /// Выражение для запуска компиляции файла
        /// </summary>
        private string compileCommandExpression;
        public string CompileCommandExpression
        {
            get { return compileCommandExpression; }
            set { compileCommandExpression = value; }
        }

        /// <summary>
        /// Выражение для запуска выполнения очистки проекта
        /// </summary>
        private string cleanCommandExpression;
        public string CleanCommandExpression
        {
            get { return cleanCommandExpression; }
            set { cleanCommandExpression = value; }
        }

        /// <summary>
        /// Выражение для запуска синтаксического анализа файла
        /// </summary>
        private string syntaxAnalyseCommandExpression;
        public string SyntaxAnalyseCommandExpression
        {
            get { return syntaxAnalyseCommandExpression; }
            set { syntaxAnalyseCommandExpression = value; }
        }

        /// <summary>
        /// Выражение для запуска елаборации файла файла
        /// </summary>
        private string elaborationCommandExpression;
        public string ElaborationCommandExpression
        {
            get { return elaborationCommandExpression; }
            set { elaborationCommandExpression = value; }
        }

        /// <summary>
        /// Выражение для запуска моделирования
        /// </summary>
        private string simulationCommandExpression;
        public string SimulationCommandExpression
        {
            get { return simulationCommandExpression; }
            set { simulationCommandExpression = value; }
        }

        /// <summary>
        /// Используется ли переменная окружения
        /// </summary>
        private bool isEnvirPathUsed;
        public bool IsEnvirPathUsed
        {
            get { return isEnvirPathUsed; }
            set { isEnvirPathUsed = value; }
        }

        /// <summary>
        /// Пть к исполняемому файлу GHDL
        /// </summary>
        private string _GHDL_Bin_Path;
        public string GHDL_BIN_Path
        {
            get { return _GHDL_Bin_Path; }
            set { _GHDL_Bin_Path = value; }
        }

        /// <summary>
        /// Получение пути к папке GHDL с переменной окружения
        /// </summary>
        /// <returns></returns>
        public static string GetGHDLEnvirPath()
        {
            string PathEnvirVariable = System.Environment.GetEnvironmentVariable("Path");

            string[] paths = PathEnvirVariable.Split(new string[]{";"}, StringSplitOptions.RemoveEmptyEntries);
            foreach(string path in paths)
            {
                if(System.IO.Directory.Exists(path))
                {
                    try
                    {
                        System.IO.DirectoryInfo dir_inf = new System.IO.DirectoryInfo(path);
                        foreach (System.IO.FileInfo file in dir_inf.GetFiles())
                            if (file.Name == "ghdl.exe")
                                return path;
                    }
                    catch (Exception ex)
                    { }
                }
            }

            return "";
        }

        /// <summary>
        /// Получить предполагаемое расположение GHDL
        /// </summary>
        /// <returns></returns>
        public static string GetPossibleGHDLPlacement()
        {
            string CurDirectory = System.IO.Directory.GetCurrentDirectory();
            string GHDLPath = @"Ghdl\bin\";
            string path = System.IO.Path.Combine(CurDirectory, GHDLPath);
            return path;
        }

        /// <summary>
        /// Сформировать имя приложения, основываясь на настройках пользователя
        /// </summary>
        /// <returns></returns>
        private string FormAppName()
        {
            if (isEnvirPathUsed == true)
                return "ghdl";
            else
                return System.IO.Path.Combine(this.GHDL_BIN_Path, "ghdl.exe");
        }

        /// <summary>
        /// Сформировать аргументы для запуска процесса
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private string FormArguments(string expression, IDictionary<string, string> arguments, IList<string> argumentsToRemove)
        {
            string res = new string(expression.ToCharArray());

            //Удаляем все лишнее
            foreach (string rem in argumentsToRemove)
                res = res.Replace(rem, string.Empty);

            //Заменяем аргументы
            foreach (KeyValuePair<string, string> arg in arguments)
                res = res.Replace(arg.Key, arg.Value);

            return res;
        }

        #region CleanCommand
        /// <summary>
        /// Сформировать объект ProcessStartInfo для запуска очистки проекта
        /// </summary>
        /// <returns></returns>
        public System.Diagnostics.ProcessStartInfo FormCleanProcessStartInfo(string workDirPath)
        {
            string AppName = string.Empty;
            string Arguments = string.Empty;

            FormCleanCommand(workDirPath, out AppName, out Arguments);

            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(AppName, Arguments);

            return psi;
        }

        /// <summary>
        /// Сформировать объект ProcessStartInfo для запуска очистки проекта
        /// </summary>
        /// <returns></returns>
        public void FormCleanCommand(string workDirPath, out string AppName, out string Arguments)
        {
            AppName = FormAppName();

            //"{GHDL} -s -fexplicit \"{filePath}\""
            Dictionary<string, string> arguments = new Dictionary<string, string>();
            arguments.Add("{workDirPath}", workDirPath);
            List<string> argumentsToRemove = new List<string>();
            argumentsToRemove.Add("\"{GHDL}\"");
            Arguments = FormArguments(this.cleanCommandExpression, arguments, argumentsToRemove);
        }
        #endregion

        #region SyntaxAnalyseCommand
        /// <summary>
        /// Сформировать объект ProcessStartInfo для запуска проверки синтаксиса
        /// </summary>
        /// <returns></returns>
        public System.Diagnostics.ProcessStartInfo FormSyntaxAnalyseProcessStartInfo(string _strFileToCheck, string workDirPath)
        {
            string AppName = string.Empty;
            string Arguments = string.Empty;

            FormSyntaxAnalyseCommand(_strFileToCheck, workDirPath, out AppName, out Arguments);

            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(AppName, Arguments);

            return psi;
        }

        /// <summary>
        /// Сформировать объект ProcessStartInfo для запуска проверки синтаксиса
        /// </summary>
        /// <returns></returns>
        public void FormSyntaxAnalyseCommand(string _strFileToCheck, string workDirPath, out string AppName, out string Arguments)
        {
            AppName = FormAppName();

            //"{GHDL} -s -fexplicit \"{filePath}\""
            Dictionary<string, string> arguments = new Dictionary<string, string>();
            arguments.Add("{filePath}", _strFileToCheck);
            arguments.Add("{workDirPath}", workDirPath);
            List<string> argumentsToRemove = new List<string>();
            argumentsToRemove.Add("\"{GHDL}\"");
            Arguments = FormArguments(this.syntaxAnalyseCommandExpression, arguments, argumentsToRemove);
        }
        #endregion

        #region CompileCommand
        /// <summary>
        /// Сформировать объект ProcessStartInfo для запуска проверки синтаксиса
        /// </summary>
        /// <returns></returns>
        public System.Diagnostics.ProcessStartInfo FormCompileProcessStartInfo(string _strFileToCompile, string workDirPath)
        {
            string AppName = string.Empty;
            string Arguments = string.Empty;

            FormCompileCommand(_strFileToCompile, workDirPath, out AppName, out Arguments);

            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(AppName, Arguments);

            return psi;
        }

        /// <summary>
        /// Сформировать объект ProcessStartInfo для запуска проверки синтаксиса
        /// </summary>
        /// <returns></returns>
        public void FormCompileCommand(string _strFileToCompile, string workDirPath, out string AppName, out string Arguments)
        {
            AppName = FormAppName();

            //"{GHDL} -a -fexplicit \"{filePath}\""
            Dictionary<string, string> arguments = new Dictionary<string, string>();
            arguments.Add("{filePath}", _strFileToCompile);
            arguments.Add("{workDirPath}", workDirPath);
            List<string> argumentsToRemove = new List<string>();
            argumentsToRemove.Add("\"{GHDL}\"");
            Arguments = FormArguments(this.compileCommandExpression, arguments, argumentsToRemove);
        }
        #endregion

        #region ElaborateCommand
        /// <summary>
        /// Сформировать объект ProcessStartInfo для запуска проверки синтаксиса
        /// </summary>
        /// <returns></returns>
        public System.Diagnostics.ProcessStartInfo FormElaborateProcessStartInfo(string _Entity, string _Architecture, string workDirPath)
        {
            string AppName = string.Empty;
            string Arguments = string.Empty;

            FormElaborateCommand(_Entity, _Architecture, workDirPath, out AppName, out Arguments);

            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(AppName, Arguments);

            return psi;
        }

        /// <summary>
        /// Сформировать объект ProcessStartInfo для запуска проверки синтаксиса
        /// </summary>
        /// <returns></returns>
        public void FormElaborateCommand(string _Entity, string _Architecture, string workDirPath, out string AppName, out string Arguments)
        {
            AppName = FormAppName();

            //"{GHDL} -e -fexplicit {TbEntityName} {TbArchitectureName}"
            Dictionary<string, string> arguments = new Dictionary<string, string>();
            arguments.Add("{TbEntityName}", _Entity);
            arguments.Add("{TbArchitectureName}", _Architecture);
            arguments.Add("{workDirPath}", workDirPath);
            List<string> argumentsToRemove = new List<string>();
            argumentsToRemove.Add("\"{GHDL}\"");
            Arguments = FormArguments(this.elaborationCommandExpression, arguments, argumentsToRemove);
        }
        #endregion

        #region SimulationCommand
        /// <summary>
        /// Сформировать объект ProcessStartInfo для запуска проверки синтаксиса
        /// </summary>
        /// <returns></returns>
        public System.Diagnostics.ProcessStartInfo FormSimulationStartInfo(string _Entity, string _Architecture, string _WaveDumpFilePath, string workDirPath)
        {
            string AppName = string.Empty;
            string Arguments = string.Empty;

            FormSimulationCommand(_Entity, _Architecture, _WaveDumpFilePath, workDirPath, out AppName, out Arguments);

            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(AppName, Arguments);

            return psi;
        }

        /// <summary>
        /// Сформировать объект ProcessStartInfo для запуска проверки синтаксиса
        /// </summary>
        /// <returns></returns>
        public void FormSimulationCommand(string _Entity, string _Architecture, string _WaveDumpFilePath, string workDirPath, out string AppName, out string Arguments)
        {
            AppName = FormAppName();

            //{GHDL} {TbEntityName} {TbArchitectureName}  --vcd=\"{FilePath}\" --stack-size=128m --stack-max-size=256m
            Dictionary<string, string> arguments = new Dictionary<string, string>();
            arguments.Add("{TbEntityName}", _Entity);
            arguments.Add("{TbArchitectureName}", _Architecture);
            arguments.Add("{FilePath}", _WaveDumpFilePath);
            arguments.Add("{workDirPath}", workDirPath);
            List<string> argumentsToRemove = new List<string>();
            argumentsToRemove.Add("\"{GHDL}\"");
            Arguments = FormArguments(this.simulationCommandExpression, arguments, argumentsToRemove);
        }
        #endregion

        #region constructors
        public GHDLOptions()
        {
        }
        public GHDLOptions(SerializationInfo info, StreamingContext ctxt)
        {
            this.cleanCommandExpression = (string)info.GetValue("CleanCommandExpression", typeof(string));
            this.compileCommandExpression = (string)info.GetValue("CompileCommandExpression", typeof(string));
            this.syntaxAnalyseCommandExpression = (string)info.GetValue("SyntaxAnalyseCommandExpression", typeof(string));
            this.elaborationCommandExpression = (string)info.GetValue("ElaborationCommandExpression", typeof(string));
            this.simulationCommandExpression = (string)info.GetValue("SimulationCommandExpression", typeof(string));
            this.isEnvirPathUsed = (bool)info.GetValue("IsEnvirPathUsed", typeof(bool));
            this._GHDL_Bin_Path = (string)info.GetValue("GHDL_Bin_Path", typeof(string));
        }
        #endregion


        #region IOptions Members

        public void LoadData(Options options)
        {
            options.SetOptionsData(this);
        }

        public void Accept(Options options)
        {
            options.GetOptionsData(this);
        }

        public void SetDefault()
        {
            this.cleanCommandExpression = "\"{GHDL}\" --remove";
            this.compileCommandExpression = "\"{GHDL}\" -a -fexplicit \"{filePath}\"";
            this.syntaxAnalyseCommandExpression = "\"{GHDL}\" -s -fexplicit \"{filePath}\"";
            this.elaborationCommandExpression = "\"{GHDL}\" -e -fexplicit {TbEntityName} {TbArchitectureName}";
            this.simulationCommandExpression = "\"{GHDL}\" -r -fexplicit  {TbEntityName} {TbArchitectureName}  --vcd=\"{FilePath}\" --stack-size=128m --stack-max-size=256m";
            this._GHDL_Bin_Path = GetPossibleGHDLPlacement();
            this.isEnvirPathUsed = string.IsNullOrWhiteSpace(GetGHDLEnvirPath()) == false;
        }

        #endregion

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("CleanCommandExpression", CleanCommandExpression);
            info.AddValue("CompileCommandExpression", CompileCommandExpression);
            info.AddValue("SyntaxAnalyseCommandExpression", SyntaxAnalyseCommandExpression);
            info.AddValue("ElaborationCommandExpression", ElaborationCommandExpression);
            info.AddValue("SimulationCommandExpression", SimulationCommandExpression);
            info.AddValue("IsEnvirPathUsed", IsEnvirPathUsed);
            info.AddValue("GHDL_Bin_Path", GHDL_BIN_Path);
        }

        #endregion
    }
}
