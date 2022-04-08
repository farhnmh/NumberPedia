#gui
from msilib.schema import Font
from tkinter import *
from tkinter import ttk
from tkinter.font import Font
import cv2, os, pygame.image, pygame.camera

#tracking
from cvzone.HandTrackingModule import HandDetector
from cvzone.ColorModule import ColorFinder
from cvzone import FPS 
import GeneralAttribute, PacketSender, UDPDataSender
import os, cv2, numpy as np, pyautogui, threading, cvzone, imutils, socket
    
def SetupNotification(content, position):
    lblNotif.config(text=content)
    lblNotif.place(x=position)

def StartExWebcam(index):
    global cap, scaleSet, webcamStatus
    cap = cv2.VideoCapture(index)

    if int(trackingType.get()) == 1:
        HandVisualizing()
    elif int(trackingType.get()) == 2:
        BallColorVisualizing()
        #webcamStatus = False
        #exWebcamButton(True)
        #SetupNotification("Ball Tracking Feature Still Under Maintenance!", 130)

def ValidationInputExWebcam():
    global webcamStatus
    if comboWebcam.get() != "Select Your Camera":
        for i in range(0, len(list_cam)):
            if comboWebcam.get() == list_cam[i]:
                StartExWebcam(i)

    else:
        webcamStatus = False
        exWebcamButton(True)
        SetupNotification("Please Select Your External Webcam!", 150)

def ScaleSetting(x):
    global scaleSet
    scaleSet = -x

def BallCircleVisualizing():
    global cap, scaleSet, webcamStatus
    
    SetupNotification("Your Camera Opened. Setup It and Launch The Game!", 100)
    GeneralAttribute.isRun = True
    thread_sender = threading.Thread(target=UDPDataSender.SendingPacket)
    thread_sender.start()

    widthScreen, heightScreen = pyautogui.size()
    winName = 'NumberPedia-HandTracking'

    greenLower = (29, 86, 6)
    greenUpper = (64, 255, 255)

    fpsReader = FPS()
    cap.set(3, 1280)
    cap.set(4, 720)

    cv2.namedWindow(winName)
    cv2.createTrackbar('Zoom Scalling', winName, 1, 100, ScaleSetting)
    cv2.setTrackbarMin('Zoom Scalling', winName, -50)
    cv2.setTrackbarMax('Zoom Scalling', winName, -1)
    cv2.setTrackbarPos('Zoom Scalling', winName, -50)

    while True:
        success, img = cap.read()

        #get the webcam size
        height, width, channels = img.shape

        #prepare the crop
        centerX, centerY = int(height/2), int(width/2)
        radiusX, radiusY = int(scaleSet * height/100), int(scaleSet * width/100)

        minX, maxX = centerX - radiusX, centerX + radiusX
        minY, maxY = centerY - radiusY, centerY + radiusY

        cropped = img[minX:maxX, minY:maxY]
        resized_cropped = cv2.resize(cropped, (width, height))

        result = imutils.resize(resized_cropped, width=600)
        hsv = cv2.cvtColor(result, cv2.COLOR_BGR2HSV)

        mask = cv2.inRange(hsv, greenLower, greenUpper)
        mask = cv2.erode(mask, None, iterations=2)
        mask = cv2.dilate(mask, None, iterations=2)

        cnts = cv2.findContours(mask.copy(), cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)[-2]
        center = None

        if len(cnts) > 0:
            c = max(cnts, key=cv2.contourArea)
            ((x, y), radius) = cv2.minEnclosingCircle(c)

            if radius > 10:
                cv2.circle(result, (int(x), int(y)), int(radius), (0, 255, 255), 2)
    
        #result = cv2.resize(result, (0, 0), None, 0.7, 0.7)
        cv2.imshow(winName, result)

        if cv2.waitKey(1) & 0xFF == 27:
            cap.release()
            cv2.destroyAllWindows()
            GeneralAttribute.isRun = False
            webcamStatus = False
            
            SetupNotification("Setup and Start Webcam First! Then Launch The Game", 105)
            exWebcamButton(True)
            break

def BallColorVisualizing():
    global cap, scaleSet, webcamStatus
    
    SetupNotification("Your Camera Opened. Setup It and Launch The Game!", 100)
    GeneralAttribute.isRun = True
    thread_sender = threading.Thread(target=UDPDataSender.SendingPacket)
    thread_sender.start()

    widthScreen, heightScreen = pyautogui.size()
    winName = 'NumberPedia-HandTracking'

    fpsReader = FPS()
    cap.set(3, 1280)
    cap.set(4, 720)

    cv2.namedWindow(winName)
    cv2.createTrackbar('Zoom Scalling', winName, 1, 100, ScaleSetting)
    cv2.setTrackbarMin('Zoom Scalling', winName, -50)
    cv2.setTrackbarMax('Zoom Scalling', winName, -1)
    cv2.setTrackbarPos('Zoom Scalling', winName, -50)

    success, img = cap.read()
    h, w, _ = img.shape
    
    myColorFinder = ColorFinder(False)
    hsvVals = {'hmin': 33, 'smin': 72, 'vmin': 126, 'hmax': 58, 'smax': 255, 'vmax': 255}

    while True:
        success, img = cap.read()

        #get the webcam size
        height, width, channels = img.shape

        #prepare the crop
        centerX, centerY = int(height/2), int(width/2)
        radiusX, radiusY = int(scaleSet * height/100), int(scaleSet * width/100)

        minX, maxX = centerX - radiusX, centerX + radiusX
        minY, maxY = centerY - radiusY, centerY + radiusY

        cropped = img[minX:maxX, minY:maxY]
        resized_cropped = cv2.resize(cropped, (width, height))

        if isNormal.get() == True:
            result = resized_cropped
        elif isVertical.get() == True and isHorizontal.get() == True:
            resultTemp = cv2.flip(resized_cropped, 0)
            result = cv2.flip(resultTemp, 1)
        elif isVertical.get() == True:
            result = cv2.flip(resized_cropped, 0)
        elif isHorizontal.get() == True:
            result = cv2.flip(resized_cropped, 1)

        imgColor, mask = myColorFinder.update(result, hsvVals)
        result, contours = cvzone.findContours(result, mask)

        if contours:
            for index, ball_landmark in enumerate(contours):
                disallowed_characters = "( )"
                totalBall = index

                if index == 0:
                    data = f"{contours[0]['center'][0]},{h - contours[0]['center'][1]},{int(contours[0]['area'])}"
                    
                    for char in disallowed_characters:
                        data = data.replace(char, "")    
                    
                    #print(f"{index},{data}")
    
        #imgStack = cvzone.stackImages([imgContour, imgColor], 1, 0.5)
        #cv2.imshow(winName, imgStack)
        result = cv2.resize(result, (0, 0), None, 0.7, 0.7)
        cv2.imshow(winName, result)

        if cv2.waitKey(1) & 0xFF == 27:
            cap.release()
            cv2.destroyAllWindows()
            GeneralAttribute.isRun = False
            webcamStatus = False
            
            SetupNotification("Setup and Start Webcam First! Then Launch The Game", 105)
            exWebcamButton(True)
            break

def HandVisualizing():
    global cap, scaleSet, webcamStatus
    
    SetupNotification("Your Camera Opened. Setup It and Launch The Game!", 100)
    GeneralAttribute.isRun = True
    #thread_sender = threading.Thread(target=UDPDataSender.SendingPacket)
    #thread_sender.start()

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

        #get the webcam size
        height, width, channels = img.shape

        #prepare the crop
        centerX, centerY = int(height/2), int(width/2)
        radiusX, radiusY = int(scaleSet * height/100), int(scaleSet * width/100)

        minX, maxX = centerX - radiusX, centerX + radiusX
        minY, maxY = centerY - radiusY, centerY + radiusY

        cropped = img[minX:maxX, minY:maxY]
        resized_cropped = cv2.resize(cropped, (width, height))

        if isNormal.get() == True:
            result = resized_cropped
        elif isVertical.get() == True and isHorizontal.get() == True:
            resultTemp = cv2.flip(resized_cropped, 0)
            result = cv2.flip(resultTemp, 1)
        elif isVertical.get() == True:
            result = cv2.flip(resized_cropped, 0)
        elif isHorizontal.get() == True:
            result = cv2.flip(resized_cropped, 1)

        hands, result = detector.findHands(result)  # With Draw
        # hands = detector.findHands(img, draw=False)  # No Draw

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

                sock.sendto(str.encode(str(GeneralAttribute.positionHand)), serverAddressPort)
                
        else:
            GeneralAttribute.positionHand = ""
        
        cv2.putText(result, f'Esc To Stop Camera', (50, 50), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 128), 2)
        cv2.putText(result, f'Slide To Right For Scalling Camera Zoom', (50, 80), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 255), 2)
        fps, img = fpsReader.update(result,pos=(50, 610),color=(128,0,0),scale=1.5,thickness=2)
        cv2.putText(result, f'Sending: {GeneralAttribute.positionHand}', (50, 640), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 128), 2)
        cv2.putText(result, f'Total Hand Detected: {totalHand + 1}', (50, 670), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 128), 2)
        
        result = cv2.resize(result, (0, 0), None, 0.7, 0.7)
        cv2.imshow(winName, result)
        
        if cv2.waitKey(1) == 27:
            cap.release()
            cv2.destroyAllWindows()
            GeneralAttribute.isRun = False
            webcamStatus = False
            
            SetupNotification("Setup and Start Webcam First! Then Launch The Game", 105)
            exWebcamButton(True)
            break

def GetListWebcam():
    pygame.camera.init()
    cameras = pygame.camera.list_cameras()
    totalCamera = len(cameras)

    for i in range(0, totalCamera):
        list_cam.append(str(cameras[i]))

#define capture device
cap = None
webcamStatus = False
list_cam = list()
GetListWebcam()

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = ("127.0.0.1", 5053)

root = Tk()
root.title("NumberPedia")
root.geometry("550x350")

#import gui
img_logoNumberPedia = PhotoImage(file='Assets/ico_logo.png')
img_handEmoji = PhotoImage(file='Assets/ico_handemoji.png')
img_ballEmoji = PhotoImage(file='Assets/ico_ballemoji.png')
img_toggle_ex = PhotoImage(file='Assets/ico_toggleoff.png')
img_launchGame = PhotoImage(file='Assets/ico_launchgame.png')

header1_font = Font(family="Poppins", size=12)
header2_font = Font(family="Poppins", size=10)

#setting gui
lblLogo = Label(root, image=img_logoNumberPedia)
lblLogo.place(x=275 - img_logoNumberPedia.width() / 2, y=75 - img_logoNumberPedia.height() / 2)

lblTracking = Label(root, text="Tracking Type", font=header1_font)
lblTracking.place(x=35, y=150)

lblWebcam = Label(root, text="Flipping Type", font=header1_font)
lblWebcam.place(x=35, y=185)

lblWebcam = Label(root, text="External Webcam", font=header1_font)
lblWebcam.place(x=35, y=220)

#tracking type
trackingType = IntVar()
trackingType.set(1)

trackingHand = Radiobutton(root, text="Hand Tracking", font=header2_font, variable=trackingType, value=1, image=img_handEmoji, compound='left')
trackingHand.place(x=205, y=150)

trackingBall = Radiobutton(root, text="Ball Tracking", font=header2_font, variable=trackingType, value=2, image=img_ballEmoji, compound='left')
trackingBall.place(x=380, y=150)

#flip type
isNormal = BooleanVar()
isVertical = BooleanVar()
isHorizontal = BooleanVar()

flipNormal = Checkbutton(root, text="Normal", font=header2_font, variable=isNormal, onvalue=True, offvalue=False)
flipNormal.place(x=205, y=185)
flipNormal.select()

flipVertical = Checkbutton(root, text="Vertically", font=header2_font, variable=isVertical, onvalue=True, offvalue=False)
flipVertical.place(x=285, y=185)

flipHorizontal = Checkbutton(root, text="Horizontally", font=header2_font, variable=isHorizontal, onvalue=True, offvalue=False)
flipHorizontal.place(x=380, y=185)

#external cam
def exWebcamButton(index):
    global webcamStatus

    if index == True:
        if webcamStatus == True:
            SetupNotification("Please Close Your Webcam With Esc Button!", 130)
        else:
            webcamStatus = False
            img_toggle_ex.configure(file='Assets/ico_toggleoff.png')
            btnExWebcam.configure(command=lambda:exWebcamButton(False))

    elif index == False:
        webcamStatus = True
        img_toggle_ex.configure(file='Assets/ico_toggleon.png')
        btnExWebcam.configure(command=lambda:exWebcamButton(True))

        if isNormal.get() == True and isVertical.get() == True or isNormal.get() == True and isHorizontal.get() == True:
            webcamStatus = False
            exWebcamButton(True)
            SetupNotification("You Only Choose Normal Or Set Horizontal And Vertical!", 100)
        elif isNormal.get() == isVertical.get() == isHorizontal.get() == False:
            webcamStatus = False
            exWebcamButton(True)
            SetupNotification("Please Select One Of Flip Type Above!", 150)
        else:
            SetupNotification("Loading... Please Wait!", 200)
            thread_webcam = threading.Thread(target=lambda:ValidationInputExWebcam())
            thread_webcam.start()

comboWebcam = ttk.Combobox(root, value=list_cam, width=25)
comboWebcam.set("Select Your Camera")
comboWebcam.place(x=200, y=225)

lblExWebcam = Label(root, text="Start?", font=header2_font)
lblExWebcam.place(x=400, y=222)

btnExWebcam = Button(root, image=img_toggle_ex, border=0, command=lambda:exWebcamButton(False))
btnExWebcam.place(x=465, y=228)

#game
def ThreadForGame():
    global webcamStatus

    if webcamStatus == True:
        thread_game = threading.Thread(target=PlayTheGame)
        thread_game.start()
    else:
        SetupNotification("Setup The Webcam First, Please!", 170)

def PlayTheGame():
    root_directory = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
    root_directory = os.path.join(root_directory, 'NumberPedia', 'NumberPedia.exe')
    game_directory = root_directory.replace("\\", "/")
    os.system(game_directory)
    #print(game_directory)

btnLaunchGame = Button(root, image=img_launchGame, border=0, command=lambda:ThreadForGame())
btnLaunchGame.place(x=275 - img_launchGame.width() / 2, y=275 - img_launchGame.height() / 2)

#notif
lblNotif = Label(root, text="Setup and Start Webcam First! Then Launch The Game", fg="red", font= header2_font)
lblNotif.place(x=105,y=300)

root.mainloop()