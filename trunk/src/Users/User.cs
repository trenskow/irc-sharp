/*
 **********************************************************************************
 * Copyright (c) 2008, Kristian Trenskow                                          *
 * All rights reserved.                                                           *
 **********************************************************************************
 * This code is subject to terms of the BSD License. A copy of this license is    *
 * included with this software distribution in the file COPYING. If you do not    *
 * have a copy of, you can contain a copy by visiting this URL:                   *
 *                                                                                *
 * http://www.opensource.org/licenses/bsd-license.php                             *
 *                                                                                *
 * Replace <OWNER>, <ORGANISATION> and <YEAR> with the info in the above          *
 * copyright notice.                                                              *
 *                                                                                *
 **********************************************************************************
 */

using System;

namespace IRC
{
	public class User : MessageReciever
	{
		private string strNick;
		private string strIdentity;
		private string strHost;
		private string strRealName;
		private string strUsingServer;
		
		internal event EventHandler InfoUpdated;
		
		internal User(ServerConnection creatorsServerConnection, string Nick, string Identity, string Host, string RealName) : base(creatorsServerConnection, Nick)
		{
			strNick = Nick;
			strIdentity = Identity;
			strHost = Host;
			strRealName = RealName;
		}
		
		internal User(ServerConnection creatorsServerConnection, string Nick, string Identity, string Host, bool askWho) : this(creatorsServerConnection, Nick, Identity, Host, null)
		{
			if (askWho)
				base.CurrentConnection.SendData("WHO {0}", strNick);
		}
		
		internal User(ServerConnection creatorsServerConnection, string Nick, bool askWho) : this(creatorsServerConnection, Nick, null, null, askWho)
		{
		}
		
		internal void SetAllInfo(UserInfo userInfo, string RealName, string UsingServer)
		{
			strNick = userInfo.Nick;
			strIdentity = userInfo.Identity;
			strHost = userInfo.Host;
			strRealName = RealName;
			strUsingServer = UsingServer;
			if (InfoUpdated != null)
				InfoUpdated(this, new EventArgs());
		}
		
		public string Nick
		{
			get { return strNick; }
			internal set
			{
				strNick = value;
				base.NetworkIdentifier = strNick;
				if (InfoUpdated != null)
					InfoUpdated(this, new EventArgs());
			}
		}
		
		public string Identity
		{
			get { return strIdentity; }
			internal set
			{ 
				strIdentity = value;
				if (InfoUpdated != null)
					InfoUpdated(this, new EventArgs());
			}
		}
		
		public string Host
		{
			get { return strHost; }
			internal set
			{
				strHost = value;
				if (InfoUpdated != null)
					InfoUpdated(this, new EventArgs());
			}
		}
		
		public string RealName
		{
			get { return strRealName; }
			internal set
			{
				strRealName = value;
				if (InfoUpdated != null)
					InfoUpdated(this, new EventArgs());
			}
		}
		
		public string UsingServer
		{
			get { return strUsingServer; }
		}
		
		/// <summary>
		/// Invite the user to a specified channel.
		/// </summary>
		/// <param name="ChannelName">Channel name to invite user to.</param>
		public void Invite(string ChannelName)
		{
			base.CurrentConnection.SendData("INVITE " + strNick + " " + ChannelName + "");
		}
	}
}
