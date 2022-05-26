import numpy as np
import cv2

def ContrastSetting(x):
    global contrastFactor
    contrastFactor = x

def BrightnessSetting(x):
    global brightnessFactor
    brightnessFactor = x

contrastFactor = 0
brightnessFactor = 0
cap = cv2.VideoCapture(3)
winName = "Brightness-Contrast"

cv2.namedWindow(winName)
cv2.createTrackbar('Contrast', winName, 100, 150, ContrastSetting)
cv2.setTrackbarPos('Contrast', winName, 125)

cv2.createTrackbar('Brightness', winName, 25, 75, BrightnessSetting)
cv2.setTrackbarPos('Brightness', winName, 50)

while(True):
    ret, frame = cap.read()
    
    frame = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV)
    frame[:,:,2] = np.clip((contrastFactor / 100) * frame[:,:,2] + brightnessFactor, 0, 255)
    frame = cv2.cvtColor(frame, cv2.COLOR_HSV2BGR)

    cv2.imshow(winName, frame)
    if cv2.waitKey(1) == 27:
        break

cap.release()
cv2.destroyAllWindows()