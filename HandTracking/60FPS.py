import mediapipe
import cv2

drawingModule = mediapipe.solutions.drawing_utils
handsModule = mediapipe.solutions.hands

cap = cv2.VideoCapture(2)
fourcc = cv2.VideoWriter_fourcc('m', 'p', '4', 'v')

with handsModule.Hands(static_image_mode=False, min_detection_confidence=0.7, min_tracking_confidence=0.7, max_num_hands=4) as hands:
     while True:
           ret, frame = cap.read()
           #flipped = cv2.flip(frame, flipCode = -1)
           frame1 = cv2.resize(frame, (640, 480))
           
           
           results = hands.process(cv2.cvtColor(frame1, cv2.COLOR_BGR2RGB))
           if results.multi_hand_landmarks != None:
              for handLandmarks in results.multi_hand_landmarks:
                  drawingModule.draw_landmarks(frame1, handLandmarks, handsModule.HAND_CONNECTIONS)
                  
                  #Below is Added Code to find and print to the shell the Location X-Y coordinates of Index Finger, Uncomment if desired
                  #for point in handsModule.HandLandmark:
                      
                      #normalizedLandmark = handLandmarks.landmark[point]
                      #pixelCoordinatesLandmark= drawingModule._normalized_to_pixel_coordinates(normalizedLandmark.x, normalizedLandmark.y, 640, 480)
                      
                      #Using the Finger Joint Identification Image we know that point 8 represents the tip of the Index Finger
                      #if point == 8:
                          #print(point)
                          #print(pixelCoordinatesLandmark)
                          #print(normalizedLandmark)
            
           cv2.imshow("Frame", frame1)
           key = cv2.waitKey(1) & 0xFF
           
           if key == ord("q"):
              break