#region License Agreement - READ THIS FIRST!!!
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
#endregion
// 26-02-2005 by Kristian Trenskow
// Removed RTF formatting and trial code.
//
// 26-02-2005 by Kristian Trenskow
// Initial Import

using System;
using System.Text;

namespace IRC
{
	#region ConnectFailedReason
	/// <summary>
	/// Used with Connection.ConnectFailed event.
	/// </summary>
	public enum ConnectFailedReason
	{
        /// <summary>
        /// Used to identify, that connection failed due to socket issues, such as connection timeout etc.
        /// </summary>
		SocketError,
        /// <summary>
        /// Used to identify, that connection failed due to the specefied nickname already is in use on server.
        /// </summary>
		NicknameInUse
	}
	#endregion

	/// <summary>
	/// The main class of the IRC namespace, and the only class that can be instanced outside the IRC namespace.
	/// </summary>
	/// <remarks>Used to access a IRC server which is connected in a worldwide network to create a giant chat network. This is used by millions
	/// of people around the world to communicate. This class is great for creating clients or bots (bots privide services to channels).
	/// If you don't know anything about IRC visit the homepage of the most widely used client in the world: www.mirc.com.</remarks>
	/// <threadsafe instance="false"/>
	public partial class Connection : IRCBase
	{
		#region Variables
		private string strNickname = "";
		private string strIdentity = "";
		private string strRealname = "-";
		private string strHost = "";
		private int intPort = 0;
		private System.Net.IPAddress myIP = System.Net.IPAddress.Loopback;

		private IdentificationServer identd = null;
		private ChannelContainer channels = null;
		private DCC dcc = null;
		private UserContainer users = null;
		private bool blInvisible = false;
		private bool blConnecting = false;

		private StringBuilder strMOTD;
		private object objTag = null;

        private ServerInfo serverInfo;
		#endregion
		
		// The events of this class in the file ConnectionEvents.cs
		
		#region Constructors
		/// <summary>
		/// Creates a new instance of the Connection.
		/// </summary>
		public Connection() : base(new ServerConnection())
		{
			identd = new IdentificationServer(base.CurrentConnection, this);
			channels = new ChannelContainer(base.CurrentConnection);
			users = new UserContainer(base.CurrentConnection);
			// Event-chain from User objects to notify user of user information updates. Kind of hacky.
			users.NewUserAdded += NewUserAdded;
			dcc = new DCC(base.CurrentConnection, this);
            serverInfo = new ServerInfo(base.CurrentConnection, this);
		}

		/// <summary>
		/// Creates a new instance of the Connection.
		/// </summary>
		/// <param name="NickName">Nickname to use.</param>
		/// <param name="Identity">User identification to use.</param>
		public Connection(string NickName, string Identity) : this()
		{
			this.Nickname = NickName;
			strIdentity = Identity;
		}
		#endregion
		#region Properties
		#region Nickname property
		/// <summary>
		/// Nickname property.
		/// </summary>
		/// <value>Gets or sets the nickname to use on server.</value>
		/// <remarks>When connected the nickname is not changed until the server has confirmed it.</remarks>
		public string Nickname
		{
			get { return strNickname; }
			set
			{
				if (value!=null&&value.Trim()!="")
				{
					if (!(base.CurrentConnection.IsConnected && IsConnecting))
					{
						strNickname = value;
						users.OwnNick = value;
					}
					else
						base.CurrentConnection.SendData("NICK " + strNickname + "");
				}
				else
					throw(new ArgumentException("Nickname cannot be null or empty.", "Nickname"));
			}
		}
		#endregion
		#region Identity property
		/// <summary>
		/// Identity property.
		/// </summary>
		/// <remarks>Will throw exception if set while connected.</remarks>
		/// <value>Gets and sets the Identity to identify the client on the IRC server.</value>
		public string Identity
		{
			get { return strIdentity; }
			set
			{
				if (!base.CurrentConnection.IsConnected)
					strIdentity = value;
				else
					throw(new Exception("Cannot change Identity while connected."));
			}
		}
		#endregion
        #region Realname property
		/// <summary>
		/// Realname property.
		/// </summary>
		/// <remarks>Will throw exception if set while connected.</remarks>
		/// <value>Gets and sets the real name of the connected user. Optional. Default is "-"</value>
		public string RealName
		{
			get { return strRealname; }
			set
			{
				if (!base.CurrentConnection.IsConnected)
					strRealname = value;
				else
					throw(new Exception("Cannot change Realname while connected."));
			}
		}
        #endregion
		#region Host property
		/// <summary>
		/// Host property.
		/// </summary>
		/// <value>Gets the hostname of the remote server.</value>
		public string Host
		{
			get { return strHost; }
		}
		#endregion
		#region Port property
		/// <summary>
		/// Port property.
		/// </summary>
		/// <value>Gets the TCP port on the remote server.</value>
		public int Port
		{
			get { return intPort; }
		}
		#endregion
		#region IsConnecting Property
		/// <summary>
		/// IsConnected property.
		/// </summary>
		/// <value>Gets wheater the control is connected to a IRC server.</value>
		internal bool IsConnecting
		{
			get { return blConnecting; }
		}
		#endregion
		#region IdentD Property
		/// <summary>
		/// IdentD property.
		/// </summary>
		/// <value>Gets the identification protocol object.</value>
		public IdentificationServer IdentD
		{
			get { return identd; }
		}
		#endregion
		#region Channels Property
		/// <summary>
		/// Channels Property
		/// </summary>
		/// <value>Gets an array of currently joined channels.</value>
		public ChannelContainer Channels
		{
			get { return channels; }
		}
		#endregion
		#region DCC Property
		/// <summary>
		/// DCC Property
		/// </summary>
		/// <value>Gets an object containing all DCC chats and transfers.</value>
		public DCC DCC
		{
			get { return dcc; }
		}
		#endregion
        #region Users Property
		/// <summary>
		/// Users Property
		/// </summary>
		/// <value>Gets an object containing all users availible on the network.</value>
		public UserContainer Users
		{
			get { return users; }
		}
        #endregion
		#region Tag Property
		/// <summary>
		/// Used to attach an object to the control.
		/// </summary>
		public object Tag
		{
			get { return objTag; }
			set { objTag = value; }
		}
		#endregion
		#region LocalIP Property
		/// <summary>
		/// Gets the IP address of this machine.
		/// </summary>
		/// <remarks>This is a server lookup and is this machines IP outside NATs or firewalls, but is only availible after connecting.</remarks>
		public System.Net.IPAddress LocalIP
		{
			get { return myIP; }
		}
		#endregion
		#region Invisible Property
		/// <summary>
	    /// Gets your invisible mode. Returns true if mode +i is set on this user.
	    /// </summary>
	    public bool Invisible
	    {
		    get { return blInvisible; }
	    }
		#endregion
        #region ServerInfo Property
        /// <summary>
        /// Returns an object containing detailed information about server behavior.
        /// </summary>
        public ServerInfo ServerInfo
        {
            get { return serverInfo; }
        }
        #endregion
        #endregion
        #region Methods
        #region Connect method
        /// <summary>
		/// Connect Method. Connect to a remote IRC server.
		/// </summary>
		/// <param name="Host">Hostname or IP of server.</param>
		/// <param name="Port">Port on which the server is listening.</param>
		public void Connect(string Host, int Port)
		{
			if (Host==null||Host.Trim()=="")
				throw(new ArgumentException("Host cannot be empty or null.", "Host"));
			if (Port==0)
				throw(new ArgumentException("Port cannot be 0 or null.", "Port"));
			if (strNickname.Trim()=="")
				throw(new Exception("Nickname not set."));
			if (strIdentity.Trim()=="")
				throw(new Exception("Identity not set"));

			strHost = Host;
			intPort = Port;

			base.CurrentConnection.OnConnected += new EventHandler(onConnected);
			base.CurrentConnection.Connect(Host, Port);
			base.CurrentConnection.OnConnectFailed += new EventHandler(onConnectFailed);
			base.CurrentConnection.OnDisconnected += new EventHandler(onDisconnected);
			base.CurrentConnection.OnDataReceived += new OnDataEventHandler(onData);
			blConnecting = true;
		}
		#endregion
		#region UpdateMOTD method
		/// <summary>
		/// Updated the servers Message of the Day
		/// </summary>
		public void UpdateMOTD()
		{
			base.CurrentConnection.SendData("MOTD");
		}
		#endregion
		#region Disconnect
		/// <summary>
		/// Disconnects from the IRC server with a reason.
		/// </summary>
		/// <param name="Reason">The reason of the quit.</param>
		public void Disconnect(string Reason)
		{
			base.CurrentConnection.SendData("QUIT :" + Reason + "", true, true);
		}

		/// <summary>
		/// Disconnects from the IRC server.
		/// </summary>
		public void Disconnect()
		{
			Disconnect("");
		}
		#endregion
		#endregion
		#region onConnected
		private void onConnected(object sender, EventArgs e)
		{
			identd.Listen = true;
			base.CurrentConnection.SendData("NICK " + strNickname + "");
			base.CurrentConnection.SendData("USER " + strIdentity + " \"\" \"" + strHost + "\" :" + strRealname + "");
		}
		#endregion
		#region onConnectFailed
		private void onConnectFailed(object sender, EventArgs e)
		{
			ShutdownClient();
			if (ConnectFailed!=null)
				ConnectFailed(this, new ConnectFailedEventArgs(ConnectFailedReason.SocketError));
		}
		#endregion
		#region onDisconnected
		private void onDisconnected(object sender, EventArgs e)
		{
			if (BeforeDisconnect!=null)
				BeforeDisconnect(this, new EventArgs());
			ShutdownClient();
			if (Disconnected!=null)
				Disconnected(this, new EventArgs());
		}
		#endregion
		#region onData
		private void onData(object sender, OnDataEventArgs e)
		{
			if (e.IsServerMessage)
			{
				#region Server Message Handling
				strHost = e.User;
				switch (e.Command)
				{
#region 001 // Connected
				case "001":
					base.CurrentConnection.SendData("USERHOST " + strNickname + "");
					break;
#endregion
#region 005 // Server Info String
				case "005":
					serverInfo.Parse005(e.Parameters, e.descriptionBeginsAtIndex);
					break;
#endregion
#region 302 // USERHOST reply
				case "302":
					if (e.Parameters[0].IndexOf("@")>-1)
					{
						e.Parameters[0] = e.Parameters[0].Substring(e.Parameters[0].IndexOf("@") + 1, e.Parameters[0].Length - e.Parameters[0].IndexOf("@") - 1).ToLower();
						myIP = System.Net.Dns.GetHostEntry(e.Parameters[0]).AddressList[0];
						if (LocalIPUpdated != null)
							LocalIPUpdated(this, new EventArgs());
					}
					break;
#endregion
#region 433 // Nickname in use
				case "433":
					if (blConnecting==true)
					{
						string strNewNick;
						if (NickNameInUse != null)
						{
							strNewNick = NickNameInUse(this, new EventArgs());
							if (strNewNick != null && strNewNick.Trim() != "")
							{
								this.Nickname = strNewNick;
								break;
							}
						}
						ShutdownClient();
						if (ConnectFailed!=null)
							ConnectFailed(this, new ConnectFailedEventArgs(ConnectFailedReason.NicknameInUse));
					}
					break;
#endregion
#region 376 // End of MOTD
				case "422": // MOTD File is missing.
				case "376":
					if (blConnecting==true)
					{
						blConnecting = false;
						identd.Listen = false;
						if (Connected!=null)
							Connected(this, new EventArgs());
					}
					serverInfo.MessageOfTheDay = strMOTD.ToString();
					strMOTD = null;
					if (MessageOfTheDayUpdated!=null)
						MessageOfTheDayUpdated(this, new EventArgs());
					break;
#endregion
#region 375, 372 // Start and content of MOTD
				case "375":
				case "372":
					if (e.Command=="375")
						strMOTD = new StringBuilder();
					strMOTD.Append(e.Parameters[e.Parameters.Length - 0] + "");
					break;
#endregion
#region 332 // Channel topic
				case "332":
					channels[e.Parameters[0]].SetTopic(e.Parameters[1]);
					break;
#endregion
#region 333 // Channel topic set time
				case "333":
					channels[e.Parameters[0]].SetTopicSetBy(e.Parameters[1], IRCDateTime(e.Parameters[2]));
					break;
#endregion
#region 353 // Names in channel
				case "353":
					string[] strNames = e.Parameters;
					for (int x = e.descriptionBeginsAtIndex ; x < strNames.Length ; x++)
					{
						ChannelUserStatus status = ChannelUserStatus.NotAStatus;
						
						bool blVoiced = false;
						bool blHalfOpped = false;
						bool blOpped = false;
						
						while ((status = serverInfo.GetUserStatusFromPrefix(strNames[x][0])) != ChannelUserStatus.NotAStatus)
						{
							switch (status)
							{
							case ChannelUserStatus.Operator:
								blOpped = true;
								break;
							case ChannelUserStatus.HalfOperator:
								blHalfOpped = true;
								break;
							case ChannelUserStatus.Voiced:
								blVoiced = true;
								break;
							}
							strNames[x] = strNames[x].Substring(1);
						}
						
						ChannelUser newUser;
						if (strNames[x]!=strNickname)
						{
							newUser = new ChannelUser(base.CurrentConnection, users.GetUser(strNames[x], false), e.Parameters[2]);
							channels[e.Parameters[2]].Users.AddUser(newUser);
						}
						else
						{
							newUser = channels[e.Parameters[2]].Users[strNames[x]];
						}
						newUser.IsOperator = blOpped;
						newUser.IsVoiced = blVoiced;
						newUser.IsHalfOperator = blHalfOpped;
					}
					break;
#endregion
#region 324 // Channel Mode
				case "324":
					string strMode = "";
					for (int x=1;x<e.Parameters.Length;x++)
						strMode += e.Parameters[x] + " ";
					strMode = strMode.Trim();
					channels[e.Parameters[0]].ParseMode(strMode);
					break;
#endregion
#region 329 // Channel created time
				case "329":
					channels[e.Parameters[0]].SetCreatedAt(IRCDateTime(e.Parameters[2]));
					break;
#endregion
#region 367 // Channel ban
				case "367":
					ChannelBan newBan = new ChannelBan(base.CurrentConnection, e.Parameters[0], e.Parameters[1], e.Parameters[2], IRCDateTime(e.Parameters[3]));
					channels[e.Parameters[1]].Bans.AddBan(newBan);
					break;
#endregion
#region 321 // Start of channel list
				case "321":
					channels.List.Clear();
					break;
#endregion
#region 322 // Channel list item
				case "322":
					ChannelList newChannel = new ChannelList(base.CurrentConnection, e.Parameters[0], int.Parse(e.Parameters[1]), e.Parameters[2]);
					channels.List.AddChannel(newChannel);
					channels.List.FireProgressEvent(this, new ChannelListUpdateProgress(this.channels.List.Count, newChannel));
					break;
#endregion
#region 323 // End of channel list
				case "323":
					this.channels.List.FireUpdatedEvent(this, new EventArgs());
					break;
#endregion
#region 352 // WHO response
				case "352":
					UserInfo cUser = new UserInfo(e.Parameters[4], e.Parameters[1], e.Parameters[2]);
					if (cUser.Nick.Length > 0 && cUser.Nick[0] == '~')
						cUser.Nick = cUser.Nick.Substring(1);
					
					string cRealName = e.description;
					if (cRealName.IndexOf(" ") > -1)
						cRealName = cRealName.Substring(cRealName.IndexOf(" ") + 1);
					
					User cFullUser = users.GetUser(cUser);
					
					foreach (char c in e.Parameters[5])
					{
						if (c == serverInfo.OperatorPrefix)
							channels[e.Parameters[0]].Users[cFullUser].IsOperator = true;
						if (c == serverInfo.VoicePrefix)
							channels[e.Parameters[0]].Users[cFullUser].IsVoiced = true;
					}
					
					cFullUser.SetAllInfo(cUser, cRealName, e.Parameters[2]);
					break;
#endregion
#region 315 // End of WHO response
				case "315":
					if (JoinComplete!=null && channels.GetChannel(e.Parameters[0]) == null)
						JoinComplete(this, new JoinCompleteEventArgs(channels.GetChannel(e.Parameters[0])));
					break;
				}
				if (ServerMessage!=null)
					ServerMessage(this, new ServerMessageEventArgs(int.Parse(e.Command), e.Parameters));
#endregion
#endregion
			}
			else
			{
				if (e.User==null)
				{
#region Direct Command Handling (eg. PING or ERROR)
					switch (e.Command.ToLower())
					{
#region PING? PONG!
					case "ping":
						base.CurrentConnection.SendData("PONG :" + e.Parameters[0] + "", false);
						if (PingPong!=null)
							PingPong(this, new EventArgs());
						break;
#endregion
#region ERROR
					case "error":
						if (BeforeError!=null)
							BeforeError(this, new ErrorEventArgs(e.Parameters[0]));
						ShutdownClient();
						if (Error!=null)
							Error(this, new ErrorEventArgs(e.Parameters[0]));
						break;
#region NOTICE
					case "notice":
						if (e.Parameters.Length>1)
						{
							string[] tmp = new string[e.Parameters.Length - 1];
							for (int x=1;x<e.Parameters.Length;x++)
								tmp[x-1] = e.Parameters[x];
							if (ServerNotice!=null)
								ServerNotice(this, new ServerNoticeEventArgs(e.Parameters[0], string.Join(" ", tmp).Trim()));
						}
						break;
#endregion
#endregion
					}
#endregion
				}
				else
				{
#region Normal Message Handling (eg. :nick!ident@host PRIVMSG #channel :Wow!)
					UserInfo user;
					if (e.User.IndexOf("@")>-1)
					{
						string strUser = e.User;
						string strNick = strUser.Substring(0, strUser.IndexOf("!"));
						strUser = strUser.Substring(strUser.IndexOf("!") + 1, strUser.Length - (strUser.IndexOf("!") + 1));
						string strIdentity = strUser.Substring(0, strUser.IndexOf("@"));
						string strHost = strUser.Substring(strUser.IndexOf("@") + 1, strUser.Length - (strUser.IndexOf("@") + 1));
						user = new UserInfo(strNick, strIdentity, strHost);
					}
					else
						user = new UserInfo(e.User, "", "");
					
					switch (e.Command.ToLower())
					{
#region JOIN
					case "join":
						EnsureInformation(user);
						if (user.Nick.ToLower()==strNickname.ToLower())
						{
							Channel newChannel = new Channel(base.CurrentConnection, e.Parameters[0]);
							channels.AddChannel(newChannel);
							base.CurrentConnection.SendData("MODE " + e.Parameters[0] + "");
							base.CurrentConnection.SendData("MODE " + e.Parameters[0] + " +b");
							base.CurrentConnection.SendData("WHO {0}", e.Parameters[0]);
						}
						ChannelUser newUser = new ChannelUser(base.CurrentConnection, users.GetUser(user), e.Parameters[0]);
						channels[e.Parameters[0]].Users.AddUser(newUser);
						if (Join!=null)
							Join(this, new JoinEventArgs(e.Parameters[0], users.GetUser(user)));
						break;
#endregion
#region MODE
					case "mode":
						EnsureInformation(user);
						string strMode = "";
						for (int x=1;x<e.Parameters.Length;x++)
							strMode += e.Parameters[x] + " ";
						strMode = strMode.Trim();
						// Checks that we are on the channel that has changed mode.
						// If not, this is properbly a user mode.
						if (channels.IsOnChannel(e.Parameters[0]))
						{
							channels[e.Parameters[0]].ParseMode(strMode);
							CheckUserChannelStatus(user, e.Parameters[0], strMode);
							if (ModeChange!=null)
								ModeChange(this, new ModeChangeEventArgs(channels.GetChannel(e.Parameters[0]), users.GetUser(user), strMode));
						}
						else
						{
							// Parse user mode
							bool blWay = false;
							for (int x = 0 ; x < strMode.Length ; x++)
							{
								switch (strMode.Substring(x, 1))
								{
								case "+":
									blWay = true;
									break;
								case "-":
									blWay = false;
									break;
								case "i":
									blInvisible = blWay;
									break;
								}
							}
						}
						break;
#endregion
#region PART
					case "part":
						EnsureInformation(user);
						if (BeforePart!=null)
							BeforePart(this, new PartEventArgs(channels.GetChannel(e.Parameters[0]), users.GetUser(user), e.Parameters.Length>1?e.Parameters[1]:""));
						if (user.Nick.ToLower()==strNickname.ToLower())
						{
							foreach (ChannelUser u in channels.GetChannel(e.Parameters[0]).Users)
								if (!channels.IsUserOnAnyChannel(u.User.Nick, e.Parameters[0]))
									users.Remove(u.User.Nick);
							channels.RemoveChannel(e.Parameters[0]);
						}
						else
						{
							channels[e.Parameters[0]].Users.RemoveUser(user.Nick);
							if (!channels.IsUserOnAnyChannel(user.Nick))
							    users.Remove(user.Nick);
						}
						if (Part!=null)
							Part(this, new PartEventArgs(channels.GetChannel(e.Parameters[0]), users.GetUser(user), e.Parameters.Length>1?e.Parameters[1]:""));
						break;
#endregion
#region KICK
					case "kick":
						EnsureInformation(user);
						if (BeforeKick!=null)
							BeforeKick(this, new KickEventArgs(users.GetUser(user), channels.GetChannel(e.Parameters[0]), e.Parameters[1], e.Parameters[2]));
						if (e.Parameters[1].ToLower()==strNickname.ToLower())
						{
							foreach (ChannelUser u in channels.GetChannel(e.Parameters[0]).Users)
								if (!channels.IsUserOnAnyChannel(u.User.Nick, e.Parameters[0]))
									users.Remove(u.User.Nick);
							channels.RemoveChannel(e.Parameters[0]);
						}
						else
						{
							channels[e.Parameters[0]].Users.RemoveUser(e.Parameters[1]);
							if (!channels.IsUserOnAnyChannel(e.Parameters[1]))
							    users.Remove(e.Parameters[1]);
						}
						if (Kick!=null)
							Kick(this, new KickEventArgs(users.GetUser(user), channels.GetChannel(e.Parameters[0]), e.Parameters[1], e.Parameters[2]));
						break;
#endregion
#region QUIT
					case "quit":
						EnsureInformation(user);
						if (BeforeQuit!=null)
							BeforeQuit(this, new QuitEventArgs(users.GetUser(user), e.Parameters[0]));
						
						for (int x=0;x<channels.Count;x++)
							for (int y=0;y<channels[x].Users.Count;y++)
								if (channels[x].Users[y].User.Nick.ToLower()==user.Nick.ToLower())
									channels[x].Users.RemoveUser(y);
						if (user.Nick.ToLower()!=strNickname.ToLower() && !channels.IsUserOnAnyChannel(user.Nick))
							users.Remove(user.Nick);
							
						
						if (Quit!=null)
							Quit(this, new QuitEventArgs(users.GetUser(user), e.Parameters[0]));
						
						if (user.Nick.ToLower()==strNickname.ToLower())
						{
							if (BeforeDisconnect!=null)
								BeforeDisconnect(this, new EventArgs());
							ShutdownClient();
							if (Disconnected!=null)
								Disconnected(this, new EventArgs());
						}
						break;
#endregion
#region NICK
					case "nick":
						EnsureInformation(user);
						if (BeforeNickChange!=null)
							BeforeNickChange(this, new BeforeNickChangeEventArgs(users.GetUser(user), e.Parameters[0]));
						if (user.Nick.ToLower() == strNickname.ToLower())
						{
							strNickname = e.Parameters[0];
							users.OwnNick = strNickname;
						}
						users.GetUser(user.Nick).Nick = e.Parameters[0];
						if (NickChange!=null)
							NickChange(this, new NickChangeEventArgs(users.GetUser(user), user.Nick));
						break;
#endregion
#region INVITE
					case "invite":
						EnsureInformation(user);
						if (Invitation!=null)
							Invitation(this, new InvitationEventArgs(users.GetUser(user), e.Parameters[1]));
						break;
#endregion
#region PRIVMSG
					case "privmsg":
						EnsureInformation(user);
						CheckVoice(e.Parameters[0], user);
						if (e.Parameters[1].Length>2&&e.Parameters[1][0]==1&&e.Parameters[1][e.Parameters[1].Length - 1]==1) // Is CTCP
							ParseCTCP(user, e.Parameters[0], e.Parameters[1].Substring(1, e.Parameters[1].Length - 2), false);
						else
						{
							if (Message!=null)
								Message(this, new MessageEventArgs(Users.GetUser(user), GetMessageReciever(e.Parameters[0]), e.Parameters[1]));
						}
						break;
#endregion
#region NOTICE
					case "notice":
						EnsureInformation(user);
						if (e.Parameters.Length > 1)
						{
							CheckVoice(e.Parameters[0], user);
							if (e.Parameters[1].Length > 2 && e.Parameters[1][0] == 1 && e.Parameters[1][e.Parameters[1].Length - 1] == 1) // Is CTPCP
								ParseCTCP(user, e.Parameters[0], e.Parameters[1].Substring(1, e.Parameters[1].Length - 2), true);
							else
							{
								if (Notice != null)
									Notice(this, new MessageEventArgs(users.GetUser(user), GetMessageReciever(e.Parameters[0]), e.Parameters[1]));
							}
						}
						break;
#endregion
					}
#endregion
				}
			}
		}
#endregion
#region CheckVoice
		private void CheckVoice(string ChannelName, UserInfo user)
		{
			if (channels.IsOnChannel(ChannelName)&&channels[ChannelName].Moderated&&!channels[ChannelName].Users[user.Nick].IsVoiced)
				onData(base.CurrentConnection, new OnDataEventArgs(strHost, "MODE", new string[] {ChannelName, "+v", user.Nick}, false));
		}
		#endregion
		#region CheckUserChannelStatus
		private void CheckUserChannelStatus(UserInfo user, string ChannelName, string Mode)
		{
			string[] strParts = Mode.Split(new char[] {' '});
			int cParam = 1;
			bool blWay = false;

			for (int x=0;x<strParts[0].Length;x++)
			{
				switch (strParts[0][x])
				{
					case '+':
						blWay = true;
						break;
					case '-':
						blWay = false;
						break;
					case 'o':
						if (UserOpStatusChange!=null)
							UserOpStatusChange(this, new UserModeEventArgs(users.GetUser(user), channels.GetChannel(ChannelName), users.GetUser(strParts[cParam++]), blWay));
						break;
					case 'v':
						if (UserVoiceStatusChange!=null)
							UserVoiceStatusChange(this, new UserModeEventArgs(users.GetUser(user), channels.GetChannel(ChannelName), users.GetUser(strParts[cParam++]), blWay));
						break;
					case 'l':
					case 'k':
						cParam++;
						break;
				}
			}
		}
		#endregion
		#region ParseCTCP
		private void ParseCTCP(UserInfo user, string Target, string CTCPData, bool blNotice)
		{
			CTCPData = CTCPData.Trim();
			string strCommand = "";
			string strParams = "";

			if (CTCPData.IndexOf(" ")>-1)
			{
				strCommand = CTCPData.Substring(0, CTCPData.IndexOf(" "));
				strParams = CTCPData.Substring(CTCPData.IndexOf(" ") + 1, CTCPData.Length - (CTCPData.IndexOf(" ") + 1));
			}			
			else
				strCommand = CTCPData;

			if (!blNotice)
			{
				#region CtCp Requests
				switch (strCommand.ToLower())
				{
					case "ping":
						base.CurrentConnection.SendData("NOTICE " + user.Nick + " :\x01PING " + strParams + "\x01", false);
						break;
					case "version":
					    string clientInfo = "TNets IRC Library v.0.7";
					    if (this.CtCpVersionNeedsClientInfo != null)
						   clientInfo = this.CtCpVersionNeedsClientInfo(this, new EventArgs());
						base.CurrentConnection.SendData("NOTICE " + user.Nick + " :\x01VERSION " + clientInfo + "\x01", false);
						break;
					case "finger":
						TimeSpan time = (TimeSpan) DateTime.Now.Subtract(base.CurrentConnection.LastActive);
						string strReadable = "";
						#region Calculate readable idle time
						if (time.TotalSeconds>60)
						{
							string strHours = "";
							string strMinutes = "";
							string strSeconds = "";
							if (time.Hours>0)
							{
								if (time.Hours>1)
									strHours = time.Hours.ToString() + " hours ";
								else
									strHours = time.Hours.ToString() + " hour ";
							}
							if (time.Minutes>0)
							{
								if (time.Minutes>1)
									strMinutes = time.Minutes.ToString() + " minutes ";
								else
									strMinutes = time.Minutes.ToString() + " minute ";
							}
							if (time.Seconds>0)
							{
								if (time.Seconds>1)
									strSeconds = time.Seconds.ToString() + " seconds ";
								else
									strSeconds = time.Seconds.ToString() + " second ";
							}
							strReadable = strHours + strMinutes + strSeconds;
							strReadable = " (" + strReadable.Trim() + ")";
						}
						#endregion
						base.CurrentConnection.SendData("NOTICE " + user.Nick + " :\x01" + "FINGER Idle " + Math.Floor(time.TotalSeconds).ToString() + " seconds" + strReadable + "\x01", false);
						break;
					case "time":
						base.CurrentConnection.SendData("NOTICE " + user.Nick + " :\x01TIME " + mIRCDateTime(DateTime.Now) + "\x01", false);
						break;
					case "action":
						if (Message!=null)
							Message(this, new MessageEventArgs(users.GetUser(user), GetMessageReciever(Target), strParams, true));
						break;
					case "dcc":
						string[] strDccParams = strParams.Split(new char[] {' '});
						if (strDccParams.Length>=4)
						{
							switch (strDccParams[0].ToLower())
							{
								case "chat":
									DCCChat newChat = new DCCChat(base.CurrentConnection, long.Parse(strDccParams[2]), int.Parse(strDccParams[3]), user.Nick, dcc.Chats);
									dcc.Chats.AddDcc(newChat);
									if (DCCChatRequest!=null)
										DCCChatRequest(this, new DCCChatRequestEventArgs(users.GetUser(user), newChat));
									break;
								case "send":
									DCCTransfer newTransfer = new DCCTransfer(base.CurrentConnection, long.Parse(strDccParams[2]), int.Parse(strDccParams[3]), user.Nick, strDccParams[1], long.Parse(strDccParams[4]), dcc.Transfer);
									dcc.Transfer.AddDcc(newTransfer);
									if (DCCTransferRequest!=null)
										DCCTransferRequest(this, new DCCTransferRequestEventArgs(users.GetUser(user), newTransfer));
									break;
								case "accept":
									for (int x=0;x<dcc.Transfer.Count;x++)
										if (dcc.Transfer[x].Identifier==int.Parse(strDccParams[2]))
										{
											dcc.Transfer[x].ResumeAccepted(long.Parse(strDccParams[3]));
											break;
										}
									break;
								case "resume":
									for (int x=0;x<dcc.Transfer.Count;x++)
										if (dcc.Transfer[x].Identifier==int.Parse(strDccParams[2]))
										{
											dcc.Transfer[x].ResumeRequested(long.Parse(strDccParams[3]));
											break;
										}
									break;
							}
						}
						break;
					default:
						if (CtCpMessage!=null)
							CtCpMessage(this, new CtCpMessageEventArgs(users.GetUser(user), strCommand, strParams));
						break;
				}
				#endregion
			}
			else
			{
				#region CtCp Responses
				switch (strCommand.ToLower())
				{
					case "ping":
						if (CtCpPingReply!=null)
							CtCpPingReply(this, new CtCpPingReplyEventArgs(users.GetUser(user), new TimeSpan(TimeSpan.TicksPerMillisecond * long.Parse(strParams))));
						break;
					case "time":
						if (CtCpTimeReply!=null)
							CtCpTimeReply(this, new CtCpReplyEventArgs(users.GetUser(user), strParams));
						break;
					case "finger":
						if (CtCpFingerReply!=null)
							CtCpFingerReply(this, new CtCpReplyEventArgs(users.GetUser(user), strParams));
						break;
					case "version":
						if (CtCpVersionReply!=null)
							CtCpVersionReply(this, new CtCpReplyEventArgs(users.GetUser(user), strParams));
						break;
					default:
						if (CtCpReply!=null)
							CtCpReply(this, new CtCpCommandReplyEventArgs(users.GetUser(user), strCommand, strParams));
						break;
				}
				#endregion
			}
		}
		#endregion
		#region EnsureInformation
		private void EnsureInformation(UserInfo user)
		{
			foreach (Channel channel in channels)
			{
				foreach (ChannelUser channelUser in channel.Users)
				{
					if (channelUser.User.Nick.ToLower()==user.Nick.ToLower())
					{
						channelUser.User.Host = user.Host;
						channelUser.User.Identity = user.Identity;
					}
				}
			}
		}
		#endregion
		#region ShutdownClient
		private void ShutdownClient()
		{
			base.CurrentConnection.Disconnect();
			blConnecting = false;
			identd.Listen = false;
			channels.Clear();
			channels.List.Clear();
			users.Clear();
		}
		#endregion
		#region IRCDateTime
		private DateTime IRCDateTime(string dt)
		{
			return (new DateTime(1970, 1, 1)).AddSeconds(double.Parse(dt));
		}
		#endregion
        #region GetMessageReciever
		public MessageReciever GetMessageReciever(string NetworkIdentifier)
		{
		    foreach (Channel c in channels)
			    if (c.NetworkIdentifier.ToLower() == NetworkIdentifier.ToLower())
					return c;
		    foreach (User u in users)
				if (u.NetworkIdentifier.ToLower() == NetworkIdentifier.ToLower())
					return u;
			
			return null;
		}
        #endregion
		#region mIRCDateTime
		private string mIRCDateTime(DateTime dt)
		{
			string[] strWeekNames = {"Sun", "Mon", "Tue", "Web", "Thu", "Fri", "Sat" };
			string[] strMonthNames = {"Jan", "Feb", "Mar", "Apr", "Mai", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

			return strWeekNames[(int) dt.DayOfWeek] + " " + strMonthNames[dt.Month - 1] + " " + dt.Day.ToString().PadLeft(2, '0') + " " + dt.Hour.ToString().PadLeft(2, '0') + ":" + dt.Minute.ToString().PadLeft(2, '0') + ":" + dt.Second.ToString().PadLeft(2, '0') + " " + dt.Year.ToString();
		}
		#endregion
		private void NewUserAdded(object user, EventArgs e)
		{
			((User)user).InfoUpdated += iUserInfoUpdated; 
		}
		
		internal void iUserInfoUpdated(object user, EventArgs e)
		{
			if (UserInfoUpdated != null)
				UserInfoUpdated(this, new UserEventArgs((User)user));
		}
    }
}