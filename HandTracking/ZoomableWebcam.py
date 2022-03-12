import cv2

def show_webcam(mirror=False):
    scale=20

    cam = cv2.VideoCapture(0)
    while True:
        ret_val, image = cam.read()
        if mirror: 
            image = cv2.flip(image, 1)


        #get the webcam size
        height, width, channels = image.shape

        #prepare the crop
        centerX,centerY=int(height/2),int(width/2)
        radiusX,radiusY= int(scale*height/100),int(scale*width/100)

        minX,maxX=centerX-radiusX,centerX+radiusX
        minY,maxY=centerY-radiusY,centerY+radiusY

        cropped = image[minX:maxX, minY:maxY]
        resized_cropped = cv2.resize(cropped, (width, height)) 

        cv2.imshow('my webcam', resized_cropped)
        if cv2.waitKey(1) == 27: 
            break

    cv2.destroyAllWindows()

def main():
    show_webcam(mirror=True)

if __name__ == '__main__':
    main()