import pyttsx


def onStart(name):
    #print 'starting', name
    pass

def onWord(name, location, length):
    #print 'word', name, location, length
    pass

def onEnd(name, completed):
    #print 'finishing', name, completed
    pass


class TTS(object):
    def __init__(self):
        self.synth_engine = pyttsx.init()
        self.synth_engine.connect('starting-utterance', onStart)
        self.synth_engine.connect('started-word', onWord)
        self.synth_engine.connect('finished-utterance', onEnd)

    def say(self, message):
        print 'JARVIS: {}'.format(message)
        self.synth_engine.say(message)
        self.synth_engine.runAndWait()
        
    
