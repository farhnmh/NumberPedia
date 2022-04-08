import time, random, socket
import GeneralAttribute

UDP_IP_ADDRESS = "127.0.0.1"
UDP_PORT_NO = 22222

def SendingPacket():
    print("Thread Sender Opened")
    clientSock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    
    while(GeneralAttribute.isRun):
        try:
            time.sleep(0.05)
            Message = f"1,{random.randint(0, 1280)},{random.randint(0, 720)}"
            clientSock.sendto(bytes(GeneralAttribute.positionHand,'utf-8'), (UDP_IP_ADDRESS, UDP_PORT_NO))
        
        except:
            clientSock.sendto(bytes(GeneralAttribute.positionHand,'utf-8'), (UDP_IP_ADDRESS, UDP_PORT_NO))
            pass
        
        #print(f"Sent: {GeneralAttribute.positionHand}")

    print("Thread Sender Closed")