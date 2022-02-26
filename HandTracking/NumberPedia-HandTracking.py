import time, cv2, pyautogui, mediapipe as mp
import XMLGenerator, HandPositionSender, GeneralAttribute, time, threading

def start_webcam():
    thread_sender = threading.Thread(target=HandPositionSender.SendPosition)
    thread_sender.start()

    widthScreen, heightScreen= pyautogui.size()
    winName = 'NumberPedia-HandTracking'

    mp_drawing = mp.solutions.drawing_utils
    mp_hands = mp.solutions.hands

    cap = cv2.VideoCapture(0)
    cap.set(3, 1280)
    cap.set(4, 720)

    with mp_hands.Hands(min_detection_confidence=0.8, min_tracking_confidence=0.8) as hands:
        while True:
            start = time.time()

            _, frame = cap.read()
            image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

            image.flags.writeable = False
            results = hands.process(image)

            image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
            heightImage, widthImage, channelImage = image.shape

            if results.multi_hand_landmarks:
                for hand_landmarks in results.multi_hand_landmarks:
                    hand_x = int(hand_landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_TIP].x * widthImage)
                    hand_y = int(hand_landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_TIP].y * heightImage)
                    
                    mp_drawing.draw_landmarks(
                        image, hand_landmarks, mp_hands.HAND_CONNECTIONS, mp_drawing.DrawingSpec(color=(0, 0, 255)),
                        mp_drawing.DrawingSpec(color=(255, 255, 255))
                    )

                    GeneralAttribute.posX = hand_x
                    GeneralAttribute.posY = hand_y
                    #XMLGenerator.GenerateXML(hand_x, hand_y)

            end = time.time()
            totalTime = end - start
            fps = 1 / totalTime

            cv2.putText(image, f'FPS: {int(fps)}', (20, 70), cv2.FONT_HERSHEY_SIMPLEX, 1.5, (0, 255, 0), 2)
            cv2.imshow(winName, image)
            cv2.moveWindow(winName, int(widthScreen / 2 - 640), int(heightScreen / 2 - 360))

            #if cv2.waitKey(1) & 0xFF == ord('q'):
            if cv2.waitKey(1) == 27:
                break

    cap.release()
    cv2.destroyAllWindows()
    GeneralAttribute.isRun = False

if __name__ == "__main__":
    start_webcam()