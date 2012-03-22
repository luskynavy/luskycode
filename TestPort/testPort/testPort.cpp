// testPort.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "Winsock2.h"

SOCKET g_socket;

static char libnet_wsa_accept[]		= "libnet_wsa_accept";
static char libnet_wsa_read[]		= "libnet_wsa_read";
static char libnet_wsa_connect[]	= "libnet_wsa_connect";

static UINT WSA_ACCEPT				= 0;
static UINT WSA_READ				= 0;
static UINT WSA_CONNECT				= 0;

#define edSocket_READABLE 1
#define edSocket_FAILED -1
#define edSocket_NO_EVENT 0

bool InitWSA()
{
	int		status;
	WSADATA WSAData;

	if ((status = WSAStartup(MAKEWORD(1,1), &WSAData)) == 0) 
	{
/*		if ((WSA_READ = RegisterWindowMessage(libnet_wsa_read)) != 0)
		{
			if ((WSA_ACCEPT = RegisterWindowMessage(libnet_wsa_accept)) != 0)
			{
				if ((WSA_CONNECT=RegisterWindowMessage(libnet_wsa_connect)) != 0)
				{
					return true;
				}
			}
		}*/

		return true;
	}
	return false;
}

int TermWSA()
{
	return WSACleanup();
}

bool InitSocket(USHORT port, bool use_udp)
{
	g_socket = socket(AF_INET, (use_udp ? SOCK_DGRAM : SOCK_STREAM),  (use_udp ? IPPROTO_UDP : IPPROTO_TCP));
	if (g_socket == INVALID_SOCKET)
	{
		printf("Failed to create the socket, error %d.\n", WSAGetLastError());
		return false;
	}

	// Bind the socket
	SOCKADDR_IN sa;
	sa.sin_family      = AF_INET;           // IP family
	sa.sin_addr.s_addr = INADDR_ANY;        // Use the only IP that's available to us
	sa.sin_port        = htons(port);
	if (bind(g_socket, (SOCKADDR*)&sa, sizeof(sa)) != 0)
	{
		printf("Failed to bind the socket, error %d.\n", WSAGetLastError());
	}

	// Mark the socket as nonblocking
	DWORD dwNonblocking = 1;
	if (ioctlsocket(g_socket, FIONBIO, &dwNonblocking) != 0)
	{
		printf("Failed to set the socket to nonblocking, error %d.\n", WSAGetLastError());
		return false;
	}

	if (!use_udp)
	{
		int ret;
		if (ret = listen(g_socket, 1) != 0)
		{
			printf("Failed to listen to the socket, error %d.\n", WSAGetLastError());
		}

		if (ret = connect(g_socket , (struct sockaddr *)&sa, sizeof(sa)) != 0)
		{
			printf("Failed to connect to the socket, error %d.\n", WSAGetLastError());
		}
	}
	

	return true;
}

char _IsReadableTimed(unsigned long time_sec, unsigned long time_msec)
{
	int				err;
	fd_set			read_set;
	struct timeval	tv;

	tv.tv_sec		= time_sec;
	tv.tv_usec		= time_msec;

	FD_ZERO(&read_set);
	FD_SET(g_socket, &read_set);

	err = select((int)g_socket + 1, (struct fd_set *)&read_set, 0, (struct fd_set *)0, &tv);
	if (err == edSocket_FAILED)
		return edSocket_FAILED;

	if (FD_ISSET(g_socket, &read_set))
		return edSocket_READABLE;

	return edSocket_NO_EVENT;
}

int SendData(USHORT port, unsigned long ip)
{
	char buffer_send[2000];
	int message_len = 0;
	sockaddr_in sa = {0};
	sa.sin_addr.S_un.S_addr	= ip;
	sa.sin_family			= AF_INET;
	sa.sin_port				= htons(port);

	int ret = sendto(g_socket, (char*)buffer_send, 100, 0, (SOCKADDR*)&sa, sizeof(sa));

	return ret;
}

bool ReadParams(int argc, char* argv[], unsigned long* ip, USHORT* port, bool* use_udp)
{
	if (argc != 3)
	{
		return false;
	}
	int b1, b2, b3, b4;

	sscanf((const char*)argv[1], "%d.%d.%d.%d:", &b1, &b2, &b3, &b4, &port);

	*ip = (b4 << 24) + (b3 << 16) + (b2 << 8) + b1;

	*use_udp = _stricmp((const char*)argv[2], "udp") == 0;

	return true;
}

//int _tmain(int argc, _TCHAR* argv[])
int main(int argc, char* argv[])
{
	USHORT port = 3000;	
	unsigned long ip = (1 << 24) + 127;
	bool use_udp = true;


	bool param_ok = ReadParams(argc, argv, &ip, &port, &use_udp);
	if (!param_ok)
	{
		printf("Format : testport ip.ip.ip.ip:port udp/tcp\n");
		exit(1);
	}

	InitWSA();
	InitSocket(port, use_udp);

	SOCKADDR_IN sa;
	INT size = sizeof(sa);
	INT ret;
	char buffer_recv[2000];

	DWORD last_sent = 0;

	while (1)
	{
		DWORD now = timeGetTime();
		while (_IsReadableTimed(0, 0))
		{
			ret = recvfrom(g_socket, (CHAR*)buffer_recv, sizeof(buffer_recv), 0, (SOCKADDR*)&sa, &size);
			if (ret != SOCKET_ERROR  &&  ret > 0  &&  ntohs(sa.sin_port) == port)
			{
				printf("Data received from %u.%u.%u.%u:%u size %d\n",
					(sa.sin_addr.S_un.S_addr & 0xFF),
					(sa.sin_addr.S_un.S_addr >> 8) & 0xFF,
					(sa.sin_addr.S_un.S_addr >> 16) & 0xFF,
					(sa.sin_addr.S_un.S_addr >> 24) & 0xFF,
					port,
					ret);
			}
			if (ret == SOCKET_ERROR)
			{
				int error = WSAGetLastError();
			}
		}

		if (now > last_sent + 1000)
		{
			ret = SendData(port, ip);
			printf("Data sent to %u.%u.%u.%u:%u size %d\n",
				(ip >> 0)  & 0xFF,
				(ip >> 8)  & 0xFF,
				(ip >> 16) & 0xFF,
				(ip >> 24) & 0xFF,
				port,
				ret);
			last_sent = now;
		}
	}

	TermWSA();
	return 0;
}

