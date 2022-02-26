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
        time.sleep(0.10)

        #x = random.randint(0, 1280)
        #y = random.randint(0, 720)
        
        position = f"{GeneralAttribute.posX},{GeneralAttribute.posY}"
        bytesToSend = str.encode(position)

        # Send to server using created UDP socket
        UDPClientSocket.sendto(bytesToSend, serverAddressPort)
        print(f"Sending {position}")

    print("Thread Sender Killed")