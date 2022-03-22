#gui
from msilib.schema import Font
from multiprocessing.connection import wait
from tkinter import *
from tkinter import ttk
from tkinter.font import Font
import cv2, webbrowser, os, time
import pygame.camera
import pygame.image

#hand tracking
import GeneralAttribute, PacketSend
import cv2, numpy as np, pyautogui, threading
from cvzone.HandTrackingModule import HandDetector
from cvzone import FPS

def SetupNotification(content, position):
    lblNotif.config(text=content)
    lblNotif.place(x=position)

def StartExWebcam(index):
    global cap, scaleSet
    cap = cv2.VideoCapture(index)

    if int(trackingType.get()) == 1:
        HandVisualizing()
    elif int(trackingType.get()) == 2:
        exWebcamButton(True)
        SetupNotification("Ball Tracking Feature Still Under Maintenance!", 130)

def StartIPWebcam(ip):
    global cap, scaleSet

    ip_temp = ip.split(':')
    response = os.popen(f"ping {ip_temp[0]}").read()
    if "Received = 4" in response:
        cap = cv2.VideoCapture(f'http://{ip}/video')
        webbrowser.open(f'http://{ip}')

        if int(trackingType.get()) == 1:
            HandVisualizing()
        elif int(trackingType.get()) == 2:
            exWebcamButton(True)
            SetupNotification("Ball Tracking Feature Still Under Maintenance!", 130)

    else:
        ipWebcamButton(True)
        SetupNotification("Please Change The IP of Your Webcam", 145)

def ValidationInputExWebcam():
    if comboWebcam.get() != "Select Your Camera":
        for i in range(0, len(list_cam)):
            if comboWebcam.get() == list_cam[i]:
                StartExWebcam(i)

    else:
        exWebcamButton(True)
        SetupNotification("Please Select Your External Webcam/IP Webcam!", 120)

def ValidationInputIPWebcam():
    if len(entIPWebcam.get()) != 0:
        StartIPWebcam(entIPWebcam.get())
    else:
        ipWebcamButton(True)
        SetupNotification("Please Select Your External Webcam/IP Webcam!", 120)

def ScaleSetting(x):
    global scaleSet

    scaleSet = -x
    print(f"Scale Factor-{scaleSet}")

def HandVisualizing():
    global cap, scaleSet
    
    GeneralAttribute.isRun = True
    thread_sender = threading.Thread(target=PacketSend.StartSending)
    thread_sender.start()

    widthScreen, heightScreen = pyautogui.size()
    winName = 'NumberPedia-HandTracking'

    fpsReader = FPS()
    cap.set(3, 1280)
    cap.set(4, 720)

    detector = HandDetector(detectionCon=0.8, maxHands=4)
    hands_array = np.empty(10, dtype=object)

    cv2.namedWindow(winName)
    cv2.createTrackbar('Zoom Scalling', winName, 1, 100, ScaleSetting)
    cv2.setTrackbarMin('Zoom Scalling', winName, -50)
    cv2.setTrackbarMax('Zoom Scalling', winName, -1)
    cv2.setTrackbarPos('Zoom Scalling', winName, -50)

    while True:
        totalHand = 0
        success, img = cap.read()

        hands, img = detector.findHands(img)  # With Draw
        # hands = detector.findHands(img, draw=False)  # No Draw

        #get the webcam size
        height, width, channels = img.shape

        #prepare the crop
        centerX, centerY = int(height/2), int(width/2)
        radiusX, radiusY = int(scaleSet * height/100), int(scaleSet * width/100)

        minX, maxX = centerX - radiusX, centerX + radiusX
        minY, maxY = centerY - radiusY, centerY + radiusY

        cropped = img[minX:maxX, minY:maxY]
        resized_cropped = cv2.resize(cropped, (width, height))

        if hands:
            for index, hand_landmark in enumerate(hands):
                hands_array[index] = hands[index]
                disallowed_characters = "( )"
                totalHand = index

                if index == 0:
                    position_index_1 = str(hands_array[0]["center"])

                    GeneralAttribute.positionHand = f"{index + 1},{position_index_1}"
                    for char in disallowed_characters:
                        GeneralAttribute.positionHand = GeneralAttribute.positionHand.replace(char, "")

                elif index == 1:
                    position_index_1 = str(hands_array[0]["center"])
                    position_index_2 = str(hands_array[1]["center"]) 

                    GeneralAttribute.positionHand = f"{index + 1},{position_index_1},{position_index_2}"
                    for char in disallowed_characters:
                        GeneralAttribute.positionHand = GeneralAttribute.positionHand.replace(char, "")

                elif index == 2:
                    position_index_1 = str(hands_array[0]["center"])
                    position_index_2 = str(hands_array[1]["center"])  
                    position_index_3 = str(hands_array[2]["center"]) 

                    GeneralAttribute.positionHand = f"{index + 1},{position_index_1},{position_index_2},{position_index_3}"
                    for char in disallowed_characters:
                        GeneralAttribute.positionHand = GeneralAttribute.positionHand.replace(char, "")

                elif index == 3:
                    position_index_1 = str(hands_array[0]["center"]) 
                    position_index_2 = str(hands_array[1]["center"])  
                    position_index_3 = str(hands_array[2]["center"]) 
                    position_index_4 = str(hands_array[3]["center"])  

                    GeneralAttribute.positionHand = f"{index + 1},{position_index_1},{position_index_2},{position_index_3},{position_index_4}"
                    for char in disallowed_characters:
                        GeneralAttribute.positionHand = GeneralAttribute.positionHand.replace(char, "")

                else:
                    GeneralAttribute.positionHand = ""

        cv2.putText(resized_cropped, f'Esc To Stop Camera', (50, 50), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 128), 2)
        cv2.putText(resized_cropped, f'Slide To Right For Scalling Camera Zoom', (50, 80), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 255), 2)
        fps, img = fpsReader.update(resized_cropped,pos=(50, 610),color=(128,0,0),scale=1.5,thickness=2)
        cv2.putText(resized_cropped, f'Sending: {GeneralAttribute.positionHand}', (50, 640), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 128), 2)
        cv2.putText(resized_cropped, f'Total Hand Detected: {totalHand}', (50, 670), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 128), 2)
        
        result = cv2.resize(resized_cropped, [1280, 720])
        cv2.imshow(winName, result)
        
        if cv2.waitKey(1) == 27:
            cap.release()
            cv2.destroyAllWindows()
            GeneralAttribute.isRun = False
            
            exWebcamButton(True)
            ipWebcamButton(True)

            comboWebcam.set("Select Your Camera")
            break

def GetListWebcam():
    pygame.camera.init()
    cameras = pygame.camera.list_cameras()
    totalCamera = len(cameras)

    for i in range(0, totalCamera):
        list_cam.append(str(cameras[i]))

#define capture device
cap = None
list_cam = list()
GetListWebcam()

root = Tk()
root.title("NumberPedia")
root.iconbitmap('Assets/logo_game.ico')
root.geometry("550x300")

#import gui
img_logoNumberPedia = PhotoImage(file='Assets/ico_logo.png')
img_handEmoji = PhotoImage(file='Assets/ico_handemoji.png')
img_ballEmoji = PhotoImage(file='Assets/ico_ballemoji.png')
img_toggle_ex = PhotoImage(file='Assets/ico_toggleoff.png')
img_toggle_ip = PhotoImage(file='Assets/ico_toggleoff.png')

header1_font = Font(family="Poppins", size=12)
header2_font = Font(family="Poppins", size=10)

#setting gui
lblLogo = Label(root, image=img_logoNumberPedia)
lblLogo.place(x=275 - img_logoNumberPedia.width() / 2, y=75 - img_logoNumberPedia.height() / 2)

lblTracking = Label(root, text="Tracking Type", font=header1_font)
lblTracking.place(x=35, y=150)

lblWebcam = Label(root, text="External Webcam", font=header1_font)
lblWebcam.place(x=35, y=185)

lblIPWebcam = Label(root, text="IP Webcam", font=header1_font)
lblIPWebcam.place(x=35, y=220)

#tracking type
trackingType = IntVar()
trackingType.set(1)

trackingHand = Radiobutton(root, text="Hand Tracking", font=header2_font, variable=trackingType, value=1, image=img_handEmoji, compound='left')
trackingHand.place(x=200, y=150)

trackingBall = Radiobutton(root, text="Ball Tracking", font=header2_font, variable=trackingType, value=2, image=img_ballEmoji, compound='left')
trackingBall.place(x=375, y=150)

#external cam
def exWebcamButton(index):
    if index == True:
        img_toggle_ex.configure(file='Assets/ico_toggleoff.png')
        btnExWebcam.configure(command=lambda:exWebcamButton(False))
    elif index == False:
        img_toggle_ex.configure(file='Assets/ico_toggleon.png')
        btnExWebcam.configure(command=lambda:exWebcamButton(True))
        ValidationInputExWebcam()

comboWebcam = ttk.Combobox(root, value=list_cam, width=25)
comboWebcam.set("Select Your Camera")
comboWebcam.place(x=200, y=190)

lblExWebcam = Label(root, text="Start?", font=header2_font)
lblExWebcam.place(x=400, y=187)

btnExWebcam = Button(root, image=img_toggle_ex, border=0, command=lambda:exWebcamButton(False))
btnExWebcam.place(x=465, y=192)

#ip webcam
def ipWebcamButton(index):
    if index == True:
        img_toggle_ip.configure(file='Assets/ico_toggleoff.png')
        btnIpWebcam.configure(command=lambda:ipWebcamButton(False))
    elif index == False:
        img_toggle_ip.configure(file='Assets/ico_toggleon.png')
        btnIpWebcam.configure(command=lambda:ipWebcamButton(True))
        ValidationInputIPWebcam()

def ipWebcamEntry(event):
    entIPWebcam.config(state=NORMAL)
    entIPWebcam.delete(0, END)

entIPWebcam = Entry(root, border=0, width=28)
entIPWebcam.insert(0, "Insert IP And Port Webcam")
entIPWebcam.config(state=DISABLED)
entIPWebcam.bind("<Button-1>", ipWebcamEntry)
entIPWebcam.place(x=200, y=228)

lblIpWebcam = Label(root, text="Start?", font=header2_font)
lblIpWebcam.place(x=400, y=222)

btnIpWebcam = Button(root, image=img_toggle_ip, border=0, command=lambda:ipWebcamButton(False))
btnIpWebcam.place(x=465, y=227)

#notif
lblNotif = Label(root, text="", fg="red", font= header2_font)
lblNotif.place(x=120,y=260)

root.mainloop()