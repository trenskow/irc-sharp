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
	public partial class Connection : IRCBase
	{
		#region Events
		/// <summary>
		/// Occurs when trying to connect to an IRC server and fails.
		/// </summary>
		/// <seealso cref="ConnectFailedEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event ConnectFailedEventHandler ConnectFailed;

		/// <summary>
		/// Occurs when the IRC control has succesfully connected and the MOTD has been retrieved.
		/// </summary>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event IRCEventHandler Connected;

		/// <summary>
		/// Occurs when the server disconnects, but before the Channels and DCC arrays is emptied.
		/// </summary>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event IRCEventHandler BeforeDisconnect;

		/// <summary>
		/// Occurs when the server disconnects.
		/// </summary>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event IRCEventHandler Disconnected;

		/// <summary>
		/// Occurs when the servers sends a server message.
		/// </summary>
		/// <seealso cref="ServerMessageEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event ServerMessageEventHandler ServerMessage;

		/// <summary>
		/// Occurs when user sends PING? request and the IRC control replyes with a PONG!.
		/// </summary>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event IRCEventHandler PingPong;

		/// <summary>
		/// Occurs when server disconnects because of an error, but before the Channels and DCC arrays is emptied.
		/// </summary>
		/// <seealso cref="ErrorEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event ErrorEventHandler BeforeError;

		/// <summary>
		/// Occurs when server disconnects because of an error.
		/// </summary>
		/// <seealso cref="ErrorEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event ErrorEventHandler Error;

		// TODO: Move to the ServerInfo class
		/// <summary>
		/// Occurs when the servers Message of the Day is updated.
		/// </summary>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event IRCEventHandler MessageOfTheDayUpdated;

		/// <summary>
		/// Occurs when the a user joins a channel.
		/// </summary>
		/// <seealso cref="JoinEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event JoinEventHandler Join;

		/// <summary>
		/// Occurs when the client has joined a channel, and all channel information has been retrieved (mode, topic and bans).
		/// </summary>
		/// <seealso cref="JoinCompleteEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event JoinCompleteEventHandler JoinComplete;

		/// <summary>
		/// Occurs when mode is changed in a channel.
		/// </summary>
		/// <seealso cref="ModeChangeEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event ModeChangeEventHandler ModeChange;

		/// <summary>
		/// Occurs when a user parts/leaves a channel, but before his/her info is removed from the Channels array.
		/// </summary>
		/// <seealso cref="PartEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event PartEventHandler BeforePart;

		/// <summary>
		/// Occurs when a user parts/leaves a channel.
		/// </summary>
		/// <seealso cref="PartEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event PartEventHandler Part;

		/// <summary>
		/// Occurs when a user disconnects from the IRC server, but before his/her info is removed from the Channls array.
		/// </summary>
		/// <seealso cref="QuitEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event QuitEventHandler BeforeQuit;

		/// <summary>
		/// Occurs when a user disconnects from the IRC server.
		/// </summary>
		/// <seealso cref="QuitEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event QuitEventHandler Quit;

		/// <summary>
		/// Occurs when a user send an invitation (/INVITE) to join a channel.
		/// </summary>
		/// <seealso cref="InvitationEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event InvitationEventHandler Invitation;

		/// <summary>
		/// Occurs when a user sends a message (/MSG) directly or to a channel.
		/// </summary>
		/// <seealso cref="MessageEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event MessageEventHandler Message;

		/// <summary>
		/// Occurs when a user sends a notice (/NOTICE) directly or to a channel.
		/// </summary>
		/// <seealso cref="MessageEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event MessageEventHandler Notice;

		/// <summary>
		/// Occurs when a user sends a CtCp request.
		/// </summary>
		/// <seealso cref="CtCpMessageEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event CtCpMessageEventHandler CtCpMessage;

		/// <summary>
		/// Occurs when the library need your client information. If not implemented, the client will return "TNets IRC Library v.0.7" 
		/// </summary>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event CtCpVersionReplyEventHandler CtCpVersionNeedsClientInfo;
	
		/// <summary>
		/// Occurs when a user replyes a CtCp message.
		/// </summary>
		/// <seealso cref="CtCpCommandReplyEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event CtCpCommandReplyEventHandler CtCpReply;

		/// <summary>
		/// Occurs when a user replyes a CtCp ping request.
		/// </summary>
		/// <seealso cref="CtCpPingReplyEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event CtCpPingReplyEventHandler CtCpPingReply;

		/// <summary>
		/// Occurs when a user replies a CtCp version request.
		/// </summary>
		/// <seealso cref="CtCpReplyEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event CtCpReplyEventHandler CtCpVersionReply;

		/// <summary>
		/// Occurs when a user replyes a CtCp finger request.
		/// </summary>
		/// <seealso cref="CtCpReplyEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event CtCpReplyEventHandler CtCpFingerReply;

		/// <summary>
		/// Occurs when a user replyes a CtCp time request.
		/// </summary>
		/// <seealso cref="CtCpReplyEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event CtCpReplyEventHandler CtCpTimeReply;

		/// <summary>
		/// Occurs whan a user han been kicked from a channel, but before his/her info is removes from the control.
		/// </summary>
		/// <seealso cref="KickEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event KickEventHandler BeforeKick;

		/// <summary>
		/// Occurs when a user han been kicked from a channel.
		/// </summary>
		/// <seealso cref="KickEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event KickEventHandler Kick;

		/// <summary>
		/// Occurs when the server sends a notice (/SNOTICE).
		/// </summary>
		/// <seealso cref="ServerNoticeEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event ServerNoticeEventHandler ServerNotice;

		/// <summary>
		/// Occurs when a user changes his nick, but before the nick is changed in the Channels array.
		/// </summary>
		/// <seealso cref="NickChangeEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event BeforeNickChangeEventHandler BeforeNickChange;

		/// <summary>
		/// Occurs when a user changes his nick.
		/// </summary>
		/// <seealso cref="NickChangeEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event NickChangeEventHandler NickChange;

		/// <summary>
		/// Occurs when a user is eather opped or deopped on a channel.
		/// </summary>
		/// <seealso cref="UserModeEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event UserModeEventHandler UserOpStatusChange;

		/// <summary>
		/// Occurs when a user is eather voiced or devoiced on a channel.
		/// </summary>
		/// <seealso cref="UserModeEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event UserModeEventHandler UserVoiceStatusChange;

		/// <summary>
		/// Occures when a user send a request for a DCC chat.
		/// </summary>
		/// <seealso cref="DCCChatRequestEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event DCCChatRequestEventHandler DCCChatRequest;

		/// <summary>
		/// Occures when a user send a file.
		/// </summary>
		/// <seealso cref="DCCTransferRequestEventArgs"/>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event DCCTransferRequestEventHandler DCCTransferRequest;

		/// <summary>
		/// Occurs when the server has confirmed your outside IP, and the LocalIP is updated.
		/// </summary>
		/// <threadsafe instance="false"/>
		/// <remarks>When this event is fired by the Connection, it is fired in another thread then the original thread 
		/// that created the class. Because of this you need to use the Invoke() method on your form if you're creating using Windows forms.</remarks>
		public event IRCEventHandler LocalIPUpdated;
		
		/// <summary>
		/// Occurs when the server cannot connect because nick already is in use on server. Return new nick.
		/// </summary>
		public event NickInUseEventHandler NickNameInUse;
		public event UserInfoEventHandler UserInfoUpdated;
		#endregion
	}
}
