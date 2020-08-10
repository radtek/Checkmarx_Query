string[] modules = new string[] {
	"pyodbc",
	"pypyodbc",
	"odbtpapi",
	"odbtp",
	"ceODBC",
	"mx.ODBC",
	"mx.ODBC.Windows",
	"mx.ODBC.iODBC",
	"mx.ODBC.unixODBC",
	"mx.ODBC.SybaseASA"
	};

string[] connStrings = new string[] {
	"connect",
	"connection",
	"win_connect_mdb",
	"DriverConnect"
	};

CxList imports = All.NewCxList();

if (param.Length == 1)
{
	try
	{
		imports = param[0] as CxList;
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
} else {
	imports = Find_Imports();
}

result = Find_DB_Imports(modules, connStrings, imports, null);