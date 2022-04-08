import socket, random, time
import GeneralAttribute

# Create a datagram socket
UDPServerSocket = socket.socket(family=socket.AF_INET, type=socket.SOCK_DGRAM)

def StartSending():
    # Bind to address and ip
    UDPServerSocket.bind((GeneralAttribute.localIP, GeneralAttribute.localPort))
    print("Thread Sender Started")

    # Listen for incoming datagrams
    #bytesAddressPair = UDPServerSocket.recvfrom(bufferSize)
    #message = bytesAddressPair[0]
    #address = bytesAddressPair[1]
    #clientIP  = "Unity IP Address:{}".format(address)
    #print(clientIP)

    while(GeneralAttribute.isRun):
        time.sleep(0.015)

        # Sending a reply to client
        messageByte = str.encode(GeneralAttribute.positionHand)
        UDPServerSocket.sendto(messageByte, ("127.0.0.1", 8000))
        #print(f"Data Sent: {GeneralAttribute.positionHand}")

    print("Thread Sender Killed")