#gui
from tkinter import *
from turtle import position
from PIL import Image, ImageTk
import cv2, imutils, webbrowser, os

#hand tracking
import HandPositionSender, GeneralAttribute
import cv2, numpy as np, pyautogui, threading
from cvzone.HandTrackingModule import HandDetector
from cvzone import FPS

def ValidationInputExWebcam():
    try:
        webcamChosen = int(listWebcam.get())
        if isinstance(webcamChosen, int):
            StartExWebcam(int(listWebcam.get()))

    except ValueError as error:
        lblNotif.config(text="Please Select Your\nExternal Webcam/IP Webcam!")

def ValidationInputIPWebcam():
    if len(entIPWebcam.get()) != 0:
        StartIPWebcam(entIPWebcam.get())
    else:
        lblNotif.config(text="Please Select Your\nExternal Webcam/IP Webcam!")

def ScaleSetting(x):
    global scaleSet

    scaleSet = -x
    print(f"Scale Factor-{scaleSet}")

def HandVisualizing():
    global cap, scaleSet
    
    GeneralAttribute.isRun = True
    thread_sender = threading.Thread(target=HandPositionSender.SendPosition)
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

        cv2.putText(resized_cropped, f'Esc To Stop Camera', (50, 50), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 128), 2)
        cv2.putText(resized_cropped, f'Slide To Right For Scalling Camera Zoom', (50, 80), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 255), 2)
        fps, img = fpsReader.update(resized_cropped,pos=(50, 610),color=(128,0,0),scale=1.5,thickness=2)
        cv2.putText(resized_cropped, f'Sending: {GeneralAttribute.positionHand}', (50, 640), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 128), 2)
        cv2.putText(resized_cropped, f'Total Hand Detected: {totalHand + 1}', (50, 670), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 128), 2)
        cv2.imshow(winName, resized_cropped)
        
        if cv2.waitKey(1) == 27:
            cap.release()
            cv2.destroyAllWindows()
            GeneralAttribute.isRun = False
            
            listWebcam.set("Select Your Camera")
            lblNotif.config(text="")
            break

def StartExWebcam(index):
    global cap, scaleSet
    cap = cv2.VideoCapture(index)

    if int(trackingType.get()) == 1:
        HandVisualizing()
    elif int(trackingType.get()) == 2:
        lblNotif.config(text="Calm Down,\nIt Still Under Maintenance")

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
            lblNotif.config(text="Calm Down,\nIt Still Under Maintenance")

    else:
        lblNotif.config(text="Please Change\nThe IP of Your Webcam")

#define capture device
cap = None
list_cam = list()
cams_test = 10
for i in range(0, cams_test):
    cap = cv2.VideoCapture(i)
    ret, frame = cap.read()
    
    if ret == True:
        list_cam.append(str(i))

root = Tk()
root.title("NumberPedia")

#import image
img_logoNumberPedia = PhotoImage(file='Assets/ico_logo.png')
img_handEmoji = PhotoImage(file='Assets/ico_handemoji.png')
img_ballEmoji = PhotoImage(file='Assets/ico_ballemoji.png')

#icon
lblHeader = Label(root)
lblHeader.grid(column=1, row=0, padx=0, pady=0)

lblLogo = Label(root, image=img_logoNumberPedia)
lblLogo.grid(column=1, row=1, padx=0, pady=0)

#tracking type
lblTracking = Label(root, text="Tracking Type:")
lblTracking.grid(column=0, row=2, padx=5, pady=2)

trackingType = IntVar()
trackingType.set(1)

trackingHand = Radiobutton(root, text="Hand Tracking", variable=trackingType, value=1, image=img_handEmoji, compound='left')
trackingHand.grid(column=1, row=2, padx=5, pady=2)

trackingBall = Radiobutton(root, text="Ball Tracking", variable=trackingType, value=2, image=img_ballEmoji, compound='left')
trackingBall.grid(column=2, row=2, padx=5, pady=2)

#external camera
listWebcam = StringVar()
listWebcam.set("Select Your Camera")

lblWebcam = Label(root, text="External Webcam:")
lblWebcam.grid(column=0, row=3, padx=5, pady=2)

dropWebcam = OptionMenu(root, listWebcam, *list_cam)
dropWebcam.grid(column=1, row=3, padx=5, pady=2)

btnStartExWebcam = Button(root, text="Start External Camera", command=ValidationInputExWebcam)
btnStartExWebcam.grid(column=2, row=3, padx=5, pady=2)

#ip webcam
lblIPWebcam = Label(root, text="IP Webcam:")
lblIPWebcam.grid(column=0, row=4, padx=5, pady=2)

entIPWebcam = Entry(root)
entIPWebcam.grid(column=1, row=4, padx=5, pady=2)

btnStartIPWebcam = Button(root, text="Start IP Camera", command=ValidationInputIPWebcam)
btnStartIPWebcam.grid(column=2, row=4, padx=5, pady=2)

#notif
lblNotif = Label(root, text="", fg="red")
lblNotif.grid(column=1, row=5, padx=5, pady=2)

root.mainloop()