import socket, time, random
import GeneralAttribute

index = 0
serverAddressPort = ("127.0.0.1", 8080)
bufferSize = 1024

# Create a UDP socket at client side
UDPClientSocket = socket.socket(family=socket.AF_INET, type=socket.SOCK_DGRAM)

def SendPosition():
    print("Thread Sender Started")
    
    while GeneralAttribute.isRun == True:
        time.sleep(0.05)
        bytesToSend = str.encode(GeneralAttribute.positionHand)

        # Send to server using created UDP socket
        UDPClientSocket.sendto(bytesToSend, serverAddressPort)
        print(f"Sending {GeneralAttribute.positionHand}")

    print("Thread Sender Killed")