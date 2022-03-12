#hand tracking
from cProfile import label
from optparse import Option
import cv2, numpy as np, pyautogui
from matplotlib import image
from cvzone.HandTrackingModule import HandDetector
from cvzone import FPS

def start_webcam():
    widthScreen, heightScreen = pyautogui.size()
    winName = 'NumberPedia-HandTracking'

    fpsReader = FPS()
    cap = cv2.VideoCapture(1)
    cap.set(3, 1280)
    cap.set(4, 720)

    detector = HandDetector(detectionCon=0.8, maxHands=10)
    hands_array = np.empty(10, dtype=object)

    while True:
        success, img = cap.read()
        fps, img = fpsReader.update(img,pos=(50,80),color=(0,255,0),scale=1.5,thickness=1)

        hands, img = detector.findHands(img)  # With Draw
        # hands = detector.findHands(img, draw=False)  # No Draw

        if hands:
            for index, hand_landmark in enumerate(hands):
                hands_array[index] = hands[index]

                disallowed_characters = "( )"
                position_index = str(hands_array[index]["center"])  # center of the hand cx,cy
                for char in disallowed_characters:
                    position_index = position_index.replace(char, "")            

                print(f"{index},{position_index}")

        cv2.imshow(winName, img)
        if cv2.waitKey(1) == 27:
            break

if __name__ == "__main__":
    start_webcam()