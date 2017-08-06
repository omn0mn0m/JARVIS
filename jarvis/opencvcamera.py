import cv2
import sys

cascPath = "haarcascade_frontalface_default.xml"

class OpenCVGUI():

    def __init__(self):
        pass
        
    def show_frame(self, frame):
        cv2.imshow('Video', frame)
        cv2.moveWindow('Video', 0,0)

class OpenCVCamera():

    def __init__(self):
        self.faceCascade = cv2.CascadeClassifier(cascPath)
        self.video_capture = cv2.VideoCapture(0)

    def recognise_faces(self):
        # Capture frame-by-frame
        ret, frame = self.video_capture.read()
        
        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        
        faces = self.faceCascade.detectMultiScale(
            gray,
            scaleFactor=1.1,
            minNeighbors=5,
            minSize=(30, 30),
        )
        
        # Draw a rectangle around the faces
        for (x, y, w, h) in faces:
            cv2.rectangle(frame, (x, y), (x+w, y+h), (255, 0, 0), 2)
            
        return frame
        
    def get_capture(self):
        return self.video_capture

    def check_for_end(self, end_word):
        return not end_word == 'end'
        
    def end_camera(self):
        cv2.destroyAllWindows()
        self.video_capture.release()

if __name__ == '__main__':
    camera = OpenCVCamera()
    gui = OpenCVGUI()
    
    end_word = ""

    while camera.check_for_end(end_word):
        frame = camera.recognise_faces()
        gui.show_frame(frame)
        
        if cv2.waitKey(1) & 0xFF == ord('q'):
            end_word = "end"

    camera.end_camera() # Cleanup
