using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ADODB;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{
    Operations operations = new Operations();
    string mathformula = string.Empty;
    DateTime date = DateTime.Now;

    public double AddNumber(double number1, double number2, Guid myGuid)
    {
        double result = number1 + number2;
        mathformula = string.Format("{0} + {1} = {2}", number1, number2, result);
        Save(mathformula, myGuid, date);
        return (result);
    }
    public double SubNumber(double number1, double number2, Guid myGuid)
    {
        double result = number1 - number2;
        mathformula = string.Format("{0} - {1} = {2}", number1, number2, result);
        Save(mathformula, myGuid, date);
        return (result);
    }
    public double MultNumber(double number1, double number2, Guid myGuid)
    {
        double result = number1 * number2;
        mathformula = string.Format("{0} * {1} = {2}", number1, number2, result);
        Save(mathformula, myGuid, date);
        return (result);
    }
    public double DivNumber(double number1, double number2, Guid myGuid)
    {
        double result = number1 / number2;
        mathformula = string.Format("{0} / {1} = {2}", number1, number2, result);
        Save(mathformula, myGuid, date);
        return (result);
    }
    public double SignChange(double number1, Guid myGuid)
    {
        double result = number1 * -1;
        mathformula = string.Format("{0} * (-1) = {1}", number1, result);
        Save(mathformula, myGuid, date);
        return (result);
    }

    public void Save(string mathformula, Guid myGuid, DateTime date)
    {
        operations.Connect();
        operations.Insert("CalcDB", date.ToString(), myGuid.ToString(), mathformula);
    }

    public List<string> HistoryReport(Guid myGuid)
    {
        List<string> resultList = new List<string>();
        string historyReport = string.Empty;

        try { operations.Connect(); }
        catch {
            //"Error. Can't connect to the database";
            return resultList; 
        }

        try { resultList = operations.ShowAll("CalcDB", myGuid, historyReport, resultList); }
        catch {
            //"Error. No operation has been done yet";
            return resultList;
        }
        return resultList;
    }

    class Operations
    {
        Connection conn = new Connection();
        Guid myGuid = Guid.NewGuid();
        public void Connect()
        {
            bool connection = false;
            do
            {
                string server = "xxxxxxx";

                string logId = "xxxxxxx";
                string logPassword = "xxxxxxx";
                string initialCatalog = "xxxxxxx";
                string connString = "Provider=xxxxxxx;Data Source=" + server + ";Initial Catalog=" + initialCatalog;
                try
                {
                    conn.Open(connString, logId, logPassword, 0);
                    connection = true;
                }
                catch { }
                return;
            }
            while (connection == false);
        }

        public void CreateCommand(Command cmd, string commandText)
        {
            cmd.ActiveConnection = conn;
            cmd.CommandText = commandText;
            cmd.CommandType = CommandTypeEnum.adCmdText;
        }

        public void CreateParameter(Command cmd, string paramName, string paramValue)
        {
            Parameter param = cmd.CreateParameter(
            paramName,
            DataTypeEnum.adVarChar,
            ParameterDirectionEnum.adParamInput,
            50,
            paramValue);
            cmd.Parameters.Append(param);
        }

        public string CreateInsertIntoTextCommand(string tableName, Recordset rs)
        {
            string textCommand = String.Format("INSERT INTO {0} (GUID, Date, Operation) VALUES (?,?,?)", tableName);
            return textCommand;
        }

        public void OpenRecordsetWithGivenSelectCmd(string selectCmd, string tableName, Recordset rs)
        {
            rs.Open(selectCmd,
                conn,
                CursorTypeEnum.adOpenForwardOnly,
                LockTypeEnum.adLockOptimistic,
                (int)CommandTypeEnum.adCmdText);
        }

        public void InsertDataIntoParameters(string guid, string date, string operation, Recordset rs, Command cmdInsert)
        {
            CreateParameter(cmdInsert, "GUID", guid);
            CreateParameter(cmdInsert, "Date", date);
            CreateParameter(cmdInsert, "Operation", operation);
        }

        public void Insert(string tableName, string date, string guid, string operation)
        {
            Recordset rs = new Recordset();
            string selectCmd = "SELECT * FROM " + tableName;
            try
            {
                OpenRecordsetWithGivenSelectCmd(selectCmd, tableName, rs);
                Command cmdInsert = new Command();
                string textCommand = CreateInsertIntoTextCommand(tableName, rs);
                CreateCommand(cmdInsert, textCommand);

                InsertDataIntoParameters(guid, date, operation, rs, cmdInsert);

                object nRecordsAffected = Type.Missing;
                object oParams = Type.Missing;

                cmdInsert.Execute(out nRecordsAffected, ref oParams,
                    (int)ExecuteOptionEnum.adExecuteNoRecords);
                rs.Close();
                Console.WriteLine("\nSuccess.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error. " + ex);
            }
        }

        public List<string> ShowAll(string tableName, Guid myGuid, string historyReport, List<string> resultList)
        {
            resultList = new List<string>();
            Recordset rs = new Recordset();
            historyReport = string.Empty;
            string selectCmd = String.Format("SELECT * FROM CalcDB where GUID = '{0}'", myGuid.ToString());

            try
            {
                OpenRecordsetWithGivenSelectCmd(selectCmd, tableName, rs);
            }
            catch {
                //String.Format("No operations with guid {0} found.", myGuid.ToString());
                return resultList;
            }

            rs.MoveFirst();

            string temp = string.Empty;

            while (!rs.EOF)
            {
                for (int i = 0; i < rs.Fields.Count - 1; i++)
                {
                    temp = rs.Fields[i + 1].Value.ToString();
                    resultList.Add(temp);
                }
                rs.MoveNext();
            }
            rs.Close();
            return resultList;
        }
    }
}