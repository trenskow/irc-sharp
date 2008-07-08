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
using IRC;

namespace IRC
{
	#region ConnectFailedEventArgs
	/// <summary>
	/// Event argument class for the ConnectFailedEventHandler.
	/// </summary>
	public class ConnectFailedEventArgs : EventArgs
	{
		private ConnectFailedReason _e;
		
		internal ConnectFailedEventArgs(ConnectFailedReason __e)
		{
			_e = __e;
		}
		/// <summary>
		/// Gets the reason of the connection failure.
		/// </summary>
		public ConnectFailedReason Reason
		{
			get { return _e; }
		}
	}
	#endregion
	#region ServerMessageEventArgs
	public class ServerMessageEventArgs : EventArgs
	{
		private int _servercode;
		private string[] _parameters;
		
		internal ServerMessageEventArgs(int __servercode, string[] __parameters)
		{
			_servercode = __servercode;
			_parameters = __parameters;
		}
		/// <summary>
		/// Gets the code of the sever message.
		/// </summary>
		public int ServerCode
		{
			get { return _servercode; }
		}
		/// <summary>
		/// Gets the parameters of the server message.
		/// </summary>
		public string[] Parameters
		{
			get { return _parameters; }
		}
	}
	#endregion
	#region ErrorEventArgs
	public class ErrorEventArgs : EventArgs
	{
		private string _errormessage;
		
		internal ErrorEventArgs(string __errormessage)
		{
			_errormessage = __errormessage;
		}
		/// <summary>
		/// Gets the error message sent by the server.
		/// </summary>
		public string ErrorMessage
		{
			get { return _errormessage; }
		}
	}
	#endregion
	#region JoinEventArgs
	public class JoinEventArgs : EventArgs
	{
		private string _channelname;
		private User _user;
		
		internal JoinEventArgs(string __channelname, User __user)
		{
			_channelname = __channelname;
			_user = __user;
		}
		/// <summary>
		/// Gets the name of the channel the user joined.
		/// </summary>
		public string ChannelName
		{
			get { return _channelname; }
		}
		/// <summary>
		/// Gets the user info of the user who joined.
		/// </summary>
		public User User
		{
			get { return _user; }
		}
	}
	#endregion
	#region JoinCompleteEventArgs
	public class JoinCompleteEventArgs : EventArgs
	{
		private Channel _channel;
		
		internal JoinCompleteEventArgs(Channel __channel)
		{
			_channel = __channel;
		}
		/// <summary>
		/// The name of the channel that has been joined.
		/// </summary>
		public Channel Channel
		{
			get { return _channel; }
		}
	}
	#endregion
	#region ModeChangeEventArgs
	public class ModeChangeEventArgs : EventArgs
	{
		private Channel _channel;
		private User _user;
		private string _mode;
		
		internal ModeChangeEventArgs(Channel __channel, User __user, string __mode)
		{
			_channel = __channel;
			_user = __user;
			_mode = __mode;
		}
		/// <summary>
		/// The name of the channel in which the mode was set.
		/// </summary>
		public Channel Channel
		{
			get { return _channel; }
		}
		/// <summary>
		/// User info of the user who changed the mode.
		/// </summary>
		public User User
		{
			get { return _user; }
		}
		/// <summary>
		/// The mode that has been set.
		/// </summary>
		public string Mode
		{
			get { return _mode; }
		}
	}
	#endregion
	#region ParEventtArgs
	public class PartEventArgs : EventArgs
	{
		private Channel _channel;
		private User _user;
		private string _reason;
		
		internal PartEventArgs(Channel __channel, User __user, string __reason)
		{
			_channel = __channel;
			_user = __user;
			_reason = __reason;
		}
		public Channel Channel
		{
			get { return _channel; }
		}
		public User User
		{
			get { return _user; }
		}
		public string Reason
		{
			get { return _reason; }
		}
	}
	#endregion
	#region QuitEventArgs
	public class QuitEventArgs : EventArgs
	{
		private User _user;
		private string _reason;
		
		internal QuitEventArgs(User __user, string __reason)
		{
			_user = __user;
			_reason = __reason;
		}
		/// <summary>
		/// User info of the user who quit.
		/// </summary>
		public User User
		{
			get { return _user; }
		}
		/// <summary>
		/// The reason why the user quit.
		/// </summary>
		public string Reason
		{
			get { return _reason; }
		}
	}
	#endregion
	#region InvitationEventArgs
	public class InvitationEventArgs : EventArgs
	{
		private User _user;
		private string _channelname;
		
		internal InvitationEventArgs(User __user, string __channelname)
		{
			_user = __user;
			_channelname = __channelname;
		}
		/// <summary>
		/// User info of the user who sent the invitation.
		/// </summary>
		public User User
		{
			get { return _user; }
		}
		/// <summary>
		/// The name of the channel invited to.
		/// </summary>
		public string ChannelName
		{
			get { return _channelname; }
		}
	}
	#endregion
	#region MessageEventArgs
	public class MessageEventArgs : EventArgs
	{
		private User _user;
		private MessageReciever _recipient;
		private string _message;
		private bool _isAction;
		
		internal MessageEventArgs(User __user, MessageReciever __recipient, string __message, bool __isAction)
		{
			_user = __user;
			_recipient = __recipient;
			_message = __message;
			_isAction = __isAction;
		}
		
		internal MessageEventArgs(User __user, MessageReciever __recipient, string __message) : this (__user, __recipient, __message, false)
		{
		}
		/// <summary>
		/// User info of the user who sent the message.
		/// </summary>
		public User User
		{
			get { return _user; }
		}
		/// <summary>
		/// The recipient to recieve the message.
		/// </summary>
		/// <remarks>This can be a nick or a channel name.</remarks>
		public MessageReciever Recipient
		{
			get { return _recipient; }
		}
		/// <summary>
		/// The message that was sent.
		/// </summary>
		public string Message
		{
			get { return _message; }
		}
		
		/// <summary>
		/// The message that was sent using the /ME command
		/// </summary>
		public bool isAction
		{
			get { return _isAction; }
		}
	}
	#endregion
	#region CtCpMessageEventArgs
	public class CtCpMessageEventArgs : EventArgs
	{
		private User _user;
		private string _command;
		private string _parameters;
		
		internal CtCpMessageEventArgs(User __user, string __command, string __parameters)
		{
			_user = __user;
			_command = __command;
			_parameters = __parameters;
		}
		/// <summary>
		/// User info of the user who sent the CtCp.
		/// </summary>
		public User user
		{
			get { return _user; }
		}
		/// <summary>
		/// The CtCp command sent.
		/// </summary>
		public string Command
		{
			get { return _command; }
		}
		/// <summary>
		/// The parameters to the CtCp commands set.
		/// </summary>
		public string Parameters
		{
			get { return _parameters; }
		}
	}
	#endregion
	#region CtCpPingReplyEventArgs
	public class CtCpPingReplyEventArgs : EventArgs
	{
		private User _user;
		private TimeSpan _pingtime;
		
		internal CtCpPingReplyEventArgs(User __user, TimeSpan __pingtime)
		{
			_user = __user;
			_pingtime = __pingtime;
		}
		/// <summary>
		/// The user which replyed the CtCp ping request.
		/// </summary>
		public User user
		{
			get { return _user; }
		}
		/// <summary>
		/// The time the ping request was to return.
		/// </summary>
		public TimeSpan PingTime
		{
			get { return _pingtime; }
		}
	}
	#endregion
	#region CtCpReplyEventArgs
	public class CtCpReplyEventArgs : EventArgs
	{
		private User _user;
		private string _reply;
		
		internal CtCpReplyEventArgs(User __user, string __reply)
		{
			_user = __user;
			_reply = __reply;
		}
		/// <summary>
		/// User info of the user who replyed the command.
		/// </summary>
		public User user
		{
			get { return _user; }
		}
		/// <summary>
		/// Text the user replyed with.
		/// </summary>
		public string Reply
		{
			get { return _reply; }
		}
	}
	#endregion
	#region CtCpCommandReplyEventArgs
	public class CtCpCommandReplyEventArgs : EventArgs
	{
		private User _user;
		private string _command;
		private string _reply;
		
		internal CtCpCommandReplyEventArgs(User __user, string __command, string __reply)
		{
			_user = __user;
			_command = __command;
			_reply = __reply;
		}
		/// <summary>
		/// User info of the user that replyed the custom CtCp request.
		/// </summary>
		public User user
		{
			get { return _user; }
		}
		/// <summary>
		/// The command that was responded.
		/// </summary>
		public string Command
		{
			get { return _command; }
		}
		/// <summary>
		/// The text the user replyed with.
		/// </summary>
		public string Reply
		{
			get { return _reply; }
		}
	}
	#endregion
	#region KickEventArgs
	public class KickEventArgs : EventArgs
	{
		private User _kicker;
		private Channel _channelname;
		private string _kickednick;
		private string _reason;
		
		internal KickEventArgs(User __kicker, Channel __channelname, string __kickednick, string __reason)
		{
			_kicker = __kicker;
			_channelname = __channelname;
			_kickednick = __kickednick;
			_reason = __reason;
		}
		public User Kicker
		{
			get { return _kicker; }
		}
		/// <summary>
		/// The channel from which the user was kicked.
		/// </summary>
		public Channel Channel
		{
			get { return _channelname; }
		}
		/// <summary>
		/// The nick name of the user that was kicked.
		/// </summary>
		public string KickedNick
		{
			get { return _kickednick; }
		}
		/// <summary>
		/// Reason the user was kicked from the channel.
		/// </summary>
		public string Reason
		{
			get { return _reason; }
		}
	}
	#endregion
	#region ServerNoticeEventArgs
	public class ServerNoticeEventArgs : EventArgs
	{
		private string _target;
		private string _message;
		
		internal ServerNoticeEventArgs(string __target, string __message)
		{
			_target = __target;
			_message = __message;
		}
		/// <summary>
		/// The recipient the server targeted.
		/// </summary>
		public string Target
		{
			get { return _target; }
		}
		/// <summary>
		/// The message the server sent.
		/// </summary>
		public string Message
		{
			get { return _message; }
		}
	}
	#endregion
	#region BeforeNickChangeEventArgs
	public class BeforeNickChangeEventArgs : EventArgs
	{
		private User _user;
		private string _newnick;
		
		internal BeforeNickChangeEventArgs(User __user, string __newnick)
		{
			_user = __user;
			_newnick = __newnick;
		}
		/// <summary>
		/// User info of the user who changed his/her nick.
		/// </summary>
		public User User
		{
			get { return _user; }
		}
		/// <summary>
		/// The new nick name the user got.
		/// </summary>
		public string NewNick
		{
			get { return _newnick; }
		}
	}
	#endregion
	#region NickChangeEventArgs
	public class NickChangeEventArgs : EventArgs
	{
		private User _user;
		private string _oldnick;
		
		internal NickChangeEventArgs(User __user, string __oldnick)
		{
			_user = __user;
			_oldnick = __oldnick;
		}
		/// <summary>
		/// User info of the user who changed his/her nick.
		/// </summary>
		public User User
		{
			get { return _user; }
		}
		/// <summary>
		/// The new nick name the user got.
		/// </summary>
		public string OldNick
		{
			get { return _oldnick; }
		}
	}
	#endregion
	#region UserModeEventArgs
	public class UserModeEventArgs : EventArgs
	{
		private User _user;
		private Channel _channelname;
		private User _victimnick;
		private bool _way;
		
		internal UserModeEventArgs(User __user, Channel __channelname, User __victimnick, bool __way)
		{
			_user = __user;
			_channelname = __channelname;
			_victimnick = __victimnick;
			_way = __way;
		}
		public User user
		{
			get { return _user; }
		}
		/// <summary>
		/// The channel name where the user was opped/deopped/voiced/devoiced.
		/// </summary>
		public Channel Channel
		{
			get { return _channelname; }
		}
		/// <summary>
		/// The nick of the user that was opped/deopped/voiced/devoiced.
		/// </summary>
		public User VictimNick
		{
			get { return _victimnick; }
		}
		/// <summary>
		/// True if the user was opped/voiced or false if the user was deopped/devoiced.
		/// </summary>
		public bool Way
		{
			get { return _way; }
		}
	}
	#endregion
	#region DCCChatRequestEventArgs
	public class DCCChatRequestEventArgs : EventArgs
	{
		private User _user;
		private DCCChat _newchat;
		
		internal DCCChatRequestEventArgs(User __user, DCCChat __newchat)
		{
			_user = __user;
			_newchat = __newchat;
		}
		/// <summary>
		/// User info of the user that requested the DCC chat.
		/// </summary>
		public User User
		{
			get { return _user; }
		}
		/// <summary>
		/// The chat object to use to responde.
		/// </summary>
		public DCCChat NewChat
		{
			get { return _newchat; }
		}
	}
	#endregion
	#region DCCTransferRequestEventArgs
	public class DCCTransferRequestEventArgs : EventArgs
	{
		private User _user;
		private DCCTransfer _newtransfer;
		
		internal DCCTransferRequestEventArgs(User __user, DCCTransfer __newtransfer)
		{
			_user = __user;
			_newtransfer = __newtransfer;
		}
		/// <summary>
		/// User info of the user who sent a DCC file transfer request.
		/// </summary>
		public User User
		{
			get { return _user; }
		}
		/// <summary>
		/// The transfer object to use to responde.
		/// </summary>
		public DCCTransfer NewTransfer
		{
			get { return _newtransfer; }
		}
	}
	#endregion
	#region DCCTransferProgressEventArgs
	public class DCCTransferProgressEventArgs : EventArgs
	{
		private long _bytessent;
		private long _bytestotal;
		internal DCCTransferProgressEventArgs(long __bytessent, long __bytestotal)
		{
			_bytessent = __bytessent;
			_bytestotal = __bytestotal;
		}
		/// <summary>
		/// The number of bytes transfered.
		/// </summary>
		public long BytesSent
		{
			get { return _bytessent; }
		}
		/// <summary>
		/// The number of bytes to be transferred.
		/// </summary>
		public long BytesTotal
		{
			get { return _bytestotal; }
		}
	}
	#endregion
	#region DCCChatMessageEventArgs
	public class DCCChatMessageEventArgs : EventArgs
	{
		private string _message;
		internal DCCChatMessageEventArgs(string __message)
		{
			_message = __message;
		}
		/// <summary>
		/// The message sent from the remote user.
		/// </summary>
		public string Message
		{
			get { return _message; }
		}
	}
	#endregion
    #region ChannelListUpdateProgress
    public class ChannelListUpdateProgress : EventArgs
    {
        private int _count;
        private ChannelList _item;

        internal ChannelListUpdateProgress(int __count, ChannelList __item)
        {
            _count = __count;
            _item = __item;
        }

        /// <summary>
        /// Gets the new number of total channels in channel list.
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// Gets a ChannelList object, describing newly arrived channel list items details.
        /// </summary>
        public ChannelList ChannelInfo
        {
            get { return _item; }
        }
    }
    #endregion
#region UserEventArgs
	public class UserEventArgs : EventArgs
	{
		private User _user;
		
		internal UserEventArgs(User __user)
		{
			_user = __user;
		}
		
		public User User
		{
			get { return _user; }
		}
	}
#endregion
}
