using System;
using System.Collections.Generic;
using System.Text;

namespace FadeFox.Database
{
	public class DatabaseServer
	{
		public string ServerID { get; set; }
		public string ServerName { get; set; }
		public string ServerAddress { get; set; }
		public string ServerUserID { get; set; }
		public string ServerUserPassword { get; set; }
		public string ServerDatabase { get; set; }
		public string DatabaseLID { get; set; }
		public string ServerPort { get; set; }

		private IDatabase mDatabase = null;

		public IDatabase Database
		{
			get { return mDatabase; }
		}

		public DatabaseServer(
			string pServerID, string pServername, string pServerAddress,
			string pServerUserID, string pServerUserPassword, string pServerDatabase,
			string pDatabaseLID, string pServerPort
			)
		{
			ServerID = pServerID;
			ServerName = pServername;
			ServerAddress = pServerAddress;
			ServerUserID = pServerUserID;
			ServerUserPassword = pServerUserPassword;
			ServerDatabase = pServerDatabase;
			DatabaseLID = pDatabaseLID;
			ServerPort = pServerPort;

			mDatabase = DatabaseCore.CreateDatabaseClass(pDatabaseLID);

			mDatabase.Server = pServerAddress;
			mDatabase.Database = pServerDatabase;
			mDatabase.UserID = pServerUserID;
			mDatabase.Password = pServerUserPassword;
			mDatabase.Port = pServerPort.ToString();

		}
	}

	public class DatabaseServerCollection
	{
		private Dictionary<string, DatabaseServer> mDatabaseServerList = new Dictionary<string, DatabaseServer>();

		public int Count
		{
			get { return mDatabaseServerList.Count; }
		}

		public DatabaseServer this[string pServerID]
		{
			get
			{
				if (mDatabaseServerList.ContainsKey(pServerID))
					return mDatabaseServerList[pServerID];
				else
					return null;
			}
		}

		public void Clear()
		{
			DisconnectAll();
			mDatabaseServerList.Clear();
		}

		public void ConnectAll()
		{
			foreach (DatabaseServer server in mDatabaseServerList.Values)
			{
				try
				{
					if (!server.Database.IsConnect)
						server.Database.Connect();
				}
				catch
				{
					;
				}
			}
		}

		public void DisconnectAll()
		{
			foreach (DatabaseServer server in mDatabaseServerList.Values)
			{
				try
				{
					if (server.Database.IsConnect)
						server.Database.Disconnect();
				}
				catch
				{
					;
				}
			}
		}

		public bool Connect(string pServerID)
		{
			try
			{
				if (mDatabaseServerList.ContainsKey(pServerID))
				{
					if (!mDatabaseServerList[pServerID].Database.IsConnect)
						mDatabaseServerList[pServerID].Database.Connect();

					return true;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				return false;
			}
		}

		public bool Disconnect(string pServerID)
		{
			try
			{
				if (mDatabaseServerList.ContainsKey(pServerID))
				{
					if (mDatabaseServerList[pServerID].Database.IsConnect)
						mDatabaseServerList[pServerID].Database.Disconnect();

					return true;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				return false;
			}
		}

		public void Add(DatabaseServer pServerInfo)
		{
			mDatabaseServerList.Add(pServerInfo.ServerID, pServerInfo);
		}

		public void Add(
			string pServerID, string pServername, string pServerAddress,
			string pServerUserID, string pServerUserPassword, string pServerDatabase,
			string pDatabaseLID, string pServerPort
			)
		{
			DatabaseServer info = new DatabaseServer(
				pServerID,
				pServername,
				pServerAddress,
				pServerUserID,
				pServerUserPassword,
				pServerDatabase,
				pDatabaseLID,
				pServerPort
				);

			mDatabaseServerList.Add(pServerID, info);
		}
	}
}
