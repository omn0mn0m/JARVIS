import speech_recognition as sr

class STT(object):

    def __init__(self):
        self.recogniser = sr.Recognizer()
        self.mic = sr.Microphone()

        
        with self.mic as source:
            print "A moment of silence please..."
            self.recogniser.adjust_for_ambient_noise(source)
    
    def listen(self):
        print "Say something!"
        
        with self.mic as source:
            audio = self.recogniser.listen(source)

        try:
            return self.recogniser.recognize_google(audio)
        except sr.UnknownValueError:
            print "I didn't quite get that..."
            return ''
        except sr.RequestError as e:
            print "Google Speech isn't working..."
            return ''
