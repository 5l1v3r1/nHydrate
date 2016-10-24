//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#region Copyright (c) 2006-2016 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2016 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace PROJECTNAMESPACE
{
	/// <summary>
	/// The database installer class
	/// </summary>
	[RunInstaller(true)]
	public partial class DatabaseInstaller : Installer
	{
		#region Members
		private string[] PARAMKEYS_DROP = new string[] { "drop" };
		private string[] PARAMKEYS_UPGRADE = new string[] { "upgrade" };
		private string[] PARAMKEYS_CREATE = new string[] { "create" };
		private string[] PARAMKEYS_MASTERDB = new string[] { "master", "masterdb" };
		private string[] PARAMKEYS_APPDB = new string[] { "applicationdb", "connectionstring" };
		private string[] PARAMKEYS_NEWNAME = new string[] { "newdb", "newdatabase", "dbname" };
		private string[] PARAMKEYS_HELP = new string[] { "showhelp" };
		private string PARAMKEYS_SCRIPT = "script";
		private string PARAMKEYS_SCRIPTFILE = "scriptfile";
		private string PARAMKEYS_SCRIPTFILEACTION = "scriptfileaction";
		private string PARAMKEYS_DBVERSION = "dbversion";
		private string PARAMKEYS_VERSIONWARN = "acceptwarnings";
		private string PARAMKEYS_SHOWSQL = "showsql";
		private string[] PARAMKEYS_TRAN = new string[] { "tranaction", "transaction" };
		private string PARAMKEYS_SKIPNORMALIZE = "skipnormalize";
		private string PARAMKEYS_HASH = "usehash";
		private string PARAMKEYS_CHECKONLY = "checkonly";
		private string PARAMKEYS_QUIET = "quiet";
		#endregion

		#region Constructor
		/// <summary>
		/// The default constructor
		/// </summary>
		public DatabaseInstaller()
		{
			InitializeComponent();
		}
		#endregion

		#region Install

		/// <summary>
		/// Performs an install of a database
		/// </summary>
		public override void Install(System.Collections.IDictionary stateSaver)
		{
			base.Install(stateSaver);

			var commandParams = GetCommandLineParameters();

			var paramUICount = 0;
			var setup = new InstallSetup();
			if (commandParams.Count > 0)
			{
				if (commandParams.ContainsKey(PARAMKEYS_SHOWSQL))
				{
					if (commandParams[PARAMKEYS_SHOWSQL].ToLower() == "true" || commandParams[PARAMKEYS_SHOWSQL].ToLower() == "1" || commandParams[PARAMKEYS_SHOWSQL].ToLower() == string.Empty)
						setup.ShowSql = true;
					else if (commandParams[PARAMKEYS_SHOWSQL].ToLower() == "false" || commandParams[PARAMKEYS_SHOWSQL].ToLower() == "0")
						setup.ShowSql = false;
					else
						throw new Exception("The /" + PARAMKEYS_SHOWSQL + " parameter must be set to 'true or false'.");
					paramUICount++;
				}

				if (commandParams.Any(x => PARAMKEYS_TRAN.Contains(x.Key)))
				{
					setup.UseTransaction = GetSetting(commandParams, PARAMKEYS_TRAN, true);
					paramUICount++;
				}

				if (commandParams.ContainsKey(PARAMKEYS_SKIPNORMALIZE))
				{
					setup.SkipNormalize = true;
					paramUICount++;
				}

				if (commandParams.ContainsKey(PARAMKEYS_HASH))
				{
					if (commandParams[PARAMKEYS_HASH].ToLower() == "true" || commandParams[PARAMKEYS_HASH].ToLower() == "1" || commandParams[PARAMKEYS_HASH].ToLower() == string.Empty)
						setup.UseHash = true;
					else if (commandParams[PARAMKEYS_HASH].ToLower() == "false" || commandParams[PARAMKEYS_HASH].ToLower() == "0")
						setup.UseHash = false;
					else
						throw new Exception("The /" + PARAMKEYS_HASH + " parameter must be set to 'true or false'.");
					paramUICount++;
				}

				if (commandParams.ContainsKey(PARAMKEYS_CHECKONLY))
				{
					setup.CheckOnly = true;
					paramUICount++;
				}

				if (commandParams.ContainsKey(PARAMKEYS_QUIET))
				{
					setup.SuppressUI = true;
					paramUICount++;
				}

				if (commandParams.ContainsKey(PARAMKEYS_VERSIONWARN))
				{
					if (commandParams[PARAMKEYS_VERSIONWARN].ToLower() == "all")
					{
						setup.AcceptVersionWarningsChangedScripts = true;
						setup.AcceptVersionWarningsNewScripts = true;
					}
					else if (commandParams[PARAMKEYS_VERSIONWARN].ToLower() == "none")
					{
						setup.AcceptVersionWarningsChangedScripts = false;
						setup.AcceptVersionWarningsNewScripts = false;
					}
					else if (commandParams[PARAMKEYS_VERSIONWARN].ToLower() == "new")
					{
						setup.AcceptVersionWarningsNewScripts = true;
					}
					else if (commandParams[PARAMKEYS_VERSIONWARN].ToLower() == "changed")
					{
						setup.AcceptVersionWarningsChangedScripts = true;
					}
					else
					{
						throw new Exception("The /" + PARAMKEYS_VERSIONWARN + " parameter must be set to 'all, none, new, or changed'.");
					}
					paramUICount++;
				}

				if (GetSetting(commandParams, PARAMKEYS_HELP, false))
				{
					ShowHelp();
					return;
				}

				//Try to drop database
				if (commandParams.Any(x => PARAMKEYS_DROP.Contains(x.Key)))
				{
					var masterConnectionString = GetSetting(commandParams, PARAMKEYS_MASTERDB, string.Empty);
					var dbname = commandParams.Where(x => PARAMKEYS_NEWNAME.Contains(x.Key)).Select(x => x.Value).FirstOrDefault();
					if (commandParams.Count == 3 && !string.IsNullOrEmpty(masterConnectionString))
					{
						if (!DropDatabase(dbname, masterConnectionString))
							throw new Exception("The database '" + dbname + "' could not dropped.");
						Console.WriteLine("Database successfully dropped.");
						return;
					}
					throw new Exception("Invalid drop database configuration.");
				}

				setup.ConnectionString = GetSetting(commandParams, PARAMKEYS_APPDB, string.Empty);
				setup.MasterConnectionString = GetSetting(commandParams, PARAMKEYS_MASTERDB, string.Empty);
				if (GetSetting(commandParams, PARAMKEYS_UPGRADE, setup.IsUpgrade))
					setup.InstallStatus = InstallStatusConstants.Upgrade;
				if (commandParams.Any(x => PARAMKEYS_CREATE.Contains(x.Key)))
					setup.InstallStatus = InstallStatusConstants.Create;

				if (commandParams.Any(x => PARAMKEYS_UPGRADE.Contains(x.Key)) && commandParams.Any(x => PARAMKEYS_CREATE.Contains(x.Key)))
					throw new Exception("You cannot specify both the create and update action.");
				if (commandParams.Count(x => PARAMKEYS_NEWNAME.Contains(x.Key)) > 1)
					throw new Exception("The new database name was specified more than once.");
				if (commandParams.Count(x => PARAMKEYS_MASTERDB.Contains(x.Key)) > 1)
					throw new Exception("The master database connection string was specified more than once.");
				if (commandParams.Count(x => PARAMKEYS_APPDB.Contains(x.Key)) > 1)
					throw new Exception("The connection string was specified more than once.");

				//Determine if calling as a script generator
				if (commandParams.ContainsKey(PARAMKEYS_SCRIPT))
				{
					var scriptAction = commandParams[PARAMKEYS_SCRIPT].ToLower();
					switch (scriptAction)
					{
						case "versioned":
						case "unversioned":
						case "create":
							break;
						default:
							throw new Exception("The script action must be 'create', 'versioned', or 'unversioned'.");
					}

					if (!commandParams.ContainsKey(PARAMKEYS_SCRIPTFILE))
						throw new Exception("The '" + PARAMKEYS_SCRIPTFILE + "' parameter must be set for script generation.");

					var dumpFile = commandParams[PARAMKEYS_SCRIPTFILE];
					if (!IsValidFileName(dumpFile))
						throw new Exception("The '" + PARAMKEYS_SCRIPTFILE + "' parameter is not valid.");

					var fileCreate = true;
					if (commandParams.ContainsKey(PARAMKEYS_SCRIPTFILEACTION) && (commandParams[PARAMKEYS_SCRIPTFILEACTION] + string.Empty) == "append")
						fileCreate = false;

					if (File.Exists(dumpFile) && fileCreate)
					{
						File.Delete(dumpFile);
						System.Threading.Thread.Sleep(500);
					}

					switch (scriptAction)
					{
						case "versioned":
							if (commandParams.ContainsKey(PARAMKEYS_DBVERSION))
							{
								if (!GeneratedVersion.IsValid(commandParams[PARAMKEYS_DBVERSION]))
									throw new Exception("The '" + PARAMKEYS_DBVERSION + "' parameter is not valid.");

								setup.Version = new GeneratedVersion(commandParams[PARAMKEYS_DBVERSION]);
							}
							else
							{
								if (string.IsNullOrEmpty(setup.ConnectionString))
									throw new Exception("Generation of versioned scripts requires a '" + PARAMKEYS_DBVERSION + "' parameter or valid connection string.");
								else
								{
									var s = new nHydrateSetting();
									s.Load(setup.ConnectionString);
									setup.Version = new GeneratedVersion(s.dbVersion);
								}
							}

							Console.WriteLine("Generate Script Started");
							setup.InstallStatus = InstallStatusConstants.Upgrade;
							File.AppendAllText(dumpFile, UpgradeInstaller.GetScript(setup));
							Console.WriteLine("Generated Create Script");
							break;
						case "unversioned":
							Console.WriteLine("Generate Script Started");
							setup.InstallStatus = InstallStatusConstants.Upgrade;
							setup.Version = UpgradeInstaller._def_Version;
							File.AppendAllText(dumpFile, UpgradeInstaller.GetScript(setup));
							Console.WriteLine("Generated Create Script");
							break;
						case "create":
							Console.WriteLine("Generate Script Started");
							setup.InstallStatus = InstallStatusConstants.Create;
							setup.Version = new GeneratedVersion(-1, -1, -1, -1, -1);
							File.AppendAllText(dumpFile, UpgradeInstaller.GetScript(setup));
							Console.WriteLine("Generated Create Script");
							break;
					}

					return;
				}

				//If we processed all parameters and they were UI then we need to show UI
				if ((paramUICount < commandParams.Count) || setup.SuppressUI)
				{
					setup.NewDatabaseName = commandParams.Where(x => PARAMKEYS_NEWNAME.Contains(x.Key)).Select(x => x.Value).FirstOrDefault();
					Install(setup);
					return;
				}
			}

			UIInstall(setup);

		}

		private bool DropDatabase(string dbname, string masterConnectionString)
		{
			try
			{
				using (var con = new System.Data.SqlClient.SqlConnection(masterConnectionString))
				{
					con.Open();
					var sqlCommandText = @"
						ALTER DATABASE [" + dbname + @"] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
						DROP DATABASE [" + dbname + "]";
					var sqlCommand = new System.Data.SqlClient.SqlCommand(sqlCommandText, con);
					sqlCommand.ExecuteNonQuery();
					return true;
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private bool IsValidFileName(string fileName)
		{
			try
			{
				new System.IO.FileInfo(fileName);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		/// <summary>
		/// Performs an install of a database
		/// </summary>
		public void Install(InstallSetup setup)
		{
			if (setup.InstallStatus == InstallStatusConstants.Create)
			{
				//Conection cannot reference an existing database
				if (SqlServers.TestConnectionString(setup.ConnectionString))
					throw new Exception("The connection string references an existing database.");

				//The new database name must be specified
				if (string.IsNullOrEmpty(setup.NewDatabaseName))
					throw new Exception("A new database name was not specified.");

				//The connection string and the new database name must be the same
				var builder = new System.Data.SqlClient.SqlConnectionStringBuilder(setup.ConnectionString);
				if (builder.InitialCatalog.ToLower() != setup.NewDatabaseName.ToLower())
					throw new Exception("A new database name does not match the specified connection string.");

				SqlServers.CreateDatabase(setup);
			}
			else if (setup.InstallStatus == InstallStatusConstants.Upgrade)
			{
				//The connection string must reference an existing database
				if (!SqlServers.TestConnectionString(setup.ConnectionString))
					throw new Exception("The connection string does not reference a valid database.");
			}

			try
			{
				UpgradeInstaller.UpgradeDatabase(setup);
			}
			catch (InvalidSQLException ex)
			{
				var sb = new StringBuilder();
				sb.AppendLine();
				sb.AppendLine("BEGIN ERROR SQL");
				sb.AppendLine(ex.SQL);
				sb.AppendLine("END ERROR SQL");
				sb.AppendLine();
				Console.WriteLine(sb.ToString());
				UpgradeInstaller.LogError(ex, sb.ToString());
				throw;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns the upgrade script for the specified database
		/// </summary>
		public string GetScript(InstallSetup setup)
		{
			if (string.IsNullOrEmpty(setup.ConnectionString) && setup.Version == null)
				throw new Exception("The connection string must be set.");
			if (setup.SkipSections == null)
				setup.SkipSections = new List<string>();
			return UpgradeInstaller.GetScript(setup);
		}

		#endregion

		#region Uninstall

		/// <summary>
		/// 
		/// </summary>
		/// <param name="savedState"></param>
		public override void Uninstall(System.Collections.IDictionary savedState)
		{
			base.Uninstall(savedState);
		}

		#endregion

		#region NeedsUpdate

		/// <summary>
		/// Determines if the specified database needs to be upgraded
		/// </summary>
		public virtual bool NeedsUpdate(string connectionString)
		{
			return UpgradeInstaller.NeedsUpdate(connectionString);
		}

		/// <summary>
		/// Determines the current version of the specified database
		/// </summary>
		public virtual string VersionInstalled(string connectionString)
		{
			return UpgradeInstaller.VersionInstalled(connectionString);
		}

		/// <summary>
		/// The database version to which this installer will upgrade a database
		/// </summary>
		public virtual string VersionLatest()
		{
			return UpgradeInstaller.VersionLatest();
		}

		/// <summary>
		/// Determines if the specified database has ever been versioned by the framework
		/// </summary>
		/// <param name="connectionString"></param>
		public virtual bool IsVersioned(string connectionString)
		{
			return UpgradeInstaller.IsVersioned(connectionString);
		}

		#endregion

		#region Helpers

		private bool GetSetting(Dictionary<string, string> commandParams, string[] keys, bool defaultValue)
		{
			foreach (var s in keys)
			{
				if (commandParams.ContainsKey(s))
				{
					if (commandParams[s] == "true" || commandParams[s] == "1")
						return true;
					else if (commandParams[s] == "false" || commandParams[s] == "0")
						return false;
					bool v;
					if (bool.TryParse(commandParams[s], out v))
						return v;
					return defaultValue;
				}
			}
			return defaultValue;
		}

		private string GetSetting(Dictionary<string, string> commandParams, string[] keys, string defaultValue)
		{
			var retVal = defaultValue;
			foreach (string s in keys)
			{
				if (commandParams.ContainsKey(s))
				{
					retVal = commandParams[s];
					break;
				}
			}
			return retVal;
		}

		private Dictionary<string, string> GetCommandLineParameters()
		{
			var retVal = new Dictionary<string, string>();
			var args = Environment.GetCommandLineArgs();

			var loopcount = 0;
			foreach (var arg in args)
			{
				var regEx = new Regex(@"\s?[-/](\w+)(:(.*))?");
				var regExMatch = regEx.Match(arg);
				if (regExMatch.Success)
				{
					retVal.Add(regExMatch.Groups[1].Value.ToLower(), regExMatch.Groups[3].Value);
				}
				else
				{
					//var tmpKey = Guid.NewGuid().ToString();
					//if (loopcount == 0)
					//  tmpKey = EXENAME_KEY;
					//else if (loopcount == 1)
					//  tmpKey = DLLNAME_KEY;
				}
				loopcount++;
			}

			return retVal;
		}

		private bool IdentifyDatabaseConnectionString(InstallSetup setup)
		{
			var F = new IdentifyDatabaseForm(setup);
			if (F.ShowDialog() == DialogResult.OK)
			{
				this.Action = F.Action;
				this.Settings = F.Settings;
				return true;
			}
			return false;
		}

		/// <summary />
		private void UIInstall(InstallSetup setup)
		{
			if (IdentifyDatabaseConnectionString(setup))
			{
				setup.ConnectionString = this.Settings.GetPrimaryConnectionString();
				setup.InstallStatus = InstallStatusConstants.Upgrade;

				if (this.Action == ActionTypeConstants.Create)
				{
					setup.InstallStatus = InstallStatusConstants.Create;
					UpgradeInstaller.UpgradeDatabase(setup);
				}
				else if (this.Action == ActionTypeConstants.Upgrade)
				{
					UpgradeInstaller.UpgradeDatabase(setup);
				}
				else if (this.Action == ActionTypeConstants.AzureCopy)
				{
					UpgradeInstaller.AzureCopyDatabase(this.Settings);
				}
			}
		}

		#endregion

		#region ShowHelp

		/// <summary />
		public static void ShowHelp()
		{
			//Create Help dialog
			var sb = new StringBuilder();
			sb.AppendLine("Creates or updates a Sql Server database");
			sb.AppendLine();
			sb.AppendLine("InstallUtil.exe PROJECTNAMESPACE.dll [/upgrade] [/create] [/master:connectionstring] [/connectionstring:connectionstring] [/newdb:name] [/showsql:true|false] [/tranaction:true|false] [/skipnormalize] [/scriptfile:filename] [/scriptfileaction:append] [/checkonly] [/usehash] [/acceptwarnings:all|none|new|changed]");
			sb.AppendLine();
			sb.AppendLine("Providing no parameters will display the default UI.");
			sb.AppendLine();
			sb.AppendLine("/upgrade");
			sb.AppendLine("Specifies that this is an update database operation");
			sb.AppendLine();
			sb.AppendLine("/create");
			sb.AppendLine("Specifies that this is a create database operation");
			sb.AppendLine();
			sb.AppendLine("/master:\"connectionstring\"");
			sb.AppendLine("Specifies the master connection string. This is only required for create database.");
			sb.AppendLine();
			sb.AppendLine("/connectionstring:\"connectionstring\"");
			sb.AppendLine("/Specifies the connection string to the upgrade database");
			sb.AppendLine();
			sb.AppendLine("newdb:name");
			sb.AppendLine("When creating a new database, this is the name of the newly created database.");
			sb.AppendLine();
			sb.AppendLine("/showsql:[true|false]");
			sb.AppendLine("Displays each SQL statement in the console window as its executed. Default is false.");
			sb.AppendLine();
			sb.AppendLine("/tranaction:[true|false]");
			sb.AppendLine("Specifies whether to use a database transaction. Outside of a transaction there is no rollback functionality if an error occurs! Default is true.");
			sb.AppendLine();
			sb.AppendLine("/skipnormalize");
			sb.AppendLine("Specifies whether to skip the normalization script. The normalization script is used to ensure that the database has the correct schema.");
			sb.AppendLine();
			sb.AppendLine("/scriptfile:filename");
			sb.AppendLine("Specifies that a script be created and written to the specified file.");
			sb.AppendLine();
			sb.AppendLine("/scriptfileaction:append");
			sb.AppendLine("Optionally you can specify to append the script to an existing file. If this parameter is omitted, the file will first be deleted if it exists.");
			sb.AppendLine();
			sb.AppendLine("/usehash:[true|false]");
			sb.AppendLine("Specifies that only scripts that have changed will be applied to the database . Default is true.");
			sb.AppendLine();
			sb.AppendLine("/checkonly");
			sb.AppendLine("Specifies check mode and that no scripts will be run against the database. If any changes have occurred, an exception is thrown with the change list.");
			sb.AppendLine();

			MessageBox.Show(sb.ToString(), "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		#endregion

		internal InstallSettings Settings { get; private set; }

		/// <summary>
		/// The action to take
		/// </summary>
		internal ActionTypeConstants Action { get; private set; }

	}

	/// <summary />
	public enum InstallStatusConstants
	{
		/// <summary />
		Create,
		/// <summary />
		Upgrade
	}

	#region InstallSetup

	/// <summary />
	public class InstallSetup
	{
		/// <summary />
		public InstallSetup()
		{
			this.SkipSections = new List<string>();
			this.InstallStatus = InstallStatusConstants.Upgrade;
			this.UseTransaction = true;
			this.UseHash = true;
			this.SkipNormalize = false;
			this.SuppressUI = false;
		}

		/// <summary>
		/// The connection information to the SQL Server master database
		/// </summary>
		public string MasterConnectionString { get; set; }

		/// <summary>
		/// The connection string to the newly created database
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary />
		public GeneratedVersion Version { get; set; }

		/// <summary />
		internal bool SuppressUI { get; set; }

		/// <summary>
		/// Determines if this is a database upgrade
		/// </summary>
		internal bool IsUpgrade
		{
			get { return this.InstallStatus == InstallStatusConstants.Upgrade; }
		}

		/// <summary />
		public bool UseHash { get; set; }
		
		/// <summary />
		public bool UseTransaction { get; set; }

		/// <summary />
		public bool SkipNormalize { get; set; }

		/// <summary>
		/// The transaction to use for this action. If null, one will be created.
		/// </summary>
		public System.Data.SqlClient.SqlTransaction Transaction { get; set; }

		/// <summary />
		public List<string> SkipSections { get; set; }

		/// <summary />
		public string NewDatabaseName { get; set; }

		/// <summary />
		public bool ShowSql { get; set; }

		/// <summary />
		public bool CheckOnly { get; set; }

		internal bool NewInstall
		{
			get { return this.InstallStatus == InstallStatusConstants.Create; }
		}

		/// <summary />
		public InstallStatusConstants InstallStatus { get; set; }

		/// <summary />
		/// <summary />
		public bool AcceptVersionWarningsChangedScripts { get; set; }

		/// <summary />
		public bool AcceptVersionWarningsNewScripts { get; set; }

		internal string DebugScriptName { get; set; }

	}

	#endregion

}
#pragma warning restore 0168