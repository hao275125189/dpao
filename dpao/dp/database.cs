using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Windows.Forms;

using System.Data.SQLite;

namespace dpao.dp
{
    class database
    {
         #region 数据连接字符串
         protected static string conn=System.Windows.Forms.Application.StartupPath+"\\all.db";
        protected static string ConnectionStr ="Data Source="+conn;
        
　　 
　　 #endregion
　　 #region SQLite操作
　　 ///

　　 /// 执行SQLite语句，返回影响的记录数
　　 ///

　　 /// 
　　 /// 
     private static string yum="Data Source={0}\\pdb.db;Version=3;";
　　 public static int ExecuteSQLite(string SQLiteString)
　　 {
       SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
　　 SQLiteCommand Cmd = new SQLiteCommand(SQLiteString, Conn);
　　 try
　　 {
　　 Conn.Open();
　　 int rows = Cmd.ExecuteNonQuery();
　　 return rows;
　　 }
　　 catch (SQLiteException E)
　　 {
　　 throw new Exception(E.Message);
　　 }
　　 }
　　 ///

　　 /// 执行两条SQLite语句，实现数据库事务
　　 ///

　　 /// 
　　 /// 
　　 public static void ExecuteSQLiteTran(string SQLiteString1, string SQLiteString2)
　　 {
　　 SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
　　 SQLiteCommand Cmd = new SQLiteCommand();
　　 Cmd.Connection = Conn;
　　 SQLiteTransaction tx = Conn.BeginTransaction();
　　 Cmd.Transaction = tx;
　　 try
　　 {
　　 Cmd.CommandText = SQLiteString1;
　　 Cmd.ExecuteNonQuery();
　　 Cmd.CommandText = SQLiteString2;
　　 Cmd.ExecuteNonQuery();
　　 tx.Commit();
　　 }
　　 catch (SQLiteException E)
　　 {
　　 throw new Exception(E.Message);
　　 }
　　 finally
　　 {
　　 Cmd.Dispose();
　　 Conn.Close();
　　 }
　　 }
　　 ///

　　 /// 执行多条SQLite语句,实现数据库事务，每条语句以";"分割
　　 ///

　　 /// 
　　 public static void ExecuteSQLiteTran(string SQLiteStringList)
　　 {
　　 SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
　　 Conn.Open();
　　 SQLiteCommand Cmd = new SQLiteCommand();
　　 SQLiteTransaction tx = Conn.BeginTransaction();
　　 Cmd.Transaction = tx;
　　 try
　　 {
　　 string[] split = SQLiteStringList.Split(new Char[] { ';' });
　　 foreach (string strSQLite in split)
　　 {
　　 if (strSQLite.Trim() != "")
　　 {
　　 Cmd.CommandText = strSQLite;
　　 Cmd.ExecuteNonQuery();
　　 }
　　 }
　　 tx.Commit();
　　 }
　　 catch (SQLiteException E)
　　 {
　　 tx.Rollback();
　　   Console.WriteLine(E.Message);
　　 }
　　 }
　　 ///

　　 /// 执行带一个存储过程参数的SQLite语句
　　 ///

　　 /// 
　　 /// 
　　 /// 
　　 public static int ExecuteSQLite(string SQLiteString, string Content)
　　 {
　　 SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
　　 SQLiteCommand Cmd = new SQLiteCommand(SQLiteString, Conn);
　　 SQLiteParameter MyParameter = new SQLiteParameter("@Content");
　　 MyParameter.Value = Content;
　　 Cmd.Parameters.Add(MyParameter);
　　 try
　　 {
　　 Conn.Open();
　　 int rows = Cmd.ExecuteNonQuery();
　　 return rows;
　　 }
　　 catch (SQLiteException E)
　　 {
　　 throw new Exception(E.Message);
　　 }
　　 finally
　　 {
　　 Cmd.Dispose();
　　 Conn.Close();
　　 }
　　 }
　　 ///

　　 /// 向数据库中插入图像格式的字段
　　 ///

　　 /// 
　　 /// 
　　 /// 
　　

　　 /// 执行一条计算结果语句，返回查询结果（整数）
　　 ///

　　 /// 
　　 /// 
　　 public static int GetCount(string SQLitestring)
　　 {
　　 SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
　　 SQLiteCommand Cmd = new SQLiteCommand(SQLitestring, Conn);
　　 try
　　 {
　　 Conn.Open();
　　 SQLiteDataReader Result = Cmd.ExecuteReader();
　　 int i = 1;
　　 while (Result.Read())
　　 {
　　 i = Result.GetInt32(0);
　　 }
　　 Result.Close();
　　 return i;
　　 }
　　 catch (SQLiteException E)
　　 {
　　 throw new Exception(E.Message);
　　 }
　　 finally
　　 {
　　 Cmd.Dispose();
　　 Conn.Close();
　　 }
　　 }
　　 ///

　　 /// 执行一条计算结果语句，返回查询结果（object）
　　 ///

　　 /// 
　　 /// 
　　 public static object GetSingle(string SQLiteString)
　　 {
　　 SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
　　 SQLiteCommand Cmd = new SQLiteCommand(SQLiteString, Conn);
　　 try
　　 {
　　 Conn.Open();
　　 object obj = Cmd.ExecuteScalar();
　　 if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
　　 {
　　 return null;
　　 }
　　 else
　　 {
　　 return obj;
　　 }
　　 }
　　 catch (SQLiteException E)
　　 {
　　 throw new Exception(E.Message);
　　 }
　　 finally
　　 {
　　 Cmd.Dispose();
　　 Conn.Close();
　　 }
　　 }
　　 ///

　　 /// 执行查询语句，返回SQLiteDataReader
　　 ///

　　 /// 
　　 /// 
　　 public static SQLiteDataReader ExecuteReader(string SQLiteString)
　　 {
　　 SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
　　 SQLiteCommand Cmd = new SQLiteCommand(SQLiteString, Conn);
　　 SQLiteDataReader MyReader;
　　 try
　　 {
　　 Conn.Open();
　　 MyReader = Cmd.ExecuteReader();
　　 return MyReader;
　　 }
　　 catch (SQLiteException E)
　　 {
　　 throw new Exception(E.Message);
　　 }
　　 finally
　　 {
　　 Cmd.Dispose();
　　 Conn.Close();
　　 }
　　 }
　　 ///

　　 /// 执行查询语句，返回DataSet
　　 ///

　　 /// 
　　 /// 
　　 public static DataSet Query(string SQLiteString)
　　 {
　　     SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
　　     DataSet DS = new DataSet();
　　     try
　　     {
　　     Conn.Open();
　　     SQLiteDataAdapter DA = new SQLiteDataAdapter(SQLiteString, Conn);
　　     DA.Fill(DS, "ds");
　　     }
　　     catch (SQLiteException E)
　　     {
　　     throw new Exception(E.Message);
　　     }
　　     return DS;
　　 }
   public static DataTable Querydt(string SQLiteString)
   {
       //Console.WriteLine(ConnectionStr);
       //ConnectionStr=@"Data Source=d:\documents\visual studio 2012\Projects\WindowsFormsApplication5\bin\Debug\all.db";
       ConnectionStr = string.Format(yum, Application.StartupPath);
       SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
       DataTable DT = new DataTable();
       try
       {
           Conn.Open();
           SQLiteDataAdapter DA = new SQLiteDataAdapter(SQLiteString, Conn);
           DA.Fill(DT);
       }
       catch (SQLiteException E)
       {
           throw new Exception(E.Message);
       }
       return DT;
   }
   public static DataTable Querybbb(string SQLiteString,DataTable dt)
   {
       //Console.WriteLine(ConnectionStr);
       //ConnectionStr=@"Data Source=d:\documents\visual studio 2012\Projects\WindowsFormsApplication5\bin\Debug\all.db";
       ConnectionStr = string.Format(yum, Application.StartupPath);
       SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
       DataTable DT = new DataTable();
       try
       {
           Conn.Open();
           SQLiteDataAdapter DA = new SQLiteDataAdapter(SQLiteString, Conn);
           DA.Fill(DT);
           SQLiteCommandBuilder builder = new SQLiteCommandBuilder(DA);
           //获得可以用来更新数据源 的 update 命令
           DA.UpdateCommand = builder.GetUpdateCommand(true);            //将数据更新回 数据库!
           DA.Update(dt);	
       }
       catch (SQLiteException E)
       {
           throw new Exception(E.Message);
       }
       return DT;
   }
　　 #endregion
　　 #region 构建存储过程执行对象
　　 ///

　　 /// 构建一个SQLiteCommand对象以此执行存储过程
　　 ///

　　 /// 
　　 /// 
　　 /// 
　　 /// 
　　 private static SQLiteCommand BuildQueryCommand(SQLiteConnection Conn, string storedProcName, IDataParameter[] parameters)
　　 {
　　     SQLiteCommand Cmd = new SQLiteCommand(storedProcName, Conn);
　　     Cmd.CommandType = CommandType.StoredProcedure;
　　 
　　     //添加存储过程参数
　　     if (parameters != null)
　　     {
　　     foreach (SQLiteParameter parameter in parameters)
　　     {
　　     Cmd.Parameters.Add(parameter);
　　     }
　　     }
　　     return Cmd;
　　     }
　　 ///

　　 /// 构建一个SQLiteDataAdapter对象以此执行存储过程
　　 ///

　　 /// 
　　 /// 
　　 /// 
　　 /// 
　　 private static SQLiteDataAdapter BuildQueryDataAdapter(SQLiteConnection Conn, string storedProcName, IDataParameter[] parameters)
　　 {
　　     SQLiteDataAdapter DA = new SQLiteDataAdapter(storedProcName, Conn);
　　     DA.SelectCommand.CommandType = CommandType.StoredProcedure;
　　     //添加存储过程参数
　　     if (parameters != null)
　　     {
　　     foreach (SQLiteParameter parameter in parameters)
　　     {
　　     DA.SelectCommand.Parameters.Add(parameter);
　　     }
　　     }
　　     return DA;
　　 }
　　 #endregion
　　 #region 构建存储过程参数
　　 ///

　　 /// 生成存储过程参数
　　 ///

　　 /// 存储过程名称
　　 /// 参数类型
　　 /// 参数大小
　　 /// 参数方向
　　 /// 参数值
　　 /// 新的 Parameter 对象
　　

　　 /// 传入输入参数
　　 ///

　　 /// 存储过程名称
　　 /// 参数类型
　　 /// 参数大小
　　 /// 参数值
　　 /// 新的 Parameter 对象
　　
　　 ///

　　 /// 传入返回值参数
　　 ///

　　 /// 存储过程名称
　　 /// 参数类型
　　 /// 参数大小
　　 /// 参数值
　　 /// 新的 Parameter 对象
　　 
　　 ///

　　 /// 传入返回值参数
　　 ///

　　 /// 存储过程名称
　　 /// 参数类型
　　 /// 参数大小
　　 /// 参数值
　　 /// 新的 Parameter 对象
　
　　 #endregion
　　 #region 执行存储过程并返回不同的值
　　 ///

　　 /// 执行一个存储过程，返回影响的行数
　　 ///

　　 /// 
　　 /// 
　　 /// 
　　 public static int RunPro(string storedProcName)
　　 {
　　     SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
　　     Conn.Open();
　　     SQLiteCommand Cmd = BuildQueryCommand(Conn, storedProcName, null);
　　     return Cmd.ExecuteNonQuery();
　　 }
　　 ///

　　 /// 执行一个带参数的存储过程，返回影响的行数
　　 ///

　　 /// 
　　 /// 
　　 /// 
　　 public static int RunPro(string storedProcName, SQLiteParameter[] parameters)
　　 {
　　     SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
　　     Conn.Open();
　　     SQLiteCommand Cmd = BuildQueryCommand(Conn, storedProcName, parameters);
　　     return Cmd.ExecuteNonQuery();
　　 }
　　 ///

　　 /// 执行一个存储过程，返回DataSet类型
　　 ///

　　 /// 
　　 /// 
　　 public static void RunPro(string storedProcName,ref DataSet DS)
　　 {
　　     SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
　　     DS = new DataSet();
　　     Conn.Open();
　　     //SQLiteDataAdapter DA = new SQLiteDataAdapter();
　　     //DA.SelectCommand = BuildQueryCommand(Conn, storedProcName, null);
　　     SQLiteDataAdapter DA = BuildQueryDataAdapter(Conn, storedProcName, null);
　　     DA.Fill(DS);
　　     Conn.Close();
　　 }
　　 ///

　　 /// 执行一个带参数的存储过程，返回DataSet类型
　　 ///

　　 /// 
　　 /// 
　　 /// 
　　 public static void RunPro(string storedProcName, SQLiteParameter[] parameters, ref DataSet DS)
　　 {
　　     SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
　　     DS = new DataSet();
　　     Conn.Open();
　　     SQLiteDataAdapter DA = BuildQueryDataAdapter(Conn, storedProcName, parameters);
　　     DA.Fill(DS);
　　     Conn.Close();
　　 }
　　 ///

　　 /// 执行一个存储过程，返回SQLiteDataReader类型
　　 ///

　　 /// 
　　 /// 
　　 public static void RunPro(string storedProcName, out SQLiteDataReader DR)
　　 {
　　     SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
　　     Conn.Open();
　　     SQLiteCommand Cmd = BuildQueryCommand(Conn, storedProcName, null);
　　     //DR = Cmd.EndExecuteReader(CommandBehavior.CloseConnection);
　　     DR = Cmd.ExecuteReader(CommandBehavior.CloseConnection);
　　 }
　　 ///

　　 /// 执行一个带参数的存储过程，返回SQLiteDataReader类型
　　 ///

　　 /// 
　　 /// 
　　 /// 
　　 public static void RunPro(string storedProcName, SQLiteParameter[] parameters, out SQLiteDataReader DR)
　　 {
　　     SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
　　     Conn.Open();
　　     SQLiteCommand Cmd = BuildQueryCommand(Conn, storedProcName, parameters);
　　     DR = Cmd.ExecuteReader(CommandBehavior.CloseConnection);
　　 }
　　 ///

　　 /// 执行一个带参数存储过程，返回一个SQLiteDataAdapter类型
　　 ///

　　 /// 
　　 /// 
　　 /// 
　　 public static void RunPro(string storedProcName, SQLiteParameter[] parameters,out SQLiteDataAdapter DA)
　　 {
　　     SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
　　     DA = new SQLiteDataAdapter();
　　     Conn.Open();
　　     SQLiteCommand Cmd = BuildQueryCommand(Conn, storedProcName, parameters);
　　     DA.SelectCommand = Cmd;
　　 //Conn.Close();
　　 }
　　 ///

　　 /// 执行一个带参数存储过程，并设置一个表名，返回一个SQLiteDataAdapter类型
　　 ///

　　 /// 
　　 /// 
　　 /// 
　　 /// 
　　 public static void RunPro(string storedProcName, SQLiteParameter[] parameters, string tableName,out SQLiteDataAdapter DA)
　　 {
　　     SQLiteConnection Conn = new SQLiteConnection(ConnectionStr);
　　     DataSet DS = new DataSet();
　　     Conn.Open();DA = new SQLiteDataAdapter();
　　     DA.SelectCommand = BuildQueryCommand(Conn, storedProcName, parameters);
　　     DA.Fill(DS, tableName);
　　     Conn.Close();
　　 }
　　 #endregion
　　
　　 
    }
}
