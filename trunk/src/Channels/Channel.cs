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
using System;

namespace IRC
{
	/// <summary>
	/// Summary description for Channel.
	/// </summary>
	public class Channel : MessageReciever
	{
		private string strName = "";
		private string strTopic = "";
		private string strTopicBy = "";
		private DateTime dtTopicSet;
		private bool blOnlyOpsChangeTopic = false;
		private bool blNoExternalMessages = false;
		private bool blInviteOnly = false;
		private bool blModerated = false;
		private bool blPrivate = false;
		private bool blSecret = false;
		private string strKey = "";
		private int intLimit = 0;
		private DateTime dtCreated;
		private object objTag = null;

		private ChannelUserContainer users;
		private ChannelBanContainer bans;

		#region Constructors
		internal Channel(ServerConnection creatorsCurrentConnection, string Name) : base(creatorsCurrentConnection, Name, true)
		{
			strName = Name;
			users = new ChannelUserContainer(creatorsCurrentConnection);
			bans = new ChannelBanContainer(creatorsCurrentConnection);
		}
		#endregion
		#region Properties
		private string ValuePlusMinus(bool Value)
		{
			if (Value)
				return "+";
			return "-";
		}

		/// <summary>
		/// Gets or sets the +t mode on the channel.
		/// </summary>
		/// <remarks>Required channel operator status to set.</remarks>
		public bool OnlyOpsChangeTopic
		{
			get { return blOnlyOpsChangeTopic; }
			set { base.CurrentConnection.SendData("MODE " + strName + " " + ValuePlusMinus(value) + "t\r\n"); }
		}

		/// <summary>
		/// Gets or sets the +n mode on the channel.
		/// </summary>
		/// <remarks>Required channel operator status to set.</remarks>
		public bool NoExternalMessages
		{
			get { return blNoExternalMessages; }
			set { base.CurrentConnection.SendData("MODE " + strName + " " + ValuePlusMinus(value) + "n\r\n"); }
		}

		/// <summary>
		/// Gets or sets the +i mode on the channel.
		/// </summary>
		/// <remarks>Required channel operator status to set.</remarks>
		public bool InviteOnly
		{
			get { return blInviteOnly; }
			set { base.CurrentConnection.SendData("MODE " + strName + " " + ValuePlusMinus(value) + "i\r\n"); }
		}

		/// <summary>
		/// Gets or sets the +m mode on the channel.
		/// </summary>
		/// <remarks>Required channel operator status to set.</remarks>
		public bool Moderated
		{
			get { return blModerated; }
			set { base.CurrentConnection.SendData("MODE " + strName + " " + ValuePlusMinus(value) + "m\r\n"); }
		}

		/// <summary>
		/// Gets or sets the +p mode on the channel.
		/// </summary>
		/// <remarks>Required channel operator status to set. This also unsets the +s mode if set.</remarks>
		public bool Private
		{
			get { return blPrivate; }
			set { base.CurrentConnection.SendData("MODE " + strName + " " + ValuePlusMinus(value) + "p\r\n"); }
		}

		/// <summary>
		/// Gets or sets the +s mode on the channel.
		/// </summary>
		/// <remarks>Required channel operator status to set. This also unsets the +p mode if set.</remarks>
		public bool Secret
		{
			get { return blSecret; }
			set { base.CurrentConnection.SendData("MODE " + strName + " " + ValuePlusMinus(value) + "s\r\n"); }
		}

		/// <summary>
		/// Gets or sets the +k on the channel. Use "" to unset.
		/// </summary>
		/// <remarks>Required channel operator status to set.</remarks>
		public string Key
		{
			get { return strKey; }
			set
			{
				if (value!=null&&value!=""&&value.IndexOf(" ")==-1)
				{
					if (value!="")
						base.CurrentConnection.SendData("MODE " + strName + " +k " + value + "\r\n");
					else
						base.CurrentConnection.SendData("MODE " + strName + " -k\r\n");
				}
				else
					throw(new ArgumentException("Key cannot be null, empty or contain spaces.", "Key"));
			}
		}

		/// <summary>
		/// Gets or sets the +l mode on the channel. Use 0 to unset.
		/// </summary>
		/// <remarks>Required channel operator status to set.</remarks>
		public int Limit
		{
			get { return intLimit; }
			set
			{
				if (value>0)
					base.CurrentConnection.SendData("MODE " + strName + " +l " + value + "\r\n");
				else
					base.CurrentConnection.SendData("MODE " + strName + " -l\r\n");
			}
		}

		/// <summary>
		/// Gets the channel name.
		/// </summary>
		public string Name
		{
			get { return strName; }
		}

		/// <summary>
		/// Gets the time wereas the channel was created.
		/// </summary>
		public DateTime CreatedAt
		{
			get { return dtCreated; }
		}

		internal void SetCreatedAt(DateTime newTime)
		{
			dtCreated = newTime;
		}

		/// <summary>
		/// Array containing users currently on channel.
		/// </summary>
		public ChannelUserContainer Users
		{
			get { return users; }
		}

		/// <summary>
		/// Array containing channel bans.
		/// </summary>
		public ChannelBanContainer Bans
		{
			get { return bans; }
		}

		/// <summary>
		/// A custom tag to use for object attachment to this channel.
		/// </summary>
		public object Tag
		{
			get { return objTag; }
			set { objTag = value; }
		}
		#endregion
		#region Part
		/// <summary>
		/// Parts/Leaves the channel.
		/// </summary>
		/// <param name="Reason">The reason the client parts.</param>
		public void Part(string Reason)
		{
			base.CurrentConnection.SendData("PART " + strName + " :" + Reason + "\r\n");
		}

		/// <summary>
		/// Parts/Leaves the channel.
		/// </summary>
		public void Part()
		{
			Part("");
		}
		#endregion
		#region Topic
		/// <summary>
		/// Gets or sets the current topic of the channel.
		/// </summary>
		/// <remarks>Required channel operator status to set.</remarks>
		public string Topic
		{
			get { return strTopic; }
			set
			{
				CurrentConnection.SendData("TOPIC " + strName + " :" + value + "\r\n");
			}
		}

		/// <summary>
		/// Gets the time whereas the topic was set.
		/// </summary>
		public DateTime TopicSetTime
		{
			get { return dtTopicSet; }
		}

		/// <summary>
		/// Gets the nickname of the user that set the topic.
		/// </summary>
		public string TopicBy
		{
			get { return strTopicBy; }
		}

		internal void SetTopic(string Topic)
		{
			strTopic = Topic;
		}

		internal void SetTopicSetBy(string TopicBy, DateTime setTime)
		{
			strTopicBy = TopicBy;
			dtTopicSet = setTime;
		}
		#endregion
		#region SetMode
		/// <summary>
		/// Sets the channel mode in a raw mode.
		/// </summary>
		/// <param name="Mode">eg. +ov-n Zues Reagan</param>
		/// <remarks>Required channel operator status to set.</remarks>
		public void SetMode(string Mode)
		{
			base.CurrentConnection.SendData("MODE " + strName + " " + Mode + "\r\n");
		}
		#endregion
		#region ParseMode
		internal void ParseMode(string newMode)
		{
			int cParam = 1;
			bool blWay = false;
			string[] strParts = newMode.Split(new char[] {' '});

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
					case 't':
						blOnlyOpsChangeTopic = blWay;
						break;
					case 'n':
						blNoExternalMessages = blWay;
						break;
					case 'i':
						blInviteOnly = blWay;
						break;
					case 'm':
						blModerated = blWay;
						break;
					case 'k':
						if (blWay)
							strKey = strParts[cParam++];
						else
							strKey = "";
						break;
					case 'l':
						if (blWay)
							intLimit = int.Parse(strParts[cParam++]);
						else
							intLimit = 0;
						break;
					case 'p':
						blPrivate = blWay;
						break;
					case 's':
						blSecret = blWay;
						break;
					case 'o':
						users[strParts[cParam++]].IsOperator = blWay;
						break;
					case 'v':
						users[strParts[cParam++]].IsVoiced = blWay;
						break;
				}
			}
		}
		#endregion
	}
}
